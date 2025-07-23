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
using System.Windows.Navigation;
using System.Windows.Shapes;
using 铁芯实验.Model;

namespace 铁芯实验.View
{
    /// <summary>
    /// UserInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoControl : UserControl
    {
        public UserInfoModel model { get; set; } = new UserInfoModel();
        public UserInfoControl()
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
