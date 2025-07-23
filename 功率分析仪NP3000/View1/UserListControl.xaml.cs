using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CommunityToolkit.Mvvm.Messaging;

namespace 功率分析仪NP3000.View1
{
    /// <summary>
    /// UserListControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserListControl : UserControl
    {
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();

        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public UserListControl()
        {
            InitializeComponent();
            this.DataContext= this;
            AddRecord("List启动完成!", false);
        }

        public void AddRecord(string strdata, bool e = false)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });
                logger.Debug(str);
            });            
        }

    }

    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }
}
