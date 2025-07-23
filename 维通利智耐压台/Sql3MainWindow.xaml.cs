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
using 维通利智耐压台.Model;

namespace 维通利智耐压台
{
    /// <summary>
    /// Sql3MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Sql3MainWindow : Window
    {
        public Sql3MainModel model { get; set; } = new Sql3MainModel();
        public Sql3MainWindow()
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
