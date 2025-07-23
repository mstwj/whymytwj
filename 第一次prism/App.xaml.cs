using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using 第一次prism.Views;
using 第一次prism_DeviceModule;
using 第一次prism_DeviceModule.Views;

namespace 第一次prism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        //启动的窗口是谁....
        protected override Window CreateShell()
        {
            //Container就是IOC容器..
            return Container.Resolve<MainView>();
        }

        //这个所谓的 PRISM就是多人开发，替换了原来的DLL凡是..
        //这样就变成了，一个主MAIN框架，下面都是 子系统了..
        //有几种放hi是.. 1指定对象，2XML配置危机，3 CONFIG 4 自动扫描..
        //IOC对象祖册..
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        //这里很主要，就是你要去加载人家的页面，你要知道，人家的位置..
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //注意这里，我使用的是添加引用的方法，这样好吗？
            //这里，就是必须的必须这要噶o...
            //如果,你要去深入了解 这个就不是 一天2 天了..
            moduleCatalog.AddModule<ModuleDevice>();
        }
        // XML配置文件
        // Config
        // 自动扫描
    }

}
