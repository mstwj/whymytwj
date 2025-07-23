using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace 我的地图.Controls
{
    /// <summary>
    /// ProgressRing.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressRing : UserControl
    {
        private Brush _forecolor = Brushes.Orange;

        
        public Brush ForeColor
        {
            get { return _forecolor; }
            set
            {
                _forecolor = value;
                this.Refresh();
            }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ProgressRing),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged))
                );

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ProgressRing;
            obj.Refresh();
        }

        public void Refresh()
        {
            var count = Math.Ceiling(Value / 100 * 360 / 8);
            for (int i = 0; i < MarkList.Count; i++)
            {
                if (i <= count)
                    this.MarkList[i].Color = this.ForeColor;
                else
                    this.MarkList[i].Color = Brushes.LightGray;
            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ProgressRing), new PropertyMetadata(""));


        public List<MarkInfo> MarkList { get; set; } = new List<MarkInfo>();
        public ProgressRing()
        {
            InitializeComponent();

            //8度一个角度 --工有多少个点呢？360 /8 就有这多个点...
            int count = 360 / 8;
            for (int i = 0; i < count; i++)
            {
                MarkList.Add(new MarkInfo() { Angle = i * 8 });
            }
        }

    }

    public class MarkInfo : INotifyPropertyChanged
    {
        public double Angle { get; set; }
        private Brush _color = Brushes.LightGray;
        public Brush Color
        {
            get => _color; set
            {
                _color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
