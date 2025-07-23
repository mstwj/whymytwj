using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using 公共类;

using 左侧菜单.Models;

namespace 左侧菜单.ViewModels
{
    public class TreeMenuViewModel
    {
        public List<MenuItemModel> Menus { get; set; } = new List<MenuItemModel>();

        private List<MenuEntity> origMenus = null;

        IRegionManager _regionManager;
        public TreeMenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            //老师是需要得到数据的，我就自己写了..
            origMenus = GlobalEntity.CurrentUserInfo.Menus;
            this.FillMenus(Menus, 0);
        }   


        ///递归
        ///
        private void FillMenus(List<MenuItemModel> menus, int parentId)
        {
            var sub = origMenus.Where(m => m.ParentId == parentId).OrderBy(o => o.Index);

            if (sub.Count() > 0)
            {
                foreach (var item in sub)
                {
                    MenuItemModel mm = new MenuItemModel(_regionManager)
                    {
                        MenuHeader = item.MenuHeader,
                        //MenuIcon = item.MenuIcon,
                        TargetView = item.TargetView
                    };
                    menus.Add(mm);

                    FillMenus(mm.Children = new List<MenuItemModel>(), item.MenuId);
                }
            }
        }

    }
}
