using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControlUI控件使用.Models;

namespace HandyControlUI控件使用.Base
{
    public class GlobalMonitor
    {
        public GlobalMonitor()
        {
            
        }

        //为什么是一个ObservableCollection 不是LIST呢？
        //因为，界面要直接去绑定到这里，这就是为什么..
        public static ObservableCollection<StudioModel> StudioList { get; set; } = new ObservableCollection<StudioModel>();

        static bool isRunning = true;
        static Task mainTask = null;

        public static void Start()
        {
            mainTask = Task.Run(async () =>
            {               
                //1001 #1 Master device info 8937-45845735 2 
                DataServer dataServer = new DataServer();
                //麻痹的，SQL必须是6.0.0.要不然，就几把报错.... 还不能是高版本..
                var list = dataServer.GetStudio();
                if (list != null)
                    foreach (var item in list)
                    {
                        StudioList.Add(item);
                    }

                while (isRunning)
                {
                    
                    await Task.Delay(1000);
                    /*
                    foreach (var item in DeviceList)
                    {
                        if (item.CommType == 2)// S7通信
                        {
                            // List<string> addrList = item.MonitorValueList.Select(v => v.Address).ToList();

                            
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
                            
                        }
                    }
                    */
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
