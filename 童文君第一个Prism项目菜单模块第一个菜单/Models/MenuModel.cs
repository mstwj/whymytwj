using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 童文君第一个Prism项目菜单模块第一个菜单.Models
{
    public class MenuModel
    {
        public string MenuHeader { get; set; }
        public string TargetView { get; set; }


        public ObservableCollection<MenuModel> Children { get; set; } = new ObservableCollection<MenuModel>();
    }
}
