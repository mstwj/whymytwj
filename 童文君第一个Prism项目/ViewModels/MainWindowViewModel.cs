using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Regions;
using 童文君第一个Prism项目.Interface;

namespace 童文君第一个Prism项目.ViewModels
{
    public class MainWindowViewModel
    {
        public DelegateCommand<string> NavCommand { get; set; }

        //这里的是IOC来处理的，你不要管，这样写就OK了..
        //IREGIONMANAGER怎么来的，你不管，反正可以使用就OK了..

        IRegionManager _regionManager;
        ICommunication _communication;

        public MainWindowViewModel(IRegionManager regionManager,
            ICommunication communication) 
        {
            NavCommand = new DelegateCommand<string>(DoNavigation);
            _regionManager = regionManager;
            

            communication.Receiveved += Communication_Receieved;
        }

        private void Communication_Receieved(object? sender, byte[] e)
        {

        }

        private void DoNavigation(string name)
        {
           
            //这里就要加载MENUVIEW
            //RegionManager 对象来进行页面导航..
            //前面是目标区，后面是目标页面..

            //注意：如果就这样写，有个BUG，就是生成了多个Reigon，每个Region对应同一个视图MenuView..

            NavigationParameters keyValuePairs = new NavigationParameters();
            keyValuePairs.Add("param", name);

            _regionManager.RequestNavigate("MenuRegion", "MenuView", keyValuePairs);
          
        }
    }
}
