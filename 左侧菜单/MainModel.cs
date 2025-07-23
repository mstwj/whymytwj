using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity.Storage;
using 左侧菜单.Views;

namespace 左侧菜单
{
    public class MainModel : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //添加一个组件到对应区域.. -- RegisterViewWithRegion("LeftMenuTreeRegion" 通过这一句来制定的。。。
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("LeftMenuTreeRegion", "TreeMenuView");
            regionManager.RegisterViewWithRegion("MainHeaderRegion", "MainHeaderView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //这里的类名称就是等下使用的...
            containerRegistry.Register<TreeMenuView>();
            containerRegistry.Register<MainHeaderView>();
        }
    }
}
