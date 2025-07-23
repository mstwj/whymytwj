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

namespace 维通利智耐压台
{
    /// <summary>
    /// ParamWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParamWindow : Window
    {
        public string GaoyadianyaSelect { get; set; } = string.Empty;
        public string GaoyadianliuSelect { get; set; } = string.Empty;
        public string DiyadianyaSelect { get; set; } = string.Empty;
        public string DiyadianliuSelect { get; set; } = string.Empty;

        public ParamWindow()
        {
            InitializeComponent();

            DataContext = this;

            GaoyadianyaSelect = ConfigurationManager.AppSettings["DEVICE_GAOYADIANYA"];
            GaoyadianliuSelect = ConfigurationManager.AppSettings["DEVICE_GAOYADIANLIU"];
            DiyadianyaSelect = ConfigurationManager.AppSettings["DEVICE_DIYADIANYA"];
            DiyadianliuSelect = ConfigurationManager.AppSettings["DEVICE_DIYADIANLIU"];
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
            float val1, val2, val3, val4;
            if (!float.TryParse(GaoyadianyaSelect, out val1))
            {
                MessageBox.Show("高压电压设置错误!");
                return;
            }
            if (!float.TryParse(GaoyadianliuSelect, out val2))
            {
                MessageBox.Show("高压电流设置错误!");
                return;
            }
            if (!float.TryParse(DiyadianyaSelect, out val3))
            {
                MessageBox.Show("低压电压设置错误!");
                return;
            }
            if (!float.TryParse(DiyadianliuSelect, out val4))
            {
                MessageBox.Show("低压电流设置错误!");
                return;
            }

            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DEVICE_GAOYADIANYA"].Value = GaoyadianyaSelect; 
            config.AppSettings.Settings["DEVICE_GAOYADIANLIU"].Value = GaoyadianliuSelect;
            config.AppSettings.Settings["DEVICE_DIYADIANYA"].Value = DiyadianyaSelect;
            config.AppSettings.Settings["DEVICE_DIYADIANLIU"].Value = DiyadianliuSelect;


            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功");
        }

    }
}
