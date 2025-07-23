using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Office.Interop.Word;
using 铁芯实验.Base;
using 铁芯实验.Table;

namespace 铁芯实验.Model
{
    public class TableStandardModel : ObservableValidator
    {
        private string productType;

        [Required(ErrorMessage = "必须输入产品类型")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(35, ErrorMessage = "最大不能超过35个字符")]
        public string ProductType { get => productType; set { SetProperty(ref productType, value, true); } }


        private string productTuhao;
        [Required(ErrorMessage = "必须输入图号")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value, true); } }


        private string productStandard;
        [Required(ErrorMessage = "必须输入空载损耗")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        public string ProductStandard { get => productStandard; set { SetProperty(ref productStandard, value, true); } }

        private string productStandardUp;
        [Required(ErrorMessage = "必须输入空载损耗上限")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        [MyValidateAttributeString("100", "0", ErrorMessage = "标准最大100,最小0")]
        public string ProductStandardUp { get => productStandardUp; set { SetProperty(ref productStandardUp, value, true); } }

        private string productStandardDown;
        [Required(ErrorMessage = "必须输入空载损耗下限")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(10, ErrorMessage = "最大不能超过10个字符")]
        [MyValidateAttributeString("100", "0", ErrorMessage = "标准最大100,最小0")]
        public string ProductStandardDown { get => productStandardDown; set { SetProperty(ref productStandardDown, value, true); } }


        private string productCurrentStandard;
        [Required(ErrorMessage = "必须输入空载电流%")]
        [MinLength(1, ErrorMessage = "空载电流不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "空载电流不能超过5个字符")]
        public string ProductCurrentStandard { get => productCurrentStandard; set { SetProperty(ref productCurrentStandard, value, true); } }

        private string productCurrentStandardUp;
        [Required(ErrorMessage = "必须输入空载电流上限%")]
        [MinLength(1, ErrorMessage = "空载电流上限不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "空载电流上限不能超过5个字符")]
        [MyValidateAttributeString("100", "0", ErrorMessage = "空载电流上限最大100,最小0")]
        public string ProductCurrentStandardUp { get => productCurrentStandardUp; set { SetProperty(ref productCurrentStandardUp, value, true); } }

        private string productCurrentStandardDown;
        [Required(ErrorMessage = "必须输入空载电流%下限")]
        [MinLength(1, ErrorMessage = "空载电流下限不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "空载电流下限不能超过5个字符")]
        [MyValidateAttributeString("100", "0", ErrorMessage = "空载电流下限标准最大100,最小0")]
        public string ProductCurrentStandardDown { get => productCurrentStandardDown; set { SetProperty(ref productCurrentStandardDown, value, true); } }

        public string SelectXinhao { get; set; }

        public ICommand CommandStandAdd { get; set; }        
        
        public TableStandardModel()
        {
            CommandStandAdd = new RelayCommand<object>(DoBtnCommandStandAdd);
        }

        private void DoBtnCommandStandAdd(object param)
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
                        Table_StandardInfo table_StandardInfo = new Table_StandardInfo();                        
                        table_StandardInfo.ProductType = ProductType;
                        table_StandardInfo.ProductTuhao = ProductTuhao;                        
                        table_StandardInfo.ProductStandard = ProductStandard;
                        table_StandardInfo.ProductStandardUpperimit = ProductStandardUp;
                        table_StandardInfo.ProductStandardDownimit = ProductStandardDown;
                        table_StandardInfo.ProductCurrentStandard = ProductCurrentStandard;
                        table_StandardInfo.ProductCurrentStandardDownimit = ProductCurrentStandardUp;
                        table_StandardInfo.ProductCurrentStandardUpperrimit = ProductCurrentStandardDown;

                        context.StandardInfo.Add(table_StandardInfo);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            // 数据添加成功6
                            MessageBox.Show("标准添加成功!");
                            WeakReferenceMessenger.Default.Send<string>("添加成功");
                        }
                        else
                        {
                            MessageBox.Show("标准添加失败!");
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
