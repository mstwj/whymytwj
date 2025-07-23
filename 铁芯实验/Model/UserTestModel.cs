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
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.ApplicationServices;
using 铁芯实验.Base;

namespace 铁芯实验.Model
{
    public class UserTestModel : ObservableValidator
    {
        private string edingzhashu_data = "20";

        [Required(ErrorMessage = "必须输入额定匝数")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "最大不能超过5个字符")]
        [MyValidateAttributeString("250", "1", ErrorMessage = "临时匝数最大250,最小1")]
        public string Edingzhashu_data { get => edingzhashu_data; set { SetProperty(ref edingzhashu_data, value); } }

        private string tempzhashu_data = "10";

        [Required(ErrorMessage = "必须输入临时匝数")]
        [MinLength(1, ErrorMessage = "最小不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "最大不能超过5个字符")]
        [MyValidateAttributeString("250", "1", ErrorMessage = "临时匝数最大250,最小1")]
        public string Tempzhashu_data { get => tempzhashu_data; set { SetProperty(ref tempzhashu_data, value); } }

        //施加电压(不可写)
        private int shijiazhashu_data;
        public int Shijiazhashu_data { get => shijiazhashu_data; set { SetProperty(ref shijiazhashu_data, value); } }

        private string _showOneOrThree;
        public string ShowOneOrThree { get => _showOneOrThree; set { SetProperty(ref _showOneOrThree, value); } }

        //表示选择 设置3P4还是其他(默认为0)
        //public int Set3P4WOr3P3W { get; set; } = 0;
        private string set3P4WOr3P3W = "3P4W";
        public string Set3P4WOr3P3W { get => set3P4WOr3P3W; set { SetProperty(ref set3P4WOr3P3W, value); } }

        private float protectcurrent = 35;
        [MyValidateAttribute(38, 1, ErrorMessage = "保护电流最大38,最小1")]
        public float ProtectCurrent { get => protectcurrent; set { SetProperty(ref protectcurrent, value); } }


        public ICommand BtnCommandSet { get; set; }

        public ICommand BtnCommandWay { get; set; }

        public UserTestModel()
        {
            BtnCommandSet = new RelayCommand<object>(DoBtnCommandSet);
            BtnCommandWay = new RelayCommand<object>(DoBtnCommandWay);
        }

        
        private void DoBtnCommandWay(object button)
        {
            MainViewModel? m_MainViewModel;
            Queue<CommandItem>? queue;
            MyMessage myMessage1 = new MyMessage();
            MyMessage myMessage2 = new MyMessage();
            myMessage1.Message = "GetQueue";
            myMessage2.Message = "GetMainViewModel";
            WeakReferenceMessenger.Default.Send(myMessage1);
            WeakReferenceMessenger.Default.Send(myMessage2);
            queue = (Queue<CommandItem>)myMessage1.obj;
            m_MainViewModel = (MainViewModel)myMessage2.obj;
            if (queue == null) return;
            if (m_MainViewModel == null) return;
            

            if (set3P4WOr3P3W == "3P4W")
            {
                if (queue.Count > 0) m_MainViewModel.AddRecord("无法执行指令,上一条指令还未执行完成", true);
                else queue.Enqueue(new CommandItem { command = 2, regaddress = 146, regaddressdata = 0 });
            }

            if (set3P4WOr3P3W == "3P3W")
                if (queue.Count > 0) m_MainViewModel.AddRecord("无法执行指令,上一条指令还未执行完成", true);
                else queue.Enqueue(new CommandItem { command = 2, regaddress = 146, regaddressdata = 2 });


            if (set3P4WOr3P3W == "3V3A")
                if (queue.Count > 0) m_MainViewModel.AddRecord("无法执行指令,上一条指令还未执行完成", true);
                else queue.Enqueue(new CommandItem { command = 2, regaddress = 146, regaddressdata = 1 });


            if (set3P4WOr3P3W == "1P3W")
                if (queue.Count > 0) m_MainViewModel.AddRecord("无法执行指令,上一条指令还未执行完成", true);
                else queue.Enqueue(new CommandItem { command = 2, regaddress = 146, regaddressdata = 3 });
            
        }
        

        private void DoBtnCommandSet(object button)
        {
            ValidateAllProperties();
            //点击设置..            
            //ValidateProperty(Edingzhashu_data, "Edingzhashu_data");
            //ValidateProperty(Tempzhashu_data, "Tempzhashu_data");
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }
            else
            {
                UserInfoModel m_userInfoModel = MyTools.GetUserInfoModel();
                UserPowerSetModel m_userPowerSetModel = MyTools.GetUserPowerSetModel();
                UserRecordModel m_userRecordModel = MyTools.GetUserRecordModel();

                if (ShowOneOrThree == null)
                {
                    MessageBox.Show("没有设置计算方式(三相/单相),请先选择产品");
                    return;
                }

                if (ShowOneOrThree == "三相")
                {
                    float t_data, t_data2;
                    if (!float.TryParse(Edingzhashu_data, out t_data))
                    {
                        MessageBox.Show("额定电压转换FLOAT失败!");
                        return;
                    }

                    if (!float.TryParse(Tempzhashu_data, out t_data2))
                    {
                        MessageBox.Show("临时电压转换FLOAT失败!");
                        return;
                    }

                    //计算三相施加电压 参数1 额定电压.. 参数2 额定扎数  参数3 临时扎数


                    
                    float data = MyTools.GetCalcAppliedVoltageThree(m_userInfoModel.RatedVoltage, t_data, t_data2);
                    if (data == -1)
                    {
                        MessageBox.Show("无法计算三相施加电压,请检查产品参数是否设置正确!");
                        return;
                    }

                    //这里设置了 施加电压..
                    Shijiazhashu_data = (int)Math.Round(data);
                    //这里需要设置                    
                    m_userPowerSetModel.Dianya_data = Shijiazhashu_data.ToString();


                    //1.启动 要求..  -- 要求 额定容量(产品) -- 施加电压(计算)
                    float result = MyTools.GetCalcRatedCurrentThree(m_userInfoModel.ProductCapacity, Shijiazhashu_data);
                    if (result == -1)
                    {
                        MessageBox.Show("无法计算额定电流-请检查输入产品信息是否正确!");
                        return;
                    }
                    //这里设置了 额定电流
                    m_userRecordModel.NoloadCurrent = result;
                    

                }

                if (ShowOneOrThree == "单相")
                {
                    float t_data, t_data2;
                    if (!float.TryParse(Edingzhashu_data, out t_data))
                    {
                        MessageBox.Show("额定电压转换FLOAT失败!");
                        return;
                    }

                    if (!float.TryParse(Tempzhashu_data, out t_data2))
                    {
                        MessageBox.Show("临时电压转换FLOAT失败!");
                        return;
                    }


                    //设置单相--这里的单相
                    
                    float data = MyTools.GetCalcAppliedVoltageOne(m_userInfoModel.RatedVoltage, t_data, t_data2);
                    if (data == -1)
                    {
                        MessageBox.Show("无法计算单相施加电压,请检查产品参数是否设置正确!");
                        return;
                    }
                    Shijiazhashu_data = (int)Math.Round(data);
                    m_userPowerSetModel.Dianya_data = Shijiazhashu_data.ToString();

                    //1.启动 要求..  -- 要求 额定容量(产品) -- 施加电压(计算)
                    float result = MyTools.GetCalcRatedCurrentOne(m_userInfoModel.ProductCapacity, Shijiazhashu_data);
                    if (result == -1)
                    {
                        MessageBox.Show("无法计算额定电流-请检查输入产品信息是否正确!");
                        return;
                    }
                    //这里设置了 额定电流
                    m_userRecordModel.NoloadCurrent = result;
                    
                }
                this.DoBtnCommandWay(null);
            }

            
        }



    }
}
