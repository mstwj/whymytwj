using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLC自己动手2.Models;

namespace PLC自己动手2.Base
{
    public class GlobalMonitor
    {
        //这里为什么，要全局的呢？很简单，因为 这些功能是无法放到任何一个页面里面的去写的，只能是全局.
        public GlobalMonitor()
        {
            
        }

        //为什么是一个ObservableCollection 不是LIST呢？
        //因为，界面要直接去绑定到这里，这就是为什么..
        public static ObservableCollection<DeviceModel> DeviceList { get; set; } = new ObservableCollection<DeviceModel>();

        static bool isRunning = true;
        static Task mainTask = null;

        public static void Start()
        {
            mainTask = Task.Run(async () =>
            {
                // 获取设备信息
                //DeviceList.Add(new DeviceModel { Name = "#1 Master device info" });
                //DeviceList.Add(new DeviceModel { Name = "#2 Master device info" });
                //DeviceList.Add(new DeviceModel { Name = "#3 Master device info" });
                //DeviceList.Add(new DeviceModel { Name = "#4 Master device info" });

                //1001 #1 Master device info 8937-45845735 2 
                DeviceService deviceService = new DeviceService();
                var list = deviceService.GetDevices();
                if (list != null)
                    foreach (var item in list)
                    {
                        DeviceList.Add(item);
                    }

                while (isRunning)
                {
                    await Task.Delay(1000);

                    foreach (var item in DeviceList)
                    {
                        if (item.CommType == 2)// S7通信
                        {
                           // List<string> addrList = item.MonitorValueList.Select(v => v.Address).ToList();

                            /*
                            //Zhaoxi.Communication.Siemens.S7Net s7Net = new Communication.Siemens.S7Net(item.S7.IP, item.S7.Port, (byte)item.S7.Rock, (byte)item.S7.Slot);

                            List<string> addrList = item.MonitorValueList.Select(v => v.Address).ToList();
                            //var result = s7Net.Read<ushort>(addrList);
                            if (result.IsSuccessed)
                            {
                                for (int i = 0; i < item.MonitorValueList.Count; i++)
                                {
                                    item.MonitorValueList[i].Value = result.Datas[i];
                                }
                            }

                            s7Net.Close();
                            */
                        }
                    }
                }
            

            });
        }

        public static void Stop()
        {
            isRunning = false;
            mainTask.ConfigureAwait(true);
        }
    }
}
