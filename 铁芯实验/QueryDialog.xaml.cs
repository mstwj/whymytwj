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
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.IdentityModel.Tokens;
using 铁芯实验.Base;

namespace 铁芯实验
{
    /// <summary>
    /// QueryDialog.xaml 的交互逻辑
    /// </summary>
    public partial class QueryDialog : Window
    {
        public List<string> QueryCBoxData { get; set; } = new List<string>();
        public int QueryCBoxIntSelect { get; set; } = 0; 

        public string MainDialog { get; set; } = string.Empty;
        public QueryDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyMessage myMessage = new MyMessage();
            if (MainDialog == "TableProduction")
                myMessage.Message = "SearchTableProduction";
            if (MainDialog == "TableRecord")
                myMessage.Message = "SearchTableRecord"; 
            myMessage.SearchIndex = QueryCBoxIntSelect;
            myMessage.Search = TboxEdit.Text;
            WeakReferenceMessenger.Default.Send(myMessage);

        }
    }
}
