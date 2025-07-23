using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Prism.Ioc;
using Prism.Modularity;
using 用户模块.Views;

namespace 用户模块
{
    public class UserMainModel : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //这里可以什么都不写，因为处理已经在菜单那一层了.
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ModeByUserDailog>();

            //这里的类名称就是等下使用的...
            containerRegistry.RegisterForNavigation<UserManagementView>();

            //这里坑了我好久，一直就不能出来，为什么呢? Register和 RegisterForNavigation是不一样的..
            //RegisterForNavigation 方法用于注册视图或视图模型，以便它们可以通过导航系统进行访问。
            //RegisterForNavigation 方法详解
            //RegisterForNavigation 是 Prism 框架中用于注册导航视图或视图模型的方法。它允许开发者在应用程序的依赖注入容器中注册视图或视图模型，以便后续可以通过导航服务（如 IRegionManager）进行导航
            //Register‌：用于注册服务或对象实例，直接将对象实例注入容器，后续可直接通过容器调用。 ‌            
            //RegisterForNavigation‌：用于将视图与视图模型绑定并注册为导航目标，支持视图模型自动绑定及多视图模型关联。 ‌
        }
    }
}
