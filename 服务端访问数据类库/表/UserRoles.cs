using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 服务端访问数据类库.表
{
    [Table("user_roles")]
    public class UserRoles
    {
        [Key]
        [Column("role_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }


        [Column("role_state")]
        public string RoleState { get; set; }

        [Column("role_power")]
        public int RolePower { get; set; }


    }
}
