using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空载负载.Table
{
    public class Table_LoadStandardInfo
    {
        public int Id { get; set; }
        public string ProductType {  get; set; }
        public string ProductTuhao { get; set; }        
        public string Loadloss { get; set; }

        public string LoadlossUpperimit { get; set; }

        public string LoadlossDownimit { get; set; }
        public string LoadMainReactance { get; set; }

        public string LoadMainReactanceUpperrimit { get; set; }

        public string LoadMainReactanceDownimit { get; set; }


    }
}
