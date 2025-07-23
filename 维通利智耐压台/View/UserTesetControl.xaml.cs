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
using 维通利智耐压台.Model;
using Osklib;
using System.Runtime.InteropServices;



namespace 维通利智耐压台.View
{
    /// <summary>
    /// UserTesetControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserTesetControl : UserControl
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        const uint SWP_NOZORDER = 0x0004; // 不改变Z顺序
        const uint SWP_NOACTIVATE = 0x0010; // 不激活窗口

        public void ResizeOnScreenKeyboard(int width, int height)
        {
            // 查找屏幕键盘的窗口
            IntPtr hWnd = FindWindow("IPTipClass", null);
            if (hWnd != IntPtr.Zero)
            {
                // 设置新的大小和位置
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 200, 300, SWP_NOZORDER);
            }
            else
            {
                Console.WriteLine("Screen keyboard window not found.");
            }
        }

        public UserTestControlModel model { get; set; } = new UserTestControlModel();
        public UserTesetControl()
        {
            InitializeComponent();
            DataContext = model;
        }

        //得到焦点
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
                ResizeOnScreenKeyboard(100,100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_GotFocusTimer(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_GotFocus2Timer(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void TextBox_GotFocus3Timer(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void  TextBox_GotFocusgaoyabaohu(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBox_GotFocusgaoliubaohu(object sender, RoutedEventArgs e)
        {
            try
            {
                //  Osklib.OnScreenKeyboard.Close();
                Osklib.OnScreenKeyboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
