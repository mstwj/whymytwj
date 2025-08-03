using System.Configuration;
using System.Data;
using System.Windows;
using 串口测试.Base;
using 串口测试.Views;

namespace 串口测试
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalMonitor.Start(
                () =>
                {
                    //成功..

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        new MainWindow().Show();
                    });
                },
                (msg) =>
                {
                    //失败..
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(msg,"系统启动失败!");
                        Application.Current.Shutdown();
                    });
                });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalMonitor.Dispose();
            base.OnExit(e);
        }
    }

}
