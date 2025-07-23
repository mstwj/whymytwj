using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Table.bbsy
{

    public class bb_report
    {
        public int Id { get; set; }
        public string ProName { get; set; }
        public string Guigexinhao { get; set; }
        public int Tuhao { get; set; }
        public string Datatimer { get; set; }
        public string Datazbfs { get; set; }
        public int Datadqfj { get; set; }
        public int Datajx { get; set; }
        public double Dataelbb { get; set; }
        public double Datafjjk { get; set; }
        public double DataKAB { get; set; }
        public double DataKBC { get; set; }
        public double DataKCA { get; set; }
        public double DataEAB { get; set; }
        public double DataEBC { get; set; }
        public double DataECA { get; set; }
        public int Dataclfs { get; set; }
        public string DataHGPD { get; set; }

        public bb_report Clone()
        {
            return (bb_report)this.MemberwiseClone();
        }
    }

}
