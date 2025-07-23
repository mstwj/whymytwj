using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Collections;
using CommunityToolkit.Mvvm.Messaging;
using 空载_负载.Base.Table;
using 空载_负载.View;
using Modbus.Device;
using System.Diagnostics.Metrics;
using System.IO.Ports;
using S7.Net;
using S7.Net.Types;
using 空载_负载.Base;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CommunityToolkit.Mvvm.ComponentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using NLog.Time;

namespace 空载_负载.Model
{
    public class Noload : ObservableObject
    {
        public Queue<int> queue { get; set; } = new Queue<int>(); //自动队列

        public Queue<int> port_queue { get; set; } = new Queue<int>(); //串口指令必须分离..
        public SerialPortHelper serialPortHelper { get; set; } = new SerialPortHelper(
            ConfigurationManager.AppSettings["DEVICE_GLFXY"], 19200, Parity.None, 8, StopBits.One);

        public int timeSelect { get; set; } = 0;
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();

        private Table_NoloadStandardInfo firstEntity = null;

        //设备
        public UserDevcelModel m_UserDevcelModel { get; set; } = null;
        //1.当前用户模块
        public UserInfoModel m_UserInfoModel { get; set; } = null;                
        //3 PLC模块
        public PlcDevcelModel m_PlcDevcelModel { get; set; } = null;
        //4.用户记录模块..
        public UserRecordModel m_UserRecordModel { get; set; } = null;

        //public UserStandardModel m_UserStandardModel { get; set; } = null;


        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public bool[] alldian_boolinput { get; set; } = new bool[50];

        
        public Plc plc200 { get; set; } = new Plc(CpuType.S7200Smart, "192.168.2.1", 0, 1);

        private CancellationTokenSource cts = new CancellationTokenSource();

        private bool _plc;

        private string _plcImage;
        public bool Plc { get { return _plc; } set { SetProperty(ref _plc, value); string _imagesource = PlcImage; PlcImage = _imagesource; } }
        public string PlcImage { get { return _plc ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcImage, value); } }


        private bool _dev;

