using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using 空载负载.Model;

namespace 空载负载.View
{
    /// <summary>
    /// NoloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoadPage : UserControl
    {
        public Load model { get; set; } = new Load();
        public LoadPage()
        {
            InitializeComponent();
            this.DataContext = model;
            MyButton.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown), true);
            MyButton.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(Button_MouseUp), true);

            MyButton2.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown2), true);
            MyButton2.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(Button_MouseUp2), true);

            model.InitiateModel(UserLoadRecordControl.model, UserLoadHead.model, UserLoadMainControl.model, UserCommunicationControl.model, PLCStateControl.model);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            float temp = 0;
            float.TryParse(TextCurrentBox.Text, out temp);

            if (temp == 0)
            {
                MessageBox.Show("请输入正常的数值!");
                return;
            }
            else
            {
                temp = (float)(temp * 1.2);
                ProtectCurrentBox.Text = temp.ToString();
                model.ProtectCurrent = temp;
            }

        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 按下时逻辑
            Debug.WriteLine("M12.0 true");
            model.DoTbCommmandUp1();
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // 弹起时逻辑
            Debug.WriteLine("M12.0 fales");
            model.DoTbCommmandDown1();
        }


        private void Button_MouseDown2(object sender, MouseButtonEventArgs e)
        {
            // 按下时逻辑
            Debug.WriteLine("M12.1 true");

            model.DoTbCommmandUp2();
        }

        private void Button_MouseUp2(object sender, MouseButtonEventArgs e)
        {
            // 弹起时逻辑
            Debug.WriteLine("M12.1 fales");
            model.DoTbCommmandDown2();
        }


    }
}
