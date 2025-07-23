using System.Configuration;
using System.Data;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using 童文君第一个Prism项目.Interface;
using 童文君第一个Prism项目.InterfaceDal;
using 童文君第一个Prism项目.Views;
using 童文君第一个Prism项目菜单模块第一个菜单;

namespace 童文君第一个prism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            //APP 入口.. 打开第一个窗口.
            //return new MainWindow();
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //业务里面的IOC的租入..
            //这里就很方便了，如果要使用SQL3 我就直接修改这里就OK了，
            //如果是 MYSQL,如果是 SQLSERVER。。
            //这里极大的进行了解耦...
            containerRegistry.Register<IDataAccess, DataAccess>();
            containerRegistry.RegisterSingleton<ICommunication, Communication>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //这里不要问为什么，反正这样就OK了..
            moduleCatalog.AddModule<BaseModule>();
        }
    }

}
