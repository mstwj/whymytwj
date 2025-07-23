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
    public class TableLoadStandardModel : ObservableValidator
    {
        private string productType;

        [Required(ErrorMessage = "必须输入产品类型")]
        [MinLength(3, ErrorMessage = "最小不能小于3个字符")]
        [MaxLength(35, ErrorMessage = "最大不能超过35个字符")]
        public string ProductType { get => productType; set { SetProperty(ref productType, value, true); } }


        private string productTuhao;
        [Required(ErrorMessage = "必须输入图号")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(25, ErrorMessage = "最大不能超过5个字符")]        
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value, true); } }

        private string loadloss;
        public string Loadloss { get => loadloss; set { SetProperty(ref loadloss, value, true); } }

        private string loadlossUpperimit;
        public string LoadlossUpperimit { get => loadlossUpperimit; set { SetProperty(ref loadlossUpperimit, value, true); } }

        private string loadlossDownimit;
        public string LoadlossDownimit { get => loadlossDownimit; set { SetProperty(ref loadlossDownimit, value, true); } }

        private string loadMainReactance;
        public string LoadMainReactance { get => loadMainReactance; set { SetProperty(ref loadMainReactance, value, true); } }

        private string loadMainReactanceUpperrimit;
        public string LoadMainReactanceUpperrimit { get => loadMainReactanceUpperrimit; set { SetProperty(ref loadMainReactanceUpperrimit, value, true); } }

        private string loadMainReactanceDownimit;
        public string LoadMainReactanceDownimit { get => loadMainReactanceDownimit; set { SetProperty(ref loadMainReactanceDownimit, value, true); } }

        



        public string SelectXinhao { get; set; }

        public ICommand CommandStandAdd { get; set; }        
        
        public TableLoadStandardModel()
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
                        Table_LoadStandardInfo table_StandardInfo = new Table_LoadStandardInfo();                        
                        table_StandardInfo.ProductType = ProductType;
                        table_StandardInfo.ProductTuhao = ProductTuhao;                        
                        table_StandardInfo.Loadloss = Loadloss;
                        table_StandardInfo.LoadlossUpperimit = LoadlossUpperimit;
                        table_StandardInfo.LoadlossDownimit = LoadlossDownimit;
                        table_StandardInfo.LoadMainReactance = LoadMainReactance;
                        table_StandardInfo.LoadMainReactanceUpperrimit = LoadMainReactanceUpperrimit;
                        table_StandardInfo.LoadMainReactanceDownimit = LoadMainReactanceDownimit;

                        context.LoadStandardInfo.Add(table_StandardInfo);
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
