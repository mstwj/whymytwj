using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空载_负载.Base.Table
{
    public class Table_ProductInfo
    {
        //ID
        public int Id { get; set; }
        //产品型号
        public string ProductType { get; set; }
        //图号
        public string ProductTuhao { get; set; }

        public string ProductCapacity { get; set; }

        //高压
        public string Highpressure { get; set; }
        //高压电流
        public string Highcurrent { get; set; }
        //低压
        public string Lowpressure { get; set; }
        //低硫
        public string Lowcurrent { get; set; }

        public string PhaseNumber { get; set; }

    }
}
