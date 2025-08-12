using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLC自己动手2.Base;

namespace PLC自己动手2.Models
{
    public class DeviceModel  : NotifyBase
    {
        private CommandBase _editCommand;

        public CommandBase EditCommand
        { 
            get 
            {
                if(_editCommand == null)
                {
                    _editCommand = new CommandBase();
                    _editCommand.DoExecute = new Action<object>(obj=>
                    {
                        //注意：这里的THIS，这里的THIS是什么意思呢？要传送给打开窗口的意思...

                        WindowManager.ShowDialog("DeviceEditWindow", this);
                    });


                }
                return _editCommand; 
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            { _name = value; this.NotifyChanged(); }
        }


        private string _sn;

        public string SN
        {
            get { return _sn; }
            set
            { _sn = value; this.NotifyChanged(); }
        }

        /// <summary>
        /// 通信方式
        /// </summary>
        public int CommType { get; set; }
        public int ProtocolType { get; set; }
        public ProtocolS7Model S7 { get;set; }

        public ProtocolModbus Modbus { get; set; }
        public ObservableCollection<MonitorValueModel> MonitorValueList { get; set; } = new ObservableCollection<MonitorValueModel>();

       
    }
}
