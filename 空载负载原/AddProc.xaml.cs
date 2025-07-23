using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using 空载_负载.Base;


namespace 空载_负载
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddProc : Window
    {
        public bool state { get; set; } = false;
        public string ProductNumber { get; set; } = "001";
        public string ProductType { get; set; }
        public string ProductTuhao { get; set; }
        
        public string Highpressure { get; set; }
        public string Highcurrent { get; set; } 
        public string Lowpressure { get; set; } 
        public string Lowcurrent { get; set; }

        public List<string> ComboBoxYanPingBianhao { get; set; } = new List<string>();
        
        public string ComboBoxSelectValueYanPingBianhao { get; set; }

        public AddProc()
        {
            InitializeComponent();
            this.DataContext = this;            

        }

        private async void BtnSetOver(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("配置完成!");
            state = true;
            this.Close();
        }

        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
