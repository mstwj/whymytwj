using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 直租变比.Table
{
    public class Table_ProductInfo
    {
        //ID
        public int Id { get; set; }
        
        //产品编号
        public string ProductNumber {  get; set; }
        //产品型号
        public string ProductType { get; set; }
        //图号
        public string ProductTuhao { get; set; }
        //容量
        public string ProductCapacity { get; set; }
        //额定电压
        public string RatedVoltage { get; set; }
        //相数
        public string PhaseNumber { get; set; }


    }
}
