using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 第一次prism_DeviceModule.Models;

namespace 第一次prism_DeviceModule.ViewModels
{
    public class DeviceDashboardViewModel
    {
        public List<DeviceItemModel> DeviceList { get; set; } = new List<DeviceItemModel>();
        public DeviceDashboardViewModel() 
        {
            DeviceItemModel dim = new DeviceItemModel();
            dim.Header = "CNC";
            // 网络图片
            dim.Image = "../Assets/Images/Device/dd_2.png";
            //dim.LightType = Controls.LightState.Fault;
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行状态", PropValue = "故障" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "工作模式", PropValue = "AUTO" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "进给倍率", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "主轴转速", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "主轴负载", PropValue = "0 r/min" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "程序编号", PropValue = "7014" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "机床坐标-X", PropValue = "-500 mm" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "机床坐标-Y", PropValue = "-120.002 mm" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "机床坐标-Z", PropValue = "-1.525 mm" });

            for (int i = 0; i < 5; i++)
            {
                dim.EventList.Add(new EventModel
                {
                    Time = "2024-09-20 20:00",
                    Type = "警告",
                    Message = "设备运行参数报警，设备运行参数报警",
                    Level = "低级",
                    DutyUser = "User001",
                    State = "已处理",
                    SolveTime = "2024-09-20 20:01"
                });
            }
            DeviceList.Add(dim);

            dim = new DeviceItemModel();
            dim.Header = "热弯机";
            dim.Image = "../Assets/Images/Device/dd_1.png";
            //dim.LightType = Controls.LightState.Warning;
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "L编号", PropValue = "31" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "N编号", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "B编号", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "停止编号", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "程序错误", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行错误", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "顺序错误", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "启动ON时间", PropValue = "0时:0分:0秒" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "加工ON时间", PropValue = "0时:0分:0秒" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "E条件编号", PropValue = "909002" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "设备机械坐标", PropValue = "暂无" });
            for (int i = 0; i < 5; i++)
            {
                dim.EventList.Add(new EventModel
                {
                    Time = "2024-09-20 20:00",
                    Type = "警告",
                    Message = "设备运行参数报警，设备运行参数报警",
                    Level = "低级",
                    DutyUser = "User001",
                    State = "已处理",
                    SolveTime = "2024-09-20 20:01"
                });
            }
            DeviceList.Add(dim);

        

        }
    }
}
