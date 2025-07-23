using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 维通利智耐压台.Base;
using 维通利智耐压台.Model;

namespace 维通利智耐压台
{
    /// <summary>
    /// ExportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExportWindow : Window
    {
        

        public ExportModel mode { get; set; } = new ExportModel();
        public ExportWindow()
        {
            InitializeComponent();
            this.DataContext = mode;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mode.SelectFineSave != string.Empty)
            {
                //先生产一图片..(这里还是个流程，不需要分开，主线程先来这里..)
                mode.Savepng = MyTools.SaveDataToBmp(ExportChart);
                Thread.Sleep(500);
            }
            else
            {
                MessageBox.Show("请先设置WORD的保存路径和文件名!");
                return;
            }

        }
    }
}
