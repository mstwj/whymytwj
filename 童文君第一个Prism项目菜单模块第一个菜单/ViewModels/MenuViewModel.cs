using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using 童文君第一个Prism项目.Interface;
using 童文君第一个Prism项目菜单模块第一个菜单.Models;

namespace 童文君第一个Prism项目菜单模块第一个菜单.ViewModels
{
    

    public class MenuViewModel :BindableBase, INavigationAware
    {
        //NavigationParameters 如何拿，必须实现接口..
        private string _testValue;
        public string TestValue
        {
            get { return _testValue; }
            set { _testValue = value; }
        }
        public ObservableCollection<MenuModel> Menus { get; set; } =
            new ObservableCollection<MenuModel>();
        IDataAccess _dataAccess;

        public MenuViewModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        //加载菜单数据..
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //一个页面打开多少次...
            //return true 打开无数次..
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //这里就是我们拿到的数据，只要关注这个..
            var v = navigationContext.Parameters["param"].ToString();
            //这里没有变，是把 -- 要实现 BindableBase
            TestValue = v;
            //刷页面 。。
            this.RaisePropertyChanged(nameof(TestValue));

            var dt = _dataAccess.GetMenuAll(TestValue);

            if (dt != null)
            {
                Menus.Clear();
                //空的就是最高的一层.. 下面的递归。。找o..
                FillMenus(Menus, "", dt);
            }

        }

        private void FillMenus(ObservableCollection<MenuModel> menus, string parentId, DataTable origMenus)
        {
            // 根据菜单ID查找对应的直接子节点，第一层的菜单，父节点为0
            var sub = origMenus.AsEnumerable().Where(m => m["pid"].ToString() == parentId).ToList();

            if (sub.Count() > 0)
            {
                foreach (var item in sub)
                {
                    MenuModel mm = new MenuModel()
                    {
                        MenuHeader = item["menu_header"].ToString(),
                        TargetView = item["target_view"].ToString()
                    };
                    menus.Add(mm);

                    FillMenus(mm.Children, item["menu_id"].ToString(), origMenus);
                }
            }
        }
    }
}
