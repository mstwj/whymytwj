using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 公共类
{
    //{"id":1,"userName":"admin","password":"123456"}
    public class UserEntity
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<MenuEntity> Menus { get; set; }
    }
}
