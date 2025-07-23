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
using 空载负载.Base;
using 空载负载.Table;

namespace 空载负载.Model
{
    public  class AddProcModel : ObservableValidator
    {
        public List<string> ProductNumberCBoxData { get; set; } = new List<string>();        

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
        private string phaseNumber;
        public string PhaseNumber { get => phaseNumber; set { SetProperty(ref phaseNumber, value, true); } }


        public ICommand BtnSetOver { get; set; }

        public bool State = false;

        public AddProcModel()
        {
            BtnSetOver = new RelayCommand<object>(DoBtnSetOver);

            using (var context = new MyDbContext())
            {
                try
                {
                    // 获取单个实体（第一个匹配）                    
                    var listNames = context.Template.ToList();
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
            MessageBox.Show("配置完成!");
            State = true;
            return;
            
        }

        //下拉关闭时候调用..
        private bool ScanTypeProc(string proctype,string tuhao)
        {

            //这里选择后已经有值了..           
            using (var context = new MyDbContext())
            {
                var firstEntity = context.NoloadStandardInfo.FirstOrDefault(e => e.ProductType == proctype && e.ProductTuhao == tuhao);
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
