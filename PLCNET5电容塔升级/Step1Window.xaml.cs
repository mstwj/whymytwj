using CommunityToolkit.Mvvm.Messaging;
using PLCNET5电容塔升级.ViewModel;
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

namespace PLCNET5电容塔升级
{
    /// <summary>
    /// Step1Window.xaml 的交互逻辑
    /// </summary>
    public partial class Step1Window : Window
    {
        public Step1WindowModel model = new Step1WindowModel();

        public Step1Window()
        {
            InitializeComponent();            
            this.DataContext = model;
            
        }

        //这样的指令无法使用ICOMMAND来处理..
        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            model.DoCommandXZJF();
        }

        private void ComboBox_SelectedAndJs(object sender, RoutedEventArgs e)
        {
            //model.StarExeCount();
        }

        private void myClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            model.OnMyClose();
        }
    }
}
