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
using 空载_负载.Model;

namespace 空载_负载.View
{
    /// <summary>
    /// UserPlcDevcel.xaml 的交互逻辑
    /// </summary>
    public partial class UserPlcDevcel : UserControl
    {
        public PlcDevcelModel Model { get; set; } = new PlcDevcelModel();
        public UserPlcDevcel()
        {
            InitializeComponent();
            this.DataContext = Model;
        }



    }
}