        private string _devImage;
        public bool Dev { get { return _dev; } set { SetProperty(ref _plc, value); string _imagesource = DevImage; DevImage = _imagesource; } }
        public string DevImage { get { return _plc ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _devImage, value); } }



        public ICommand Command1 { get; set; }
        public ICommand Command2 { get; set; }
        public ICommand Command3 { get; set; }
        public ICommand Command4 { get; set; }
        public ICommand Command5 { get; set; }
        public ICommand Command6 { get; set; }
        public ICommand Command7 { get; set; }
        public ICommand Command8 { get; set; }
        public ICommand Command9 { get; set; }
        public ICommand Command10 { get; set; }
        public ICommand Command11 { get; set; }
        public ICommand Command12 { get; set; }


        public ICommand Command13 { get; set; }
        public ICommand Command14 { get; set; }


        public ICommand Command5A { get; set; }
        public ICommand Command10A { get; set; }
        public ICommand Command25A { get; set; }
        public ICommand Command50A { get; set; }
        public ICommand Command100A { get; set; }
        public ICommand Command200A { get; set; }

        public ICommand CommandDev1 { get; set; }
        public ICommand CommandDev2 { get; set; }
        public ICommand CommandDev3 { get; set; }
        public ICommand CommandDev4 { get; set; }
        public ICommand CommandDev5 { get; set; }
        public ICommand CommandDev6 { get; set; }

        public ICommand Command30 { get; set; }

        //进入联机模式..
        private bool DeveclIsConnect = false;

        //进入工作模式..
        private bool DeveclIsWork = false;

        public void SetModelData(UserDevcelModel UserDevcelModel,
                                 PlcDevcelModel PlcDevcelModel,
                                 UserRecordModel UserRecordModel,
                                 UserInfoModel UserInfoModel                       
                                 )
        {
            m_UserDevcelModel = UserDevcelModel;            
            m_PlcDevcelModel = PlcDevcelModel;
            m_UserRecordModel = UserRecordModel;
            m_UserInfoModel = UserInfoModel;            
        }

        void UpDatePlc()
        {
            m_PlcDevcelModel.Plcidic3 = alldian_boolinput[23];
            m_PlcDevcelModel.Plcidic4 = alldian_boolinput[24];
            m_PlcDevcelModel.Plcidic5 = alldian_boolinput[25];
            m_PlcDevcelModel.Plcidic6 = alldian_boolinput[26];
            m_PlcDevcelModel.Plcidic7 = alldian_boolinput[27];
            
            m_PlcDevcelModel.Plcidid0 = alldian_boolinput[30];
            m_PlcDevcelModel.Plcidid1 = alldian_boolinput[31];
            m_PlcDevcelModel.Plcidid2 = alldian_boolinput[32];
            m_PlcDevcelModel.Plcidid3 = alldian_boolinput[33];
            m_PlcDevcelModel.Plcidid4 = alldian_boolinput[34];
            m_PlcDevcelModel.Plcidid5 = alldian_boolinput[35];
            m_PlcDevcelModel.Plcidid6 = alldian_boolinput[36];
            m_PlcDevcelModel.Plcidid7 = alldian_boolinput[37];

            m_PlcDevcelModel.Plcidie0 = alldian_boolinput[40];
            m_PlcDevcelModel.Plcidie1 = alldian_boolinput[41];
            m_PlcDevcelModel.Plcidie2 = alldian_boolinput[42];
        }

        void UpDateCom()
        {
            //三相平均电压..
            m_UserRecordModel.ATPVoltage = m_UserDevcelModel.Urms_vc;
            //三相有效值.电压..(和上一个一样..)
            m_UserRecordModel.RMSvalue = m_UserDevcelModel.Urms_vc;

            //三相平均电流..
            m_UserRecordModel.IRMSvalue = m_UserDevcelModel.Irms_ic;

            //空载损耗..
            m_UserRecordModel.NoloadLoss = m_UserDevcelModel.Pow_loss;

            //空载百分比..
            m_UserRecordModel.PercentageNoloadCurrent = m_UserDevcelModel.flt_UkIoPercent;

            //标准损耗是什么..
            float resultloss, resultuploss;
            float.TryParse(firstEntity.LossStandard, out resultloss);
            float.TryParse(firstEntity.LossStandardUp, out resultuploss);
            resultloss = resultloss + (resultloss / resultuploss);

            float resultcurrent, resultcurrentup;
            float.TryParse(firstEntity.NoloadCurrentStandard, out resultcurrent);
            float.TryParse(firstEntity.NoloadCurrentStandardUp, out resultcurrentup);
            resultcurrent = resultcurrent + (resultcurrent / resultcurrentup);


            //如果损耗大于标准损耗..
            if (m_UserRecordModel.NoloadLoss > resultloss || m_UserRecordModel.PercentageNoloadCurrent > resultcurrent)
            {
                m_UserRecordModel.QualifiedJudgment = "不合格";
            }
            else
            {
                m_UserRecordModel.QualifiedJudgment = "合格";
            }


        }



        public bool Close()
        {
            if (m_UserRecordModel != null)
            {

            }
            WeakReferenceMessenger.Default.UnregisterAll(this);
            cts.Cancel();
            return true;
        }

        void AddRecord(string strdata, bool e = false)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });
            });
            logger.Error(strdata);
        }

        bool SaveRecord()
        {
            Table_NoloadRocreadInfo temp = new Table_NoloadRocreadInfo();
            temp.ReportcheckStartTime = System.DateTime.Now;
            temp.ProductNumber = m_UserInfoModel.ProductNumber;
            temp.ProductType = m_UserInfoModel.ProductType;
            temp.ProductTuhao = m_UserInfoModel.ProductTuhao;
            temp.Highpressure = m_UserInfoModel.Highpressure;
            temp.Highcurrent = m_UserInfoModel.Highcurrent;
            temp.Lowpressure = m_UserInfoModel.Lowpressure;
            temp.Lowcurrent = m_UserInfoModel.Lowcurrent;

            temp.ATPVoltage = m_UserRecordModel.ATPVoltage.ToString();
            temp.RMSvalue = m_UserRecordModel.RMSvalue.ToString();
            temp.IRMSvalue = m_UserRecordModel.IRMSvalue.ToString();
            temp.LossStandard = m_UserRecordModel.NoloadLoss.ToString();
            temp.NoloadCurrentStandard = m_UserRecordModel.PercentageNoloadCurrent.ToString();
            temp.Qualified = m_UserRecordModel.QualifiedJudgment;

            try
            {
                using (var context = new MyDbContext())
                {                    
                    context.NoloadRocreadInfo.Add(temp);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {                        
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }



        }


        void ByteToBoolInput(byte[] alldian)
        {
            //Input起始地址100...
            int k = 0;
            //取PLC O-0 2000中频机
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[0], (byte)i);
                alldian_boolinput[k] = value;
                // 0 -- 7 
            }

            //取PLC O-0 2000中频机
            k = 10;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[1], (byte)i);
                alldian_boolinput[k] = value;
                // 8 -- 15
            }

            //取PLC O-8
            k = 20;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[2], (byte)i);
                alldian_boolinput[k] = value;
                //16 -- 23
            }

            //取PLC O-9
            k = 30;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[3], (byte)i);
                alldian_boolinput[k] = value;
                //24 -- 31
            }

            //取PLC O-12
            k = 40;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[4], (byte)i);
                alldian_boolinput[k] = value;
            }
        }

        public bool Initiate(Sampleinformation m_sampleinformation)
        {
        
            if (m_sampleinformation.ProductNumber == null || m_sampleinformation.ProductNumber.Length == 0)
            {
                MessageBox.Show("样品还没有配置,出产编号不能为空,初始化失败!");
                return false;
            }


            //这里保存了当前用户...
            m_UserInfoModel.ProductNumber = m_sampleinformation.ProductNumber;
            m_UserInfoModel.ProductType = m_sampleinformation.ProductType;
            m_UserInfoModel.ProductTuhao = m_sampleinformation.ProductTuhao;
            m_UserInfoModel.Highpressure = m_sampleinformation.Highpressure;
            m_UserInfoModel.Highcurrent = m_sampleinformation.Highcurrent;
            m_UserInfoModel.Lowpressure = m_sampleinformation.Lowpressure;
            m_UserInfoModel.Lowcurrent = m_sampleinformation.Lowcurrent;

            /*
             *这里不需要了..
            m_UserStandardModel.ProductType = firstEntity.ProductType;
            m_UserStandardModel.ProductTuhao = firstEntity.ProductTuhao;
            m_UserStandardModel.LossStandard = firstEntity.LossStandard;
            m_UserStandardModel.LossStandardUp = firstEntity.LossStandardUp;
            m_UserStandardModel.LossStandardDown = firstEntity.LossStandardDown;

            m_UserStandardModel.NoloadCurrentStandard = firstEntity.NoloadCurrentStandard;
            m_UserStandardModel.NoloadCurrentStandardUp = firstEntity.NoloadCurrentStandardUp;
            m_UserStandardModel.NoloadCurrentStandardDown = firstEntity.NoloadCurrentStandardDown;
            m_UserStandardModel.ImpedanceStandard = firstEntity.ImpedanceStandard;
            */

            AddRecord("用户产品和标准匹配完成..");


            WeakReferenceMessenger.Default.Register<MessageInit>(this, (r, user) =>
            {
              
                //这里只是处理串口消息..
                if (user.Message == "PortMessage_Connect") 
                {
                    AddRecord("联机完成!");
                    DeveclIsConnect = true;
                }

                if (user.Message == "PortMessage_ComeTestState")
                {
                    AddRecord("设备进入空载测试状态");
                    DeveclIsWork = true;
    }

                if (user.Message == "PortMessage_Stop")
                {                    
                    if (DeveclIsWork == true)
                    {
                        DeveclIsWork = false;
                        AddRecord("设备退出测试状态!");
                    }
                    else
                    {
                        if (DeveclIsConnect == true)
                        {
                            DeveclIsConnect = false;
                            AddRecord("设备退出联机状态!");
                        }
                    }
                }

                if (user.Message == "PortMessage_Error")
                {
                    AddRecord("设备返回错误!");
                }

                if (user.Message == "PortMessage_Data")
                {
                    AddRecord("数据成功返回!");
                    m_UserDevcelModel.Umn_ab = user.Data.fltVoltage[0];
                    m_UserDevcelModel.Urms_ab = user.Data.fltMeanVoltage[0];
                    m_UserDevcelModel.Irms_ab = user.Data.fltCurrent[0];
                    m_UserDevcelModel.P_ab = user.Data.fltPower[0];

                    m_UserDevcelModel.Umn_bc = user.Data.fltVoltage[1];
                    m_UserDevcelModel.Urms_bc = user.Data.fltMeanVoltage[1];
                    m_UserDevcelModel.Irms_bc = user.Data.fltCurrent[1];
                    m_UserDevcelModel.P_bc = user.Data.fltPower[1];

                    m_UserDevcelModel.Umn_ca = user.Data.fltVoltage[2];
                    m_UserDevcelModel.Urms_ca = user.Data.fltMeanVoltage[2];
                    m_UserDevcelModel.Irms_ca = user.Data.fltCurrent[2];
                    m_UserDevcelModel.P_ca = user.Data.fltPower[2];

                    m_UserDevcelModel.Urms_vc = user.Data.fltAverageVoltage;
                    m_UserDevcelModel.Irms_ic = user.Data.fltAverageCurrent;
                    m_UserDevcelModel.Is_pv = user.Data.fltFreq;
                    m_UserDevcelModel.Pow_loss = user.Data.fltMeasureSumPower;

                    m_UserDevcelModel.flt_UkIoPercent = user.Data.fltUkIoPercent;
                    m_UserDevcelModel.flt_CosQ = user.Data.fltCosQ;

                    UpDateCom();
                }

                if (user.Message == "PortMessage_UndefinedError")
                {
                    AddRecord("串口返回未知字符!",true);
                }
                
            });


            Task task = new Task(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;
                AddRecord("管理线程开始Init...");
                
                try
                {
                    //开始先去链接PLC..
                    //如果没有异常，就没事..
                    plc200.Open();
                    AddRecord("连接PLC成功！");
                    Plc = true;

                    serialPortHelper.Open();
                    AddRecord("打开串口成功！");
                    Dev = true;

                    /*
                    //开始先去链接设备..                    
                    master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                    master.Transport.ReadTimeout = 500;// 设置超时时间默认为500毫秒
                    serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_GLFXY"];
                    serialPort.Open(); //打开串口                            
                    AddRecord("连接设备OPEN成功！");
                    */

                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接PLC或者打开串口失败!" + ex.Message);
                    return;
                }
                

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

                            //读PLC 输入点
                            ByteToBoolInput(plc200.ReadBytes(DataType.Input, 0, 0, 5));
                            //刷新显示..
                            UpDatePlc();
                            if (DeveclIsWork)
                            {
                                //52 53 03 04 00 00 07 00 45 44
                                ExeportCommand(3);
                                //UpDateCom();
                            }

                            precurrentTime = currentTime;
                            continue;
                        }
                        if (port_queue.Count() >= 1)
                        {
                             int command = port_queue.Dequeue();
                             ExeportCommand(command);
                             precurrentTime = System.DateTime.UtcNow;
                        }
                    }
                    catch (Exception ex)
                    {
                        AddRecord($"管理线程异常:{ex.Message}");
                        return;
                    }
                }
            }, cts.Token);

            //previousTask 前一个任务..
            //ContinueWith 方法会启动一个新的任务
            //以下为监控代码.. 监控前一个任务是不是完成..
            task.ContinueWith((previousTask) =>
            {
                //这里还有个问题，就是异常也会这样...
                Debug.WriteLine("主线程回调");
                //这里返回了..
                //注意这里回来的主线程..--有个问题，如果线程超时了呢?                
            }, TaskScheduler.FromCurrentSynchronizationContext());            
            //任务开始执行..
            task.Start();
            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");
            return true;
        }

        void ExeportCommand(int command)
        {
            try
            {
                switch (command)
                {
                    case 1:
                        {
                            //联机..
                            string hexData = "52530301000004004544"; // 示例16进制字符串
                            byte[] bytes = Convert.FromHexString(hexData); // 直接转换
                            serialPortHelper.cuucentcommand = command;
                            serialPortHelper.Write(bytes, 0, bytes.Length); // 发送数据
                        }
                        break;
                    case 2:
                        {
                            //开始空载(不连续)                            
                            string hexData = "525303130200000018004544"; // 示例16进制字符串            
                            byte[] bytes = Convert.FromHexString(hexData); // 直接转换
                            serialPortHelper.cuucentcommand = command;
                            serialPortHelper.Write(bytes, 0, bytes.Length); // 发送数据
                        }
                        break;
                    case 3:
                        {
                            //读取数据...                            
                            string hexData = "52530304000007004544"; // 示例16进制字符串
                            byte[] bytes = Convert.FromHexString(hexData); // 直接转换
                            serialPortHelper.cuucentcommand = command;
                            serialPortHelper.Write(bytes, 0, bytes.Length); // 发送数据                                                                                        
                        }
                        break;
                    case 4:
                        {
                            //结束测试
                            string hexData = "5253030D000010004544"; // 示例16进制字符串
                            byte[] bytes = Convert.FromHexString(hexData); // 直接转换
                            serialPortHelper.cuucentcommand = command;
                            serialPortHelper.Write(bytes, 0, bytes.Length); // 发送数据            
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                AddRecord("执行串口指令异常" + ex.Message);
            }
        }

        public Noload()
        {
            
            Command1 = new RelayCommand<object>(DoCommand1);
            Command2 = new RelayCommand<object>(DoCommand2);
            Command3 = new RelayCommand<object>(DoCommand3);
            Command4 = new RelayCommand<object>(DoCommand4);
            Command5 = new RelayCommand<object>(DoCommand5);
            Command6 = new RelayCommand<object>(DoCommand6);
            Command7 = new RelayCommand<object>(DoCommand7);
            Command8 = new RelayCommand<object>(DoCommand8);
            Command9 = new RelayCommand<object>(DoCommand9);
            Command10 = new RelayCommand<object>(DoCommand10);
            Command11 = new RelayCommand<object>(DoCommand11);
            Command12 = new RelayCommand<object>(DoCommand12);

            Command25A = new RelayCommand<object>(DoCommand25A);
            Command50A = new RelayCommand<object>(DoCommand50A);
            Command100A = new RelayCommand<object>(DoCommand100A);
            Command200A = new RelayCommand<object>(DoCommand200A);
            

            Command13 = new RelayCommand<object>(DoCommand13);
            Command14 = new RelayCommand<object>(DoCommand14);

            Command5A = new RelayCommand<object>(DoCommand5A);
            Command10A = new RelayCommand<object>(DoCommand10A);

            CommandDev1 = new RelayCommand<object>(DoCommandDev1);
            CommandDev2 = new RelayCommand<object>(DoCommandDev2);
            CommandDev3 = new RelayCommand<object>(DoCommandDev3);
            CommandDev4 = new RelayCommand<object>(DoCommandDev4);
            CommandDev5 = new RelayCommand<object>(DoCommandDev5);
            CommandDev6 = new RelayCommand<object>(DoCommandDev6);


            Command30 = new RelayCommand<object>(DoCommand30); 

            /*
            MessageInit message = new MessageInit();
            message.Message = "Noloadinit";
            WeakReferenceMessenger.Default.Send(message);
            alldian_boolinput = (bool[])message.alldian_boolinputobj;            
            Table_ProductInfo productInfo = (Table_ProductInfo)message.Tab_data;

            //1.初始化.. 用户信息..
            if (m_UserInfoModel == null)
            {
                MessageBox.Show("m_UserInfoModel为空!");
                return;
            }
            */

        }

        private bool is_wordk = false;

        private async void ExePlcCommand(int command)
        {
            if (is_wordk == true)
            {
                AddRecord("正在执行其他指令,请等待...",true);
                return ;
            }
            is_wordk = true;
            AddRecord("开始执行PLC指令...");            
            await Task.Run(async () =>
            {
                
                try
                {
                    switch (command)
                    {
                        case 100: Thread.Sleep(2000); break;
                        case 1:
                            {
                                //升压
                                plc200.Write("M12.1", true);
                                Thread.Sleep(1000);                                
                                if (timeSelect == 1) Thread.Sleep(2000);
                                if (timeSelect == 2) Thread.Sleep(5000);
                                if (timeSelect == 3) Thread.Sleep(8000);
                                plc200.Write("M12.1", false);
                            }
                            break;
                        case 2:
                            {
                                //降压
                                plc200.Write("M12.0", true);
                                Thread.Sleep(1000);
                                if (timeSelect == 1) Thread.Sleep(2000);
                                if (timeSelect == 2) Thread.Sleep(5000);
                                if (timeSelect == 3) Thread.Sleep(8000);
                                plc200.Write("M12.0", false);
                            }
                            break;
                        case 3:
                            {
                                //电源合闸
                                plc200.Write("M11.7", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.7", false);
                            }
                            break;
                        case 4:
                            {
                                //电源分闸
                                plc200.Write("M11.6", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.6", false);
                            }
                            break;
                        case 5:
                            {
                                //电动机启动
                                plc200.Write("M10.7", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.7", false);
                            }
                            break;
                        case 6:
                            {
                                //电动机停止
                                plc200.Write("M10.6", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.6", false);
                            }
                            break;
                        case 7:
                            {
                                //特性回路
                                plc200.Write("M10.3", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.3", false);
                            }
                            break;
                        case 8:
                            {
                                //工频回路
                                plc200.Write("M10.5", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.5", false);
                            }
                            break;
                        case 9:
                            {
                                //急停
                                plc200.Write("M10.0", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.0", false);
                            }
                            break;
                        case 10:
                            {
                                //感应回路
                                plc200.Write("M10.4", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.4", false);
                            }
                            break;
                        case 11:
                            {
                                //复位
                                plc200.Write("M10.2", true);
                                Thread.Sleep(500);
                                plc200.Write("M10.2", false);
                            }
                             break;
                        case 12:
                            {
                                //保存数据
                                if (SaveRecord())
                                {
                                    AddRecord("保存记录成功!");
                                }
                                else
                                {
                                    AddRecord("保存记录失败!", true);
                                }
                            }
                            break;

                        case 50:
                            {
                                //5A
                                plc200.Write("M11.0", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.0", false);
                            }
                            break;
                        case 51:
                            {
                                //10A
                                plc200.Write("M11.1", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.1", false);
                            }
                            break;
                        case 52:
                            {
                                //25A
                                plc200.Write("M11.2", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.2", false);
                            }
                            break;
                        case 53:
                            {
                                //50A
                                plc200.Write("M11.3", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.3", false);
                            }

                            break;
                        case 54:
                            {
                                //200A
                                plc200.Write("M11.4", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.4", false);
                            }
                            break;
                        case 55:
                            {
                                //100A
                                plc200.Write("M11.5",true);
                                Thread.Sleep(500);
                                plc200.Write("M11.5", false);
                            }
                            break;

                    }                                    
                    return;
                }
                catch (Exception ex)
                {
                    //总异常:                    
                    AddRecord("工作线程-执行PLC指令异常:" + ex.Message);
                    return;
                }
            });

            AddRecord("执行PLC指令完成...");
            //这里有个问题，就是主线程现在在这里等着...

            //var completedTask = await Task.WhenAny(timeout, t);
            //if (completedTask == timeout)
            //{
            //AddRecord("执行PLC指令超时12秒,请检查设备是否正常!");                
            //}
            is_wordk = false;
            return ;
        }

        private void DoCommand5A(object param) { ExePlcCommand(50); }
        private void DoCommand10A(object param) {  ExePlcCommand(51); }
        
        private void DoCommand25A(object param) {  ExePlcCommand(52); }
        private void DoCommand50A(object param) {  ExePlcCommand(53); }
        private void DoCommand100A(object param) {  ExePlcCommand(54); }
        private void DoCommand200A(object param) {  ExePlcCommand(55); }


        private void DoCommand1(object param) {  ExePlcCommand(1); }
        private void DoCommand2(object param) {  ExePlcCommand(2); }
        private void DoCommand3(object param) {  ExePlcCommand(3); }
        private void DoCommand4(object param) {  ExePlcCommand(4); }
        private void DoCommand5(object param) {  ExePlcCommand(5); }
        private void DoCommand6(object param) {  ExePlcCommand(6); }
        private void DoCommand7(object param) {  ExePlcCommand(7);  }
        private void DoCommand8(object param) {  ExePlcCommand(8); }
        private void DoCommand9(object param) {  ExePlcCommand(9); }
        private void DoCommand10(object param) {  ExePlcCommand(10); }
        private void DoCommand11(object param) {  ExePlcCommand(11); }
        private void DoCommand12(object param) {  ExePlcCommand(12); }
        

        private async void DoCommand13(object param) {  }
        private async void DoCommand14(object param) { }
        private async void DoCommandDev1(object param) 
        {
            //联机..
            port_queue.Enqueue(1); //入队..
        }

        private async void DoCommandDev2(object param)
        {
            if (DeveclIsConnect == false)
            {
                MessageBox.Show("必须先进行联机!");
                return;
            }

            if (DeveclIsWork == true)
            {
                MessageBox.Show("当前已是,空载测试状态..");
                return;
            }
            port_queue.Enqueue(2); //入队..
        }

        private async void DoCommandDev3(object param) 
        {
            //开始负载
            //string hexData = "525303030200010009004544"; // 示例16进制字符串
            //string hexData = "525303130200010019004544"; // 示例16进制字符串            
            //byte[] bytes = Convert.FromHexString(hexData); // 直接转换
            //serialPortHelper.Write(bytes, 0, bytes.Length); // 发送数据
            //返回: 52 53 03 03 4F 4B A0 00 45 44 然后返回自动一直返回空载结果(负载结果)
        }
        private async void DoCommandDev4(object param) 
        {
            //退出...
            port_queue.Enqueue(4); //入队..
        }

        private async void DoCommandDev5(object param) 
        {
            //设置参数,具体参数见通讯协议
            //设置参数,具体参数见通讯协议
            //发送: 52 53 03 02 28 00 00 00 20 41 D7 A3 B8 40 CD CC CC 3E CD 4C 10 43 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F FF 00 64 00 00 00 00 00 F0 09 45 44
            //返回: 52 53 03 02 4F 4B 9F 00 45 44
        }
        private async void DoCommandDev6(object param)         
        {

        }


        private async void DoCommand30(object param)         
        {
            //这里去编译制动化...
            queue.Enqueue(1); //入队;
            queue.Enqueue(100); //入队;--等1秒...

            while (queue.Count > 0)
            {
                int command = queue.Dequeue(); //出列;离队
                ExePlcCommand(command);
            }

        }

    }

    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }
}