using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Regions;

namespace 左侧菜单.Models
{
    public class MenuItemModel : BindableBase
    {
        public string MenuIcon { get; set; }
        public string MenuHeader { get; set; }
        public string TargetView { get; set; }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        //自己切套自己.,,
        public List<MenuItemModel> Children { get; set; }

        public ICommand OpenViewCommand
        {
            get => new DelegateCommand(() =>
            {
                if ((this.Children == null || this.Children.Count == 0) &&
                    !string.IsNullOrEmpty(this.TargetView))
                {
                    
                    // 页面跳转--- 这里我很不理解，页面跳转后，应该就相当于，ADD了一个 TABITEM。。
                    _regionManager.RequestNavigate("MainContentRegion", this.TargetView);
                }
                else
                    this.IsExpanded = !this.IsExpanded;
            });
        }
        IRegionManager _regionManager = null;
        public MenuItemModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
