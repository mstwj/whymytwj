using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 服务端访问数据类库.表
{
    [Table("sys_user_info")]
    public class SystemUserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("_id")]
        public int id { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [NotMapped]
        public List<MenuInfo> Menus { get; set; }
    }
}
