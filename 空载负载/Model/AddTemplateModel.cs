using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using 空载负载.Base;
using 空载负载.Table;

namespace 空载负载.Model
{
    public class AddTemplateModel : ObservableValidator
    {

        private string productType;

        [Required(ErrorMessage = "必须输入规格类型")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(35, ErrorMessage = "最大不能超过35个字符")]
        public string ProductType { get => productType; set { SetProperty(ref productType, value, true); } }


        private string productTuhao;
        [Required(ErrorMessage = "必须输入图号")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(25, ErrorMessage = "最大不能超过25个字符")]
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value, true); } }


        private string productCapacity;
        [Required(ErrorMessage = "必须输入额定容量")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        public string ProductCapacity { get => productCapacity; set { SetProperty(ref productCapacity, value, true); } }


        private string highpressure;
        [Required(ErrorMessage = "必须输入电压")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        public string Highpressure { get => highpressure; set { SetProperty(ref highpressure, value, true); } }

        private string highcurrent;
        public string Highcurrent { get => highcurrent; set { SetProperty(ref highcurrent, value, true); } }

        private string lowpressure;
        public string Lowpressure { get => lowpressure; set { SetProperty(ref lowpressure, value, true); } }

        private string lowcurrent;
        public string Lowcurrent { get => lowcurrent; set { SetProperty(ref lowcurrent, value, true); } }


        //如果text绑定 -- 那就是TEXT绑定为主...
        private string phaseNumber = "三相";
        public string PhaseNumber { get => phaseNumber; set { SetProperty(ref phaseNumber, value, true); } }






        public ICommand BtnSetOver { get; set; }

        public AddTemplateModel()
        {
            BtnSetOver = new RelayCommand<object>(DoBtnSetOver);
        }


        private void DoBtnSetOver(object param)
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }
            else
            {
                try
                {
                    using (var context = new MyDbContext())
                    {
                        Table_Template table_ProductInfo = new Table_Template();                            
                        table_ProductInfo.ProductType = ProductType;
                        table_ProductInfo.ProductTuhao = ProductTuhao;
                        table_ProductInfo.ProductCapacity = ProductCapacity;
                        table_ProductInfo.Highpressure = Highpressure;
                        table_ProductInfo.Highcurrent = Highcurrent;
                        table_ProductInfo.Lowpressure = Lowpressure;
                        table_ProductInfo.Lowcurrent = Lowcurrent;
                        table_ProductInfo.PhaseNumber = PhaseNumber;

                        context.Template.Add(table_ProductInfo);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("添加完成!");
                        }
                        else
                        {
                            MessageBox.Show("添加数据失败!");
                            return;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

    }
}
