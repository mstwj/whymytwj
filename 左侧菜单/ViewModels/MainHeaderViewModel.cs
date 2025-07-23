using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using 公共类;


namespace 左侧菜单.ViewModels
{
    public class MainHeaderViewModel
    {
        public string CurrentUserName { get; set; }

        public MainHeaderViewModel(IContainerProvider containerProvider)
        {
            if (GlobalEntity.CurrentUserInfo != null)
            {
                CurrentUserName = GlobalEntity.CurrentUserInfo.UserName;
            }
        }
    }
}
