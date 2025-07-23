using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using 空载负载;
using 空载负载.Base;
using 空载负载.Model;
using 空载负载.Table;


namespace 空载负载
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddProc : Window
    {       
        public AddProcModel model { get; set; } = new AddProcModel();
        

        public AddProc()
        {
            InitializeComponent();

            this.DataContext = model;

        }


        //private async void ClickCommand1(object sender, RoutedEventArgs e)
        //{
            //MessageBox.Show("配置完成!");

            //this.Close();
        //}


        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //下拉关闭时候调用..
        private void BoxDropDownClose(object sender, EventArgs e)
        {

            //这里选择后已经有值了..           
            using (var context = new MyDbContext())
            {
                var firstEntity = context.Template.FirstOrDefault(e => e.ProductType == model.ProductType);
                if (firstEntity != null)
                {
                    //注意这里不是比较，是赋值了..(有数据就是修改了.)
                    model.ProductType = firstEntity.ProductType;
                    model.ProductTuhao = firstEntity.ProductTuhao;
                    model.ProductCapacity = firstEntity.ProductCapacity;
                    model.Highpressure = firstEntity.Highpressure;
                    model.Highcurrent = firstEntity.Highcurrent;
                    model.Lowpressure = firstEntity.Lowpressure;
                    model.Lowcurrent = firstEntity.Lowcurrent;
                    model.PhaseNumber = firstEntity.PhaseNumber;
                }
            }        

        }

    }
}
