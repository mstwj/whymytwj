using System.Configuration;
using System.Data;
using System.Windows;
using HandyControlUI控件使用.Base;

namespace HandyControlUI控件使用
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        GlobalMonitor globalMonitor = new GlobalMonitor();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalMonitor.Start();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalMonitor.Stop();
            base.OnExit(e);
        }
    }

}
