using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 公共类
{
    public class MenuEntity
    {
        public int MenuId { get; set; }

        public string MenuHeader { get; set; }


        public string TargetView { get; set; }

        public int ParentId { get; set; }


        public int Index { get; set; }

        public int MenuType { get; set; }
        public int State { get; set; }

    }
}
