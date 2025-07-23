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
using CommunityToolkit.Mvvm.Messaging;
using 维通利智耐压台.Base;
using 维通利智耐压台.Model;

namespace 维通利智耐压台
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        public MainViewModel model { get; set; } = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = model;
            model.userMainControlModel = userMainControl.model;
            model.userTestControlModel = userTesetControl.model;

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, user) =>
            {
                if (user.Message == "DisAutoQueue")
                {
                    //自动..
                    BtnCommandElectric.IsEnabled = false;
                    BtnCommandUp.IsEnabled = false;
                    BtnCommandDown.IsEnabled = false;
                    BtnCommandClose.IsEnabled = false;
                    BtnCommandOpen.IsEnabled = false;
                    BtnCommandTimer.IsEnabled = true;
                    BtnCommandStopTime.IsEnabled = true;                    
                }
                if (user.Message == "DisMutiQueue")
                {
                    //手动..
                    BtnCommandElectric.IsEnabled = true;
                    BtnCommandUp.IsEnabled = true;
                    BtnCommandDown.IsEnabled = true;
                    BtnCommandClose.IsEnabled = true;
                    BtnCommandOpen.IsEnabled = true;
                    BtnCommandTimer.IsEnabled = false;
                    BtnCommandStopTime.IsEnabled = false;
                }

                if (user.Message == "GetChart")
                {
                    user.obj1 = Chart;
                }

                if (user.Message == "UpdataJuFan")
                {
                    //每1秒来一下...
                    model.userMainControlModel.PartialNumber = user.fobj1;
                    model.userMainControlModel.HighVoltage = user.fobj2;
                }
            });

            /*
            UserLogin userLogin = new UserLogin();
            bool? mylog = userLogin.ShowDialog();
            if (mylog == true) return;
            else this.Close();
            */
        }

        private async void DoBtnMenu1_1Click(object sender, RoutedEventArgs e)
        {
            AddProc addProc = new AddProc();
            addProc.ShowDialog();
            model.SetCurrentUserInfo(addProc.model.SelectProduct);
        }

        private async void DoBtnMenu1_2Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void DoBtnMenu2_1Click(object sender, RoutedEventArgs e)
        {
            DeveclWindow deveclWindow = new DeveclWindow();
            deveclWindow.ShowDialog();

        }

        private async void DoBtnMenu2_2Click(object sender, RoutedEventArgs e)
        {
            ParamWindow deveclWindow = new ParamWindow();
            deveclWindow.ShowDialog();

        }

        private async void DoBtnMenu3_1Click(object sender, RoutedEventArgs e)
        {

            TableProductInfo tableProductInfo = new TableProductInfo();
            tableProductInfo.ShowDialog();
            
        }

        private async void DoBtnMenu3_2Click(object sender, RoutedEventArgs e)
        {
            TableRecordInfo tableRecordInfo = new TableRecordInfo();
            tableRecordInfo.ShowDialog();
        }

        private void BtnCommandTimer_Click(object sender, RoutedEventArgs e)
        {
           
            //BtnCommandTimer.IsEnabled = false;
            //BtnCommandStopTime.IsEnabled = true;
        }

        private void BtnCommandStopTime_Click(object sender, RoutedEventArgs e)
        {
            //BtnCommandTimer.IsEnabled = true;
            //BtnCommandStopTime.IsEnabled = false;
        }
    }
}
