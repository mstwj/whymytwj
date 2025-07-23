using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 维通利智耐压台.MyTable
{
    public class Tabe_Record
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        //产品编号
        public string ProductNumber { get; set; } = string.Empty;
        //产品型号
        public string ProductType { get; set; } = string.Empty;
        //图号
        public string ProductTuhao { get; set; } = string.Empty;
        //标准电压
        public string ProductStardVotil { get; set; } = string.Empty;
        //标准局放
        public string ProductStardPartial { get; set; } = string.Empty;

        public DateTime RecordDateTimer { get; set; }   

        public string ProductParts { get; set; } =  string.Empty;

        //开始时间
        public string BeginTimer { get; set; } = string.Empty;

        public string  LevelCode { get; set; } = string.Empty;
        //电压
        public string Votil { get; set; } = string.Empty;
        //局放
        public string Partial{ get; set; } = string.Empty;

        //开始时间
       
        public string ProductQualified { get; set; } = string.Empty;
    }
}
