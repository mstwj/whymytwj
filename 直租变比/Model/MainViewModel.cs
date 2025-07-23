using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;

using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using 直租变比.Base;
using System.Windows.Controls;
using System.Reflection.Metadata;
using System.Security.Cryptography;

using System.Windows.Markup;

namespace 直租变比.Model
{
    public class MainViewModel : ObservableValidator
    {
        //功率分析-- 这里有个问题 -- 就是我不需要 回写到UI..
        //这里只有UI 写给我的数据.. 所以，我不需要SetProerty        
        [MyValidateAttribute(250, 45, ErrorMessage = "频率最大250-最小45")] 
        public float Hz_data { get; set; }

        [MyValidateAttribute(1000, 10,ErrorMessage = "电压最大1000-最小10")] //使用通过验证类的自定义验证特性
        public float Dianya_data { get; set; }

        [MyValidateAttribute(1000, 10, ErrorMessage = "三相同调电压最大1000-最小10")]
        public float Tongtiao_data { get; set; }

        [MyValidateAttribute(1000, 10, ErrorMessage = "A电压最大1000-最小10")]
        public float Adainya_data { get; set; }

        [MyValidateAttribute(1000, 10, ErrorMessage = "B电压最大1000-最小10")]
        public float Bdainya_data { get; set; }

        [MyValidateAttribute(1000, 10, ErrorMessage = "C电压最大1000-最小10")]
        public float Cdainya_data { get; set; }

        [MyValidateAttribute(100, 2, ErrorMessage = "额定扎数最大100-最小2")]
        public float Edingzhashu_data { get; set; }//额定扎数

        [MyValidateAttribute(100, 2, ErrorMessage = "临时扎数最大100-最小2")]
        public float Tempzhashu_data { get; set; }//零食扎数


        private float shijiazhashu_data;//施加电压(不可写)
        public float Shijiazhashu_data { get => shijiazhashu_data; set { SetProperty(ref shijiazhashu_data, value); } }
    
