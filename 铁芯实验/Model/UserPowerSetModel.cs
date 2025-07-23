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

namespace 铁芯实验.Model
{
    public class UserPowerSetModel : ObservableValidator
    {
        private string hz_data = "50";
        [Required(ErrorMessage = "必须输入赫兹")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "最大不能超过5个字符")]
        [MyValidateAttributeString("250", "45", ErrorMessage = "频率最大250-最小45")]
        public string Hz_data { get => hz_data; set { SetProperty(ref hz_data, value); } }


        private string dianya_data;

        [Required(ErrorMessage = "必须输入电压")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(6, ErrorMessage = "最大不能超过6个字符")]
        [MyValidateAttributeString("600", "10", ErrorMessage = "600-最小10")] //使用通过验证类的自定义验证特性
        public string Dianya_data { get => dianya_data; set { SetProperty(ref dianya_data, value); } }

        private string adainya_data;
        public string Adainya_data { get => adainya_data; set { SetProperty(ref adainya_data, value); } }

        private string bdainya_data;
        public string Bdainya_data { get => bdainya_data; set { SetProperty(ref bdainya_data, value); } }

        private string cdainya_data;
        public string Cdainya_data { get => cdainya_data; set { SetProperty(ref cdainya_data, value); } }


        public ICommand BtnDyCommandSet { get; set; }
        public ICommand BtnDyCommandStart { get; set; }
        public ICommand BtnDyCommandStop { get; set; }


        public UserPowerSetModel()
        {
            BtnDyCommandStart = new RelayCommand<object>(DoBtnDyCommandStart);
            BtnDyCommandStop = new RelayCommand<object>(DoBtnDyCommandStop);
            BtnDyCommandSet = new RelayCommand<object>(DoBtnDyCommandSet);
        }

        private void DoBtnDyCommandStart(object param)
        {
            PowerStateModel m_powerStateModel = MyTools.GetPowerStateModel();
            Queue<CommandItem> queue = MyTools.GetQueue();
            MainViewModel m_MainViewModel = MyTools.GetMainViewModel();

            if (m_powerStateModel.Powerstate == "启动")
            {
                MessageBox.Show("现在电源已经是启动状态!");
                return;
            }

            //启动..--检频率..       
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }
            else
            {
                if (queue.Count > 0)
                {
                    m_MainViewModel.AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 12, regaddress = 17, regaddressdata = 1 });
            }
        }

        private void DoBtnDyCommandStop(object param)
        {
            PowerStateModel m_powerStateModel = MyTools.GetPowerStateModel();
            Queue<CommandItem> queue = MyTools.GetQueue();
            MainViewModel m_MainViewModel = MyTools.GetMainViewModel();


            if (m_powerStateModel.Powerstate == "待机")
            {
                MessageBox.Show("现在电源已经是停止状态!");
                return;
            }

            queue.Enqueue(new CommandItem { command = 12, regaddress = 17, regaddressdata = 0 });

        }

        private void DoBtnDyCommandSet(object param)
        {
            PowerStateModel m_powerStateModel = MyTools.GetPowerStateModel();
            Queue<CommandItem> queue = MyTools.GetQueue();
            MainViewModel m_MainViewModel = MyTools.GetMainViewModel();

            if (m_powerStateModel.Powerstate != "启动")
            {
                MessageBox.Show("电源必须是启动状态,才能设置!");
                return;
            }
            //启动..--检频率..            
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }
            else
            {

                if (queue.Count > 0)
                {
                    m_MainViewModel.AddRecord("无法执行指令,上一条指令还未执行完成", true);
                }


                queue.Enqueue(new CommandItem { command = 11, regaddress = 12, regaddressdata = 0 });

            }

            /*
                ValidateProperty(Dianya_data, "Dianya_data");
                ValidateProperty(Hz_data, "Hz_data");
            */
        }
    }
}
