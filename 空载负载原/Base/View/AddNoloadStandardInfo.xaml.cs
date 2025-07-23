using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using 空载_负载.Base;
using 空载_负载.Base.Table;


namespace 空载_负载
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddNoloadStandardInfo : Window
    {
        //这里好像只有GET 没有SET... -- 这里也只能 初始化第一次给值..
        public string ProductType { get; set; } = "SB/0001/0.5";
        public string ProductTuhao { get; set; } = "Y-12";
        public string LossStandard { get; set; } = "5";
        public string LossStandardUp { get; set; } = "10";
        public string LossStandardDown { get; set; } = "10";
        public string NoloadCurrentStandard { get; set; } = "10";
        public string NoloadCurrentStandardUp { get; set; } = "10";
        public string NoloadCurrentStandardDown { get; set; } = "10";
        public string ImpedanceStandard { get; set; } = "0.5";

        private List<object> myControls = new List<object>();
        public AddNoloadStandardInfo()
        {
            InitializeComponent();

            this.DataContext = this;


            myControls.Add(ProductTypeBox);
            myControls.Add(ProductTuhaoBox);
            myControls.Add(LossStandardBox);
            myControls.Add(LossStandardUpBox);
            myControls.Add(LossStandardDownBox);
            myControls.Add(NoloadCurrentStandardBox);
            myControls.Add(NoloadCurrentStandardUpBox);
            myControls.Add(NoloadCurrentStandardDownBox);
            myControls.Add(ImpedanceStandardBox);



        }

        private async void ClickCommand1(object sender, RoutedEventArgs e)
        {
            foreach (var control in myControls) // myControls是你要检查的控件集合
            {
                if (Validation.GetHasError((DependencyObject)control))
                {
                    MessageBox.Show("数据验证错误!");
                    return;
                }
            }

            if (Validation.GetHasError(ProductTypeBox))
            {
                MessageBox.Show("数据验证错误!");
                return;
            }


            try
            {
                using (var context = new MyDbContext())
                {
                    var firstEntity = context.NoloadStandardInfo.FirstOrDefault(e => e.ProductType == ProductType && e.ProductTuhao == ProductTuhao);
                    if (firstEntity != null)
                    {
                        MessageBox.Show("标准库已经有了这个标准");
                        return ;
                    }                    
                    Table_NoloadStandardInfo table_StandardInfo = new Table_NoloadStandardInfo();
                    //产品型号
                    table_StandardInfo.ProductType = ProductType;
                    table_StandardInfo.ProductTuhao = ProductTuhao; 
                    table_StandardInfo.LossStandard = LossStandard;
                    table_StandardInfo.LossStandardUp = LossStandardUp;
                    table_StandardInfo.LossStandardDown = LossStandardDown;
                    table_StandardInfo.NoloadCurrentStandard = NoloadCurrentStandard;
                    table_StandardInfo.NoloadCurrentStandardUp = NoloadCurrentStandardUp;
                    table_StandardInfo.NoloadCurrentStandardDown = NoloadCurrentStandardDown;                    

                    context.NoloadStandardInfo.Add(table_StandardInfo);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功6
                        MessageBox.Show("空载标准添加成功!");                        
                    }
                    else
                    {
                        MessageBox.Show("空载标准添加失败!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //图号
        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
