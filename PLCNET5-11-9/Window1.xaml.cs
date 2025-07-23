using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace PLCNET5_11_9
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public List<string> ComboBoxItems { get; set; } = new List<string>();

        public string SelectItem { get; set; } = string.Empty;


        public Window1()
        {
            InitializeComponent();

            DataContext = this;
            ComboBoxItems.Add("COM1");
            ComboBoxItems.Add("COM2");
            ComboBoxItems.Add("COM3");
            ComboBoxItems.Add("COM4");
            ComboBoxItems.Add("COM5");
            ComboBoxItems.Add("COM6");
            ComboBoxItems.Add("COM7");
            ComboBoxItems.Add("COM8");
            ComboBoxItems.Add("COM9");

            SelectItem = ConfigurationManager.AppSettings["DEVICE_COM"];
        
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
            
            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DEVICE_COM"].Value = SelectItem; //Device_1CBox.Text;
          
            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功");
        }


    }
}
