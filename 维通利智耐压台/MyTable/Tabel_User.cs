using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 维通利智耐压台.MyTable
{
    //下面必须和数据库字段一致..
    public class Tabel_User
    {
        public int Id { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
    }
}
