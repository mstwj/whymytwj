using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using 铁芯实验;
using 铁芯实验.Base;
using 铁芯实验.Model;
using 铁芯实验.Table;


namespace 铁芯实验
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddProc : Window
    {       
        AddProcModel model = new AddProcModel();
        

        public AddProc()
        {
            InitializeComponent();

            this.DataContext = model;

        }

        //额定电压
        /*
        private void TextBox_PreviewTextInputVoltage(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            //只能输入数字.
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        //容量
        
        private void TextBox_PreviewTextInputCapacity(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            //匹配只能输入一个小数点的浮点数
            Regex numbeRegex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled =
                !numbeRegex.IsMatch(
                    ProductCapacityTBox.Text.Insert(
                        ProductCapacityTBox.SelectionStart, e.Text));
            ProductCapacityTBox.Text = ProductCapacityTBox.Text.Trim();

        }

        //图号
        private void TextBox_PreviewTextInputTuhao(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            //只能输入数字.
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }
        */
        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //下拉关闭时候调用..
        private void BoxDropDownClose(object sender, EventArgs e)
        {
            
            //这里选择后已经有值了..
            /*
            using (var context = new MyDbContext())
            {
                var firstEntity = context.ProductInfo.FirstOrDefault(e => e.ProductNumber == model.ProductNumber);
                if (firstEntity != null)
                {
                    //注意这里不是比较，是赋值了..(有数据就是修改了.)
                    model.ProductNumber = firstEntity.ProductNumber;
                    model.ProductType = firstEntity.ProductType;
                    model.ProductTuhao = firstEntity.ProductTuhao;
                    model.ProductCapacity = firstEntity.ProductCapacity;
                    model.RatedVoltage = firstEntity.RatedVoltage; 
                    model.PhaseNumber = firstEntity.PhaseNumber;
                }
            }
            */
            
        }

    }
}
