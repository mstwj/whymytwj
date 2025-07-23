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
    /// WaitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WaitWindow : Window
    {
        public string mySelectItem { get; set; } = string.Empty;
        public List<string> ComboBoxItems { get; set; } = new List<string>();

        public WaitWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            ComboBoxItems.Add("COM1");
            ComboBoxItems.Add("COM2");
            ComboBoxItems.Add("COM3");
            ComboBoxItems.Add("COM4");
            ComboBoxItems.Add("COM5");
            ComboBoxItems.Add("COM6");
            ComboBoxItems.Add("COM7");
            ComboBoxItems.Add("COM8");
            ComboBoxItems.Add("COM9");
            ComboBoxItems.Add("COM10");
            ComboBoxItems.Add("COM11");
            ComboBoxItems.Add("COM12");

            mySelectItem = ConfigurationManager.AppSettings["DEVICE_COM"];

        }

        
    }
}
