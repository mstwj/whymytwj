using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 维通利智耐压台.MyTable
{
    public class Table_Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        //产品编号
        public string ProductNumber { get; set; }
        //产品型号
        public string ProductType { get; set; }
        //图号
        public string ProductTuhao { get; set; }

        public string ProductParts { get; set; }
        //容量
        public string ProductTestVotil { get; set; }
        //额定电压
        public string ProductTestPartial { get; set; }
    }
}
