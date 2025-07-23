using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp外观UI.ViewModel;


namespace WpfApp外观UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel model = new MainWindowViewModel();
        public MainWindow()
        {
            //OK了，这里我测试了一个分页，是OK的.. 其他就不需要测试了...
            InitializeComponent();
            this.DataContext = model;
        }



        /*
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Tag)
            {
                case "Info":
                    Message.Push(App.Current.MainWindow, "This is a info message", MessageBoxImage.Information);
                    break;
                case "Error":
                    Message.Push("This is a error message", MessageBoxImage.Error, true);
                    break;
                case "Warning":
                    Message.Push("This is a warning message", MessageBoxImage.Warning, true);
                    break;
                case "Question":
                    Message.Push("This is a question message", MessageBoxImage.Question);
                    break;
                default:
                    Message.Push("这是一条很长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长长消息", MessageBoxImage.Information);
                    break;
            }
        }
        */

    }
}