using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PLC自己动手2.Base;

namespace PLC自己动手2.Models
{
    public class DataGridItemModel
    {
        public string Name
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        private CommandBase _detailCommmand;

        public CommandBase DetailCommmand
        {
            get
            {
                if (_detailCommmand == null) {
                    _detailCommmand = new CommandBase();
                    _detailCommmand.DoExecute = new Action<object>(ShowDetail);
                }
                return _detailCommmand;           
            }
        }

        //点击先秦..
        private void ShowDetail(object obj)
        {
            MessageBox.Show(this.Name);
        }
    }
}
