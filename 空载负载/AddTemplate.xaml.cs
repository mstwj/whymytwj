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
using 空载负载.Model;

namespace 空载负载
{
    /// <summary>
    /// AddTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class AddTemplate : Window
    {
        public AddTemplateModel model { get; set; } = new AddTemplateModel();
        public AddTemplate()
        {
            InitializeComponent();
            this.DataContext = model;

        }

        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
