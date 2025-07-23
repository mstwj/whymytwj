using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Prism.Ioc;
using 童文君第一个Prism项目.Interface;

namespace 童文君第一个Prism项目.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IContainerProvider containerProvider,ICommunication communication)
        {
            //InitializeComponent 这个代码就是解析. MainWidonw.xaml文件的..
            InitializeComponent();

            if (containerProvider.Resolve<LoginView>().ShowDialog() != true)
            {
                Application.Current.Shutdown();
                return;
            }
            //如果没有.. 登入成功了..
            communication.Connect("127.0.0.1","1883");


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
