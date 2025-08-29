using System.Data;
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
using HandyControlUI控件使用.ViewModels;
using HandyControlUI控件使用.Views;

namespace HandyControlUI控件使用
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void StartLoading_Click(object sender, RoutedEventArgs e)
        {
            string sql = "Select Stuid,stuName from StudentInfos where IsDelete=0";
           // DataTable dt = SqlHelper.GetDataTable();

            /*
            loadingOverlay.Visibility = Visibility.Visible;

            // 模拟耗时操作
            Task.Run(() => {
                Thread.Sleep(3000);

                // 回到UI线程更新
                Dispatcher.Invoke(() => {
                    loadingOverlay.Visibility = Visibility.Collapsed;
                });
            });
            */
        }

        private async void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var d = HandyControl.Controls.Dialog.Show(new TextDialog());
            await Task.Delay(3000);
            d.Close();

        }

        private void SNBtn_Click(object sender, RoutedEventArgs e)
        {
            new GuidWindow().ShowDialog();
        }

        private void TCBtn_Click(object sender, RoutedEventArgs e)
        {
            HandyControl.Controls.Growl.Success("消息通知");
        }

        private void MessBtn_Click(object sender, RoutedEventArgs e)
        {
            HandyControl.Controls.MessageBox.Show("提交信息成功！", "成功 ", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
    }
}