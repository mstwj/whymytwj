using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using 铁芯实验.Base;

namespace 铁芯实验.Model
{
    public class BaseParamModel : ObservableValidator
    {
        
        public List<string> ProductNumberCBoxData { get; set; } = new List<string>();

        private string data1 = ConfigurationManager.AppSettings["EDZS_DATA"];
        [Required(ErrorMessage = "必须额定匝数")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "最大不能超过5个字符")]
        [MyValidateAttributeString("250", "1", ErrorMessage = "额定匝数最大500,最小1")]

        public string Data1 { get => data1; set { SetProperty(ref data1, value); } }

        private string data2 = ConfigurationManager.AppSettings["LSZS_DATA"];
        [Required(ErrorMessage = "必须临时匝数")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "最大不能超过5个字符")]
        [MyValidateAttributeString("250", "1", ErrorMessage = "临时匝数最大250,最小1")]

        public string Data2 { get => data2; set { SetProperty(ref data2, value); } }


        private string productType = ConfigurationManager.AppSettings["TYPE_DATA"];
        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }


        private string productTuhao = ConfigurationManager.AppSettings["TUHAO_DATA"];
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }


        private string productCapacity = ConfigurationManager.AppSettings["CAPACITY_DATA"];
        public string ProductCapacity { get => productCapacity; set { SetProperty(ref productCapacity, value); } }


        private string productStandard = ConfigurationManager.AppSettings["STANDARD_DATA"];
        public string ProductStandard { get => productStandard; set { SetProperty(ref productStandard, value); } }


        private string productCurrentStandard = ConfigurationManager.AppSettings["STANDARDCURRENT_DATA"];
        public string ProductCurrentStandard { get => productCurrentStandard; set { SetProperty(ref productCurrentStandard, value); } }


        public ICommand BtnCommandSave { get; set; }

        public BaseParamModel()
        {
            BtnCommandSave = new RelayCommand<object>(DoBtnCommandSave);

            using (var context = new MyDbContext())
            {
                try
                {
                    // 获取单个实体（第一个匹配）                    
                    var listNames = context.StandardInfo.ToList();
                    foreach (var proname in listNames)
                    {
                        //刷新..                                                
                        ProductNumberCBoxData.Add(proname.ProductType.ToString() + "||" + proname.ProductTuhao.ToString());
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    MessageBox.Show(message);
                }
            }

        }

        private void DoBtnCommandSave(object param)
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }

            // 写入配置
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //注意这里不是比较，是赋值了..(有数据就是修改了.)
            config.AppSettings.Settings["EDZS_DATA"].Value = Data1;
            config.AppSettings.Settings["LSZS_DATA"].Value = Data2;
            config.AppSettings.Settings["TYPE_DATA"].Value = ProductType;
            config.AppSettings.Settings["TUHAO_DATA"].Value = ProductTuhao;
            config.AppSettings.Settings["CAPACITY_DATA"].Value = ProductCapacity;
            config.AppSettings.Settings["STANDARD_DATA"].Value = ProductStandard;
            config.AppSettings.Settings["STANDARDCURRENT_DATA"].Value = ProductCurrentStandard;

            config.Save(ConfigurationSaveMode.Modified);

            // 强制重新加载配置
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("保存成功");


            return;
        }

    }

}
