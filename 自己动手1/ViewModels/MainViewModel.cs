using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using 自己动手1.Base;
using 自己动手1.Models;

namespace 自己动手1.ViewModels
{
    class MainViewModel
    {
                                   
        //为什么这,get,set 这么只重要，不写就拿不到呢?????
        public List<MenuItemModel> TreeList { get; set; }

        //页面结合.
        public ObservableCollection<PageItemModel> Pages { get; set; }

        public MainViewModel()
        {
            TreeList = new List<MenuItemModel>();
            MenuItemModel tim = new MenuItemModel();
            tim.Header = "工艺";
            tim.IconCode = "\ue610";
            TreeList.Add(tim);

            tim.Children.Add(new MenuItemModel
            {
                Header = "EBOM",
                TargetView = "BlankPage",
                OpenViewCommand = new Command<MenuItemModel>(OpenView)
            });
            tim.Children.Add(new MenuItemModel
            {
                Header = "设备看板",
                TargetView = "DevicePage",
                OpenViewCommand = new Command<MenuItemModel>(OpenView)
            });

            tim.Children.Add(new MenuItemModel
            {
                Header = "PBOM",
                TargetView = "BlankPage",
                OpenViewCommand = new Command<MenuItemModel>(OpenView)

            });
            MenuItemModel subMenu = new MenuItemModel();
            subMenu.Header = "二级菜单";
            subMenu.Children.Add(
                new MenuItemModel
                {
                    Header = "三级菜单"
                }
               );
            tim.Children.Add(subMenu);

            //这里需要理解一下，双击后，会向上抛一个双击COMMAND的指令给我们..
            Pages = new ObservableCollection<PageItemModel>();

            //Pages.Add("11111");
            //Pages.Add("22222");
            //Pages.Add("33333");
        }

        private void OpenView(MenuItemModel menu)
        {
            MenuItemModel min = menu as MenuItemModel;

            var page = Pages.ToList().FirstOrDefault(p => p.Header == menu.Header);

            //这里有一个隐藏BUG，就是非 选中项目.. 也就是说，一开始打开的页面，不是选中的..
            if (page == null)
            {
                Type type = Assembly.GetExecutingAssembly().
                    GetType("自己动手1.Views.Pages." + menu.TargetView);
                object p = Activator.CreateInstance(type);

                Pages.Add(new PageItemModel
                {
                    Header = menu.Header,
                    PageView = p,
                    IsSelected = true,
                    CloseTabCommand = new Command<PageItemModel>(ClosePage)
                });
            }
            else
                page.IsSelected = true;
        }
        
        private void ClosePage(PageItemModel menu)
        {
            Pages.Remove(menu);
        }

    }
    
}
