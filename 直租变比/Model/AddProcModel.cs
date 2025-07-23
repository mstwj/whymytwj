using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using 直租变比.Base;
using 直租变比.Table;

namespace 直租变比.Model
{
    public  class AddProcModel : ObservableValidator
    {

        public List<string> ProductNumberCBoxData { get; set; } = new List<string>();

        private string productNumber;

        [Required(ErrorMessage = "必须输入产品编号")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(15, ErrorMessage = "最大不能超过15个字符")]
        public string ProductNumber { get => productNumber; set { SetProperty(ref productNumber, value,true); } }


        private string productType;

        [Required(ErrorMessage = "必须输入产品类型")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(15, ErrorMessage = "最大不能超过15个字符")]
        public string ProductType { get => productType; set { SetProperty(ref productType, value, true); } }


        private float productTuhao;
        [MyValidateAttribute(250, 0,ErrorMessage ="图号最大250,最小1")]
        public float ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value, true); } }


        private float productCapacity;
        [MyValidateAttribute(100000, 10, ErrorMessage = "图号最大10000,最小10")]
        public float ProductCapacity { get => productCapacity; set { SetProperty(ref productCapacity, value, true); } }


        private float ratedVoltage;
        [MyValidateAttribute(10000, 0, ErrorMessage = "额定电压10000,最小0")]
        public float RatedVoltage { get => ratedVoltage; set { SetProperty(ref ratedVoltage, value, true); } }

        //如果text绑定 -- 那就是TEXT绑定为主...
        private string phaseNumber = "三相";
        public string PhaseNumber { get => phaseNumber; set { SetProperty(ref phaseNumber, value, true); } }
        

        public ICommand BtnSetOver { get; set; }

        public AddProcModel()
        {
            BtnSetOver = new RelayCommand<object>(DoBtnSetOver);


            using (var context = new MyDbContext())
            {
                try
                {
                    // 获取单个实体（第一个匹配）                    
                    var listNames = context.ProductInfo.ToList();
                    foreach (var proname in listNames)
                    {
                        //刷新..
                        ProductNumberCBoxData.Add(proname.ProductNumber);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    MessageBox.Show(message);
                }
            }
        }

        private void DoBtnSetOver(object param)
        {
            //ValidateProperty(Stringmytest, "Stringmytest");
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
                        var firstEntity = context.ProductInfo.FirstOrDefault(e => e.ProductNumber == ProductNumber);

                        // 更新已有数据
                        if (firstEntity != null)
                        {
                            MainWindow.Current_ProductNumber = firstEntity.ProductNumber;
                            MessageBox.Show("产品选定完成!"); 
                            return;
                        }
                        else
                        {
                            Table_ProductInfo table_ProductInfo = new Table_ProductInfo();
                            table_ProductInfo.ProductNumber = ProductNumber;
                            table_ProductInfo.ProductType = ProductType;
                            table_ProductInfo.ProductTuhao = ProductTuhao.ToString();
                            table_ProductInfo.ProductCapacity = ProductCapacity.ToString();
                            table_ProductInfo.RatedVoltage = RatedVoltage.ToString();
                            table_ProductInfo.PhaseNumber = PhaseNumber;

                            context.ProductInfo.Add(table_ProductInfo);
                            int rowsAffected = context.SaveChanges();
                            if (rowsAffected > 0)
                            {
                                // 数据添加成功6
                                MessageBox.Show("产品选定完成!");
                                MainWindow.Current_ProductNumber = table_ProductInfo.ProductNumber;                                
                            }
                            else
                            {
                                MessageBox.Show("产品选定失败-添加数据失败!");
                                return;
                            }
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
