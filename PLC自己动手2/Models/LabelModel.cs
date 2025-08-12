using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLC自己动手2.Base;

namespace PLC自己动手2.Models
{
    public class LabelModel : NotifyBase
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; this.NotifyChanged(); }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; this.NotifyChanged(); }
        }
    }
}
