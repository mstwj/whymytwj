﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 公共类
{
    public class UserDetailedEntity
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserIcon { get; set; }
        public int Age { get; set; }
        public string RealName { get; set; }

    }
}
