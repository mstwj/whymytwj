using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Modbus.Device;
using 空载负载.Table;
using System.Configuration;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using CommunityToolkit.Mvvm.Messaging;
using S7.Net;
using 空载负载.Base;
using S7.Net.Types;

using System.Diagnostics;
using NLog.Time;
using System.Windows.Controls;


namespace 空载负载.Model
{
    public class Noload : ObservableObject
    {
        public int selectbiaofa { get; set; } = 1;

        public float gaoyacezhizu { get; set; }  // 高压

        public float diyacezhizu { get; set; }// 低压

        public byte zhizuwendu { get; set; } //直租温度

        public byte shipinwendu { get; set; } //试品温度

        public ushort mubiaowendu { get; set; } //校正的目标温度

        //试验电压..
        public float TextV { get; set; }

        //手动或者程控
        public int IsAutoOrMu { get; set; }

        private float protectVoltage;
        //保护电压.
        public float ProtectVoltage { get => protectVoltage; set { SetProperty(ref protectVoltage, value); } }

        //保护电流.
        public float ProtectCurrent { get; set; }


        private Plc plc200 { get; set; } = new Plc(CpuType.S7200Smart, "192.168.2.1", 0, 1);
        private SerialPortHelper serialPortHelper { get; set; } = new SerialPortHelper(
           ConfigurationManager.AppSettings["DEVICE_GLFXY"], 19200, Parity.None, 8, StopBits.One);

        private CancellationTokenSource cts = new CancellationTokenSource();

        private Table_ProductInfo m_sampleinformation { get; set; }
        private Table_NoloadStandardInfo m_sampleinNoload { get; set; }

        private UserRecordModel m_UserRecordModel { get; set; }
        private UserStandardModel m_UserStandardModel { get; set; }
        private UserMainModel m_UserMainModel { get; set; }

        private PlcStateModel m_PlcStateModel { get; set; }
        public bool[] alldian_boolinput { get; set; } = new bool[50];

        private UserCommunicationModel m_UserCommunicationModel { get; set; }

        private bool StartScanSing = false;
        private bool DeveclIsConnect = false;
        private bool DeveclIsWork = false;
        private bool DevecSetParam = false;

        public Queue<int> queue { get; set; } = new Queue<int>();

        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();
        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public WaitWindow waitWindow { get; set; } = null;

        public int timeSelect { get; set; } = 0;
        public ICommand CommandPlc1 { get; set; }
        public ICommand CommandPlc2 { get; set; }
        public ICommand CommandPlc3 { get; set; }
        public ICommand CommandPlc4 { get; set; }
        public ICommand CommandPlc5 { get; set; }
        public ICommand CommandPlc6 { get; set; }
        public ICommand CommandPlc7 { get; set; }
        public ICommand CommandPlc8 { get; set; }
        public ICommand CommandPlc9 { get; set; }
        public ICommand CommandPlc10 { get; set; }
        public ICommand CommandPlc11 { get; set; }
        public ICommand CommandPlc12 { get; set; }

        public ICommand CommandPlc50 { get; set; }
        public ICommand CommandPlc51 { get; set; }
        public ICommand CommandPlc52 { get; set; }
        public ICommand CommandPlc53 { get; set; }
        public ICommand CommandPlc54 { get; set; }
        public ICommand CommandPlc55 { get; set; }
        public ICommand CommandPlc56 { get; set; }


        public ICommand CommandDev1 { get; set; }
        public ICommand CommandDev2 { get; set; }
        public ICommand CommandDev3 { get; set; }

        public ICommand CommandDev4 { get; set; }
        public ICommand CommandDev5 { get; set; }



        public ICommand BtnCommandStart { get; set; }
        public ICommand BtnCommandStop { get; set; }

        public ICommand BtnCommandSave { get; set; }

        public ICommand BtnCommandQuit { get; set; }

        public ICommand BtnCommandConnect { get; set; }
        public ICommand BtnCommandComin { get; set; }

        public ICommand CommandAuto { get; set; }
        public ICommand CommandAutoStop { get; set; }

        public ICommand CommandK { get; set; }

        public ICommand CommandM { get; set; }




        //第2....
        public void Initiate(Table_ProductInfo table_ProductInfo, Table_NoloadStandardInfo table_NoloadStandardInfo)
        {
            m_sampleinformation = table_ProductInfo;
            m_sampleinNoload = table_NoloadStandardInfo;

            m_UserStandardModel.ProductType = m_sampleinNoload.ProductType;
            m_UserStandardModel.ProductTuhao = m_sampleinNoload.ProductTuhao;
            m_UserStandardModel.ProductStandard = m_sampleinNoload.ProductStandard;
            m_UserStandardModel.ProductCurrentStandard = m_sampleinNoload.ProductCurrentStandard;

            //这里才开始管理线程...
            //InitializationManager();
        }

