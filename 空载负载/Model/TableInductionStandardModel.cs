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
using 空载负载.Base;
using 空载负载.Table;

namespace 空载负载.Model
{
    public class TableInductionStandardModel : ObservableValidator
    {
        private string productType;

        [Required(ErrorMessage = "必须输入产品类型")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(35, ErrorMessage = "最大不能超过35个字符")]
        public string ProductType { get => productType; set { SetProperty(ref productType, value, true); } }


        private string productTuhao;
        [Required(ErrorMessage = "必须输入图号")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(25, ErrorMessage = "最大不能超过25个字符")]
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value, true); } }


        private string voltage;
        [Required(ErrorMessage = "必须输入电压")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(25, ErrorMessage = "最大不能超过25个字符")]
        public string Voltage { get => voltage; set { SetProperty(ref voltage, value, true); } }

        private string frequency;
        [Required(ErrorMessage = "必须输入频率")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(25, ErrorMessage = "最大不能超过25个字符")]
        public string Frequency { get => frequency; set { SetProperty(ref frequency, value, true); } }

        private string times;
        [Required(ErrorMessage = "必须输入时间")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(25, ErrorMessage = "最大不能超过25个字符")]
        public string Times { get => times; set { SetProperty(ref times, value, true); } }


        
            
            

        public string SelectXinhao { get; set; }

        public ICommand CommandStandAdd { get; set; }        
        
        public TableInductionStandardModel()
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
                        Table_InductionStandardInfo table_StandardInfo = new Table_InductionStandardInfo();                                                
                        table_StandardInfo.ProductType = ProductType;
                        table_StandardInfo.ProductTuhao = ProductTuhao;
                        table_StandardInfo.Voltage = Voltage;
                        table_StandardInfo.Frequency = Frequency;
                        table_StandardInfo.Times = Times;

                        context.InductionStandardInfo.Add(table_StandardInfo);
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
