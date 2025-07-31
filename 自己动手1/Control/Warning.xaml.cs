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

namespace 自己动手1.Control
{
    /// <summary>
    /// Warning.xaml 的交互逻辑
    /// </summary>
    public partial class WarningLight : UserControl
    {

        // 灯光状态的依赖属性
        public LightState LightState
        {
            get { return (LightState)GetValue(LightStateProperty); }
            set { SetValue(LightStateProperty, value); }
        }

        //为什么要加下面这个东西，如果是一般属性，可以不加。如果要是 BIND就必须加上这些东西..
        public static readonly DependencyProperty LightStateProperty =
            DependencyProperty.Register("LightState", typeof(LightState),
                typeof(WarningLight),
                new PropertyMetadata(LightState.None, new PropertyChangedCallback(OnLightStateChanged)));


        private static void OnLightStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //不要问为什么，反正就这样去写就OK了.
            //e.NewValue.ToString();
            VisualStateManager.GoToState(d as WarningLight, e.NewValue.ToString(), false);
        }

        public WarningLight()
        {
            InitializeComponent();
        }
    }

    public enum LightState
    {
        None,Fault,Warning,Run
    }
}
