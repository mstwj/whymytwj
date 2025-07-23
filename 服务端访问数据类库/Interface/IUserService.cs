using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 服务端访问数据类库.表;

namespace 服务端访问数据类库.Interface
{
    public interface IUserService : IServiceBase
    {
        //string GetAll();
        List<UserRoles> GetRolesByUserId(int userId);
    }
}
