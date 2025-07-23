using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using 童文君第一个Prism项目.Interface;

namespace 童文君第一个Prism项目.ViewModels
{
    public class LoginViewModel
    {
        public DelegateCommand<object> LoginCommand { get; set; }
        public string UserName { get; set; } =   "admin";
        public string Password { get; set; } = "123456";

        IDataAccess _dataAccess;

        //问题，IDataAccess 认识吗？不认识呀...
        public LoginViewModel(IDataAccess dataAccess) 
        {
            _dataAccess = dataAccess;
            LoginCommand = new DelegateCommand<object>(DoLogin);
        }

        private void DoLogin(object obj)
        {
            try
            {
                //用户名和密码比对..
                if (_dataAccess.Login(UserName, Password))
                {
                    //比对成功，关闭串口..
                    (obj as Window).DialogResult = true;
                }
                else
                    throw new Exception("用户密码错误");
            }
            catch(Exception ex)
            {
                //不能上炮了..(不管是地下错误，还是 用户密码错，都走这里.)
                MessageBox.Show(ex.Message);
            }


        }
    }
}
