using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;

namespace 用户模块.Models
{
    public class RoleModel : BindableBase
    {

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int State { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { SetProperty<bool>(ref _isSelected, value); }
        }


        public ICommand DeleteCommand { get; set; }
        public ICommand ItemSelectedCommand { get; set; }
    }
}
