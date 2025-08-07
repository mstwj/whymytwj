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
using 串口测试.Base;
using 串口测试.ViewModels;

namespace 串口测试.Views
{
    /// <summary>
    /// SystemMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class SystemMonitor : UserControl
    {
        public SystemMonitor()
        {
            InitializeComponent();
            //这里为什么不需要这样呢？因为，在XAML文件里面，很神奇.. NEW了一个
            //我也不知道，下面这样的写法，和在XAML文件里面，NEW一个，又有什么区别呢？
            //this.DataContext = new SystemMonitorViewModel();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //test1
            RTU rtuInstance = RTU.GetInstance(null);

            //Task<bool> Send(int slaveAddr, byte funcCode, int startAddr, int len)
            await rtuInstance.Send(1, (byte)3, 0, 2);            
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //test2
            RTU rtuInstance = RTU.GetInstance(null);
            await rtuInstance.Send(1, (byte)3, 1, 2);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //test3
            RTU rtuInstance = RTU.GetInstance(null);
            await rtuInstance.Send(1, (byte)3, 2, 2);
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //test4

            RTU rtuInstance = RTU.GetInstance(null);
            await rtuInstance.Send(1, (byte)3, 3, 2);
        }
    }
}
