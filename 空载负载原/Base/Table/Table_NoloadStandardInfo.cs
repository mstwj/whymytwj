using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空载_负载.Base.Table
{
    public class Table_NoloadStandardInfo
    {
        //ID
        public int Id { get; set; }

        //产品型号
        public string ProductType { get; set; }
        //图号
        public string ProductTuhao { get; set; }
        public string LossStandard { get; set; }
        public string LossStandardUp { get; set; }
        public string LossStandardDown { get; set; }

        public string NoloadCurrentStandard { get; set; }
        public string NoloadCurrentStandardUp { get; set; }
        public string NoloadCurrentStandardDown { get; set; }
        
    }
}
