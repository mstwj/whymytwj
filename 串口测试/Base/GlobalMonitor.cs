using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace 串口测试.Base
{
    public class GlobalMonitor
    {

        public static SerialInfo SerialInfo { get; set; }

        public static List<StorageModel> StorageList { get; set; }
        public static List<DeviceModel> DeviceList { get; set; }

        static bool isRunning = true;
        static Task mainTask = null;
        static RTU rtuInstance = null;

        public static void Start(Action successAction, Action<string> faultAction)
        {
            mainTask = Task.Run(async () =>
            {
                //这里设计的很巧妙... 可以说，写的很好..
                //初始化，就应该这样去写...
                IndustrialBLL bll = new IndustrialBLL();
                var si = bll.InitSerialInfo();

                if (si.State)
                    SerialInfo = si.Data;
                else
                {
                    faultAction(si.Message);
                    return;
                }

                // 获取存储区信息
                //DataResult<List<StorageModel>> 这个是返回的..
                var sa = bll.InitStorageArea();
                if (sa.State)
                    StorageList = sa.Data; //sa.Data是什么.. 是一个LIST。。。
                else
                {
                    faultAction(sa.Message); return;
                }

                // 初始化设备变量集合及警戒值
                var dr = bll.InitDevices();
                if (dr.State)
                    DeviceList = dr.Data;
                else
                {
                    faultAction(dr.Message); return;
                }


                /// 初始化ModbusRTU串口通信
                /// //这里也就一个呀...
                /// //如果是2个呢？那不是使用同一个了吗？
                rtuInstance = RTU.GetInstance(SerialInfo);
                rtuInstance.ResponseData = new Action<int, List<byte>>(ParsingData);
             
                if (rtuInstance.Connection())
                {
                    successAction.Invoke();

                    while (isRunning)
                    {
                        int startAddress = 0;
                        Task.Delay(1000);

                        //int readCount = 1;
                        //
                        //                    
                        //Task<bool> Send(int slaveAddr, byte funcCode, int startAddr, int len)
                        await rtuInstance.Send(1, (byte)3, 0, 1);
                        /*
                        if (item.Length > 100)
                        {
                            startAddr = item.StartAddress;
                            int readCount = item.Length / 100;
                            for (int i = 0; i < readCount; i++)
                            {
                                int readLen = i == readCount ? item.Length - 100 * i : 100;
                                await rtuInstance.Send(item.SlaveAddress, (byte)int.Parse(item.FuncCode), startAddr + 100 * i, readLen);
                            }
                        }
                        if (item.Length % 100 > 0)
                        {
                            await rtuInstance.Send(item.SlaveAddress, (byte)int.Parse(item.FuncCode), startAddr + 100 * (item.Length / 100), item.Length % 100);
                        }
                        好像以上代码在循环发送，发送什么呢？好像是在不停发送，MONDBUS...
                        */

                    }
                }
                else
                {
                    faultAction.Invoke("程序无法启动，串口连接初始化失败！请检查设备是否连接正常。");
                }
                

            });
        }

        private static void ParsingData(int start_addr, List<byte> byteList)
        {
            //这里和我预料的是一样的，回的数据是对的.. 11 22 33 44 我发送一次回一次..
            //串口回调到这里了..
            if (byteList != null && byteList.Count > 0)
            {
                int startByte = 4;
                byte[] res = null;

                res = new byte[4] { 0,0,byteList[startByte], byteList[startByte + 1] };
                //item.CurrentValue = res.ByteArrsyToFloat();
                // 查找设备监控点位与当前返回报文相关的监控数据列表
                // 根据从站地址、功能码、起始地址
                //这里没有一直去读取数据库。只是初始化读了一次，然后就开始工作了.. 
                //没有一直读取数据库呀...
                /*
                var mvl = (from q in DeviceList
                           from m in q.MonitorValueList
                           where m.StorageAreaId == (byteList[0].ToString() + byteList[1].ToString("00") + start_addr.ToString())
                           select m
                         ).ToList();
                */
            }
        }
        public static void Dispose()
        {
            isRunning = false;
            //foreach (var item in CommList)
             //   item.Close();

            if (mainTask != null)
                mainTask.Wait();
        }
    }

    public static class ExtendClass
    {
        public static float ByteArrsyToFloat(this byte[] value)
        {
            float fValue = 0f;
            uint nRest = ((uint)value[2]) * 256
                + ((uint)value[3]) +
                65536 * (((uint)value[0]) * 256 + ((uint)value[1]));
            unsafe
            {
                float* ptemp;
                ptemp = (float*)(&nRest);
                fValue = *ptemp;
            }
            return fValue;
        }
    }
}
