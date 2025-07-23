using CommunityToolkit.Mvvm.Messaging;
using PLCNET5电容塔.Data;
using S7.Net.Types;
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

namespace PLCNET5电容塔
{
    /// <summary>
    /// myButtonTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class myButtonTemplate : UserControl
    {
        //这里可以在属性里面看到了...
        public static readonly DependencyProperty MyParameterProperty =
        DependencyProperty.Register("MyParameter", typeof(string), typeof(myButtonTemplate), new PropertyMetadata(null));

        public static readonly DependencyProperty MyParameterProperty2 =
        DependencyProperty.Register("MyParameter2", typeof(string), typeof(myButtonTemplate), new PropertyMetadata(null));


        public string MyParameter
        {
            get { return (string)GetValue(MyParameterProperty); }
            set { SetValue(MyParameterProperty, value); }
        }

        public string MyParameter2
        {
            get { return (string)GetValue(MyParameterProperty2); }
            set { SetValue(MyParameterProperty2, value); }
        }


        public myButtonModel myButtonModel { get; set; } = new myButtonModel();
        public myButtonTemplate()
        {
            InitializeComponent();
            this.DataContext = myButtonModel;
            myButtonModel.Data = false;
            this.Loaded += MyUserControl_Loaded;

        }

        //这里都初始化OK了..
        private void MyUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 在这里，您可以安全地访问UserControl的属性和子元素(隐藏下面按钮..)
            if (MyParameter == "Hide")
            {
                myButtonFen.Visibility = Visibility.Hidden;
                myButtonHe.Visibility = Visibility.Hidden;
            }

            //设置为竖图像...
            if (MyParameter2 == "VV")
            {
                myButtonModel.Shu = true;
            }
        }

        //合...
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Name == "myButtonGA") { WeakReferenceMessenger.Default.Send<string, string>("myButtonGA_He", "SendPLCCommand"); }
            if (this.Name == "myButtonGB") { WeakReferenceMessenger.Default.Send<string, string>("myButtonGB_He", "SendPLCCommand"); }
            if (this.Name == "myButtonGC") { WeakReferenceMessenger.Default.Send<string, string>("myButtonGC_He", "SendPLCCommand"); }

            if (this.Name == "myButtonG1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonG2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonG3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonG4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonG5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG5_He", "SendPLCCommand"); }

            if (this.Name == "myButton1G1") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G1_He", "SendPLCCommand"); }
            if (this.Name == "myButton1G2") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G2_He", "SendPLCCommand"); }
            if (this.Name == "myButton1G3") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G3_He", "SendPLCCommand"); }
            if (this.Name == "myButton1G4") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G4_He", "SendPLCCommand"); }
            if (this.Name == "myButton1G5") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G5_He", "SendPLCCommand"); }
            if (this.Name == "myButton1G6") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G6_He", "SendPLCCommand"); }

            if (this.Name == "myButton2G1") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G1_He", "SendPLCCommand"); }
            if (this.Name == "myButton2G2") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G2_He", "SendPLCCommand"); }
            if (this.Name == "myButton2G3") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G3_He", "SendPLCCommand"); }
            if (this.Name == "myButton2G4") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G4_He", "SendPLCCommand"); }
            if (this.Name == "myButton2G5") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G5_He", "SendPLCCommand"); }
            if (this.Name == "myButton2G6") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G6_He", "SendPLCCommand"); }

            if (this.Name == "myButton3G1") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G1_He", "SendPLCCommand"); }
            if (this.Name == "myButton3G2") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G2_He", "SendPLCCommand"); }
            if (this.Name == "myButton3G3") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G3_He", "SendPLCCommand"); }
            if (this.Name == "myButton3G4") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G4_He", "SendPLCCommand"); }
            if (this.Name == "myButton3G5") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G5_He", "SendPLCCommand"); }
            if (this.Name == "myButton3G6") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G6_He", "SendPLCCommand"); }

            if (this.Name == "myButton1K1") { WeakReferenceMessenger.Default.Send<string, string>("myButton1K1_He", "SendPLCCommand"); }
            if (this.Name == "myButton1K2") { WeakReferenceMessenger.Default.Send<string, string>("myButton1K2_He", "SendPLCCommand"); }
            if (this.Name == "myButton2K1") { WeakReferenceMessenger.Default.Send<string, string>("myButton2K1_He", "SendPLCCommand"); }
            if (this.Name == "myButton2K2") { WeakReferenceMessenger.Default.Send<string, string>("myButton2K2_He", "SendPLCCommand"); }
            if (this.Name == "myButton3K1") { WeakReferenceMessenger.Default.Send<string, string>("myButton3K1_He", "SendPLCCommand"); }
            if (this.Name == "myButton3K2") { WeakReferenceMessenger.Default.Send<string, string>("myButton3K2_He", "SendPLCCommand"); }

            if (this.Name == "myButtonQ1_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_5_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_6_He", "SendPLCCommand"); }

            if (this.Name == "myButtonQ2_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_5_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_6_He", "SendPLCCommand"); }

            if (this.Name == "myButtonQ3_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_5_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_6_He", "SendPLCCommand"); }

            if (this.Name == "myButtonQ4_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_5_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_6_He", "SendPLCCommand"); }

            if (this.Name == "myButtonQ5_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_5_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_6_He", "SendPLCCommand"); }

            if (this.Name == "myButtonQ6_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_1_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_2_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_3_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_4_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_5_He", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_6_He", "SendPLCCommand"); }

        }

        //分
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Name == "myButtonGA") { WeakReferenceMessenger.Default.Send<string, string>("myButtonGA_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonGB") { WeakReferenceMessenger.Default.Send<string, string>("myButtonGB_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonGC") { WeakReferenceMessenger.Default.Send<string, string>("myButtonGC_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonG1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonG2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonG3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonG4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonG5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonG5_Fen", "SendPLCCommand"); }

            if (this.Name == "myButton1G1") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton1G2") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton1G3") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton1G4") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton1G5") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton1G6") { WeakReferenceMessenger.Default.Send<string, string>("myButton1G6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButton2G1") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2G2") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2G3") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2G4") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2G5") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2G6") { WeakReferenceMessenger.Default.Send<string, string>("myButton2G6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButton3G1") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3G2") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3G3") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3G4") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3G5") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3G6") { WeakReferenceMessenger.Default.Send<string, string>("myButton3G6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButton1K1") { WeakReferenceMessenger.Default.Send<string, string>("myButton1K1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton1K2") { WeakReferenceMessenger.Default.Send<string, string>("myButton1K2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2K1") { WeakReferenceMessenger.Default.Send<string, string>("myButton2K1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton2K2") { WeakReferenceMessenger.Default.Send<string, string>("myButton2K2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3K1") { WeakReferenceMessenger.Default.Send<string, string>("myButton3K1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButton3K2") { WeakReferenceMessenger.Default.Send<string, string>("myButton3K2_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonQ1_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ1_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ1_6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonQ2_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ2_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ2_6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonQ3_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ3_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ3_6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonQ4_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ4_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ4_6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonQ5_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ5_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ5_6_Fen", "SendPLCCommand"); }

            if (this.Name == "myButtonQ6_1") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_1_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_2") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_2_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_3") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_3_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_4") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_4_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_5") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_5_Fen", "SendPLCCommand"); }
            if (this.Name == "myButtonQ6_6") { WeakReferenceMessenger.Default.Send<string, string>("myButtonQ6_6_Fen", "SendPLCCommand"); }

        }
    }
}
