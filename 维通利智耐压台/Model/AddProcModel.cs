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
using 维通利智耐压台.Base;
using 维通利智耐压台.MyTable;

namespace 维通利智耐压台.Model
{
    public  class AddProcModel : ObservableValidator
    {
        public List<Table_Product> tempData { get; set; } = null;

        public Table_Product SelectProduct {  get; set; } = new Table_Product();

        
        public List<string> ProductNumberCBoxData { get; set; } = new List<string>();

        private string productName;

        [Required(ErrorMessage = "必须输入产品名称")]
        [MinLength(3, ErrorMessage = "产品名称不能小于3个字符")]
        [MaxLength(15, ErrorMessage = "产品名称不能超过15个字符")]
        public string ProductName { get => productName; set { SetProperty(ref productName, value, true); } }

        


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
        [MaxLength(5, ErrorMessage = "最大不能超过5个字符")]        
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value, true); } }

        private string productParts;

        [Required(ErrorMessage = "必须输入施加部位")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(15, ErrorMessage = "最大不能超过15个字符")]
        public string ProductParts { get => productParts; set { SetProperty(ref productParts, value, true); } }



        private string productTestVotil;
        [Required(ErrorMessage = "必须输入试验电压")]
        [MinLength(1, ErrorMessage = "试验电压不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "试验电压不能超过10个字符")]        
        public string ProductTestVotil { get => productTestVotil; set { SetProperty(ref productTestVotil, value, true); } }


        private string productTestPartial;
        [Required(ErrorMessage = "必须输入局放量")]
        [MinLength(0, ErrorMessage = "局放量不能小于0个字符")]
        [MaxLength(10, ErrorMessage = "局放量不能超过10个字符")]        
        public string ProductTestPartial { get => productTestPartial; set { SetProperty(ref productTestPartial, value, true); } }


        public ICommand BtnSetOver { get; set; }

        public AddProcModel()
        {
            BtnSetOver = new RelayCommand<object>(DoBtnSetOver);

            // 获取单个实体（第一个匹配）
            using (var context = new MyDbContext())
            {
                tempData = context.naiya_product.ToList();
                foreach (var proname in tempData)
                {
                    //刷新..
                    ProductNumberCBoxData.Add(proname.ProductNumber);
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
                var fister = tempData.FirstOrDefault(n => n.ProductNumber == this.ProductNumber);
                if (fister == null)
                {
                    //新的..
                    //Table_Product table_ProductInfo = new Table_Product();
                    SelectProduct.ProductName = ProductName;
                    SelectProduct.ProductNumber = ProductNumber;
                    SelectProduct.ProductType = ProductType;
                    SelectProduct.ProductTuhao = ProductTuhao;
                    SelectProduct.ProductParts = ProductParts;
                    SelectProduct.ProductTestVotil = ProductTestVotil;
                    SelectProduct.ProductTestPartial = ProductTestPartial;
                    using (var context = new MyDbContext())
                    {
                        context.naiya_product.Add(SelectProduct);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("产品选定完成!");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("产品选定失败-添加数据失败!");
                            return;
                        }
                    }
                }
                else
                {

                    //旧的..
                    ObjectComparer.CompareObjects(SelectProduct, fister);
                    MessageBox.Show("配置完成!");
                    return;
                }
            }

        }

        //下拉关闭时候调用..
        private bool ScanTypeProc(string proctype,string tuhao)
        {

            //这里选择后已经有值了..           
            /*
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
            */
            return false;
        }



    }
}
