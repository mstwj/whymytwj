using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLCNET5电容塔升级.Control
{
    /// <summary>
    /// UserMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserMainPage : UserControl
    {
        public UserMainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var task = new Task(() =>
            {
                Debug.WriteLine("子线程1开始!");
                //Things to wait for
                Thread.Sleep(5000);
                Debug.WriteLine("子线程1结束!");
            });

            //previousTask 前一个任务..
            //ContinueWith 方法会启动一个新的任务
            //以下为监控代码.. 监控前一个任务是不是完成..
            task.ContinueWith((previousTask) =>
            {
                //注意这里回来的主线程..--有个问题，如果线程超时了呢?
                //WPFDevelopers.Minimal.Controls.Loading.Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            //显示一个等待窗口..
            //WPFDevelopers.Minimal.Controls.Loading.Show();
            //任务开始执行..
            task.Start();
            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");
        }
    }
}
