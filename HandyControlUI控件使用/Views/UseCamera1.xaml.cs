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
using HandyControlUI控件使用.ViewModels;

namespace HandyControlUI控件使用.Views
{
    /// <summary>
    /// UseCamera1.xaml 的交互逻辑
    /// </summary>
    public partial class UseCamera1 : UserControl
    {
        public UseCamera1()
        {
            InitializeComponent();
            this.DataContext = new UserCamer1();
        }
    }
}
