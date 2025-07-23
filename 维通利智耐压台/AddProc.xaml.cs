using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using 维通利智耐压台;
using 维通利智耐压台.Model;


namespace 维通利智耐压台
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddProc : Window
    {       
        public AddProcModel model = new AddProcModel();
        
        public AddProc()
        {
            InitializeComponent();
            this.DataContext = model;
        }

        public async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //下拉关闭时候调用..
        private void BoxDropDownClose(object sender, EventArgs e)
        {
            int selecti = myCombox.SelectedIndex;
            if (selecti != -1)
            {
                model.ProductName= model.tempData[selecti].ProductName;
                model.ProductNumber = model.tempData[selecti].ProductNumber;
                model.ProductType = model.tempData[selecti].ProductType;
                model.ProductTuhao = model.tempData[selecti].ProductTuhao;
                model.ProductParts = model.tempData[selecti].ProductParts;
                model.ProductTestVotil = model.tempData[selecti].ProductTestVotil;
                model.ProductTestPartial = model.tempData[selecti].ProductTestPartial;
            }

        }

    }
}
