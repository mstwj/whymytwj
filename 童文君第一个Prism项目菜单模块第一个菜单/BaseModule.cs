
using Prism.Ioc;
using Prism.Modularity;
using 童文君第一个Prism项目菜单模块第一个菜单.Views;

namespace 童文君第一个Prism项目菜单模块第一个菜单
{
    public class BaseModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //这里很主要，这里的名字 必须和你使用时候是一模一样。
            //这里你也可以这样去理解，就是有一个 名字MenuView 对应了这个MenuView的页面..
            containerRegistry.RegisterForNavigation<MenuView>();
        }
    }

}
