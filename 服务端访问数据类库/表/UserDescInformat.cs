using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 服务端访问数据类库.表
{
    [Table("user_desc_info")]
    public class UserDescInformat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("_id")]
        public int id { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("user_icon")]
        public string UserIcon { get; set; }

        [Column("user_age")]
        public int Age { get; set; }

        [Column("user_real_name")]
        public string RealName { get; set; }
       
    }
}
