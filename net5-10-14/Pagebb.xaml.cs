using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.ViewModel;
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

namespace net5_10_14
{
    /// <summary>
    /// Pagebb.xaml 的交互逻辑
    /// </summary>
    public partial class Pagebb : Page
    {
        private PagebbViewModel myPagebbViewModel;
        public Pagebb()
        {
            // 注册消息..(必须在前面..)
            WeakReferenceMessenger.Default.Register<string, string>(this, "BBShowProc", BBShowProc);
            InitializeComponent();

            myPagebbViewModel = new PagebbViewModel();
            this.DataContext = myPagebbViewModel;
        }

        //这里必须出示..
        public async Task<bool> PagebbInitializing()
        {            
            dataGrid.ItemsSource = myPagebbViewModel.DataRecordList;
            return await myPagebbViewModel.PagebbViewModeinit();
        }

        public bool PagebbSwitch()
        {
            return myPagebbViewModel.PagebbPlcSwitch();
        }

        ~Pagebb()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        private void BBShowProc(object recipient, string message)
        {
            if (myPagebbViewModel.m_newproinfo != null)
            {
                TB1.Text = myPagebbViewModel.m_newproinfo.ProName;
                TB2.Text = myPagebbViewModel.m_newproinfo.GuigeXinhao;
                TB3.Text = myPagebbViewModel.m_newproinfo.Tuhao.ToString();
                TB4.Text = myPagebbViewModel.m_newproinfo.BiaoHao;
                TB5.Text = myPagebbViewModel.m_newproinfo.GaoyaedingDianya.ToString();
                TB6.Text = myPagebbViewModel.m_newproinfo.EdingRonglang.ToString();
                TB7.Text = myPagebbViewModel.m_newproinfo.DiyaedingDianya.ToString();
                TB8.Text = myPagebbViewModel.m_newproinfo.DanWeiNumber.ToString();
            }
            if (myPagebbViewModel.m_experimentstandard_bianbishiyan != null)
            {
                TB9.Text = myPagebbViewModel.m_experimentstandard_bianbishiyan.Guigexinhao;
                TB10.Text = myPagebbViewModel.m_experimentstandard_bianbishiyan.Tuhao.ToString();
                TB11.Text = myPagebbViewModel.m_experimentstandard_bianbishiyan.Bianbiwucha.ToString();
                TB12.Text = myPagebbViewModel.m_experimentstandard_bianbishiyan.Createuser;
                TB13.Text = myPagebbViewModel.m_experimentstandard_bianbishiyan.Ccreateuserdate;
            }
            return;
        }

        private void dataGrid_Selected(object sender, RoutedEventArgs e)
        {
            DataRecord p = (DataRecord)dataGrid.SelectedItem;
            //为什么这里会空....
            if (p != null)
            {
                TBWZ.Text = p.Id.ToString();
            }
            
        }
    }
}
