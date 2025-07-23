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
using CommunityToolkit.Mvvm.Messaging;
using 功率分析仪NP3000.Model;

namespace 功率分析仪NP3000.View1
{
    /// <summary>
    /// UserPanelKongzai.xaml 的交互逻辑
    /// </summary>
    public partial class UserPanelKongzai : UserControl
    {
        public UserPanelKongzaiModel userPanelKongzaiModel { get; set; } = new UserPanelKongzaiModel();
        public UserPanelKongzai()
        {
            InitializeComponent();
            this.DataContext = userPanelKongzaiModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send<string, string>("开始初始化", "InitShowProc");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send<string, string>("开始初始化", "InitShowProc");
        }
    }
}
