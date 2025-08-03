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

namespace 自己动手1.Views.Dialog
{
    /// <summary>
    /// PBomEditWin.xaml 的交互逻辑
    /// </summary>
    public partial class PBomEditWin : Window
    {
        public PBomEditWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //关闭了对话框，也返回为TRUE了..
            this.DialogResult = true;
        }
    }
}
