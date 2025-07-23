using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using 铁芯实验;
using 铁芯实验.Base;
using 铁芯实验.Model;
using 铁芯实验.View;

namespace 铁芯实验
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Current_ProductNumber { get; set; } = null;
        public static string Current_StandardType { get; set; } = null;
        public static string Current_StandardTuhao { get; set; } = null;

        MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainViewModel(
                UserMainControl.model,
                UserRecordControl.model,
                UserInfoControl.model,
                UserStandardControl.model,
                PowerStateControl.model,     
                UserPowerSetControl.model,
                UserTestControl.model,
                UserCommunicationControl.model                
                );
            this.DataContext = viewModel;

            UserLogin userLogin = new UserLogin();
            bool? mylog = userLogin.ShowDialog();
            if (mylog == true) return;            
            else this.Close();
            
        }


        private async void  DoBtnMenu1_1Click(object sender, RoutedEventArgs e)
        {
            AddProc addProc = new AddProc();            
            addProc.ShowDialog();
            viewModel.SetCurrentUserInfo();
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
            BaseParamSet baseParamSet = new BaseParamSet();
            baseParamSet.ShowDialog();
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

        private async void DoBtnMenu3_3Click(object sender, RoutedEventArgs e)
        {
            TableStandardInfo tableStandardInfo = new TableStandardInfo();
            tableStandardInfo.ShowDialog();
        }

        private void UserPowerSetControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserTestControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
