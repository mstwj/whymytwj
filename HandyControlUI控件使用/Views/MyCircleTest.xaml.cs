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

namespace HandyControlUI控件使用.Views
{
    /// <summary>
    /// MyCircleTest.xaml 的交互逻辑
    /// </summary>
    public partial class MyCircleTest : UserControl
    {

        //dp是什么意思呢？这个是什么意思，外面要去设置数据，必须 PROPDP这样去定义..
        //如何按下2次    TABLE就OK了..
        //对外部，怎么样去响应变化呢？

       


        public double Value
        {
            get { return (double)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(MyCircleTest), new PropertyMetadata (default(double),new PropertyChangedCallback(OnPropertyChanged)));


        public MyCircleTest()
        {
            InitializeComponent();

            this.Loaded += MyUserControl_Loaded;

        }

        private void MyUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 在这里，您可以安全地访问UserControl的属性和子元素(隐藏下面按钮..)
            //注意这里可以得到20.2... 有个问题，就是 看不到20.2这个数据显示。。。
            //这里在页面，不能{BIDNGING }
            //这里有一个固定写法： 很长，必须这样去写，才可以看到数据...(不要问为什么，必须这样写)
            double mydata = Value;
           

        }



        private static void OnPropertyChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            //每次外部修改的时候，会跳到这里来..
            (d as MyCircleTest).UpdateValue();
        }

        private void UpdateValue()
        {
            //我就简单点，不处理修改WITHD的情况..
            //这里，老师使用很复杂的算法，来进行数据，数学的技术，计算后，得到数据，在进行设置..
            //这里需要基本数学，知道什么是COS SIN等等。去算，我就算了..

        }
    }
}
