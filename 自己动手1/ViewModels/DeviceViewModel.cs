using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 自己动手1.Models;

namespace 自己动手1.ViewModels
{
    public class DeviceViewModel
    {
        public bool MonitorState { get; set; }  

        //这里要理解的关键就是 数据模型和页面绑定的关系..
        public List<DeviceItemModel> DeviceList { get; set; }

        public DeviceViewModel()
        {
            //也就是说，数据有了，怎么去显示 呢？
            //靠的就是 数据模版了.. ItemControl.DataTemaplate..
            DeviceList = new List<DeviceItemModel>();

            App.MqttMessageReceived += App_MqttMessageReceived;

            DeviceItemModel dim = new DeviceItemModel();
            // 网络图片
            dim.Image = "pack://application:,,,/;component/Assets/Images/Device/d_1.png";
            dim.LightType = Control.LightState.Fault;
            
            dim.Title = "加工中心";

            
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行状态", PropValue = "故障" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "工作模式", PropValue = "AUTO" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "进给倍率", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "主轴转速", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "主轴负载", PropValue = "0 r/min" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "程序编号", PropValue = "7014" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "机床坐标-X", PropValue = "-500 mm" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "机床坐标-Y", PropValue = "-120.002 mm" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "机床坐标-Z", PropValue = "-1.525 mm" });
            DeviceList.Add(dim);

            dim = new DeviceItemModel();
            dim.Image = "pack://application:,,,/;component/Assets/Images/Device/d_2.png";
            dim.LightType = Control.LightState.Warning;
            dim.Title = "电火花";
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
            DeviceList.Add(dim);

            dim = new DeviceItemModel();
            dim.Image = "pack://application:,,,/;component/Assets/Images/Device/d_3.png";
            dim.LightType = Control.LightState.Run;
            dim.Title = "机器臂";
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行状态", PropValue = "运行" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "工作模式", PropValue = "手动" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J1", PropValue = "-97.979°" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J2", PropValue = "-31.493°" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J3", PropValue = "-24.517°" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J4", PropValue = "-0.032°" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J5", PropValue = "-34.038°" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J6", PropValue = "-8.532°" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "关节轴J7", PropValue = "3182.790°" });
            DeviceList.Add(dim);

            dim = new DeviceItemModel();
            dim.Image = "pack://application:,,,/;component/Assets/Images/Device/d_4.png";
            dim.LightType = Control.LightState.None;
            dim.Title = "三坐标";
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行状态", PropValue = "运行" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "工作模式", PropValue = "手动" });
            DeviceList.Add(dim);


            dim = new DeviceItemModel();
            dim.Image = "pack://application:,,,/;component/Assets/Images/Device/d_5.png";
            dim.LightType = Control.LightState.Run;
            dim.Title = "线切割";
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行状态", PropValue = "运行" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "工作模式", PropValue = "自动" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "停止编号", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "程序错误", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行错误", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "顺序错误", PropValue = "0" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "启动设定为ON的时间", PropValue = "0时:0分:0秒" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "加工设定为ON的时间", PropValue = "0时:0分:0秒" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "加工条件(E条件)编号", PropValue = "909002" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "设备机械坐标", PropValue = "暂无" });
            DeviceList.Add(dim);

        }

        private void App_MqttMessageReceived(object? sender, string e)
        {
            //这样就是直接给数据了..
            // DeviceList[0].Properties[3].PropValue = e;

            // 真实对接    作业 
            //e  就是一个Json字符串  反序列化 -》 对象 

        }
    }
}
