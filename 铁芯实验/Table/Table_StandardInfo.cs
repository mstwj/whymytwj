using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 铁芯实验.Table
{
    public class Table_StandardInfo
    {
        public int Id { get; set; }
        public string ProductType {  get; set; }
        public string ProductTuhao { get; set; }        
        public string ProductStandard { get; set; }

        public string ProductStandardUpperimit { get; set; }

        public string ProductStandardDownimit { get; set; }
        public string ProductCurrentStandard { get; set; }

        public string ProductCurrentStandardUpperrimit { get; set; }

        public string ProductCurrentStandardDownimit { get; set; }


    }
}
