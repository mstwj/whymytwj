using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NotUserEnPhone.Models;

namespace NotUserEnPhone.ViewModels
{
    //INotifyPropertyChanged 这里为什么要这样呢？这里是需要页面的变化，来反向通知我..
    //所以，我这里需要这样.
    //这里还是WPF那一套，我通过数据变化，通知给界面，
    //界面通过变化通知给我..
    public class DeviceViewModel : INotifyPropertyChanged
    {
        //DeviceList 这个集合只是关联 轮播图..
        public List<DeviceItemModel> DeviceList { get; set; }

        private string _deviceName;

        //这里，DeviceName 就是页面监控的变量..
        //DeviceName关联 显示
        public string DeviceName
        {
            get { return _deviceName; }
            set 
            {
                _deviceName = value;
                //这里的意思就是 页面变了，这样写，就返回给我了..
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DeviceName"));
            }
        }

        //DeviceProperties 关联显示具体信息..
        public ObservableCollection<DevicePropertyItemModel> DeviceProperties { get; set; }
        = new ObservableCollection<DevicePropertyItemModel>();

        public ICommand RouteBack { get; set; }

        public DeviceViewModel()
        {
            RouteBack = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));

            DeviceList = new List<DeviceItemModel>();
            DeviceItemModel dim = new DeviceItemModel();

            dim.DeviceImage = "d_1.png";
            dim.DeviceName = "加工中心";
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
            dim.DeviceImage = "d_2.png";
            dim.DeviceName = "电火花";
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
            dim.DeviceImage = "d_4.png";
            dim.DeviceName = "三坐标";
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "运行状态", PropValue = "运行" });
            dim.Properties.Add(new DevicePropertyItemModel { PropName = "工作模式", PropValue = "手动" });
            DeviceList.Add(dim);


            dim = new DeviceItemModel();
            dim.DeviceImage = "d_5.png";
            dim.DeviceName = "线切割";
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

            //这里就是设置了.DeviceName ..
            this.DeviceName = DeviceList[0].DeviceName;
            //这里是一个赋值.. 吧上面的都循环了一下，付给了新的DeviceProperties集合..
            DeviceList[0].Properties.ForEach(p => DeviceProperties.Add(p));

        }

        //这里要实现接口，反向通知 。。。页面变动通知给我..
        public event PropertyChangedEventHandler? PropertyChanged;

        public void SwtichContent(int index)
        {
            DeviceProperties.Clear();
            this.DeviceName = DeviceList[index].DeviceName;
            DeviceList[index].Properties.ForEach(p => DeviceProperties.Add(p));
        }

    }
}
