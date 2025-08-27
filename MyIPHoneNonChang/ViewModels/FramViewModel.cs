using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using CommunityToolkit.Maui.Core;
using MyIPHoneNonChang.Models;
using MyIPHoneNonChang.ViewModels.Dialog;

namespace MyIPHoneNonChang.ViewModels
{
    public class FramViewModel:INotifyPropertyChanged
    {
        public List<string> CameraList { get; set; } = new List<string>()
        {
            "dp_1.jpg","dp_2.jpg","dp.jpg","dp.jpg"
        };
        //ErrorModel 每个信息里面有什么内容..
        public List<ErrorModel> ErrorList { get; set; } = new List<ErrorModel>();

        //MonitorModel 这里就是每个信息里面有什么内容..
        public List<MonitorModel> MonitorList { get; set; } = new List<MonitorModel>();

        public Command SettingsCommand { get; set; }
        public Command DataCommand { get; set; }
        public FramViewModel(IPopupService popupService)
        {
            ErrorList.Add(new ErrorModel
            {
                Header = "棚内温度过高",
                Description = "超过预警值3℃",
                Address = "温室大棚1",
                Color = "OrangeRed"
            });

            ErrorList.Add(new ErrorModel
            {
                Header = "土壤温度过低",
                Description = "低于预警值5%",
                Address = "温室大棚2",
                Color = "#3090FF"
            });

            MonitorList.Add(new MonitorModel { Header = "室外温度", Value = 30, Unit = "℃", Icon = "\ue610" });
            MonitorList.Add(new MonitorModel { Header = "室外湿度", Value = 96.7, Unit = "%", Icon = "\ue60a", ChangeFlag = 1, ChangeValue = 6, TextColor = "#FE7C60" });
            MonitorList.Add(new MonitorModel { Header = "风向", Value = "正东", Icon = "\ue63d" });
            MonitorList.Add(new MonitorModel { Header = "瞬时风速", Value = "20", Unit = "m/s", Icon = "\ue64d" });
            MonitorList.Add(new MonitorModel { Header = "瞬时雨量", Value = "30", Unit = "m3", Icon = "\ue622" });
            MonitorList.Add(new MonitorModel { Header = "紫外线强度", Value = "10", Unit = "μW/cm2", Icon = "\ue603" });
            MonitorList.Add(new MonitorModel { Header = "光照强度", Value = "10", Unit = "Lux", Icon = "\ue898" });
            MonitorList.Add(new MonitorModel { Header = "气压", Value = "1", Unit = "kPa", Icon = "\ue6c0" });
            SettingsCommand = new Command(() =>
            {
                // 打开弹窗
                popupService.ShowPopup<SettingsViewModel>();
            });
            DataCommand = new Command(() =>
            {
                popupService.ShowPopup<DataViewModel>();
            });

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
