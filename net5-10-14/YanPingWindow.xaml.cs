using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.ViewModel;
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
    /// YanPingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class YanPingWindow : Window
    {
        private YanPingViewModel myYanPingViewModel = new YanPingViewModel();
        public YanPingWindow()
        {
            InitializeComponent();
            this.DataContext = myYanPingViewModel;
            autoCompleteBox.ItemsSource = myYanPingViewModel.Items;
            autoCompleteBox.DataContext = myYanPingViewModel.Items;
            autoCompleteBox.SelectedItem = myYanPingViewModel.SelectedItem;

            // 注册消息..
            WeakReferenceMessenger.Default.Register<string, string>(this, "Inspect", ReceiveMessage);            
            WeakReferenceMessenger.Default.Register<string,string>(this,"Close", ReceiveMessage2);                       

        }

        ~YanPingWindow()
        {
            // 取消注册消息接收，避免内存泄漏
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        private void ReceiveMessage2(object recipient, string message)
        {
            this.Close();
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        private void ReceiveMessage(object recipient, string message)
        {
            //为什么要这样搞，以为，我无法得到autoCompleteBox的值...(以后在解决.)
            myYanPingViewModel.Data1 = autoCompleteBox.Text;

            if (Validation.GetHasError(TextBox1))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }

            if (Validation.GetHasError(TextBox2))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }            

            if (Validation.GetHasError(TextBox3))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }

            if (Validation.GetHasError(TextBox4))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }

           

            if (Validation.GetHasError(TextBox5))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }

            if (Validation.GetHasError(TextBox6))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }

            if (Validation.GetHasError(TextBox7))
            {
                myYanPingViewModel.Inspection = false;
                return;
            }


            myYanPingViewModel.Inspection = true;
            return;
        }

        private void AutoCompleteBox_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Key == Key.Enter)
            {
                myYanPingViewModel.SetViewData(autoCompleteBox.Text);
            }
        }

        private void TextBoxEdingGaoYa_KeyDown(object sender, KeyEventArgs e) 
        {
            TextBox textbox = sender as TextBox;
            textbox.Text = textbox.Text.Trim(); //去掉空格.

            if (textbox.Text == null || textbox.Text.Length == 0) return;
            if (e.Key == Key.Enter)
            {
                // 在这里添加你想要执行的代码
                if (autoCompleteBox.Text == null || autoCompleteBox.Text.Length == 0) return;
                

                char[] delimiters = new char[] { '/' };
                string[] substrings = autoCompleteBox.Text.Split(delimiters);
                if (substrings.Length == 2)
                {
                    string newstr = null; 
                    substrings[1] = textbox.Text;
                    autoCompleteBox.Text = substrings[0] + '/' + substrings[1];
                }

                if (substrings.Length == 1)
                {
                    autoCompleteBox.Text = autoCompleteBox.Text + '/' + textbox.Text ;
                }

                if (substrings.Length == 3)
                {
                    string newstr = null;
                    substrings[1] = textbox.Text;
                    autoCompleteBox.Text = substrings[0] + '/' + substrings[1] + '/' + substrings[2];
                }


                // 如果你想要防止默认的回车键行为（比如文本框换行），可以添加下面这行
                e.Handled = true;

            }
        }

        private void TextBoxEdingDiYa_KeyDown(object sender, KeyEventArgs e) 
        {
            TextBox textbox = sender as TextBox;
            textbox.Text = textbox.Text.Trim(); //去掉空格.

            if (textbox.Text == null || textbox.Text.Length == 0) return;
            if (e.Key == Key.Enter)
            {
                // 在这里添加你想要执行的代码
                if (autoCompleteBox.Text == null || autoCompleteBox.Text.Length == 0) return;


                char[] delimiters = new char[] { '/' };
                string[] substrings = autoCompleteBox.Text.Split(delimiters);
                if (substrings.Length == 2)
                {                                        
                    autoCompleteBox.Text = autoCompleteBox.Text + '/' + textbox.Text;
                }

                if (substrings.Length == 3)
                {                                        
                    autoCompleteBox.Text = substrings[0] + '/' + substrings[1] + '/' + textbox.Text;
                }


                // 如果你想要防止默认的回车键行为（比如文本框换行），可以添加下面这行
                e.Handled = true;

            }
        }



        private void TextBoxProType_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            textbox.Text = textbox.Text.Trim(); //去掉空格.

            if (textbox.Text == null || textbox.Text.Length == 0) return;
            if (e.Key == Key.Enter)
            {
                string str = autoCompleteBox.Text;
                // 在这里添加你想要执行的代码
                if (autoCompleteBox.Text == null || autoCompleteBox.Text.Length == 0)
                {
                    autoCompleteBox.Text = textbox.Text;
                    return;
                }
                
                char[] delimiters = new char[] { '|' };
                string[] substrings = autoCompleteBox.Text.Split(delimiters);
                if (substrings.Length == 2) 
                {
                    substrings[0] = textbox.Text;
                    autoCompleteBox.Text = substrings[0] = substrings[0] + "|" + substrings[1];
                }                

                // 如果你想要防止默认的回车键行为（比如文本框换行），可以添加下面这行
                e.Handled = true;

            }
        }

        private void TextBoxEdingRonglang_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            textbox.Text = textbox.Text.Trim(); //去掉空格.
            if (e.Key == Key.Enter)
            {
                if (textbox.Text == null || textbox.Text.Length == 0) return;

                string str = autoCompleteBox.Text;
                // 在这里添加你想要执行的代码
                if (autoCompleteBox.Text == null || autoCompleteBox.Text.Length == 0)
                {
                    return;
                }

                char[] delimiters = new char[] { '-' };
                string[] substrings = autoCompleteBox.Text.Split(delimiters);
                if (substrings.Length == 2)
                {
                    substrings[1] = textbox.Text;
                    autoCompleteBox.Text = substrings[0] = substrings[0] + "-" + substrings[1];
                }
                if (substrings.Length == 1)
                {
                    autoCompleteBox.Text = autoCompleteBox.Text + "-" + textbox.Text;
                }

                // 如果你想要防止默认的回车键行为（比如文本框换行），可以添加下面这行
                e.Handled = true;

            }

        }



        private void TextBoxTuhao_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            textbox.Text = textbox.Text.Trim(); //去掉空格.
            if (e.Key == Key.Enter)
            {
                if (textbox.Text == null || textbox.Text.Length == 0) return;

                string str = autoCompleteBox.Text;
                // 在这里添加你想要执行的代码
                if (autoCompleteBox.Text == null || autoCompleteBox.Text.Length == 0)
                {
                    return;
                }

                char[] delimiters = new char[] { '|' };
                string[] substrings = autoCompleteBox.Text.Split(delimiters);
                if (substrings.Length == 2)
                {
                    substrings[1] = textbox.Text;
                    autoCompleteBox.Text = substrings[0] = substrings[0] + "|" + substrings[1];
                }
                if (substrings.Length == 1)
                {
                    autoCompleteBox.Text = autoCompleteBox.Text + "|" + textbox.Text;
                }

                // 如果你想要防止默认的回车键行为（比如文本框换行），可以添加下面这行
                e.Handled = true;

            }

        }




    }
}
