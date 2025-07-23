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

using 空载负载;
using 空载负载.Base;
using 空载负载.Model;
using 空载负载.Table;
using 空载负载.View;

namespace 空载负载
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    

        MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;

            /*
                UserLogin userLogin = new UserLogin();
                bool? mylog = userLogin.ShowDialog();
                if (mylog == true) return;            
                else this.Close();
            */
            
        }


        private async void  DoBtnMenu1_1Click(object sender, RoutedEventArgs e)
        {
            AddProc addProc = new AddProc();            
            addProc.ShowDialog();
            if (addProc.model.State == true)
            {

                //这里要判断，输入对不对...
                viewModel.m_sampleinformation.ProductNumber = addProc.model.ProductNumber;
                viewModel.m_sampleinformation.ProductType = addProc.model.ProductType;
                viewModel.m_sampleinformation.ProductTuhao = addProc.model.ProductTuhao;
                viewModel.m_sampleinformation.ProductCapacity = addProc.model.ProductCapacity;
                viewModel.m_sampleinformation.Highpressure = addProc.model.Highpressure;
                viewModel.m_sampleinformation.Highcurrent = addProc.model.Highcurrent;
                viewModel.m_sampleinformation.Lowpressure = addProc.model.Lowpressure;
                viewModel.m_sampleinformation.Lowcurrent = addProc.model.Lowcurrent;
                viewModel.m_sampleinformation.PhaseNumber = addProc.model.PhaseNumber;


                if (viewModel.ContentView != null)
                    viewModel.DoInit(viewModel.ContentView.GetType().Name);
                
                
            }
        }

        private async void DoBtnMenu1_2Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void DoBtnMenu2_1Click(object sender, RoutedEventArgs e)
        {           

            if (viewModel.m_sampleinformation.ProductNumber == null)
            {
                MessageBox.Show("必须先配置产品!");
                return;
            }
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_NoloadStandardInfo firstEntity = context.NoloadStandardInfo.FirstOrDefault(e => e.ProductType == viewModel.m_sampleinformation.ProductType && e.ProductTuhao == viewModel.m_sampleinformation.ProductTuhao);
                    if (firstEntity == null)
                    {
                        MessageBox.Show("配置产品,在空载标准库里,没有找到这个标准!");
                        return;
                    }
                    viewModel.m_sampleinNoload.ProductType = firstEntity.ProductType;
                    viewModel.m_sampleinNoload.ProductTuhao = firstEntity.ProductTuhao;
                    viewModel.m_sampleinNoload.ProductStandard = firstEntity.ProductStandard;
                    viewModel.m_sampleinNoload.ProductStandardUpperimit = firstEntity.ProductStandardUpperimit;
                    viewModel.m_sampleinNoload.ProductStandardDownimit = firstEntity.ProductStandardDownimit;
                    viewModel.m_sampleinNoload.ProductCurrentStandard = firstEntity.ProductCurrentStandard;
                    viewModel.m_sampleinNoload.ProductCurrentStandardUpperrimit = firstEntity.ProductCurrentStandardUpperrimit;
                    viewModel.m_sampleinNoload.ProductCurrentStandardDownimit = firstEntity.ProductCurrentStandardDownimit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            //生成主题..
            if (viewModel.DoBtnCommandStart("NoloadPage") == true)
            {
                ((NoloadPage)viewModel.ContentView).model.Initiate(viewModel.m_sampleinformation, viewModel.m_sampleinNoload);
                ((NoloadPage)viewModel.ContentView).model.InitializationManager();
            }
            


        }

        private async void DoBtnMenu2_2Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.m_sampleinformation.ProductNumber == null)
            {
                MessageBox.Show("必须先配置产品!");
                return;
            }
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_LoadStandardInfo firstEntity = context.LoadStandardInfo.FirstOrDefault(e => e.ProductType == viewModel.m_sampleinformation.ProductType && e.ProductTuhao == viewModel.m_sampleinformation.ProductTuhao);
                    if (firstEntity == null)
                    {
                        MessageBox.Show("配置产品,在负载标准库里,没有找到这个标准!");
                        return;
                    }
                    viewModel.m_sampleinLoad.ProductType = firstEntity.ProductType;
                    viewModel.m_sampleinLoad.ProductTuhao = firstEntity.ProductTuhao;
                    viewModel.m_sampleinLoad.Loadloss = firstEntity.Loadloss;
                    viewModel.m_sampleinLoad.LoadlossUpperimit = firstEntity.LoadlossUpperimit;
                    viewModel.m_sampleinLoad.LoadlossDownimit = firstEntity.LoadlossDownimit;
                    viewModel.m_sampleinLoad.LoadMainReactance = firstEntity.LoadMainReactance;
                    viewModel.m_sampleinLoad.LoadMainReactanceUpperrimit = firstEntity.LoadMainReactanceUpperrimit;
                    viewModel.m_sampleinLoad.LoadMainReactanceDownimit = firstEntity.LoadMainReactanceDownimit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (viewModel.DoBtnCommandStart("LoadPage") == true)
            {
                ((LoadPage)viewModel.ContentView).model.Initiate(viewModel.m_sampleinformation, viewModel.m_sampleinLoad);
                ((LoadPage)viewModel.ContentView).model.InitializationManager();
            }



        }

        private async void DoBtnMenu2_3Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.m_sampleinformation.ProductNumber == null)
            {
                MessageBox.Show("必须先配置产品!");
                return;
            }
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_InductionStandardInfo firstEntity = context.InductionStandardInfo.FirstOrDefault(e => e.ProductType == viewModel.m_sampleinformation.ProductType && e.ProductTuhao == viewModel.m_sampleinformation.ProductTuhao);
                    if (firstEntity == null)
                    {
                        MessageBox.Show("配置产品,在感应标准库里,没有找到这个标准!");
                        return;
                    }
                    viewModel.m_sampleinInduction.ProductType = firstEntity.ProductType;
                    viewModel.m_sampleinInduction.ProductTuhao = firstEntity.ProductTuhao;
                    viewModel.m_sampleinInduction.Voltage = firstEntity.Voltage;
                    viewModel.m_sampleinInduction.Frequency = firstEntity.Frequency;
                    viewModel.m_sampleinInduction.Times = firstEntity.Times;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (viewModel.DoBtnCommandStart("InductionPage") == true)
            {
                //感应试验..
                ((InductionPage)viewModel.ContentView).model.Initiate(viewModel.m_sampleinformation, viewModel.m_sampleinInduction);
                ((InductionPage)viewModel.ContentView).model.InitializationManager();
            }

        }

        private async void DoBtnMenu2_4Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.m_sampleinformation.ProductNumber == null)
            {
                MessageBox.Show("必须先配置产品!");
                return;
            }
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_UtilityStandardInfo firstEntity = context.UtilityStandardInfo.FirstOrDefault(e => e.ProductType == viewModel.m_sampleinformation.ProductType && e.ProductTuhao == viewModel.m_sampleinformation.ProductTuhao);
                    if (firstEntity == null)
                    {
                        MessageBox.Show("配置产品,在工频耐压标准库里,没有找到这个标准!");
                        return;
                    }
                    viewModel.m_sampleinUtitiy.ProductType = firstEntity.ProductType;
                    viewModel.m_sampleinUtitiy.ProductTuhao = firstEntity.ProductTuhao;
                    viewModel.m_sampleinUtitiy.Voltage = firstEntity.Voltage;
                    viewModel.m_sampleinUtitiy.Frequency = firstEntity.Frequency;
                    viewModel.m_sampleinUtitiy.Times = firstEntity.Times;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            
            if (viewModel.DoBtnCommandStart("UtilityPage") == true)
            {
                
                //空载试验..
                ((UtilityPage)viewModel.ContentView).model.Initiate(viewModel.m_sampleinformation, viewModel.m_sampleinUtitiy);
                ((UtilityPage)viewModel.ContentView).model.InitializationManager();
            }


        }

        private async void DoBtnMenu3_1Click(object sender, RoutedEventArgs e)
        {
            DeveclWindow deveclWindow = new DeveclWindow();
            deveclWindow.ShowDialog();
        }




        private async void DoBtnMenu4_1Click(object sender, RoutedEventArgs e)
        {
            //这里本质就是模版表..
            TableProductInfo tableProductInfo = new TableProductInfo();
            tableProductInfo.ShowDialog();
        }

        private async void DoBtnMenu4_2Click(object sender, RoutedEventArgs e)
        {
            //空载记录表.
            TableRecordInfo tableRecordInfo = new TableRecordInfo();
            tableRecordInfo.ShowDialog();

        }

        private async void DoBtnMenu4_3Click(object sender, RoutedEventArgs e)
        {
            //空载标准表../
            TableStandardInfo tableStandardInfo = new TableStandardInfo();
            tableStandardInfo.ShowDialog();
        }


        private async void DoBtnMenu4_4Click(object sender, RoutedEventArgs e)
        {
            //负载记录表..
            TableLoadRecordInfo tableStandardInfo = new TableLoadRecordInfo();
            tableStandardInfo.ShowDialog();

        }

        private async void DoBtnMenu4_5Click(object sender, RoutedEventArgs e)
        {
            //负载标准表...
            TableLoadStandardInfo tableStandardInfo = new TableLoadStandardInfo();
            tableStandardInfo.ShowDialog();

        }

        private async void DoBtnMenu4_6Click(object sender, RoutedEventArgs e)
        {

            //感应记录表
            TableInductionRecordDialog tableInductionRecordDialog = new TableInductionRecordDialog();
            tableInductionRecordDialog.ShowDialog();

        }

        private async void DoBtnMenu4_7Click(object sender, RoutedEventArgs e)
        {
            //感应标准表
            TableInductionStandardDialog table = new TableInductionStandardDialog();
            table.ShowDialog();
        }

        private async void DoBtnMenu4_8Click(object sender, RoutedEventArgs e)
        {
            //工频记录表 
            TableUtilityRecordDialog table = new TableUtilityRecordDialog();
            table.ShowDialog();
        }

        private async void DoBtnMenu4_9Click(object sender, RoutedEventArgs e)
        {
            //工频标准表
            TableUtilityStandardDialog table = new TableUtilityStandardDialog();
            table.ShowDialog();
        }



    }
}
