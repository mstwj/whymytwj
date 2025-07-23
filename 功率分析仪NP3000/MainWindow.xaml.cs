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
using 功率分析仪NP3000.Table;
using 功率分析仪NP3000.View1;

namespace 功率分析仪NP3000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        table_proinfo Current_tableinfo = new table_proinfo();
        PageDeveclKongzai PageDeveclKongzai { get; set; } = new PageDeveclKongzai();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DoBtnMenu1_1Click(object sender, RoutedEventArgs e)
        {
            AddProc addProc = new AddProc();
            addProc.m_Current_tableinfo = Current_tableinfo;
            addProc.ShowDialog();
        }

        private async void DoBtnMenu1_2Click(object sender, RoutedEventArgs e)
        {

        }

        private async void DoBtnMenu2_1Click(object sender, RoutedEventArgs e)
        {
            PageDeveclKongzai.pageDeveclKongzaiModelcs.InitializeAsync(Current_tableinfo);
            FrameNv.Navigate(PageDeveclKongzai);
        }

        private async void DoBtnMenu2_2Click(object sender, RoutedEventArgs e)
        {
        }


        private async void DoBtnMenu3_1Click(object sender, RoutedEventArgs e)
        {
        }

        private async void DoBtnMenu3_2Click(object sender, RoutedEventArgs e)
        {
        }

        private async void DoBtnMenu4_1Click(object sender, RoutedEventArgs e)
        {
        }

        private async void DoBtnMenu4_2Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
