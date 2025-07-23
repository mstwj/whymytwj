using CommunityToolkit.Mvvm.Messaging;
using PLCNET5_11_9.Data;
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

namespace PLCNET5_11_9
{
    /// <summary>
    /// myButtonTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class myButtonTemplate : UserControl
    {

        public static readonly DependencyProperty MyParameterProperty =
        DependencyProperty.Register("MyParameter", typeof(string), typeof(myButtonTemplate), new PropertyMetadata(null));


        public string MyParameter
        {
            get { return (string)GetValue(MyParameterProperty); }
            set { SetValue(MyParameterProperty, value); }
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

            //设置为竖图像...
            if (MyParameter == "VV")
            {
                myButtonModel.Shu = 1;
            }

            if (MyParameter == "HH")
            {
                myButtonModel.Shu = 2;
            }
        }

        //合...
    }
}
