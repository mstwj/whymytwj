using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using 铁芯实验.Base;
using 铁芯实验.Model;

namespace 铁芯实验
{
    /// <summary>
    /// BaseParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class BaseParamSet : Window
    {
        BaseParamModel baseParamModel = new BaseParamModel();
        public BaseParamSet()
        {
            InitializeComponent();
            this.DataContext = baseParamModel;
        }


        private async void Button_Click_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BoxDropDownClose(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem == null) return;
            string seelctstring = comboBox.SelectedItem.ToString();

            if (seelctstring != null)
            {                
                string[] parts = seelctstring.Split(new string[] { "||" }, StringSplitOptions.None);

                //这里选择后已经有值了..           
                using (var context = new MyDbContext())
                {
                    //判断2个值...
                    var firstEntity = context.StandardInfo.FirstOrDefault(e => e.ProductType == parts[0] && e.ProductTuhao == parts[1]);
                    if (firstEntity != null)
                    {
                        //注意这里不是比较，是赋值了..(有数据就是修改了.)
                        baseParamModel.ProductType = firstEntity.ProductType;
                        baseParamModel.ProductTuhao = firstEntity.ProductTuhao;
                        //baseParamModel.ProductCapacity = firstEntity.ProductCapacity;
                        baseParamModel.ProductStandard = firstEntity.ProductStandard;
                        baseParamModel.ProductCurrentStandard = firstEntity.ProductCurrentStandard;
                    }
                }
            }
        }
    }
}
