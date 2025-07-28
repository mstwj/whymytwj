using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using 自己动手1.Base;

namespace 自己动手1.Models
{
    public class PageItemModel : NotifyBase
    {
        public string Header { get; set; }
        public object PageView { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public ICommand CloseTabCommand { get; set; }

    }
}
