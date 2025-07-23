using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using 空载负载.Base;
using 空载负载.Model;

namespace 空载负载
{
    /// <summary>
    /// BaseParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class BaseParamSet : Window
    {
        BaseParamModel baseParamModel = new BaseParamModel();
        public BaseParamSet()
        {
            InitializeComponent();
            this.DataContext = baseParamModel;
        }


        private async void Button_Click_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
