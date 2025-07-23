using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 车库客户端接口.Interface
{
    public interface ILoginDal
    {
        Task<string> Login(string username, string password);
    }
}
