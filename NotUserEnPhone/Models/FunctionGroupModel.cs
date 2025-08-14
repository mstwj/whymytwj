using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotUserEnPhone.Models
{
    public class FunctionGroupModel : List<FunctionItemModel>
    {
        public string Name { get;private set; }

        public FunctionGroupModel(string name,List<FunctionItemModel>fudncs ) : base (fudncs)
        {
            Name = name;
            
        }
    }
}
