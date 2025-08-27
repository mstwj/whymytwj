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
        }

        private void StartLoading_Click(object sender, RoutedEventArgs e)
        {
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

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            //HandyControl.Controls.Dialog.Show("提交成功");

        }
    }
}