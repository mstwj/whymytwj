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

namespace net5_10_14
{
    /// <summary>
    /// DevedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DevedWindow : Window
    {
        public List<string> ComboBoxItems { get; set; } = new List<string>();

        public string mySelectItem1 { get; set; } = null;

        public string mySelectItem2 { get; set; } = null;
        public string mySelectItem3 { get; set; } = null;
        public string mySelectItem4 { get; set; } = null;
        public string mySelectItem5 { get; set; } = null;

        public string mySelectItem6 { get; set; } = null;
        public string mySelectItem7 { get; set; } = null;


        public DevedWindow()
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


            mySelectItem1 = ConfigurationManager.AppSettings["DEVICE_1"];
            mySelectItem2 = ConfigurationManager.AppSettings["DEVICE_2"];
            mySelectItem3 = ConfigurationManager.AppSettings["DEVICE_3"];
            mySelectItem4 = ConfigurationManager.AppSettings["DEVICE_4"];
            mySelectItem5 = ConfigurationManager.AppSettings["DEVICE_5"];
            mySelectItem6 = ConfigurationManager.AppSettings["DEVICE_6"];
            mySelectItem7 = ConfigurationManager.AppSettings["DEVICE_7"];
            //Device_6CBox_GY



            LoadIni();

        }

        public void LoadIni()
        {
            //读取配置
            //Device_1CBox.Text = ConfigurationManager.AppSettings["DEVICE_1"];
            Device_4CBox.SelectedItem = ConfigurationManager.AppSettings["DEVICE_4"];
            Device_6CBox.SelectedItem = ConfigurationManager.AppSettings["DEVICE_6"];
            Device_7CBox.SelectedItem = ConfigurationManager.AppSettings["DEVICE_7"];
        }

        public void SaveIni()
        {

            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DEVICE_1"].Value = mySelectItem1; //Device_1CBox.Text;
            config.AppSettings.Settings["DEVICE_4"].Value = Device_4CBox.SelectedItem.ToString();
            config.AppSettings.Settings["DEVICE_6"].Value = Device_6CBox.SelectedItem.ToString();
            config.AppSettings.Settings["DEVICE_7"].Value = Device_7CBox.SelectedItem.ToString();

            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功");
        }


        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            SaveIni();
        }

        private void Button_Click_Load(object sender, RoutedEventArgs e)
        {
            LoadIni();
        }

    }
}
