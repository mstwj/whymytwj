using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace 空载负载
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "StikyNotesAPP", out ret);

            if (!ret)
            {
                MessageBox.Show("程序已经运行了");
                Environment.Exit(0);
            }

            base.OnStartup(e);

        }
    }
}
