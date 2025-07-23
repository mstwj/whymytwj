using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 童文君第一个Prism项目.Interface
{
    public interface IDataAccess
    {
        bool Login(string username, string password);

        DataTable GetMenuAll(string index);
    }
}