        //第一
        public void InitiateModel(UserRecordModel userRecordModel,
                                  UserStandardModel userStandardModel,
                                  UserMainModel userMainModel,
                                  UserCommunicationModel userCommunicationModel,
                                  PlcStateModel plcStateModel)
        {
            m_UserRecordModel = userRecordModel;
            m_UserStandardModel = userStandardModel;
            m_UserMainModel = userMainModel;
            m_UserCommunicationModel = userCommunicationModel;
            m_PlcStateModel = plcStateModel;

        }

        public Noload()
        {
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
                        DeveclIsConnect = false;
                    } else DeveclIsConnect = false;
                }

                if (user.Message == "PortMessage_Error")
                {
                    AddRecord("设备返回错误!", true);
                }

                if (user.Message == "PortMessage_Data")
                {
                    m_UserMainModel.Umn_ab = user.Data.fltVoltage[0];
                    m_UserMainModel.Urms_ab = user.Data.fltMeanVoltage[0];
                    m_UserMainModel.Irms_ab = user.Data.fltCurrent[0];
                    m_UserMainModel.P_ab = user.Data.fltPower[0];

                    m_UserMainModel.Umn_bc = user.Data.fltVoltage[1];
                    m_UserMainModel.Urms_bc = user.Data.fltMeanVoltage[1];
                    m_UserMainModel.Irms_bc = user.Data.fltCurrent[1];
                    m_UserMainModel.P_bc = user.Data.fltPower[1];

                    m_UserMainModel.Umn_ca = user.Data.fltVoltage[2];
                    m_UserMainModel.Urms_ca = user.Data.fltMeanVoltage[2];
                    m_UserMainModel.Irms_ca = user.Data.fltCurrent[2];
                    m_UserMainModel.P_ca = user.Data.fltPower[2];


                    user.Data.fltAverageVoltage = (float)Math.Round(user.Data.fltAverageVoltage, 3); // 保留3位小数
                    user.Data.fltAverageCurrent = (float)Math.Round(user.Data.fltAverageCurrent, 3); // 保留3位小数
                    user.Data.fltMeasureSumPower = (float)Math.Round(user.Data.fltMeasureSumPower, 3); // 保留3位小数;
                    user.Data.fltUkIoPercent = (float)Math.Round(user.Data.fltUkIoPercent, 3); // 保留3位小数;                    
                    user.Data.fltCosQ = (float)Math.Round(user.Data.fltCosQ, 3); // 保留3位小数;                    
                    user.Data.fltFreq = (float)Math.Round(user.Data.fltFreq, 3);

                    m_UserMainModel.Urms_vc = user.Data.fltAverageVoltage;

                    if (m_UserMainModel.Urms_vc >= ProtectVoltage)
                    {
                        AddRecord("请注意:已经大于保护电压!", true);
                        AddRecord("电源被强制复位...", true);
                        //这里强制停止升压
                        plc200.Write("M12.1", false);
                        Thread.Sleep(100);
                        //这里强制复位..
                        plc200.Write("M10.2", true);
                        Thread.Sleep(200);
                        plc200.Write("M10.2", false);

                    }

                    m_UserMainModel.Irms_ic = user.Data.fltAverageCurrent;
                    m_UserMainModel.Is_pv = user.Data.fltFreq;
                    m_UserMainModel.Pow_loss = user.Data.fltMeasureSumPower;

                    m_UserMainModel.flt_UkIoPercent = user.Data.fltUkIoPercent;
                    m_UserMainModel.flt_CosQ = user.Data.fltCosQ;

                    ////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////                    
                    m_UserRecordModel.ATPVoltage = user.Data.fltAverageVoltage;
                    m_UserRecordModel.RMSvalue = user.Data.fltAverageVoltage;
                    m_UserRecordModel.IRMSvalue = user.Data.fltAverageCurrent;
                    m_UserRecordModel.PercentageNoloadCurrent = user.Data.fltUkIoPercent;
                    m_UserRecordModel.NoloadLoss = user.Data.fltMeasureSumPower;


                    //合格
                    float loss, lossup;
                    float.TryParse(m_UserStandardModel.ProductStandard, out loss);
                    float.TryParse(m_UserStandardModel.ProductStandardUpperimit, out lossup);
                    loss = loss + (loss / lossup);

                    float losscurrent, losslosscurrentup;
                    float.TryParse(m_UserStandardModel.ProductCurrentStandard, out losscurrent);
                    float.TryParse(m_UserStandardModel.ProductCurrentStandardUpperrimit, out losslosscurrentup);

                    losscurrent = losscurrent + (losscurrent / losslosscurrentup);

                    if (user.Data.fltMeasureSumPower < loss && user.Data.fltUkIoPercent < losscurrent)
                        m_UserRecordModel.QualifiedJudgment = "合格";
                    else
                        m_UserRecordModel.QualifiedJudgment = "不合格";
                }

