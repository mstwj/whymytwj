using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 维通利智耐压台.Model;

namespace 维通利智耐压台
{
    /// <summary>
    /// UserLogin.xaml 的交互逻辑
    /// </summary>
    public partial class UserLogin : Window
    {
        UserLoginModel model = new UserLoginModel();
        public UserLogin()
        {
            InitializeComponent();
            this.DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            if (model.DoBtnCommandOK(userBox.Text,passwordBox.Password))
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
