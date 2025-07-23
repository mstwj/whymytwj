using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Table.zzsy
{
    public class zz_report
    {
        public int Id { get; set; }
        public string ProName { get; set; }
        public string Guigexinhao { get; set; }
        public int Tuhao { get; set; }
        public string Datatimer { get; set; }        
        public double DataAB { get; set; }
        public double DataBC { get; set; }
        public double DataCA { get; set; }
        public double DataABBCCAXJC { get; set; }
        public string DataHGPD { get; set; }
        public double Dataabx { get; set; }
        public double Databcx { get; set; }
        public double Datacax { get; set; }
        public double Dabbccaxjcx { get; set; }
        public double Dataanx { get; set; }
        public double Databnx { get; set; }
        public double Datacnx { get; set; }
        public double Dnbncnxjcx { get; set; }
        public string Datahgqdx { get; set; }        

        public zz_report Clone()
        {
            return (zz_report)this.MemberwiseClone();
        }


    }
}
