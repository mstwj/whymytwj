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
        public  MainViewModel mainViewModel =  new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
        }

        private void StartLoading_Click(object sender, RoutedEventArgs e)
        {
            //string sql = "Select Stuid,stuName from StudentInfos where IsDelete=0";
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


        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //双击可以进来...
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                Type type=null;
                if (item.Header.ToString().Equals("摄像头1", StringComparison.Ordinal))
                {

                     type = Type.GetType("HandyControlUI控件使用.Views.UseCamera1");
                    //这里他去创建了一个UI对象...
               
                }
                if (item.Header.ToString().Equals("摄像头2", StringComparison.Ordinal))
                {
                     type = Type.GetType("HandyControlUI控件使用.Views.UserCamer2");
                    //这里他去创建了一个UI对象...
                    //mainViewModel.MainContent = (System.Windows.UIElement)Activator.CreateInstance(type);
                }

                /*
                if (item.Header.ToString().Equals("摄像头3", StringComparison.Ordinal))
                {
                    type = Type.GetType("HandyControlUI控件使用.Views.UserCamer2");

                    HandyControl.Controls.Growl.Warning("消息通知");
                    //HandyControl.Controls.Growl.Success("消息通知");
                }
                */
                mainViewModel.MainContent = (System.Windows.UIElement)Activator.CreateInstance(type);

            }
        }

      
    }
}