using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using 维通利智耐压台.Base;

namespace 维通利智耐压台.Model
{
    public class UserLoginModel : ObservableValidator
    {

        [Required(ErrorMessage = "必须输入用户名")]
        [MinLength(2, ErrorMessage = "用户名不能小于2个字符")]
        [MaxLength(15, ErrorMessage = "用户名不能超过15个字符")]

        public string m_UserName { get; set; }

        [Required(ErrorMessage = "必须输入密码")]
        [MinLength(2, ErrorMessage = "密码不能小于2个字符")]
        [MaxLength(15, ErrorMessage = "密码不能超过15个字符")]
        public string m_UserPassword { get; set; }
        

        public UserLoginModel()
        {            
            
        }

        public bool DoBtnCommandOK(string UserName,string UserPassword)
        {
            m_UserName = UserName;
            m_UserPassword = UserPassword;
            //ValidateProperty(UserName, "UserName");
            //ValidateProperty(UserPassword, "UserPassword");
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return false;
            }


            using (var context = new MyDbContext())
            {
                var firstEntity = context.naiya_user.FirstOrDefault(e => e.Username == UserName && e.Password == UserPassword);
                if (firstEntity != null)
                {
                    //找到了..
                    return true;
                }
                else
                {
                    //没找到..
                    MessageBox.Show("用户名或密码错误!");
                    return false;
                }
            }
        }
    }
}