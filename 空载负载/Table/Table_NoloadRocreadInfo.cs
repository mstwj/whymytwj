using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空载负载.Table
{
    public class Table_NoloadRocreadInfo
    {
        public int Id { get; set; }

        public DateTime ReportcheckStartTime { get; set; }
        //(ID 产品表)
        public string ProductNumber { get; set; }

        public string ProductType { get; set; }
        public string ProductTuhao { get; set; }
        public string ProductCapacity { get; set; }
        public string Highpressure { get; set; }
        public string Highcurrent { get; set; }
        public string Lowpressure { get; set; }
        public string Lowcurrent { get; set; }


        public string PhaseNumber { get; set; }
        //三相电压平均值
        public string ATPVoltage { get; set; }
        //三相电压有效值
        public string RMSvalue { get; set; }
        //三相电流有效值
        public string IRMSvalue { get; set; }
        //空载电流%
        public string LossStandard { get; set; }
        //空载损耗
        public string NoloadCurrentStandard { get; set; }
        //合格判断
        public string Qualified { get; set; }
    }
}
