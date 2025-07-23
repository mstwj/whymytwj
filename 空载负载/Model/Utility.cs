using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveCharts.Defaults;
using LiveCharts;
using Modbus.Device;
using S7.Net;
using S7.Net.Types;
using 空载负载.Base;
using 空载负载.Table;
using 空载负载.View;
using System.Windows.Markup;

namespace 空载负载.Model
{
    public class Utility : ObservableObject
    {
        
        public ChartValues<ObservableValue> ServerLineData { get; set; } = new ChartValues<ObservableValue>();        

        private Plc plc200 { get; set; } = new Plc(CpuType.S7200Smart, "192.168.2.1", 0, 1);

        //private SerialPortHelper serialPortHelper { get; set; } = new SerialPortHelper(
        //ConfigurationManager.AppSettings["DEVICE_GLFXY"], 19200, Parity.None, 8, StopBits.One);

        private SerialPort serialPort = new SerialPort();
        private ModbusSerialMaster master = null;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private Table_ProductInfo m_sampleinformation { get; set; }
        private Table_UtilityStandardInfo m_sampleinUtitlity { get; set; }

        
        private UtilityHeadModel m_UtilityHeadModel { get; set; }

        private UtilityPage m_Mainpage { get; set; }

        //private UserLoadMainModel m_UserLoadMainModel { get; set; }

        private UtitlityPlcStateModel m_UtitlityPlcStateModel { get; set; }
        public bool[] alldian_boolinput { get; set; } = new bool[50];

        private UserCommunicationModel m_UserCommunicationModel { get; set; }
        

        //设置的时间..        
        public int MyTime { get; set; }

        private bool startcount = false;

        //试验电压..
        public float TextV { get; set; } = 1f;


        //保护电压..
        public float ProtectVoltage { get; set; } = 1.2f;

        //保护电流..
        public float ProtectCurrent { get; set; } = 1f;

        private float currentGaoyaVoild;
        public float CurrentGaoyaVoild { get => currentGaoyaVoild; set { SetProperty(ref currentGaoyaVoild, value); } }

        private float currentDiyaVoild;
        public float CurrentDiyaVoild { get => currentDiyaVoild; set { SetProperty(ref currentDiyaVoild, value); } }


        //高压电流
        private float currentElectricCurrent;
        public float CurrentElectricCurrent { get => currentElectricCurrent; set { SetProperty(ref currentElectricCurrent, value); } }

        //显示的时间..
        private float naiYaTime;
        public float NaiYaTime { get => naiYaTime; set { SetProperty(ref naiYaTime, value); } }

        public Queue<int> queue { get; set; } = new Queue<int>();

        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();
        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public WaitWindow waitWindow { get; set; } = null;

        public int timeSelect { get; set; } = 0;


        public ICommand CommandSet { get; set; }

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


        

        

        public ICommand CommandAuto { get; set; }
        public ICommand CommandAutoStop { get; set; }

        public ICommand CommandK { get; set; }

        public ICommand CommandM { get; set; }

        public ICommand CommandStratTime { get; set; }

