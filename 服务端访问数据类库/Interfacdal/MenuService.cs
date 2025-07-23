using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using 服务端访问数据类库.Interface;
using 服务端访问数据类库.表;

namespace 服务端访问数据类库.Interfacdal
{
    public class MenuService : ServiceBase, IMenuService
    {


        public List<MenuInfo> GetAllMenus()
        {
            return (from menus in Context.Set<MenuInfo>()
                    where menus.State == 1
                    select menus).ToList();
        }
    }
}
