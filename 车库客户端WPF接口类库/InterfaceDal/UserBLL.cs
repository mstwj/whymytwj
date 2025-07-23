using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using 公共类;
using 车库客户端WPF接口类库.Interface;

namespace 车库客户端WPF接口类库.InterfaceDal
{
    public class UserBLL : WebDataAccess,IUserBLL
    {
        public Task<string> GetAll()
        {
            return this.GetDatas("user/all");
        }

        public Task<string> GetRolesByUserId(int id)
        {
            return this.GetDatas($"user/roles/{id}");
        }
    }
}
