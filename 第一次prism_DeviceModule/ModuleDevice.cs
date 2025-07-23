
using Prism.Modularity;

namespace 第一次prism_DeviceModule
{
    //这里是使用PRSM的关键，你使用PRISM为什么，就是要多人开发..
    //ModuleDevice -- 入口类..
    //注意这里，这个页面，是外部 别人去加载你的..
    public class ModuleDevice : IModule
    {
        //加载时候..
        //这个,我不写任何东西,主界面运行的时候,就好像 乐高一样,要调用到这里..
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        //注册//
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册导航..
            containerRegistry.RegisterForNavigation<Views.DeviceDashboardView>();
            containerRegistry.RegisterForNavigation<Views.BlankPage>();
            containerRegistry.RegisterForNavigation<Views.ProcessPage>();
        }
    }

}
