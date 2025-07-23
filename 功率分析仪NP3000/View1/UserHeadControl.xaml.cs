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

namespace 功率分析仪NP3000.View1
{
    /// <summary>
    /// UserHeadControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserHeadControl : UserControl
    {
        public UserHeadControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //关闭进程..
            //this.Close();
        }

    }
}
