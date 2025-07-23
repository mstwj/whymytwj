using Microsoft.Toolkit.Mvvm.Messaging;
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
using System.Windows.Shapes;

namespace net5_10_14
{
    /// <summary>
    /// WaitInitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WaitInitWindow : Window
    {
        public WaitInitWindow()
        {
            InitializeComponent();

            // 注册消息..(必须在前面..)
            WeakReferenceMessenger.Default.Register<string, string>(this, "InitShowProc", InitShowProc);
            // 注册消息..(必须在前面..)
            WeakReferenceMessenger.Default.Register<string, string>(this, "CloseShowProc", CloseShowProc);
            WeakReferenceMessenger.Default.Register<string, string>(this, "SetShowProc", SetShowProc);
        }

        ~WaitInitWindow()
        {
            // 取消注册消息接收，避免内存泄漏
            WeakReferenceMessenger.Default.UnregisterAll(this);

        }

        private void InitShowProc(object recipient, string message)
        {
            this.Visibility = Visibility.Visible;
        }

        private void CloseShowProc(object recipient, string message)
        {
            this.Visibility = Visibility.Hidden;     
        }

        private void SetShowProc(object recipient, string message)
        {
            TitleTB.Text = message;            
        }


    }
}
