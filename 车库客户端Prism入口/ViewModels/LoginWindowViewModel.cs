using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using 公共类;
using 车库客户端Prism入口.Base;
using 车库客户端WPF接口类库.Interface;


namespace 车库客户端Prism入口.ViewModels
{
    public class LoginWindowViewModel
    {

        // 这里是第一次调教
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "123456";

        public DelegateCommand<object> LoginCommand { get; set; }

        ILoginDal _dataAccess;

        public LoginWindowViewModel(ILoginDal dataAccess)
        {
            LoginCommand = new DelegateCommand<object>(DoLogin);
            _dataAccess = dataAccess;
        }
        
    
        private void DoLogin(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    throw new Exception("请输入用户名");
                }

                if (string.IsNullOrEmpty(Password))
                {
                    throw new Exception("请输入密码");
                }


                //用户名和密码比对..
                var loginStr = _dataAccess.Login(UserName, Password).GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(loginStr))
                {
                    throw new Exception("用户密码错误");
                }
                else
                {
                    //{"id":1,"userName":"admin","password":"123456"}
                    UserEntity userEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<UserEntity>(loginStr);

                    GlobalEntity.CurrentUserInfo = userEntity;

                    //比对成功，关闭串口..
                    (obj as Window).DialogResult = true;

                }

            }
            catch (Exception ex)
            {
                //不能上炮了..(不管是地下错误，还是 用户密码错，都走这里.)
                MessageBox.Show(ex.Message);
            }

        }


    }
}
