using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 维通利智耐压台
{
    /// <summary>
    /// DeveclWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeveclWindow : Window
    {
        public string ComboBoxSelect { get; set; } = string.Empty;//PLC ip
        public string SelectItem { get; set; } = string.Empty;//PLC ip

        public List<string> ComboBoxItems { get; set; } = new List<string>();

        public DeveclWindow()
        {
            InitializeComponent();

            DataContext = this;
          
            SelectItem = ConfigurationManager.AppSettings["DEVICE_PLC"];
            ComboBoxSelect = ConfigurationManager.AppSettings["DEVICE_COMJUFAN"];
            ComboBoxItems.Add("COM1");
            ComboBoxItems.Add("COM2");
            ComboBoxItems.Add("COM3");
            ComboBoxItems.Add("COM4");
            ComboBoxItems.Add("COM5");
            ComboBoxItems.Add("COM6");
            ComboBoxItems.Add("COM7");
            ComboBoxItems.Add("COM8");
            ComboBoxItems.Add("COM9");
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            SaveIni();
        }

        private void Button_Click_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public void SaveIni()
        {
            string ipAddress = txtIPAddress.Text;

            if (!IsValidIPAddress(ipAddress))
            {
                MessageBox.Show("IP地址设置错误!");
                return;
            }

            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DEVICE_PLC"].Value = SelectItem; //Device_1CBox.Text;
            config.AppSettings.Settings["DEVICE_COMJUFAN"].Value = ComboBoxSelect;


            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功");
        }

      
        private bool IsValidIPAddress(string ipAddress)
        {
            string pattern = @"^([0-9]{1,3}\.){3}[0-9]{1,3}$";
            return Regex.IsMatch(ipAddress, pattern);
        }
    }
}
