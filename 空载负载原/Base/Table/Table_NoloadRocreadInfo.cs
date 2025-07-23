using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空载_负载.Base.Table
{
    public class Table_NoloadRocreadInfo
    {
        public int Id { get; set; }
        public DateTime ReportcheckStartTime { get; set; }
        public string ProductNumber { get; set; }
        public string ProductType { get; set; }
        public string ProductTuhao { get; set; }
        public string Highpressure { get; set; }
        public string Highcurrent { get; set; }
        public string Lowpressure { get; set; }        
        public string Lowcurrent { get; set; }

        public string ATPVoltage { get; set; }
        public string RMSvalue { get; set; }
        public string IRMSvalue { get ; set; }

        public string LossStandard { get; set; }
        public string NoloadCurrentStandard { get; set; }                
        public string Qualified { get; set; }
    }
}
