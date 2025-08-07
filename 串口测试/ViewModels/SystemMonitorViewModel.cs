using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 串口测试.Base;
using 串口测试.Models;

namespace 串口测试.ViewModels
{
    public class SystemMonitorViewModel : NotifyPropertyBase
    {
        public ObservableCollection<LogModel> LogList { get; set; } = new ObservableCollection<LogModel>();

        private DeviceModel _currentDevice;

        public DeviceModel CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; this.RaisePropertyChanged(); }
        }

        private bool _isShowDetail = false;

        public bool IsShowDetail
        {
            get { return _isShowDetail; }
            set { _isShowDetail = value; this.RaisePropertyChanged(); }
        }

        public CommandBase ComponentCommand { get; set; }

        public SystemMonitorViewModel() 
        {
            InitLogInfo();

            //这里很复杂，是 冷却塔绑定了这个按钮...
            //其实，我还要去画个 冷却塔出来..
            this.ComponentCommand = new CommandBase(new Action<object>(DoTowerCommand));
        }

        void InitLogInfo()
        {
            this.LogList.Add(new LogModel { RowNumber = 1, DeviceName = "冷却塔 1#", LogInfo = "已启动", LogType = LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 2, DeviceName = "冷却塔 2#", LogInfo = "已启动", LogType = LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 3, DeviceName = "冷却塔 3#", LogInfo = "液位极低", LogType = LogType.Warn });
            this.LogList.Add(new LogModel { RowNumber = 4, DeviceName = "循环水泵 1#", LogInfo = "频率过大", LogType = LogType.Warn });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "循环水泵 2#", LogInfo = "已启动", LogType = LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "循环水泵 3#", LogInfo = "已启动", LogType = LogType.Info });
        }

        private void DoTowerCommand(object param)
        {
            //好像是显示 那啥的...
            CurrentDevice = param as DeviceModel;

            //也就是说，如果这个为TRUE就显示，如果为FALSE就不显示，就怎么的简单..
            this.IsShowDetail = true;
        }


    }
}
