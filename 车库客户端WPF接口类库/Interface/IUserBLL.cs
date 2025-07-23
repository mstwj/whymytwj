using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 公共类;

namespace 车库客户端WPF接口类库.Interface
{
    //接口必须是STRING 返回..
    public interface IUserBLL
    {
        Task<string> GetAll();
        Task<string> GetRolesByUserId(int id);
    }
}
