using Microsoft.Maps.MapControl.WPF;
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

namespace 我的地图.View.Pages
{
    /// <summary>
    /// FirstPage.xaml 的交互逻辑
    /// </summary>
    public partial class FirstPage : UserControl
    {
        public FirstPage()
        {
            InitializeComponent();

            //添加..
            LocationCollection locations = new LocationCollection();
            locations.Add(new Location(30.55201, 114.23246));
            locations.Add(new Location(34.30017, 108.95116));

            MapPolyline mapPolyline = new MapPolyline();
            mapPolyline.Locations = locations;
            mapPolyline.Stroke = Brushes.Blue; //颜色..
            mapPolyline.StrokeThickness = 2; //先狂.
            this.lineLayer.Children.Add(mapPolyline);

        }
    }
}
