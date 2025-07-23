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
using System.Windows.Shapes;
using 空载_负载.Base.Table;

namespace 空载_负载.Base.View
{
    /// <summary>
    /// AddProcWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddProcWindow : Window
    {
        public string SelectBoxItem { get; set; } = "三相";

        //这里好像只有GET 没有SET... -- 这里也只能 初始化第一次给值..
        public string ProductType { get; set; } = "SB/1000/0.5/1000/100";
        public string ProductTuhao { get; set; } = "YBCSE-12";
        public string ProductCapacity { get; set; } = "10";
        public string Highpressure { get; set; } = "10";
        public string Highcurrent { get; set; } = "10";
        public string Lowpressure { get; set; } = "10";
        public string Lowcurrent { get; set; } = "10";
        public string PhaseNumber { get; set; }


        public AddProcWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //添加产品.
            try
            {
                using (var context = new MyDbContext())
                {
                    var firstEntity = context.ProductInfo.FirstOrDefault(e => e.ProductType == ProductType && e.ProductTuhao == ProductTuhao);
                    if (firstEntity != null)
                    {
                        MessageBox.Show("标准库已经有了这个标准");
                        return;
                    }
                    Table_ProductInfo productInfo = new Table_ProductInfo();
                    //产品型号
                    productInfo.ProductType = ProductType;
                    productInfo.ProductTuhao = ProductTuhao;
                    productInfo.ProductCapacity = ProductCapacity;
                    productInfo.Highpressure = Highpressure;
                    productInfo.Highcurrent = Highcurrent;
                    productInfo.Lowpressure = Lowpressure;
                    productInfo.Lowcurrent = Lowcurrent;
                    productInfo.PhaseNumber = SelectBoxItem;

                    context.ProductInfo.Add(productInfo);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功6
                        MessageBox.Show("产品添加成功!");
                    }
                    else
                    {
                        MessageBox.Show("产品添加失败!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
