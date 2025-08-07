﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口测试.Base
{
    public class WarningMessageModel
    {
        public string ValueId { get; set; }
        public string Message { get; set; }
    }

    public class DeviceModel : NotifyPropertyBase
    {
        public string DeviceID { get; set; }
        public string DeviceName { get; set; }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isWarning = false;

        public bool IsWarning
        {
            get { return _isWarning; }
            set
            {
                _isWarning = value;
                this.RaisePropertyChanged();
            }
        }


        public ObservableCollection<MonitorValueModel> MonitorValueList { get; set; } = new ObservableCollection<MonitorValueModel>();

        public ObservableCollection<WarningMessageModel> WarningMessageList { get; set; } = new ObservableCollection<WarningMessageModel>();



    }
}
