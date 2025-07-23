using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.Base;
using net5_10_14.ViewModel;
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

namespace net5_10_14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pagebb pagebb = new Pagebb();
        Pagezz pagezz = new Pagezz();

        WaitInitWindow initdialog = new WaitInitWindow();

        

        public MainWindow()
        {
            InitializeComponent();

            //initdialog.Show();
            initdialog.Visibility = Visibility.Hidden;

            ActionStack.Register("ShowDialogdialogYanPing", ShowDialogdialogYanPing); //样品
            ActionStack.Register("ShowDialogdialogBianbiReport", ShowDialogdialogBianbiReport); //变比报告
            ActionStack.Register("ShowDialogdialogBianbiReport2", ShowDialogdialogBianbiReport2); //变比报告
            ActionStack.Register("ShowDialogdialogBianbiBiaoReport", ShowDialogdialogBianbiBiaoReport); //变比报告标准表

            ActionStack.Register("ShowDialogdialogZhizuReport", ShowDialogdialogZhizuReport); //变比报告
            ActionStack.Register("ShowDialogdialogZhizuReport2", ShowDialogdialogZhizuReport2); //变比报告
            ActionStack.Register("ShowDialogdialogZhizuBiaoReport", ShowDialogdialogZhizuBiaoReport); //变比报告告标准表


            ActionStack.Register("ShowDialogdialogDevelWindows", ShowDialogdialogDevelWindows); //设备窗口.
            ActionStack.Register("ShowDialogdialogYanPingShowWindows", ShowDialogdialogYanPingShowWindows); //设备窗口.
        }

        public bool ShowDialogdialogYanPing(object param) { YanPingWindow dialog = new YanPingWindow(); return dialog.ShowDialog() == true; }
        public bool ShowDialogdialogBianbiReport(object param) { BianbiReportWindow dialog = new BianbiReportWindow(); return dialog.ShowDialog() == true; }
        public bool ShowDialogdialogBianbiReport2(object param) { BianbiReportWindow2 dialog = new BianbiReportWindow2(); return dialog.ShowDialog() == true; }
        public bool ShowDialogdialogBianbiBiaoReport(object param) { BianbiReportBiaoWindow dialog = new BianbiReportBiaoWindow(); return dialog.ShowDialog() == true; }

        public bool ShowDialogdialogZhizuReport(object param) { ZhiLiuReportWindow dialog = new ZhiLiuReportWindow(); return dialog.ShowDialog() == true; }

        public bool ShowDialogdialogZhizuReport2(object param) { ZhiLiuReportWindow2 dialog = new ZhiLiuReportWindow2(); return dialog.ShowDialog() == true; }

        public bool ShowDialogdialogZhizuBiaoReport(object param) { ZhiLiuBiaoWindow dialog = new ZhiLiuBiaoWindow(); return dialog.ShowDialog() == true; }


        public bool ShowDialogdialogDevelWindows(object param) { DevedWindow dialog = new DevedWindow(); return dialog.ShowDialog() == true; }
        public bool ShowDialogdialogYanPingShowWindows(object param) { YanpingshowWindow dialog = new YanpingshowWindow(); return dialog.ShowDialog() == true; }


        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            FrameworkElement content = FrameNv.Content as FrameworkElement;
            if (content != null)
            {
                FrameNv.Width = 1024;
                FrameNv.Height = 680;
            }
        }

        bool pagebbisinit = false;
        private async void DoBtnMenu2_1Click(object sender, RoutedEventArgs e)
        {
            if (App_Config.currendProName == null) 
            {
                MessageBox.Show("必须先配置样品");
                return; 
            }
            this.myImage.Visibility = Visibility.Hidden;

            if (pagebbisinit == false)
            {
                bool result = await pagebb.PagebbInitializing();
                if (!result) return;
                pagebbisinit = true;
            }

            pagebb.PagebbSwitch();
            FrameNv.Navigate(pagebb);
        }


        bool pagezzisinit = false;
        private async void DoBtnMenu2_2Click(object sender, RoutedEventArgs e)
        {
            if (App_Config.currendProName == null) { MessageBox.Show("必须先配置样品"); return; }
            this.myImage.Visibility = Visibility.Hidden;
            
            if (pagezzisinit == false)
            {
                bool result = await pagezz.PagezzInitializing();
                if (!result) return;
                pagezzisinit = true;
            }

            FrameNv.Navigate(pagezz);
        }

        private void MainClose(object sender, EventArgs e)
        {
            initdialog.Close();
        }
    }
}
