using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using 用户模块.Models;

namespace 用户模块.ViewModels
{
    public class ModeByUserDailogViewModel :BindableBase, IDialogAware
    {
        public string Title => "用户信息编辑";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //接受信息状态...
            MainModel = parameters.GetValue<UserModel>("model");
        }
        public UserModel MainModel { get => _userModel; set { SetProperty<UserModel>(ref _userModel, value); } }
        private UserModel _userModel = new UserModel();

        public ICommand ConfirmCommand
        {
            get => new DelegateCommand(() =>
            {
                //发送保存..
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));

            });
            
        }

        public ICommand CancelCommand
        {
            get => new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            });
        }
    }
}
