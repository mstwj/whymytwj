using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIPHoneNonChang.Models
{
    public class MonitorModel
    {
        public string Header { get; set; }
        public string Icon { get; set; }
        public object Value { get; set; }
        public string Unit { get; set; }
        public string TextColor { get; set; } = "#0AC677";

        public int ChangeFlag { get; set; }
        public double ChangeValue { get; set; }
    }
}
