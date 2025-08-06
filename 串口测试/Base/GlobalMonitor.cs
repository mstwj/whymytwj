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

        static bool isRunning = true;
        static Task mainTask = null;

        public static void Start(Action successAction, Action<string> faultAction)
        {
            mainTask = Task.Run(() =>
            {
                IndustrialBLL bll = new IndustrialBLL();
                var si = bll.InitSerialInfo();

                if (si.State)
                    SerialInfo = si.Data;
                else
                {
                    faultAction(si.Message);
                    return;
                }


                /// 初始化ModbusRTU串口通信
                /// //这里也就一个呀...
                /// //如果是2个呢？那不是使用同一个了吗？
                var rtu = RTU.GetInstance(SerialInfo);
                rtu.ResponseData = new Action<int, List<byte>>(ParsingData);
             
                if (rtu.Connection())
                {
                    successAction.Invoke();

                    while (isRunning)
                    {
                        int startAddress = 0;
                        Task.Delay(1000);
                        //

                    }
                }
                else
                {
                    faultAction.Invoke("程序无法启动，串口连接初始化失败！请检查设备是否连接正常。");
                }
                

            });
        }

        private static void ParsingData(int start_addr,List<byte> byteList)
        {
            //串口回调到这里了..

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
}
