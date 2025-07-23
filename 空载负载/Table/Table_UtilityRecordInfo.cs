using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空载负载.Table
{
    public class Table_UtilityRecordInfo
    {
        public int Id { get; set; }

        public DateTime ReportcheckStartTime { get; set; }

        //产品编号
        public string ProductNumber { get; set; }
        //产品型号
        public string ProductType { get; set; }
        //图号
        public string ProductTuhao { get; set; }
        //容量
        public string ProductCapacity { get; set; }


        //高电压
        public string Highpressure { get; set; }

        //高电压
        public string Highcurrent { get; set; }

        //高电压
        public string Lowpressure { get; set; }

        //高电压
        public string Lowcurrent { get; set; }

        //相数
        public string PhaseNumber { get; set; }

        public string Voltage { get; set; }

        public string Frequency { get; set; }
        public string Times { get; set; }



    }
}
