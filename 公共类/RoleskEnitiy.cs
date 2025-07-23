using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 公共类
{
    public class RoleskEnitiy
    {
        
        public int RoleId { get; set; }

        public string RoleName { get; set; }


        public string RoleState { get; set; }
        
        public int RolePower { get; set; }
    }
}
