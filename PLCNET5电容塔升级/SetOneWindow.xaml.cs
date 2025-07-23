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

namespace PLCNET5电容塔升级
{
    /// <summary>
    /// SetOneWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetOneWindow : Window
    {
        public SetOneWindow()
        {
            InitializeComponent();
            TB.Text = ConfigurationManager.AppSettings["BASEDATA"];            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {  
            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["BASEDATA"].Value = TB.Text;

            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功");

        }
    }
}
