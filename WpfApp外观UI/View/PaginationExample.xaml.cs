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
using WpfApp外观UI.Model;

namespace WpfApp外观UI.View
{
    /// <summary>
    /// PaginationExample.xaml 的交互逻辑
    /// </summary>
    public partial class PaginationExample : UserControl
    {
        public PaginationExampleVM NormalPaginationViewModel { get; set; } = new PaginationExampleVM();
        public PaginationExampleVM LitePaginationViewModel { get; set; } = new PaginationExampleVM();


        public PaginationExample()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