        private bool _imageshebei1 = false;
        private string imageshebei1;
        public string ImageShebei1 { get => _imageshebei1 ? imageshebei1 = "pack://application:,,,/Asset/1.png" : imageshebei1 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei1, value); } }

        private bool _imageshebei2 = false;
        private string imageshebei2;
        public string ImageShebei2 { get => _imageshebei2 ? imageshebei2 = "pack://application:,,,/Asset/1.png" : imageshebei1 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei2, value); } }

        private SerialPort serialPort = new SerialPort();
        //private ModbusSerialMaster master = null;

        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<int> queue = new Queue<int>();
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();
        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private string _showOneOrThree;        
        public string ShowOneOrThree { get => _showOneOrThree; set { SetProperty(ref _showOneOrThree, value); } }

        //表示选择 设置3P4还是其他(默认为0)
        //public int Set3P4WOr3P3W { get; set; } = 0;
        private string set3P4WOr3P3W = "3P4W";
        public string Set3P4WOr3P3W { get => set3P4WOr3P3W; set { SetProperty(ref set3P4WOr3P3W, value); } }



        public UserMainModel m_userMainModel { get; set; }
        public UserRecordModel m_userRecordModel{ get; set; }
        public UserInfoModel m_userInfoModel{ get; set; }

        public ICommand BtnCommandStart { get; set; }
        public ICommand BtnCommandStop { get; set; }
        public ICommand BtnCommandSave { get; set; }


        public ICommand BtnDyCommandStart { get; set; }
        public ICommand BtnDyCommandStop { get; set; }

        public ICommand BtnFxyCommandSet { get; set; }

        public ICommand SendGLFXYWoShou { get; set; }

        public ICommand SendBPDYWoShou { get; set; }


        private bool StartScanSing = false; //开始扫描信号..

        public MainViewModel(UserMainModel userMainModel, 
            UserRecordModel userRecordModel,
            UserInfoModel userInfoModel
            ) 
        {
            m_userMainModel = userMainModel;
            m_userRecordModel = userRecordModel;
            m_userInfoModel = userInfoModel;

            BtnCommandStart = new RelayCommand<object>(DoBtnCommandStart);
            BtnCommandStop = new RelayCommand<object>(DoBtnCommandStop);
            BtnCommandSave = new RelayCommand<object>(DoBtnCommandSave);

            BtnDyCommandStart = new RelayCommand<object>(DoBtnDyCommandStart);
            BtnDyCommandStop = new RelayCommand<object>(DoBtnDyCommandStop);
            BtnFxyCommandSet = new RelayCommand<object>(DoBtnFxyCommandSet);

            SendGLFXYWoShou = new RelayCommand<object>(DoSendGLFXYWoShou);
            SendBPDYWoShou = new RelayCommand<object>(DoSendBPDYWoShou);

            //string temp = ComBoxSelect3;
            //ComBoxSelect3 = "三相";

            //启动的时候，就打开串口，如果失败了. 就要重新启动软件...
            InitializationLink();
        }

        private void DoSendGLFXYWoShou(object param)
        {
            if (queue.Count > 0)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成",true);                    
            }
            queue.Enqueue(1);
            return;
        }

        private void DoSendBPDYWoShou(object param)
        {
            if (queue.Count > 0)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成", true);
            }
            queue.Enqueue(2);

            return;
        }

        private void DoBtnCommandStart(object param)
        {

            if (Shijiazhashu_data == 0)
            {
                MessageBox.Show("无法启动,施加电压为0");
                return;
            }

            StackPanel? stackPanel = param as StackPanel;
            Button? buttons = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BCommandStart");
            Button? buttont = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BCommandStop");

            buttons.IsEnabled = false;
            buttont.IsEnabled = true;
            
            StartScanSing = true;


        }

        private void DoBtnCommandStop(object param)
        {
            StackPanel? stackPanel = param as StackPanel;
            Button? buttons = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BCommandStart");
            Button? buttont = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BCommandStop");

            buttons.IsEnabled = true;
            buttont.IsEnabled = false;
            StartScanSing = false;
        }


        private void DoBtnCommandSave(object button)
        {
            //保存数据..
            if (MyTools.SaveDataToDataBase(m_userRecordModel,m_userInfoModel.ProductNumber))
            {
                AddRecord("添加数据成功!",false);
            }else
            {
                AddRecord("添加数据失败!",true);
            }
        }

        private void DoBtnDyCommandStart(object param)
        {
            //启动..--检频率..
            ValidateProperty(Hz_data, "Hz_data");
            ValidateProperty(Dianya_data, "Dianya_data");
            ValidateProperty(Tongtiao_data, "Tongtiao_data");
            ValidateProperty(Adainya_data, "Adainya_data");
            ValidateProperty(Bdainya_data, "Bdainya_data");
            ValidateProperty(Cdainya_data, "Cdainya_data");
            //ValidateAllProperties();
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
                    //AddRecord("无法执行指令,上一条指令还未执行完成");                    
                }
                //queue.Enqueue(1);
                
            }

            StackPanel? stackPanel = param as StackPanel;
            Button? buttons = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BtnDyCommandStart");
            Button? buttont = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BtnDyCommandStop");

            buttons.IsEnabled = false;
            buttont.IsEnabled = true;

        }

        private void DoBtnDyCommandStop(object param)
        {
            StackPanel? stackPanel = param as StackPanel;
            Button? buttons = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BtnDyCommandStart");
            Button? buttont = VisualTreeHelpers.ScanButtonFromStackPanel(stackPanel, "BtnDyCommandStop");

            buttons.IsEnabled = true;
            buttont.IsEnabled = false;

        }

        private void DoBtnFxyCommandSet(object button)
        {
            //点击设置..            
            ValidateProperty(Edingzhashu_data, "Edingzhashu_data");
            ValidateProperty(Tempzhashu_data, "Tempzhashu_data");            
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }
            else
            {
                if (ShowOneOrThree == null)
                {
                    MessageBox.Show("没有设置计算方式(三相/单相),请先选择产品");
                    return;
                }

                if (ShowOneOrThree == "三相")
                {                    
                    //计算三相施加电压 参数1 额定电压.. 参数2 额定扎数  参数3 临时扎数
                    float data = MyTools.GetCalcAppliedVoltageThree(m_userInfoModel.RatedVoltage, Edingzhashu_data, Tempzhashu_data);
                    if (data == -1)
                    {
                        MessageBox.Show("无法计算三相施加电压,请检查产品参数是否设置正确!");
                        return;
                    }

                    //这里设置了 施加电压..
                    Shijiazhashu_data = data;


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
                    //设置单相--这里的单相
                    float data = MyTools.GetCalcAppliedVoltageOne(m_userInfoModel.RatedVoltage, Edingzhashu_data, Tempzhashu_data);
                    if (data == -1)
                    {
                        MessageBox.Show("无法计算单相施加电压,请检查产品参数是否设置正确!");
                        return;
                    }
                    Shijiazhashu_data = data;

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
            }
        }

        //设置用户信息..
        public bool SetCurrentUserInfo()
        {
            if (MainWindow.Current_ProductNumber == null)
            {
                return false;
            }
            if (MyTools.GetCurrentUserInfo(m_userInfoModel))
            {
                ShowOneOrThree = m_userInfoModel.PhaseNumber;
                //施加电压设置为0;
                Shijiazhashu_data = 0;
                return true;
            }

            return false;
        }

        public bool InitializationLink()
        {                        
            //先打开端口...
            try
            {                
                AddRecord("启动完成...", false);
             
                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议
                //master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                //master.Transport.ReadTimeout = 1000;// 设置超时时间默认为1000毫秒
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_GLFXY"];
                serialPort.Open(); //打开串口                                
                AddRecord("打开串口完成",false);
            }
            catch (Exception ex)
            {
                AddRecord("打开串口异常"+ex.Message, true);

                return false;
            }

            Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        //这里是需要TRY的。。 我执行代码的时候，遇到了一个问题，就是主线程没有加线程分发.. 导致.. 报错..
                        Thread.Sleep(10);
                        System.DateTime currentTime = System.DateTime.UtcNow;

                        TimeSpan ts = (TimeSpan)(currentTime - precurrentTime);
                        //得到毫秒数...
                        double milliseconds = ts.TotalMilliseconds;
                        
                        if (milliseconds > 1000)
                        {
                            //定时时间到了..(1秒1次)
                            if (StartScanSing == true)
                            {
                                var myresult1 = await ExePlcCommand(999999);
                                if (!myresult1)
                                {
                                    AddRecord("定时器指令执行失败", true);
                                    return;
                                }
                            }
                            precurrentTime = currentTime;                            
                            continue;
                        }
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();                        
                        var myresult2 = await ExePlcCommand(Command);
                        if (myresult2)
                        {
                            AddRecord("指令执行成功!", false);
                        }
                        else
                        {
                            AddRecord("指令执行失败!-后台线程退出", true);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        AddRecord($"{ex.Message}", true);                        
                        return;
                    }
                }
            }, cts.Token);

            return true;
        }

        private async Task<bool> ExePlcCommand(int command)
        {
            bool Success = false;
            string error = string.Empty;

            //AddRecord("管理线程-请等待,设备回应!", false);
            var timeout = Task.Delay(8000);
            var t = Task.Run(async () =>
            {
                //AddRecord("工作线程-开始执行"+command.ToString(), false);  
                try
                {
                    switch (command)
                    {
                        case 1:
                            {
                                var result = ReadModbusData(1, 2, 3);                                                                
                                if (!result.Item1)
                                {
                                    Success = false;
                                    error = "功率分析仪器握手失败"; 
                                    return; 
                                }                                
                            }
                            break; // 握手..
                        case 2:
                            {
                                //写入         (92)
                                //01  10    00 5C    00 02     04   00 00 00 01    37 06 --（监视得到的数据）--(软件模拟的)         
                                //(设备回的.) 01 10 00 5C 00 02 81 DA
                                //01  10    00 92    00 02     04   00 00 00 01 
                                ushort[] data = new ushort[2]{ 0, 1 };                                
                                //这里可以不封装了..只要返回不超时就是OK的..
                                //this.master.WriteMultipleRegisters(1, 146, data);
                            }
                            break;
                        case 3: break; //设置功率分析
                        case 4: break;
                        case 999999: Testsimulation(); break; //仿真数据.
                    }
                    Success = true;
                    //AddRecord("工作线程-执行结束" + command.ToString(), false);                    
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    AddRecord("工作线程-执行异常:" + error + command.ToString(), true);
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {                
                AddRecord("管理线程-超时,请检查设备是否正常!", true);
                return false;
            }

            if (Success)
            {
                return true;
            }
            else
            {                
                AddRecord("管理线程-工作线程异常:" + error, true);
                return false;
            }
        }


        void AddRecord(string strdata, bool e)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });
                logger.Error(str);
            });
        }

        void Testsimulation()
        {
            //仿真数据...(设备功率分析)
            int[] sdata = new int[12];
            for (int i = 0;i<12;i++)
            {
                Random ran = new Random();
                int n = ran.Next(100, 10000);
                sdata[i] = n;
            }

            /*
            m_userMainModel.Umn_ab = sdata[0];
            m_userMainModel.Umn_bc = sdata[1];
            m_userMainModel.Umn_ca = sdata[2];
            m_userMainModel.Urms_ab  = sdata[3];
            m_userMainModel.Urms_bc  = sdata[4];
            m_userMainModel.Urms_ca = sdata[5];
            m_userMainModel.Irms_ab  = sdata[6];
            m_userMainModel.Irms_bc  = sdata[7];
            m_userMainModel.Irms_ca = sdata[8];
            m_userMainModel.P_ab  = sdata[9];
            m_userMainModel.P_bc  = sdata[10];
            m_userMainModel.P_ca = sdata[11];
            */

            m_userMainModel.Umn_ab = 286.0f;
            m_userMainModel.Umn_bc = 286.0f;
            m_userMainModel.Umn_ca = 286.0f;
            m_userMainModel.Urms_ab = sdata[3];
            m_userMainModel.Urms_bc = sdata[4];
            m_userMainModel.Urms_ca = sdata[5];
            m_userMainModel.Irms_ab = 16.0f;
            m_userMainModel.Irms_bc = 16.0f;
            m_userMainModel.Irms_ca = 16.0f;
            m_userMainModel.P_ab = 3465.0f;
            m_userMainModel.P_bc = 1.0f;
            m_userMainModel.P_ca = 1.0f;

            //计算结果..(计算是工作线程计算的..)(以下为 三相显示..)
            if (ShowOneOrThree == "三相")
            {                
                m_userRecordModel.ATPVoltage = (m_userMainModel.Umn_ab + m_userMainModel.Umn_bc + m_userMainModel.Umn_ca) / 3;
                m_userRecordModel.RMSvalue = (m_userMainModel.Urms_ab + m_userMainModel.Urms_bc + m_userMainModel.Urms_ca) / 3;
                m_userRecordModel.IRMSvalue = (m_userMainModel.Irms_ab + m_userMainModel.Irms_bc + m_userMainModel.Irms_ca) / 3;

                //计算 电流%.(显示 电流%)
                m_userRecordModel.PercentageNoloadCurrent = MyTools.GetCalcRatedCurrentPercentageThree(m_userRecordModel.IRMSvalue, m_userRecordModel.NoloadCurrent);
                m_userRecordModel.PercentageNoloadCurrent = (float)Math.Round(m_userRecordModel.PercentageNoloadCurrent, 3);

                //计算 空载损耗.
                m_userRecordModel.NoloadLoss = MyTools.GetCalcNoloadlossThree(m_userMainModel.P_ab + m_userMainModel.P_bc + m_userMainModel.P_ca,
                    m_userRecordModel.ATPVoltage, Shijiazhashu_data);
                m_userRecordModel.NoloadLoss = (float)Math.Round(m_userRecordModel.NoloadLoss, 3);

            }
            //计算结果..(计算是工作线程计算的..)(以下为 单相显示..)
            if (ShowOneOrThree == "单相")
            {
                m_userRecordModel.ATPVoltage = (m_userMainModel.Umn_ab + m_userMainModel.Umn_bc + m_userMainModel.Umn_ca) / 3;
                m_userRecordModel.RMSvalue = (m_userMainModel.Urms_ab + m_userMainModel.Urms_bc + m_userMainModel.Urms_ca) / 3;
                m_userRecordModel.IRMSvalue = (m_userMainModel.Irms_ab + m_userMainModel.Irms_bc + m_userMainModel.Irms_ca) / 3;

                //计算 电流%.(显示 电流%)
                m_userRecordModel.PercentageNoloadCurrent = MyTools.GetCalcRatedCurrentPercentageThree(m_userRecordModel.IRMSvalue, m_userRecordModel.NoloadCurrent);
                m_userRecordModel.PercentageNoloadCurrent = (float)Math.Round(m_userRecordModel.PercentageNoloadCurrent, 3);

                //计算 空载损耗.
                m_userRecordModel.NoloadLoss = MyTools.GetCalcNoloadlossThree(m_userMainModel.P_ab + m_userMainModel.P_bc + m_userMainModel.P_ca,
                    m_userRecordModel.ATPVoltage, Shijiazhashu_data);
                m_userRecordModel.NoloadLoss = (float)Math.Round(m_userRecordModel.NoloadLoss, 3);

            }
        }


        private (bool, float[]) ReadModbusData(byte slaveId, ushort start, ushort length)
        {
            float[] FloatValue = new float[3];
            try
            {
                switch(start)
                {
                    //2 表示 读取仪表名称..
                    case 2:
                        {
                            //这里只要有数据返回就算握手OK..
                            //ushort[] data = this.master.ReadHoldingRegisters(slaveId, start, length);
                            return (true, FloatValue);
                        }
                        break;
                }

                //这里肯定要转REAL。。。
                /*
                byte[] _cbuffer = new byte[4];

                if (startAddress == 0)
                {
                    if (numberOfPoints == 2)
                    {
                        Buffer.BlockCopy(registerBuffer, 0, _cbuffer, 0, 4);
                        //数值翻转得到大端模式... Array.Reverse(_cbuffer);
                        FloatValue[0] = BitConverter.ToSingle(_cbuffer, 0);
                        FloatValue[1] = BitConverter.ToSingle(_cbuffer, 0);
                        FloatValue[2] = BitConverter.ToSingle(_cbuffer, 0);
                    }
                }
                */
                return (false,FloatValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }


    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }
}
