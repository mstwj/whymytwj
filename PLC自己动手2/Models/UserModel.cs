﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLC自己动手2.Base;

namespace PLC自己动手2.Models
{
    public class UserModel : NotifyBase
    {
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; this.NotifyChanged(); }
        }

        private string _password = "";

        public string Password
        {
            get { return _password; }
            set { _password = value; this.NotifyChanged(); }
        }

        public string RealName { get; set; }
        public string Avatar { get; set; }
    }
}
