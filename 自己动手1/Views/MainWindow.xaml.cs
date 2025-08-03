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
using 自己动手1.Base;
using 自己动手1.ViewModels;
using 自己动手1.Views.Dialog;

namespace 自己动手1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //页面和 Mainviewmodel 关联了..
            this.DataContext = new MainViewModel();
       
            //启动就直接注册死了..
            ActionManager.Register("PBOM",DoAction);

            ActionManager.Register("PBOM-F", DoFunc);

        }

        private void DoAction(object data)
        {
            //这里的OWNER=THIS，就是切换的时候，如果点下面的窗口.
            //主窗口和打开的子窗口就都出来了..
            new PBomEditWin() 
            {
                Owner = this,
                DataContext = data
            }.ShowDialog();
        }

        private bool DoFunc(object data)
        {
            return new PBomEditWin()
            {
                Owner = this,
                DataContext = data
            }.ShowDialog() == true;
        }
    }
}