                if (user.Message == "PortMessage_Set")
                {
                    AddRecord("设备设置成功!");
                    DevecSetParam = true;
                }


                if (user.Message == "PortMessage_UndefinedError")
                {
                    AddRecord("串口返回未知字符!", true);
                }

            });


            CommandPlc1 = new RelayCommand<object>(DoCommandPlc1);
            CommandPlc2 = new RelayCommand<object>(DoCommandPlc2);
            CommandPlc3 = new RelayCommand<object>(DoCommandPlc3);
            CommandPlc4 = new RelayCommand<object>(DoCommandPlc4);
            CommandPlc5 = new RelayCommand<object>(DoCommandPlc5);
            CommandPlc6 = new RelayCommand<object>(DoCommandPlc6);
            CommandPlc7 = new RelayCommand<object>(DoCommandPlc7);
            CommandPlc8 = new RelayCommand<object>(DoCommandPlc8);
            CommandPlc9 = new RelayCommand<object>(DoCommandPlc9);
            CommandPlc10 = new RelayCommand<object>(DoCommandPlc10);
            CommandPlc11 = new RelayCommand<object>(DoCommandPlc11);
            CommandPlc12 = new RelayCommand<object>(DoCommandPlc12);

            CommandPlc50 = new RelayCommand<object>(DoCommandPlc50);
            CommandPlc51 = new RelayCommand<object>(DoCommandPlc51);
            CommandPlc52 = new RelayCommand<object>(DoCommandPlc52);
            CommandPlc53 = new RelayCommand<object>(DoCommandPlc53);
            CommandPlc54 = new RelayCommand<object>(DoCommandPlc54);
            CommandPlc55 = new RelayCommand<object>(DoCommandPlc55);

            //CommandDev1 = new RelayCommand<object>(DoCommandDev1);
            //CommandDev2 = new RelayCommand<object>(DoCommandDev2);
            //CommandDev3 = new RelayCommand<object>(DoCommandDev3);
            //CommandDev4 = new RelayCommand<object>(DoCommandDev4);
            //CommandDev5 = new RelayCommand<object>(DoCommandDev5);


            BtnCommandStart = new RelayCommand<object>(DoBtnCommandStart);
            BtnCommandStop = new RelayCommand<object>(DoBtnCommandStop);
            BtnCommandSave = new RelayCommand<object>(DoBtnCommandSave);

            BtnCommandConnect = new RelayCommand<object>(DoBtnCommandConnect);
            BtnCommandComin = new RelayCommand<object>(DoBtnCommandComin);
            BtnCommandQuit = new RelayCommand<object>(DoBtnCommandQuit);

            CommandAuto = new RelayCommand<object>(DoCommandAuto);
            CommandAutoStop = new RelayCommand<object>(DoCommandAutoStop);

            CommandK = new RelayCommand<object>(DoCommandK);
            CommandM = new RelayCommand<object>(DoCommandM);


            return;
        }

        private void ExeCommand(int command)
        {
            if (ManThreadIsWork == true)
            {
                MessageBox.Show("上一条指令还在执行中,请等待!");
                return;
            }
            if (queue.Count > 0)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成");
                return;
            }
            queue.Enqueue(command);

            return;
        }

        private void DoCommandPlc1(object param) { ExeCommand(1); }
        private void DoCommandPlc2(object param) { ExeCommand(2); }
        private void DoCommandPlc3(object param) { ExeCommand(3); }
        private void DoCommandPlc4(object param) { ExeCommand(4); }
        private void DoCommandPlc5(object param) { ExeCommand(5); }
        private void DoCommandPlc6(object param) { ExeCommand(6); }
        private void DoCommandPlc7(object param) { ExeCommand(7); }
        private void DoCommandPlc8(object param) { ExeCommand(8); }
        private void DoCommandPlc9(object param) { ExeCommand(9); }
        private void DoCommandPlc10(object param) { ExeCommand(10); }
        private void DoCommandPlc11(object param) { ExeCommand(11); }
        private void DoCommandPlc12(object param) { ExeCommand(12); }

        private void DoCommandPlc50(object param) { ExeCommand(50); }
        private void DoCommandPlc51(object param) { ExeCommand(51); }
        private void DoCommandPlc52(object param) { ExeCommand(52); }
        private void DoCommandPlc53(object param) { ExeCommand(53); }
        private void DoCommandPlc54(object param) { ExeCommand(54); }
        private void DoCommandPlc55(object param) { ExeCommand(55); }

        //特殊指令1
        public void DoTbCommmandUp1()
        {
            if (ManThreadIsWork == true)
            {
                MessageBox.Show("上一条指令还在执行中,请等待!");
                return;
            }
           
            
            plc200.Write("M12.0", true);

        }


        //特殊指令2
        public void DoTbCommmandUp2()
        {
            if (ManThreadIsWork == true)
            {
                MessageBox.Show("上一条指令还在执行中,请等待!");
                return;
            }
            plc200.Write("M12.1", true);

        }

        public void DoTbCommmandDown1()
        {
            if (ManThreadIsWork == true)
            {
                MessageBox.Show("上一条指令还在执行中,请等待!");
                return;
            }
            plc200.Write("M12.0", false);
        }

        public void DoTbCommmandDown2()
        {
            if (ManThreadIsWork == true)
            {
                MessageBox.Show("上一条指令还在执行中,请等待!");
                return;
            }
            plc200.Write("M12.1", false);
        }

        private void DoBtnCommandStart(object param) {
            if (DeveclIsWork == false)
            {
                MessageBox.Show("请先设置设备为空载工作状态!");
                return;
            }
            StartScanSing = true;
        }
        private void DoBtnCommandStop(object param) { StartScanSing = false; }
        private void DoBtnCommandSave(object param) { ExeCommand(62); }

        private void DoBtnCommandQuit(object param) { ExeCommand(104); }

        private void DoBtnCommandComin(object param) {
            if (ProtectVoltage < 10)
            {
                MessageBox.Show("必须先设设置保护电压!");
                return;
            }
            ExeCommand(102); 
        }

        private void DoBtnCommandConnect(object param) { ExeCommand(101); }


        private bool isauto = false;

        private bool isautosave = false;

        private void DoCommandAuto(object param)         
        {
            if (isauto == true)
            {
                MessageBox.Show("已经在走自动流程,请等待流程结束!");
                return;
            }

            Task task = new Task(async () =>
            {
                if (isauto == false) 
                    isauto = true;
                //这里判断.. 设备必须进入空载..
                if (DeveclIsWork == false)
                {
                    MessageBox.Show("设备必须进入空载状态..");
                    return;
                }

                //这里判断，必须是程控..
                if (IsAutoOrMu != 1)
                {
                    MessageBox.Show("请选择,程控..");
                    return;
                }

                //这里判断吧必须是已经是回路状态..
                if (m_PlcStateModel.Plcidid4 == false)
                {
                    MessageBox.Show("请先选择,特性回路..");
                    return;
                }


                if (ProtectCurrent == 0)
                {
                    MessageBox.Show("错误,保护电流为0");
                    return;
                }

                if (TextV == 0)
                {
                    MessageBox.Show("错误,试验电压为0");
                    return;
                }



                //ProtectVoltage = (float)(TextV * 1.2);

                //零位判断..
                if (m_PlcStateModel.Plcidic4 == false)
                {
                    MessageBox.Show("错误,升压必须先到零位!");
                    return;
                }

                if (m_PlcStateModel.Plcidie0 == false)
                {
                    MessageBox.Show("错误,电源必须是合闸状态!");
                    return;
                }


                AddRecord("开始自动....");
   
                
                AddRecord("升压..");
                Thread.Sleep(500);
                plc200.Write("M12.1", true);


                while (true)
                {

                    if(m_UserMainModel.Irms_ic > ProtectCurrent)
                    {
                        //强制停止升压..
                        plc200.Write("M12.1", false);
                        AddRecord("严重错误,高于保护电流,强制停止升压...");
                        //强制停止升压..
                        plc200.Write("M12.1", false);
                        Thread.Sleep(100);
                        //强制停止复位..
                        plc200.Write("M10.2", true);
                        Thread.Sleep(200);
                        plc200.Write("M10.2", false);
                        return;
                    }

                    if (isauto == false)
                    {
                        //强制停止升压..
                        plc200.Write("M12.1", false);
                        AddRecord("强制停止升压...");
                        return;
                    }
                    //平均电压..
                    if (m_UserMainModel.Urms_vc >= TextV - 7)
                    {
                        //这里就不需要在升压了..
                        plc200.Write("M12.1", false);
                        break;
                    }

                    if (m_UserMainModel.Urms_vc >= ProtectVoltage)
                    {
                        AddRecord("严重错误,高于保护电压,被强制复位...",true);
                        //强制停止升压..
                        plc200.Write("M12.1", false);
                        //这里强制复位..
                        plc200.Write("M10.2", true);
                        Thread.Sleep(200);
                        plc200.Write("M10.2", false);
                        return;
                    }
                    Thread.Sleep(500);
                }


                AddRecord("到达指定电压..");
                Thread.Sleep(500);
                isautosave = false;
                if (SaveRecord() == true)
                {
                    AddRecord("保存数据成功!");
                    isautosave = true;

                }
                else
                {
                    AddRecord("保存数据失败!", true);
                }


                AddRecord("降压..");
                Thread.Sleep(500);
                plc200.Write("M12.0", true);
                while (true)
                {
                    if (isauto == false)
                    {
                        plc200.Write("M12.0", false);
                        return;
                    }
                    //零位判断..        Plcidid7
                    if (m_PlcStateModel.Plcidic4 == true)
                    {
                        plc200.Write("M12.0", false);
                        break;
                    }
                    Thread.Sleep(1000);
                }

                AddRecord("已经到达零位..");

                plc200.Write("M11.6", true);
                Thread.Sleep(500);
                plc200.Write("M11.6", false);

                AddRecord("电源分闸..");

            });

            task.ContinueWith((previousTask) =>
            {
                //这里还有个问题，就是异常也会这样...
                Debug.WriteLine("主线程回调");
                if (isautosave ==true )
                {
                    MessageBox.Show("数据保存成功,试验已完成!");
                }
                AddRecord("自动流程结束...");
                //这里返回了..
                //注意这里回来的主线程..--有个问题，如果线程超时了呢?                
                isauto = false;

            }, TaskScheduler.FromCurrentSynchronizationContext());
            //任务开始执行..
            task.Start();

            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");

        }

        private void DoCommandAutoStop(object param) {
            if (isauto == true) isauto = false;
        }

        private void DoCommandK(object param) { ExeCommand(13); }
        private void DoCommandM(object param) { ExeCommand(14); }




        void UpDatePlc()
        {
            m_PlcStateModel.Plcidic3 = alldian_boolinput[23];
            m_PlcStateModel.Plcidic4 = alldian_boolinput[24];
            m_PlcStateModel.Plcidic5 = alldian_boolinput[25];
            m_PlcStateModel.Plcidic6 = alldian_boolinput[26];
            m_PlcStateModel.Plcidic7 = alldian_boolinput[27];

            m_PlcStateModel.Plcidid0 = alldian_boolinput[30];
            m_PlcStateModel.Plcidid1 = alldian_boolinput[31];
            m_PlcStateModel.Plcidid2 = alldian_boolinput[32];
            m_PlcStateModel.Plcidid3 = alldian_boolinput[33];
            m_PlcStateModel.Plcidid4 = alldian_boolinput[34];
            m_PlcStateModel.Plcidid5 = alldian_boolinput[35];
            m_PlcStateModel.Plcidid6 = alldian_boolinput[36];
            m_PlcStateModel.Plcidid7 = alldian_boolinput[37];

            m_PlcStateModel.Plcidie0 = alldian_boolinput[40];
            m_PlcStateModel.Plcidie1 = alldian_boolinput[41];
            m_PlcStateModel.Plcidie2 = alldian_boolinput[42];

            if (alldian_boolinput[49] == true)
            {
                m_PlcStateModel.Plck = true;
                m_PlcStateModel.Plcm = false;
            }

            if (alldian_boolinput[49] == false)
            {
                m_PlcStateModel.Plcm = true;
                m_PlcStateModel.Plck = false;
            }

        }


        void ExeportCommand(int command, float dianliudata = 0)
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
                    case 5:
                        {
                            //设置设备
                            myClass myClasstest = new myClass();
                            float Highpressure, Highcurrent, Lowpressure, Lowcurrent;
                            ushort ProductCapacity;
                            byte PhaseNumber = 0;

                            if (!float.TryParse(m_sampleinformation.Highpressure, out Highpressure))
                            {
                                MessageBox.Show("转换失败,请检查高压侧电压！");
                                return;
                            }

                            if (!float.TryParse(m_sampleinformation.Highcurrent, out Highcurrent))
                            {
                                MessageBox.Show("转换失败,请检查高压侧电流！");
                                return;
                            }

                            if (!float.TryParse(m_sampleinformation.Lowpressure, out Lowpressure))
                            {
                                MessageBox.Show("转换失败,请检查低压侧电压！");
                                return;
                            }

                            if (!float.TryParse(m_sampleinformation.Lowcurrent, out Lowcurrent))
                            {
                                MessageBox.Show("转换失败,请检查低压侧电流！");
                                return;
                            }
                            if (!ushort.TryParse(m_sampleinformation.ProductCapacity, out ProductCapacity))
                            {
                                MessageBox.Show("转换失败,请检查容量！");
                                return;
                            }

                            if (m_sampleinformation.PhaseNumber == "单相") PhaseNumber = 0;
                            if (m_sampleinformation.PhaseNumber == "三相") PhaseNumber = 1;                

                            myClasstest.Highpressure = Highpressure;
                            myClasstest.Highcurrent = Highcurrent;
                            myClasstest.Lowpressure = Lowpressure;
                            myClasstest.Lowcurrent = Lowcurrent;
                            myClasstest.gaoyacezhizu = gaoyacezhizu;
                            myClasstest.diyacezhizu = diyacezhizu;
                            myClasstest.Data7 = 1;
                            myClasstest.Dianliudata = dianliudata;
                            myClasstest.PhaseNumber = PhaseNumber;
                            if (selectbiaofa == 0)
                            {
                                myClasstest.selectbiaofa = 0;
                            }
                            else 
                            {
                                myClasstest.selectbiaofa = 0x0f;
                            }
                            
                            
                            myClasstest.ProductCapacity = ProductCapacity;
                            myClasstest.zhizuwendu = zhizuwendu;
                            myClasstest.shipinwendu = shipinwendu;
                            myClasstest.mubiaowendu = mubiaowendu;
                            myClasstest.Conversion();

                            //string hexData = "5253030D000010004544"; // 示例16进制字符串
                            //byte[] bytes = Convert.FromHexString(hexData); // 直接转换
                            serialPortHelper.cuucentcommand = command;
                            //serialPortHelper.Write(bytes, 0, bytes.Length); // 发送数据            
                            serialPortHelper.Write(myClasstest.destinationallcac, 0, myClasstest.destinationallcac.Length); // 发送数据            
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                AddRecord("执行串口指令异常" + ex.Message);
            }
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


            var p = plc200.Read("M13.2");
            alldian_boolinput[49] = (bool)p;

        }

        bool SaveRecord()
        {
            Table_NoloadRocreadInfo temp = new Table_NoloadRocreadInfo();
            temp.ReportcheckStartTime = System.DateTime.Now;
            temp.ProductNumber = m_sampleinformation.ProductNumber;
            temp.ProductType = m_sampleinformation.ProductType;
            temp.ProductTuhao = m_sampleinformation.ProductTuhao;
            temp.ProductCapacity = m_sampleinformation.ProductCapacity;
            temp.Highpressure = m_sampleinformation.Highpressure;
            temp.Highcurrent = m_sampleinformation.Highcurrent;
            temp.Lowpressure = m_sampleinformation.Lowpressure;
            temp.Lowcurrent = m_sampleinformation.Lowcurrent;
            temp.PhaseNumber = m_sampleinformation.PhaseNumber;

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
        private bool ManThreadIsOver = false;
        private bool ManThreadIsWork = false;

        private async Task<bool> ExePlcCommand(int command)
        {
            bool Success = false;
            string error = string.Empty;

            //AddRecord("管理线程-请等待,设备回应!", false);
            var timeout = Task.Delay(15000);
            var t = Task.Run(async () =>
            {
                try
                {
                    switch (command)
                    {
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

                        case 13:
                            {
                                //快慢
                                plc200.Write("M12.2", true); 
                            }
                            break;

                        case 14:
                            {
                                //快慢
                                plc200.Write("M12.2", false);
                            }
                            break;


                        case 50:
                            {
                                //5A
                                plc200.Write("M11.0", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.0", false);
                                //这里还需要配合电流设置..

                                //结束
                                DevecSetParam = false;
                                ExeportCommand(5,5);
                                //设置完成后要等待...
                                Thread.Sleep(3000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置电流互感器失败!", true);
                                }
                            }
                            break;
                        case 51:
                            {
                                //10A
                                plc200.Write("M11.1", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.1", false);

                                //结束
                                DevecSetParam = false;
                                ExeportCommand(5, 10);
                                //设置完成后要等待...
                                Thread.Sleep(3000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置电流互感器失败!", true);
                                }

                            }
                            break;
                        case 52:
                            {
                                //20A
                                plc200.Write("M11.2", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.2", false);

                                //结束
                                DevecSetParam = false;
                                ExeportCommand(5, 20);
                                //设置完成后要等待...
                                Thread.Sleep(3000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置电流互感器失败!", true);
                                }

                            }
                            break;
                        case 53:
                            {
                                //50A
                                plc200.Write("M11.3", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.3", false);


                                //结束
                                DevecSetParam = false;
                                ExeportCommand(5, 50);
                                //设置完成后要等待...
                                Thread.Sleep(3000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置电流互感器失败!", true);
                                }

                            }

                            break;
                        case 54:
                            {
                                //100A
                                plc200.Write("M11.4", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.4", false);

                                //结束
                                DevecSetParam = false;
                                ExeportCommand(5, 100);
                                //设置完成后要等待...
                                Thread.Sleep(3000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置电流互感器失败!", true);
                                }

                            }
                            break;
                        case 55:
                            {
                                //200A
                                plc200.Write("M11.5", true);
                                Thread.Sleep(500);
                                plc200.Write("M11.5", false);

                                //结束
                                DevecSetParam = false;
                                ExeportCommand(5, 200);
                                //设置完成后要等待...
                                Thread.Sleep(3000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置电流互感器失败!", true);
                                }

                            }
                            break;

                        case 62:
                            {
                                //保存数据..
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

                        case 100:
                            {
                                //读PLC 输入点
                                ByteToBoolInput(plc200.ReadBytes(DataType.Input, 0, 0, 5));
                                UpDatePlc();
                                if (DeveclIsWork == true)
                                {
                                    if (StartScanSing == true) ExeportCommand(3);
                                }                                
                            }
                            break;
                        case 101:
                            {
                                //联机..
                                ExeportCommand(1);
                            }
                            break;
                        case 102:
                            {
                                //开始空载
                                float isCurrent = 0;
                                if (m_PlcStateModel.Plcidic6 == true) //5A
                                {
                                    isCurrent = 5;
                                }

                                if (m_PlcStateModel.Plcidic7 == true)//10A
                                {
                                    isCurrent = 10;
                                }

                                if (m_PlcStateModel.Plcidid0 == true) //20A
                                {
                                    isCurrent = 20;
                                }

                                if (m_PlcStateModel.Plcidid1 == true) //50A
                                {
                                    isCurrent = 50;
                                }

                                if (m_PlcStateModel.Plcidid2 == true) //100A
                                {
                                    isCurrent = 100;
                                }

                                if (m_PlcStateModel.Plcidid3 == true) //200A
                                {
                                    isCurrent = 200;
                                }

                                if (isCurrent == 0)
                                {
                                    MessageBox.Show("没有选择电流!");
                                    return;
                                }



                                //开始空载
                                ExeportCommand(2);
                                Thread.Sleep(4000);
                                if (DeveclIsWork == false)
                                {
                                    AddRecord("开始空载失败...", true);
                                    return;
                                }
                                else
                                {
                                    AddRecord("开始空载成功...");
                                }

                                //这里先要去设置一下..
                                AddRecord("发送设置参数指令...", false);
                                DevecSetParam = false;
                                //得到当前PLC a5 a10
                                //这里需要得到当前PLC到底是什么设置....
                                ExeportCommand(5, isCurrent);
                                Thread.Sleep(2000);
                                if (DevecSetParam == false)
                                {
                                    AddRecord("设置参数指令失败...", true);
                                    return;
                                }
                            }
                            break;
                        case 103:
                            {
                                //读取数据
                                ExeportCommand(3);
                            }
                            break;
                        case 104:
                            {
                                //结束
                                ExeportCommand(4);
                            }
                            break;

                    }
                    Success = true;
                    //AddRecord("工作线程-执行结束" + command.ToString(), false);
                }
                catch (Exception ex)
                {
                    //总异常:
                    error = ex.Message;
                    AddRecord("工作线程-执行异常:" + error + "当前指令:" + command.ToString(), true);
                    Success = false;
                    return;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                AddRecord("管理线程-超时,请检查设备是否正常!" + command.ToString(), true);
                return false;
            }

            if (Success)
            {             
                return true;
            }
            else
            {
                return false;
            }
        }





        public void InitializationManager()
        {

            

            //先打开端口...
            try
            {
                //开始先去链接PLC..
                //如果没有异常，就没事..
                
                plc200.Open();
                AddRecord("连接PLC成功！");
                m_UserCommunicationModel.Image1 = true;
                
                serialPortHelper.Open();
                AddRecord("打开串口成功！");
                m_UserCommunicationModel.Image2 = true;
                
                
                //开始先去链接设备..                    
                //master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                //master.Transport.ReadTimeout = 500;// 设置超时时间默认为500毫秒
                //serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_GLFXY"];
                //serialPort.Open(); //打开串口                            
                //AddRecord("连接设备OPEN成功！");
                

            }
            catch (Exception ex)
            {
                AddRecord("串口或网口异常" + ex.Message, true);
                MessageBox.Show("初始化失败!请关闭软件检查设备是否异常:" + ex.Message);
                return;
            }



            //可以等一下管家线程..
            var t = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;
                

                AddRecord("管理线程发送退出指令...", false);
                ExeportCommand(4);
                Thread.Sleep(2000);

                AddRecord("管理线程发送联机指令...", false);
                ExeportCommand(1);
                Thread.Sleep(2000);
                if (DeveclIsConnect == false)
                {
                    AddRecord("管理线程联机失败,请检查设备....", true);
                    ManThreadIsOver = true;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (waitWindow != null)
                        {
                            waitWindow.Close();
                            waitWindow = null;
                        }
                    });

                    return;
                }
                AddRecord("管理线程联机成功", false);


                
                AddRecord("管理线程发送设置参数指令...", false);
                ExeportCommand(5);
                Thread.Sleep(2000);
                if (DevecSetParam == false)
                {
                    AddRecord("设置参数指令失败...", true);
                    ManThreadIsOver = true;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (waitWindow != null)
                        {
                            waitWindow.Close();
                            waitWindow = null;
                        }
                    });

                    return;
                }
                

                AddRecord("所有初始化完成!", false);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (waitWindow != null)
                    {
                        waitWindow.Close();
                        waitWindow = null;
                    }
                });


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

                        if (milliseconds >= 500)
                        {
                            
                            var myresult1 = await ExePlcCommand(100);
                            if (!myresult1)
                            {
                                AddRecord("管理线程退出-定时器指令执行失败", true);
                                ManThreadIsOver = true;
                                return;
                            }
                            

                            precurrentTime = currentTime;
                            continue;
                        }
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();
                        ManThreadIsWork = true;
                        var myresult2 = await ExePlcCommand(Command);
                        if (myresult2)
                        {
                            AddRecord("管理员-指令执行成功!", false);
                        }
                        else
                        {
                            AddRecord("管理员-指令执行失败!", true);
                        }
                        precurrentTime = System.DateTime.UtcNow;
                        ManThreadIsWork = false;
                    }
                    catch (Exception ex)
                    {
                        AddRecord($"管理线程异常:{ex.Message}", true);
                        ManThreadIsOver = true;
                        return;
                    }
                }
                ManThreadIsOver = true;
                return;
            }, cts.Token);

            if (waitWindow == null)
            {
                waitWindow = new WaitWindow();
                waitWindow.SetText("请等待,正在空载初始化..");
                waitWindow.ShowDialog();
            }


            return;
        }

        public bool Close()
        {
            if (DeveclIsWork == true)
            {
                MessageBox.Show("必须先退出测量!");
                return false;
            }

            WeakReferenceMessenger.Default.UnregisterAll(this);

            Task task = new Task(async () =>
            {
                AddRecord("关闭管理线程..");
                cts.Cancel();
                while (true)
                {
                    Thread.Sleep(1500);
                    if (ManThreadIsOver == true) break;
                }
                AddRecord("关闭管理线程完成..");
                Thread.Sleep(2000);
                plc200.Close();
                AddRecord("关闭PLC..");
                Thread.Sleep(2000);
                serialPortHelper.Close();
                AddRecord("关闭串口..");
                Thread.Sleep(2000);

            });

            task.ContinueWith((previousTask) =>
            {
                //这里还有个问题，就是异常也会这样...
                Debug.WriteLine("主线程回调");
                //这里返回了..
                //注意这里回来的主线程..--有个问题，如果线程超时了呢?                

                if (waitWindow != null)
                {
                    waitWindow.Close();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
            //任务开始执行..
            task.Start();

            if (waitWindow == null)
            {
                waitWindow = new WaitWindow();
                waitWindow.SetText("请等待,正在退出空载..");
                waitWindow.ShowDialog();
            }


            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");
            return true;
        }

    }
    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }

}
