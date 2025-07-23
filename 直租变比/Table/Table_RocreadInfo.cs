using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 直租变比.Table
{
    public class Table_RocreadInfo
    {
        public int Id { get; set; }

        public DateTime ReportcheckStartTime { get; set; }
        //(ID 产品表)
        public string ProductNumber { get; set; }
        //三相电压平均值
        public string ATPVoltage { get; set; }
        //三相电压有效值
        public string RMSvalue { get; set; }
        //三相电流有效值
        public string IRMSvalue { get; set; }
        //空载电流
        public string NoloadCurrent { get; set; }
        //空载电流%
        public string PercentageNoloadCurrent { get; set; }
        //空载损耗
        public string NoloadLoss { get; set; }
        //合格判断
        public string QualifiedJudgment { get; set; }
    }
}
