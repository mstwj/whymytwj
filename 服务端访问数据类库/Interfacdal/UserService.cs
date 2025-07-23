using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 服务端访问数据类库.Interface;
using 服务端访问数据类库.表;
using System.Text.Json;
using Newtonsoft.Json;

namespace 服务端访问数据类库.Interfacdal
{
    public class UserService : ServiceBase, IUserService
    {
        //public string GetAll()
        //{
        //  return Query<UserDescInformat>(u => true);
        //Json()
        //}
        public List<UserRoles> GetRolesByUserId(int userId)
        {
            return (from ur in Context.Set<UserRoles>() 
                    where ur.RolePower == userId
                    select ur).ToList();
        }
    }
}
