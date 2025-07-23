using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using 左侧菜单;
using 用户模块;
using 车库客户端Prism入口.Views;
using 车库客户端WPF接口类库.Interface;
using 车库客户端WPF接口类库.InterfaceDal;


namespace 车库客户端Prism入口
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            if (Container.Resolve<LoginWindow>().ShowDialog() == false)
            {
                Application.Current?.Shutdown();
            }
            else
                base.InitializeShell(shell);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //业务里面的IOC的租入..
            //这里就很方便了，如果要使用SQL3 我就直接修改这里就OK了，
            //如果是 MYSQL,如果是 SQLSERVER。。
            //这里极大的进行了解耦...
            //containerRegistry.Register<IDataAccess, DataAccess>();

            //新招数 -- 选择 汽车服务端，到工程目录下面，CMD -- 然后运行 dotnet run ,工程就运行起来了..
            //这样就不需要，你在去运行一个 服务端啥的了..
            containerRegistry.RegisterSingleton<ILoginDal, LoginDal>();
            containerRegistry.RegisterSingleton<IUserBLL, UserBLL>();
            

        }

        //区域注册使用..
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //可以改为自动扫描..
            moduleCatalog.AddModule<MainModel>();
            moduleCatalog.AddModule<UserMainModel>();

        }
    }
}