        public ICommand CommandStopTime { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand CommandStratAuto { get; set; }
        public ICommand CommandStopAuto { get; set; }

        //第2....
        public void Initiate(Table_ProductInfo table_ProductInfo, Table_UtilityStandardInfo table_UtilityStandardInfo )
        {
            m_sampleinformation = table_ProductInfo;
            m_sampleinUtitlity = table_UtilityStandardInfo;

            m_UtilityHeadModel.ProductType = table_UtilityStandardInfo.ProductType;
            m_UtilityHeadModel.ProductTuhao = table_UtilityStandardInfo.ProductTuhao;
            m_UtilityHeadModel.Voltage = table_UtilityStandardInfo.Voltage;
            m_UtilityHeadModel.Frequency = table_UtilityStandardInfo.Frequency;
            m_UtilityHeadModel.Times = table_UtilityStandardInfo.Times;

            //这里才开始管理线程...
            //InitializationManager();
        }

        //第一
        public void InitiateModel(UtilityHeadModel utilityHeadModel,
                                  UserCommunicationModel userCommunicationModel,
                                  UtitlityPlcStateModel utitlityPlcStateModel,
                                    UtilityPage mainpage)
        {
            m_Mainpage = mainpage;
            m_UtilityHeadModel = utilityHeadModel;            
            m_UserCommunicationModel = userCommunicationModel;
            m_UtitlityPlcStateModel = utitlityPlcStateModel;

        }

        public Utility()
        {

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

            CommandStratAuto = new RelayCommand<object>(DoCommandStratAuto);
            CommandStopAuto = new RelayCommand<object>(DoCommandStopAuto);



            CommandAuto = new RelayCommand<object>(DoCommandAuto);
            CommandAutoStop = new RelayCommand<object>(DoCommandAutoStop);

            CommandK = new RelayCommand<object>(DoCommandK);
            CommandM = new RelayCommand<object>(DoCommandM);

            CommandSet = new RelayCommand<object>(DoCommandSet);

            //开始即使
            CommandStratTime = new RelayCommand<object>(DoCommandStratTime);

            CommandStopTime = new RelayCommand<object>(DoCommandStopTime);

            CommandSave = new RelayCommand<object>(DoCommandSave);


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

        private void DoCommandSave(object param) { ExeCommand(62); }

        private bool isauto = false;

        private bool isautosave = false;


        private void DoCommandStratAuto(object param) 
        {

            //开始自动..
            if (isauto == true)
            {
                MessageBox.Show("已经在走自动流程,请等待流程结束!");
                return;
            }

            MyTools.setUtiltyPageButton(false);

            Task task = new Task(async () =>
            {

                

                if (isauto == false)
                    isauto = true;

                if (m_UtitlityPlcStateModel.Plck != true)
                {
                    MessageBox.Show("错误，请选择慢速度!");
                    return;
                }

                if (m_UtitlityPlcStateModel.Plcidid6 == false)
                {
                    MessageBox.Show("错误，请选择工频回路!");
                    return;
                }

                if (NaiYaTime < 30)
                {
                    MessageBox.Show("错误，请设置耐压时间!");
                    return;

                }

                if (TextV == 0)
                {
                    MessageBox.Show("请设置试验电压!");
                    return;
                }

                if (ProtectVoltage == 0)
                {
                    MessageBox.Show("请设置保护电压!");
                    return;
                }

                if (ProtectCurrent < 0.1)
                {
                    MessageBox.Show("请设置保护电流!");
                    return;
                }


                //零位判断..
                if (m_UtitlityPlcStateModel.Plcidic4 == false)
                {
                    MessageBox.Show("错误,升压必须先到零位!");
                    return;
                }
                

                AddRecord("开始自动....");

                ServerLineData.Clear();



                AddRecord("电源合闸..");
                plc200.Write("M11.7", true);
                Thread.Sleep(500);
                plc200.Write("M11.7", false);
                //等待电源
                while (true)
                {
                    Thread.Sleep(1000);
                    if (isauto == false)
                    {
                        //强制停止升压..
                        AddRecord("强制停止...");
                        return;
                    }

                    if (m_UtitlityPlcStateModel.Plcidie0 == true)
                    {
                        AddRecord("电源已经合闸....");
                        break;
                    }
                    
                }


                AddRecord("升压..");
                Thread.Sleep(500);
                plc200.Write("M12.1", true);

                ServerLineData.Add(new ObservableValue(0));//一个点..

                int temp = 0;
                while (true)
                {
                    Thread.Sleep(100);
                    ReadBiaoAndShow();

                    if (temp == 5)
                    {
                        ServerLineData.Add(new ObservableValue(CurrentGaoyaVoild));//一个点..
                        temp = 0;
                    }
                    else
                    {
                        temp++;
                    }
                    

                    if (isauto == false)
                    {
                        //强制停止升压..
                        plc200.Write("M12.1", false);
                        AddRecord("强制停止...");
                        return;
                    }

                    //平均电压..
                    if (CurrentGaoyaVoild >= TextV - (TextV / 20))
                    {
                        //这里就不需要在升压了..
                        plc200.Write("M12.1", false);
                        break;
                    }                    
                }


                AddRecord("计时..");
                Thread.Sleep(500);

                while (true)
                {
                    if (isauto == false)
                    {
                        AddRecord("强制停止...");
                        return;
                    }
                    Thread.Sleep(1000);
                    NaiYaTime--;
                    ServerLineData.Add(new ObservableValue(CurrentGaoyaVoild));//一个点..
                    if (NaiYaTime <= 0)
                    {
                        AddRecord("记时完成!");
                        break;
                    }
                }

                isautosave = false;
                isautosave = SaveRecord();

                AddRecord("降压..");
                Thread.Sleep(500);
                plc200.Write("M12.0", true);
                while (true)
                {
                    //零位判断..        Plcidid7
                    if (m_UtitlityPlcStateModel.Plcidic4 == true)
                    {
                        plc200.Write("M12.0", false);
                        break;
                    }
                    Thread.Sleep(500);
                }

                ServerLineData.Add(new ObservableValue(0));//一个点..

                AddRecord("电源分闸..");
                plc200.Write("M11.6", true);
                Thread.Sleep(500);
                plc200.Write("M11.6", false);
;
                AddRecord("自动流程完成..");

            });

            task.ContinueWith((previousTask) =>
            {
                //这里还有个问题，就是异常也会这样...
                Debug.WriteLine("主线程回调");
                //这里返回了..
                //注意这里回来的主线程..--有个问题，如果线程超时了呢?                
                if (isautosave == true)
                {
                    MessageBox.Show("数据保存成功,试验已完成!");
                }
                AddRecord("自动流程结束...");
                isauto = false;
                MyTools.setUtiltyPageButton(true);
            }, TaskScheduler.FromCurrentSynchronizationContext());
            //任务开始执行..
            task.Start();

            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");



        }

        private void DoCommandStopAuto(object param)
        {
            //停止自动..
            isauto = false;

        }




        private void DoCommandStopTime(object param)
        {
            startcount = false;
        }
        private void DoCommandStratTime(object param)
        {
            if (NaiYaTime <= 10)
            {
                MessageBox.Show("无法开始计时:耐压时间为0,请重新设置一次耐压时间!");
                return;
            }

            startcount = true;
        }

        private void DoCommandSet(object param) {

            if (startcount == true)
            {
                MessageBox.Show("无法设置，请先暂停..");
                return;
            }

            if (MyTime < 30 || MyTime > 120)
            {
                MessageBox.Show("时间设置错误,只能是30秒或者120秒之间.");
                return;
            }
            m_Mainpage.SetWImage(MyTime+30, (int)ProtectVoltage);
            NaiYaTime = MyTime;
            ServerLineData.Clear();            
        }

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

        private void DoBtnCommandStart(object param)
        {
        }

        private void DoBtnCommandSave(object param) { ExeCommand(62); }

        
        private void DoCommandAuto(object param) { }

        private void DoCommandAutoStop(object param) { }

        private void DoCommandK(object param) { ExeCommand(13); }
        private void DoCommandM(object param) { ExeCommand(14); }




        void UpDatePlc()
        {
            m_UtitlityPlcStateModel.Plcidic3 = alldian_boolinput[23];
            m_UtitlityPlcStateModel.Plcidic4 = alldian_boolinput[24];
            m_UtitlityPlcStateModel.Plcidic5 = alldian_boolinput[25];
            m_UtitlityPlcStateModel.Plcidic6 = alldian_boolinput[26];
            m_UtitlityPlcStateModel.Plcidic7 = alldian_boolinput[27];

            m_UtitlityPlcStateModel.Plcidid0 = alldian_boolinput[30];
            m_UtitlityPlcStateModel.Plcidid1 = alldian_boolinput[31];
            m_UtitlityPlcStateModel.Plcidid2 = alldian_boolinput[32];
            m_UtitlityPlcStateModel.Plcidid3 = alldian_boolinput[33];
            m_UtitlityPlcStateModel.Plcidid4 = alldian_boolinput[34];
            m_UtitlityPlcStateModel.Plcidid5 = alldian_boolinput[35];
            m_UtitlityPlcStateModel.Plcidid6 = alldian_boolinput[36];
            m_UtitlityPlcStateModel.Plcidid7 = alldian_boolinput[37];

            m_UtitlityPlcStateModel.Plcidie0 = alldian_boolinput[40];
            m_UtitlityPlcStateModel.Plcidie1 = alldian_boolinput[41];
            m_UtitlityPlcStateModel.Plcidie2 = alldian_boolinput[42];

            if (alldian_boolinput[49] == true)
            {
                m_UtitlityPlcStateModel.Plck = true;
                m_UtitlityPlcStateModel.Plcm = false;
            }

            if (alldian_boolinput[49] == false)
            {
                m_UtitlityPlcStateModel.Plcm = true;
                m_UtitlityPlcStateModel.Plck = false;
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
            Table_UtilityRecordInfo temp = new Table_UtilityRecordInfo();
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

            temp.Voltage = CurrentGaoyaVoild.ToString();
            temp.Frequency = "0";
            temp.Times = MyTime.ToString();

            try
            {
                using (var context = new MyDbContext())
                {
                    context.UtilityRecordInfo.Add(temp);
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

        void ReadBiaoAndShow()
        {
            //读2个表..
            float data = ReadModbusData(1, 6, 2);

            CurrentGaoyaVoild = data;
            CurrentGaoyaVoild = CurrentGaoyaVoild / 1000;
            CurrentGaoyaVoild = (float)Math.Round(CurrentGaoyaVoild, 2);

            float data2 = ReadModbusData(2, 6, 2);
            CurrentDiyaVoild = data2;

            //CurrentDiyaVoild = CurrentDiyaVoild / 10;
            CurrentDiyaVoild = (float)Math.Round(CurrentDiyaVoild, 3);

            float data3 = ReadModbusData(3, 6, 2);
            CurrentElectricCurrent = data3;

            CurrentElectricCurrent = CurrentElectricCurrent * 1000;

            CurrentElectricCurrent = (float)Math.Round(CurrentElectricCurrent, 2);

            if (CurrentGaoyaVoild > ProtectVoltage || CurrentElectricCurrent > ProtectCurrent)
            {
                //当前电压大于保护电压..
                AddRecord("请注意:已经大于保护电压,或者大于保护电流!", true);
                AddRecord("电源被强制复位...", true);
                //这里强制停止升压
                plc200.Write("M12.1", false);
                Thread.Sleep(100);
                //这里强制复位..
                plc200.Write("M10.2", true);
                Thread.Sleep(200);
                plc200.Write("M10.2", false);
            }


            //if (isauto == true)
            //{
                //如果是自动..
                //ServerLineData.Add(new ObservableValue(CurrentGaoyaVoild));//一个点..
                //NaiYaTime--;
            //}
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

                                ReadBiaoAndShow();
                                if (NaiYaTime > 0)
                                {
                                    if (startcount == true)
                                    {                                        
                                        ServerLineData.Add(new ObservableValue(CurrentGaoyaVoild));//一个点..
                                        NaiYaTime--;
                                    }
                                }
                            }
                            break;

                        case 200:
                            {

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

        private float ReadModbusData(byte slaveId, ushort start, ushort length)
        {
            //float[] FloatValue = new float[3];
            try
            {
                //这里我懂了，这里多此一举了.. 本质读取的是WORD 2个字的，可是我自己又转变成了BYTE了...
                //ushort[] data = this.master.ReadHoldingRegisters(slaveId, start, length);

                //这里我懂了，这里多此一举了.. 本质读取的是WORD 2个字的，可是我自己又转变成了BYTE了...
                //ushort[] ushortArray = this.master.ReadHoldingRegisters(1, 0, 6);
                ushort[] ushortArray = this.master.ReadHoldingRegisters(slaveId, start, length);

                ushort[] ushortArray1 = new ushort[2];
                //ushort[] ushortArray2 = new ushort[2];
                //ushort[] ushortArray3 = new ushort[2];

                ushortArray1[0] = ushortArray[0];
                ushortArray1[1] = ushortArray[1];
                //ushortArray2[0] = ushortArray[2];
                //ushortArray2[1] = ushortArray[3];
                //ushortArray3[0] = ushortArray[4];
                //ushortArray3[1] = ushortArray[5];



                //Array.Reverse(ushortArray1);
                //Array.Reverse(ushortArray2);
                //Array.Reverse(ushortArray3);
                //
                //Convert.ToSingle(data, Type(float));//因为是 ToSingle所以自动4个 。。

                byte[] byteArray1 = new byte[4];
                Buffer.BlockCopy(ushortArray1, 0, byteArray1, 0, 4);
                float aaa = BitConverter.ToSingle(byteArray1, 0);

                
                //byte[] byteArray2 = new byte[4];
                //Buffer.BlockCopy(ushortArray2, 0, byteArray2, 0, 4);
                //float bbb = BitConverter.ToSingle(byteArray2, 0);

                //byte[] byteArray3 = new byte[4];
                //Buffer.BlockCopy(ushortArray3, 0, byteArray3, 0, 4);
                //float ccc = BitConverter.ToSingle(byteArray3, 0);

                //float roundedNumber1 = aaa;
                //roundedNumber1 = (float)Math.Round(aaa, 2);
                //bbb = bbb / 1000;
                //float roundedNumber2 = (float)Math.Round(bbb, 2);
                //float roundedNumber2 = bbb;
                //float roundedNumber3 = ccc;

                //float[] data = new float[3];
                //data[0] = roundedNumber1;
                //data[1] = roundedNumber2;
                //data[2] = roundedNumber3;

                //FloatValue[0] = aaa;
                //FloatValue[1] = bbb;
                //FloatValue[2] = ccc;
                
                return aaa;
            }
            catch (Exception ex)
            {
                throw;
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

               serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议

                master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                master.Transport.ReadTimeout = 500;// 设置超时时间默认为500毫秒
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_BPDY"];
                serialPort.Open(); //打开串口                
                AddRecord("打开串口完成");

                m_UserCommunicationModel.Image2 = true;
                

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

                        if (milliseconds >= 1000)
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
                waitWindow.SetText("请等待,正在工频耐压初始化..");
                waitWindow.ShowDialog();
            }


            return;
        }

        public bool Close()
        {

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
                master = null;
                serialPort.Close();
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
                waitWindow.SetText("请等待,正在退出工频耐压..");
                waitWindow.ShowDialog();
            }


            //这里主线程就返回了..
            Debug.WriteLine("主线程返回!");
            return true;
        }

    }
}
