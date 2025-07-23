using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 第一次prism.Model;

namespace 第一次prism.ViewModels
{
    public class MainViewModel
    {
        public List<MenuModel> MenuList { get; set; } = new List<MenuModel>();

        public DelegateCommand<SubItemModel> OpenViewCommand { get; set; }

        IRegionManager regionManager;

        //这个是UNITY给我的..
        //IRegionManager类型会去找 -- 这就是 因爱主任..--这就是IOC的控制反转，

        //现在，你只要理解，这样搞，就可以有这个对象了.
        //看到没有,这就是PRISM对于主框架变态的地方,
        //1. 必须有 VIEWS目录,主窗口必须要 MainView
        //2. 必须有ViewModels目录, 必须叫 MainViewModel
        //3. 必须有Models目录,下面可以是空的.. 
        //这就是 PRISM变态的地方,无法修改,必须这样,你必须这样搞才OK...
        public MainViewModel(IRegionManager _regionManager)
        {
            regionManager = _regionManager;
            MenuModel mm = new MenuModel();
            mm.Header = "工艺";
            mm.Icon = "\ue610";
            MenuList.Add(mm);
            mm.Children.Add(new SubItemModel
            {
                Header = "EBOM",
                TargetView = "BlankPage"
            });
            mm.Children.Add(new SubItemModel
            {
                Header = "PBOM",
                TargetView = "PBOMView"
            });
            mm.Children.Add(new SubItemModel
            {
                Header = "加工工艺",
                TargetView = "BlankPage",
            });            

            mm = new MenuModel();
            mm.Header = "排程";
            mm.Icon = "\ue655";
            MenuList.Add(mm);
            mm.Children.Add(new SubItemModel
            {
                Header = "手动排程",
                TargetView = "SchedulView"
            });
            mm.Children.Add(new SubItemModel
            {
                Header = "自动排程",
                TargetView = "BlankPage"
            });
            mm.Children.Add(new SubItemModel
            {
                Header = "加工程序管理",
                TargetView = "ProcessPage"
            });

            mm = new MenuModel();
            mm.Header = "设备";
            mm.Icon = "\ue661";
            MenuList.Add(mm);
            mm.Children.Add(new SubItemModel
            {
                Header = "料仓管理",
                TargetView = "BlankPage"
            });
            mm.Children.Add(new SubItemModel
            {
                Header = "设备看板",
                TargetView = "DeviceDashboardView"
            });



            OpenViewCommand = new DelegateCommand<SubItemModel>(DoOpenView);
        }

        private void DoOpenView(SubItemModel model)
        {
            //model.TargetView

            // Prism 框架的区域管理
            // 主窗口里 注册一个区域    区域名称    MainRegion "方正一般都是顶死的.."
            // 请求向这个区域放置一个页面   页面名称  model.TargetView
            // Prism的区域导航对象
            //Prims说到低，就这一行代码..
            regionManager.RequestNavigate("MainRegion", model.TargetView);
            //我们现在使用unity容器..
        }
    }
}
