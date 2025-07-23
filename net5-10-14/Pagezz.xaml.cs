using Microsoft.Toolkit.Mvvm.Messaging;
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
    /// Pagezz.xaml 的交互逻辑
    /// </summary>
    public partial class Pagezz : Page
    {
        private PagezzViewModel myPagezzViewModel;

        public Pagezz()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<string, string>(this, "ZZShowProc", ZZShowProc);
            myPagezzViewModel = new PagezzViewModel();
            this.DataContext = myPagezzViewModel;
        }

        public async Task<bool> PagezzInitializing()
        {
            dataGrid.ItemsSource = myPagezzViewModel.DataRecordList;
            return await myPagezzViewModel.PagezzViewModeinit();
        }

        ~Pagezz()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        private void ZZShowProc(object recipient, string message)
        {
            if (myPagezzViewModel.m_newproinfo != null)
            {
                TB1.Text = myPagezzViewModel.m_newproinfo.ProName;
                TB2.Text = myPagezzViewModel.m_newproinfo.GuigeXinhao;
                TB3.Text = myPagezzViewModel.m_newproinfo.Tuhao.ToString();
                TB4.Text = myPagezzViewModel.m_newproinfo.BiaoHao;
                TB5.Text = myPagezzViewModel.m_newproinfo.GaoyaedingDianya.ToString();
                TB6.Text = myPagezzViewModel.m_newproinfo.EdingRonglang.ToString();
                TB7.Text = myPagezzViewModel.m_newproinfo.DiyaedingDianya.ToString();
                TB8.Text = myPagezzViewModel.m_newproinfo.DanWeiNumber.ToString();
            }
            if (myPagezzViewModel.m_experimentstandard_ziliudianzushiyan != null)
            {
                TB9.Text = myPagezzViewModel.m_experimentstandard_ziliudianzushiyan.Guigexinhao;
                TB10.Text = myPagezzViewModel.m_experimentstandard_ziliudianzushiyan.Tuhao.ToString();
                TB11.Text = myPagezzViewModel.m_experimentstandard_ziliudianzushiyan.Xiandianzhupinghen.ToString();
                TB12.Text = myPagezzViewModel.m_experimentstandard_ziliudianzushiyan.Xdianzhupinghen.ToString();
                TB13.Text = myPagezzViewModel.m_experimentstandard_ziliudianzushiyan.Createuser.ToString();
                TB14.Text = myPagezzViewModel.m_experimentstandard_ziliudianzushiyan.Ccreateuserdate.ToString();
            }
            return;
        }

    }
}
