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
using 空载负载.Base;
using 空载负载.Model;

namespace 空载负载.View
{
    /// <summary>
    /// NoloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class UtilityPage : UserControl
    {
        public Utility model { get; set; } = new Utility();
        public UtilityPage()
        {
            InitializeComponent();
            this.DataContext = model;            
            
            model.InitiateModel(UtilityHeadControl.model,  UserCommunicationControl.model, UtilityPLCStateControl.model,this);

            MyButton.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown), true);
            MyButton.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(Button_MouseUp), true);

            MyButton2.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown2), true);
            MyButton2.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(Button_MouseUp2), true);

            MyTools.UtiltyPageButton[0] = MyButton;
            MyTools.UtiltyPageButton[1] = MyButton2;
            MyTools.UtiltyPageButton[2] = ButtonDYHZ;
            MyTools.UtiltyPageButton[3] = ButtonDYFZ;
            MyTools.UtiltyPageButton[4] = ButtonGPHL;
            MyTools.UtiltyPageButton[5] = ButtonFW;
            MyTools.UtiltyPageButton[6] = ButtonKM;
            MyTools.UtiltyPageButton[7] = ButtonKSJS;
            MyTools.UtiltyPageButton[8] = ButtonTZJS;
            MyTools.UtiltyPageButton[9] = ButtonQRSZ;

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            float temp = 0;
            float.TryParse(TextVBox.Text, out temp);

            if (temp == 0)
            {
                MessageBox.Show("请输入正常的数值!");
                return;
            }
            else
            {
                temp = (float)(temp * 1.2);
                ProtectVoltageBox.Text = temp.ToString();
                model.ProtectVoltage = temp;
            }

        }

        public void SetWImage(int myimageW, int myimageH)
        {
            MyAxis.SetRange(0, myimageW);
            MyAyis.SetRange(0, myimageH);            
            MessageBox.Show("设置完成!");
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
