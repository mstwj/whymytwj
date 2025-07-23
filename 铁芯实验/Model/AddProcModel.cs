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
using 铁芯实验.Base;
using 铁芯实验.Table;

namespace 铁芯实验.Model
{
    public  class AddProcModel : ObservableValidator
    {
        public List<string> ProductNumberCBoxData { get; set; } = new List<string>();
        public List<string> ProductNumberCBoxData2 { get; set; } = new List<string>();

        private string productNumber;

        [Required(ErrorMessage = "必须输入出厂编号")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(15, ErrorMessage = "最大不能超过15个字符")]
        public string ProductNumber { get => productNumber; set { SetProperty(ref productNumber, value,true); } }


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
        [MyValidateAttributeString("100000", "1", ErrorMessage = "额定容量最大100000,最小1")]
        public string ProductCapacity { get => productCapacity; set { SetProperty(ref productCapacity, value, true); } }


        private string ratedVoltage;
        [Required(ErrorMessage = "必须输入电压")]
        [MinLength(0, ErrorMessage = "最小不能小于0个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        [MyValidateAttributeString("100000", "0", ErrorMessage = "额定电压最大100000,最小0")]
        public string RatedVoltage { get => ratedVoltage; set { SetProperty(ref ratedVoltage, value, true); } }

        //如果text绑定 -- 那就是TEXT绑定为主...
        private string phaseNumber = "三相";
        public string PhaseNumber { get => phaseNumber; set { SetProperty(ref phaseNumber, value, true); } }



        /// <summary>
        /// 这里比较特殊.. 名字无法修改了，我直接加个2算了..
        /// </summary>
        private string productType2 = string.Empty;
        private string productTuhao2 = string.Empty;
        private string productStandard2 = string.Empty;
        private string productStandardUpperimit2 = string.Empty;
        private string productStandardDownimit2 = string.Empty;
        private string productCurrentStandard2 = string.Empty;
        private string productCurrentStandardUpperrimit2 = string.Empty;
        private string productCurrentStandardDownimit2 = string.Empty;



        public ICommand BtnSetOver { get; set; }

        public AddProcModel()
        {
            BtnSetOver = new RelayCommand<object>(DoBtnSetOver);


            using (var context = new MyDbContext())
            {
                try
                {
                    // 获取单个实体（第一个匹配）                    
                    var listNames = context.StandardInfo.ToList();
                    
                    foreach (var proname in listNames)
                    {
                        //刷新..
                        ProductNumberCBoxData.Add(proname.ProductType);
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
                            if (ScanTypeProc(firstEntity.ProductType, firstEntity.ProductTuhao))
                            {
                                MessageBox.Show("产品选定完成!");
                                MainWindow.Current_ProductNumber = firstEntity.ProductNumber;
                                MainWindow.Current_StandardType = firstEntity.ProductType;
                                MainWindow.Current_StandardTuhao = firstEntity.ProductTuhao;
                                return;
                            }
                            else
                            {
                                MessageBox.Show("错误,没有找到与产品对应的规格型号!");
                                return;
                            }
                        }
                        else
                        {
                            Table_ProductInfo table_ProductInfo = new Table_ProductInfo();
                            table_ProductInfo.ProductNumber = ProductNumber;
                            table_ProductInfo.ProductType = ProductType;
                            table_ProductInfo.ProductTuhao = ProductTuhao;
                            table_ProductInfo.ProductCapacity = ProductCapacity;
                            table_ProductInfo.RatedVoltage = RatedVoltage;
                            table_ProductInfo.PhaseNumber = PhaseNumber;

                            // 数据添加成功6                                                                
                            if (!ScanTypeProc(table_ProductInfo.ProductType, table_ProductInfo.ProductTuhao))
                            {
                                MessageBox.Show("错误,没有找到与产品对应的规格型号!");
                                return;
                            }

                            context.ProductInfo.Add(table_ProductInfo);
                            int rowsAffected = context.SaveChanges();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("产品选定完成!");
                                MainWindow.Current_ProductNumber = table_ProductInfo.ProductNumber;
                                MainWindow.Current_StandardType = table_ProductInfo.ProductType;
                                MainWindow.Current_StandardTuhao = table_ProductInfo.ProductTuhao;
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

        //下拉关闭时候调用..
        private bool ScanTypeProc(string proctype,string tuhao)
        {

            //这里选择后已经有值了..           
            using (var context = new MyDbContext())
            {
                var firstEntity = context.StandardInfo.FirstOrDefault(e => e.ProductType == proctype && e.ProductTuhao == tuhao);
                if (firstEntity != null)
                {
                    //注意这里不是比较，是赋值了..(有数据就是修改了.)
                    return true;
                }
                return false;
            }
        }



    }
}
