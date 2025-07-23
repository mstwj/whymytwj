using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace 直租变比
{
    /// <summary>
    /// DevedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DevedWindow : Window
    {
        
        public string SelectItemBB { get; set; } = string.Empty;
        public List<string> ComboBoxItemsBB { get; set; } = new List<string>();

        public string SelectItemDY { get; set; } = string.Empty;
        public List<string> ComboBoxItemsDY { get; set; } = new List<string>();

        public string SelectItemGY { get; set; } = string.Empty;
        public List<string> ComboBoxItemsGY { get; set; } = new List<string>();

        public DevedWindow()
        {
            InitializeComponent();
            DataContext = this;
            ComboBoxItemsBB.Add("COM1");
            ComboBoxItemsBB.Add("COM2");
            ComboBoxItemsBB.Add("COM3");
            ComboBoxItemsBB.Add("COM4");
            ComboBoxItemsBB.Add("COM5");
            ComboBoxItemsBB.Add("COM6");
            ComboBoxItemsBB.Add("COM7");
            ComboBoxItemsBB.Add("COM8");
            ComboBoxItemsBB.Add("COM9");

            ComboBoxItemsDY.Add("COM1");
            ComboBoxItemsDY.Add("COM2");
            ComboBoxItemsDY.Add("COM3");
            ComboBoxItemsDY.Add("COM4");
            ComboBoxItemsDY.Add("COM5");
            ComboBoxItemsDY.Add("COM6");
            ComboBoxItemsDY.Add("COM7");
            ComboBoxItemsDY.Add("COM8");
            ComboBoxItemsDY.Add("COM9");

            ComboBoxItemsGY.Add("COM1");
            ComboBoxItemsGY.Add("COM2");
            ComboBoxItemsGY.Add("COM3");
            ComboBoxItemsGY.Add("COM4");
            ComboBoxItemsGY.Add("COM5");
            ComboBoxItemsGY.Add("COM6");
            ComboBoxItemsGY.Add("COM7");
            ComboBoxItemsGY.Add("COM8");
            ComboBoxItemsGY.Add("COM9");

            LoadIni();

        }

        public void SaveIni()
        {

            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DEVICE_PLC"].Value = Device_PLC.Text; //Device_1CBox.Text;
            config.AppSettings.Settings["DEVICE_COM_BB"].Value = SelectItemBB;
            config.AppSettings.Settings["DEVICE_COM_ZD"].Value = SelectItemDY;
            config.AppSettings.Settings["DEVICE_COM_ZG"].Value = SelectItemGY;

            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功,请重新启动软件.");
        }

        public void LoadIni()
        {
            //读取配置            
            Device_PLC.Text = ConfigurationManager.AppSettings["DEVICE_PLC"];
            SelectItemBB = ConfigurationManager.AppSettings["DEVICE_COM_BB"];
            SelectItemDY = ConfigurationManager.AppSettings["DEVICE_COM_ZD"];
            SelectItemGY = ConfigurationManager.AppSettings["DEVICE_COM_ZG"];
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
