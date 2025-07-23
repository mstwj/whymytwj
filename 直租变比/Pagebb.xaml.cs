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
using 直租变比.Model;

namespace 直租变比
{
    /// <summary>
    /// Pagebb.xaml 的交互逻辑
    /// </summary>
    public partial class Pagebb : Page
    {
        //public PagebbModel m_pagebbModel { get; set; } = new PagebbModel();
        public Pagebb()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void dataGrid_Selected(object sender, RoutedEventArgs e)
        {
            var p = dataGrid.SelectedItem;
            //为什么这里会空....
            if (p != null)
            {
                //TBWZ.Text = p.Id.ToString();
            }

        }
    }
}
