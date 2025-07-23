using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPFDevelopers.Minimal.Controls;

namespace WpfApp外观UI.Control
{
    /// <summary>
    /// NotifyIconControl.xaml 的交互逻辑
    /// </summary>
    public partial class NotifyIconControl : UserControl
    {
        public NotifyIconControl()
        {
            InitializeComponent();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            //new AboutWindow().Show();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            NotifyIcon.ShowBalloonTip("Message", " Welcome to WPFDevelopers.Minimal ", NotifyIconInfoType.None);
        }

        private void Twink_Click(object sender, RoutedEventArgs e)
        {
            WpfNotifyIcon.IsTwink = !WpfNotifyIcon.IsTwink;
            menuItemTwink.IsChecked = WpfNotifyIcon.IsTwink;
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
                Debug.WriteLine("主线程回调");
                //注意这里回来的主线程..--有个问题，如果线程超时了呢?
                WPFDevelopers.Minimal.Controls.Loading.Close();

            }, TaskScheduler.FromCurrentSynchronizationContext());
            //显示一个等待窗口..
            WPFDevelopers.Minimal.Controls.Loading.Show();
            //任务开始执行..
            task.Start();
            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            var task = new Task(() =>
            {
                Debug.WriteLine("子线程开始!");
                for (int i = 0; i < 5; i++)
                {
                    //这里做自己的事情
                    if (tokenSource.IsCancellationRequested)
                    {
                        Debug.WriteLine("子线程被取消了!");
                        return;
                    }
                    Thread.Sleep(5000);
                }
                Debug.WriteLine("子线程退出!");
            }, cancellationToken);
            task.ContinueWith(previousTask =>
            {
                Debug.WriteLine("主线程得到消息.");
                if (tokenSource.IsCancellationRequested) return;
                Loading.Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Loading.Show(true);
            Loading.LoadingQuitEvent += delegate
            {
                Debug.WriteLine("点击了取消!");
                tokenSource.Cancel();
            };
            task.Start();
            Debug.WriteLine("主线程返回!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WPFDevelopers.Minimal.Controls.MessageBox.Show("文件删除成功。", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
            WPFDevelopers.Minimal.Controls.MessageBox.Show("当前文件不存在！", "警告", MessageBoxImage.Warning);
            WPFDevelopers.Minimal.Controls.MessageBox.Show("当前文件不存在。", "错误", MessageBoxImage.Error);
            WPFDevelopers.Minimal.Controls.MessageBox.Show("当前文件不存在,是否继续?", "询问", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }

}
