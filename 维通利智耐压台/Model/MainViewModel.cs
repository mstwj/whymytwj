using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveCharts;
using LiveCharts.Defaults;
using S7.Net;
using S7.Net.Types;
using Syncfusion.Windows.Shared;
using 维通利智耐压台.Base;
using 维通利智耐压台.MyTable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using CommunityToolkit.Mvvm.ComponentModel;

namespace 维通利智耐压台.Model
{
    public class MainViewModel : ObservableObject
    {
        private bool IsAuto = false; //开始默认就是手动..
        private int StartScanAndSave = 0; //开始扫描信号..
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();
        //注意2个文件必须拷贝到EXE文件目录下..
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource cts2 = new CancellationTokenSource();
        public Queue<CommandItem> queue { get; set; } = new Queue<CommandItem>();

        public ICommand BtnCommandAutoOrMatu { get; set; }
        public ICommand BtnCommandElectric { get; set; }
        public ICommand BtnCommandUp { get; set; }

        public ICommand BtnCommandDown { get; set; }
        public ICommand BtnCommandClose { get; set; }
        public ICommand BtnCommandOpen { get; set; }
        public ICommand BtnCommandTimer { get; set; }
        public ICommand BtnCommandStopTime { get; set; }
        public ICommand BtnCommandSave { get; set; }
        public ICommand  BtnCommandWarring { get; set; }

        //有参数double,返回值string.
        //public Func<double, string> Formatter { get; set; } = new Func<double, string>(FunWithPara);        

        //时间点..
        public static List<string> TimePoints { get; set; } = new List<string>();

        public ChartValues<ObservableValue> ServerLineData { get; set; } = new ChartValues<ObservableValue>();
        public ChartValues<ObservableValue> ServerLineData2 { get; set; } = new ChartValues<ObservableValue>();

        private Plc plc21 = new Plc(CpuType.S7200Smart, ConfigurationManager.AppSettings["DEVICE_PLC"], 0, 1);

        public UserMainControlModel userMainControlModel { get; set; }
        public UserTestControlModel userTestControlModel { get; set; }
        public Tabe_Record record { get; set; } = new Tabe_Record();

        public SerialPortHelper serialPortHelper { get; set; } = new SerialPortHelper(
            ConfigurationManager.AppSettings["DEVICE_COMJUFAN"], 115200, Parity.None,8, StopBits.One);

        private string productNumber; 
        public string ProductNumber { get => productNumber; set { SetProperty(ref productNumber, value); } }

        private string productType;
        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }

        public Func<double, string> CustomFormatterX { get; set; }

        void Inspectregister()
        {
            //我自己加的注册...
            /*
            System.DateTime activationDate = new System.DateTime(2025, 1, 1); // 示例激活日期
            TimeSpan duration = new TimeSpan(30, 0, 0, 0); // 示例有效期30天..
            System.DateTime expirationDate = activationDate.Add(duration);//到期日期..

            //这里是读本地时间片..
            if (DateTime.Now > expirationDate)
            {
                MessageBox.Show("软件已过期，请续费");
                Application.Current.Shutdown(); // 或禁用某些功能
            }
            */
        }

        public MainViewModel() 
        {
            CustomFormatterX = CustomFormattersX;

            Debug.WriteLine("开始了..");
            BtnCommandAutoOrMatu = new RelayCommand<object>(DoBtnCommandAutoOrMatu);
            BtnCommandElectric = new RelayCommand<object>(DoBtnCommandElectric);
            BtnCommandUp = new RelayCommand<object>(DoBtnCommandUp);
            BtnCommandDown = new RelayCommand<object>(DoBtnCommandDown);
            BtnCommandClose = new RelayCommand<object>(DoBtnCommandClose);
            BtnCommandOpen = new RelayCommand<object>(DoBtnCommandOpen);
            BtnCommandTimer = new RelayCommand<object>(DoBtnCommandTimer);
            BtnCommandStopTime = new RelayCommand<object>(DoBtnCommandStopTime);
            BtnCommandWarring = new RelayCommand<object>(DoBtnCommandWarring);

            //可以等一下管家线程..
            Task ReTs = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;                
                AddRecord("管理线程PLC连接..", false);
                //开始先去链接电源..
                var initresult = await ExePlcCommand(1, false);

                if (!initresult)
                {
                    AddRecord("管理线程init指令执行失败!", true);
                    MessageBox.Show("管理线程-开始连接PLC失败，请检查设备!");
                    return;
                }
                else
                {
                    AddRecord("管理线程PLC连接成功!", false);
                }
                
                AddRecord("初始化串口...", false);
                var initresult2 = await ExePlcCommand(12, false);
                if (!initresult2)
                {
                    AddRecord("串口连接失败!", true);
                    MessageBox.Show("管理线程-串口连接失败，请检查设备!");
                    return;
                }
                else
                {
                    AddRecord("串口连接成功!", false);
                }
                

                InitAuto();

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

                        
                        if (milliseconds > 500)
                        {
                            //每0.5秒一次..
                            Inspectregister();
                            //定时时间到了..(0.5秒1次 -- 读PLC。。)
                            var myresult1 = await ExePlcCommand(10, false);
                            if (!myresult1)
                            {
                                AddRecord("管理线程退出-定时器指令执行失败", true);
                                return;
                            }
                            
                            precurrentTime = currentTime;
                            continue;
                        }
                        
                        if (queue.Count <= 0) continue;
                        CommandItem Command = queue.Dequeue();
                        if (Command.command != 11) //11为机器人指令..
                        AddRecord("管理员-指令开始执行!:" + Command.command.ToString(), false);
                        var myresult = await ExePlcCommand(Command.command, Command.data, Command.Data1, Command.Data2, Command.Data3);
                        if (myresult)
                        {
                            if (Command.command != 11)  AddRecord("管理员-指令执行成功!", false);
                        }
                        else
                        {
                            AddRecord("管理员-指令执行失败!", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        AddRecord($"管理线程异常:{ex.Message}", true);
                        return;
                    }
                }
                AddRecord($"管理线程退出:", true);
            }, cts.Token);

        }


        private string CustomFormattersX(double val)
        {
            //var now = System.DateTime.Now;
            //now = now.AddSeconds(1);
            //return System.DateTime.Now.ToString("HH:mm:ss");
            //return now.ToString("HH:mm:ss");//string.Format("{0}天", val);
            return string.Format("{0}秒", val);
        }

        private async Task<bool> ExePlcCommand(int command,bool data,float Data1 = 0,float Data2 = 0,int Data3 = -1)
        {
            bool Success = false;
            string error = string.Empty;

            //AddRecord("管理线程-请等待,设备回应!", false);
            var timeout = Task.Delay(8000);
            var t = Task.Run(async () =>
            {
                try
                {
                    //plc31.ReadBytes(DataType.Output, 0, 0, 18)
                    switch (command)
                    {
                        case 1:
                            {
                                plc21.Open();
                                //plc21.Write("M1.4", true);
                                Thread.Sleep(100);
                                plc21.Write("M1.4", false);
                                Thread.Sleep(100);
                                plc21.Write("M0.4", false);
                                Thread.Sleep(100);
                                plc21.Write("M1.1", false);
                                Thread.Sleep(100);
                                plc21.Write("M1.7", false);
                                Thread.Sleep(100);
                                plc21.Write("M1.0", false);
                                Thread.Sleep(100);
                                plc21.Write("M0.0", false);
                                Thread.Sleep(100);
                                plc21.Write("M1.3", false);
                                Thread.Sleep(100);
                                plc21.Write("M0.3", false);

                                
                            }
                            break;
                        
                        case 2: plc21.Write("M10.7", data); break; //手动-自动.
                        case 3:
                            {
                                userMainControlModel.Image9 = true;
                                plc21.Write("M1.4", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.4", false);
                                userMainControlModel.Image9 = false ;
                            }
                            break; //手动-降压
                        case 4:
                            {
                                plc21.Write("M0.4", true);
                                Thread.Sleep(500);
                                plc21.Write("M0.4", false);
                            }
                            break; //自动-降压
                        case 5:
                            {
                                //手动-合闸
                                plc21.Write("M1.1", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.1", false);

                            }
                            break; 
                        case 6:
                            {
                                plc21.Write("M1.7", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.7", false);
                            }

                            break; //自动-合闸
                        case 7:
                            {
                                plc21.Write("M1.0", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.0", false);
                            }
                            break; //手动-分闸
                        case 8:
                            {
                                plc21.Write("M1.0", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.0", false);
                            }

                            break; //自动-分闸
                        case 9:
                            {
                                userMainControlModel.Image8 = true;
                                plc21.Write("M1.3", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.3", false);
                                userMainControlModel.Image8 = false;
                            }
                            break; //手动-生压
                        case 14:
                            {

                                plc21.Write("M0.3", true);
                                Thread.Sleep(500);
                                plc21.Write("M0.3", false);
                            }
                            break; //自动-生压

                        case 19:
                            {
                                //写入OK...

                                //plc21.Write(DataType.DataBlock, 1, 104, 1000);
                                plc21.Write("M1.5", true);
                                Thread.Sleep(1000);
                                plc21.Write("M1.5", false);

                            }
                            break; //自动-生压

                        case 10:
                            {
                                //I电压读取成功，这里的I不能写.. 只能读.. O是可读可写的..
                                byte[]? result = plc21.ReadBytes(DataType.Input, 0, 0, 1);
                                byte resultbit = result[0];
                                bool[] valuebit = new bool[8];
                                for (int i = 0; i < 8; i++) valuebit[i] = Bit.FromByte(resultbit, (byte)i);
                                  
                                userMainControlModel.Image0 = valuebit[0];
                                userMainControlModel.Image1 = valuebit[1];
                                userMainControlModel.Image2 = valuebit[2];
                                userMainControlModel.Image3 = valuebit[3];
                                userMainControlModel.Image4 = valuebit[4];
                                userMainControlModel.Image5 = valuebit[5];
                                userMainControlModel.Image6 = valuebit[6];

                                //高压电压。。 -- 
                                byte[]? result2 = plc21.ReadBytes(DataType.DataBlock, 1,2820, 4);
                                Array.Reverse(result2);
                                userMainControlModel.PlcHighVoltage = BitConverter.ToSingle(result2, 0);

                                string gayadianya = ConfigurationManager.AppSettings["DEVICE_GAOYADIANYA"];
                                int itemp;
                                if (int.TryParse(gayadianya, out itemp))
                                {
                                    userMainControlModel.PlcHighVoltage = (userMainControlModel.PlcHighVoltage * itemp) / 1000;
                                    userMainControlModel.PlcHighVoltage = (float)Math.Round(userMainControlModel.PlcHighVoltage, 2);
                                }

                                //float value1 = BitConverter.ToSingle(result2, 0);

                                //VD 和 DB块，为什么是1呢？这个就不知道了..
                                //PLC 有个问题，就是 有时候，不直接控制O，非要使用一个M点来控制输出，不知道为什么..
                                //低压电压。。 VD电压读取成功..
                                byte[]? result3 = plc21.ReadBytes(DataType.DataBlock, 1, 1516, 4);
                                Array.Reverse(result3);
                                float roundedNumber  = BitConverter.ToSingle(result3, 0);
                                roundedNumber = (float)Math.Round(roundedNumber, 1);
                                userMainControlModel.LowVoltage = roundedNumber;
                          
                                //OK 200写成功了..
                                //plc21.Write("M1.1", true);

                                //高压电流。。
                                byte[]? result4 = plc21.ReadBytes(DataType.DataBlock, 1, 1820, 4);
                                Array.Reverse(result4);
                                userMainControlModel.HighCurrent = BitConverter.ToSingle(result4, 0);
                                if (userMainControlModel.HighCurrent < 0)
                                    userMainControlModel.HighCurrent = 0;
                                else
                                    userMainControlModel.HighCurrent = (float)Math.Round(userMainControlModel.HighCurrent, 2);
                                

                                //低压电流。。
                                byte[]? result5 = plc21.ReadBytes(DataType.DataBlock, 1, 1116, 4);
                                Array.Reverse(result5);
                                userMainControlModel.LowCurrent = BitConverter.ToSingle(result5, 0);
                                if (userMainControlModel.LowCurrent < 0)
                                    userMainControlModel.LowCurrent = 0;
                                else 
                                    userMainControlModel.LowCurrent = (float)Math.Round(userMainControlModel.LowCurrent, 1);                                
                            }
                            break;
                        case 11:
                            {
                                //画点..
                                record.LevelCode = string.Empty;
                                if (Data3 == 1)
                                {
                                    record.LevelCode = "Level1";
                                }

                                if (Data3 == 2)
                                {

                                    record.LevelCode = "Level2";
                                }

                                if (Data3 == 3)
                                {
                                    record.LevelCode = "Level3";

                                }
                                ServerLineData.Add(new ObservableValue(Data1));//一个点..
                                ServerLineData2.Add(new ObservableValue(Data2));//一个点..
                              
                                record.RecordDateTimer = System.DateTime.Now;
                                
                                record.Partial = Data1.ToString();
                                record.Votil = Data2.ToString();
                                
                                record.BeginTimer = userMainControlModel.IsTimer.ToString();
                                if (!MyTools.SaveRecordDataToDataBase(record)) 
                                {
                                    AddRecord("数据保存失败！", true);
                                }
                                userMainControlModel.IsTimer++; //每次都加1..
                                
                            }
                            break;
                        case 12:
                            {
                                serialPortHelper.Open();
                            }
                            break;

                        case 13:
                            {
                                //停止画画..画最后一个点...
                                /*
                                record.RecordDateTimer = System.DateTime.Now;
                                record.BeginTimer1 = "End";
                                record.Partial1 = "0";
                                record.Votil1 = "0";                                
                                MyTools.SaveRecordDataToDataBase(record);
                                //包含了分闸指令...
                                plc21.Write("M1.0", true);
                                Thread.Sleep(200);
                                plc21.Write("M1.0", false);
                                */
                            }
                            break;

                        case 15:
                            {
                                //停止画画..画最后一个点...
                                /*
                                record.RecordDateTimer = System.DateTime.Now;
                                record.BeginTimer1 = "Begin";
                                record.Partial1 = "0";
                                record.Votil1 = "0";
                                //if (data == true)
                                MyTools.SaveRecordDataToDataBase(record);
                                //15 包含了合闸指令..
                                plc21.Write("M1.7", true);
                                Thread.Sleep(500);
                                plc21.Write("M1.7", false);
                                */

                            }
                            break;
                            //case 999999: Testsimulation(); break; //仿真数据.
                    }
                    Success = true;
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
                //setLinkBreakOrConnection(command, false);
                AddRecord("管理线程-超时,请检查设备是否正常!" + command.ToString(), true);
                return false;
            }

            if (Success)
            {
                //setLinkBreakOrConnection(command, true);
                return true;
            }
            else
            {
                //setLinkBreakOrConnection(command, false);
                //The operation is not allowed on non-connected sockets.
                AddRecord("管理线程-工作线程异常:" + error, true);
                return false;
            }
        }

        private void DoBtnCommandWarring(object param)
        {
            //直接主线程手动分闸..            
            AddRecord("紧急停止..自动化结束", true);
            StartScanAndSave = 0;
            //强行写入END
            //DrawPoint(3);
        }

        private void DoBtnCommandAutoOrMatu(object param)
        {
            //手动或者自动..
            Button self = param as Button;
            
            if (self.Content.Equals("手动"))
            {
                self.Content = "自动";
                IsAuto = true;

                MyMessage myMessage = new MyMessage();
                myMessage.Message = "DisAutoQueue";
                WeakReferenceMessenger.Default.Send(myMessage);

                //设置PLC 为手动..
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 2, data = true });
                return;
            }

            if (self.Content.Equals("自动"))
            {
                self.Content = "手动";
                IsAuto = false ;
                
                MyMessage myMessage = new MyMessage();
                myMessage.Message = "DisMutiQueue";
                WeakReferenceMessenger.Default.Send(myMessage);

                //设置PLC 为手动..
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 2, data = false });

                return;
            }
        }

        private void DoBtnCommandElectric(object param)
        {
            /*
            //电灵..
            //这里等待了... (主线程还是等了..)
            if (queue.Count >= 1)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成", true);
                return;
            }
            queue.Enqueue(new CommandItem { command = 2, data = true });
            */
            //这里等待了... (主线程还是等了..)
            if (queue.Count >= 1)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成", true);
                return;
            }

            queue.Enqueue(new CommandItem { command = 19, data = true });
        }

        
        private void DoBtnCommandUp(object param)
        {
            if (IsAuto == false)
            {
                //手动
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 9, data = true });
                
            }

            if (IsAuto == true)
            {
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 14, data = true });
            }

        }


        private void DoBtnCommandDown(object param)
        {
            if (IsAuto == false)
            {
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 3, data = true });
                
            }

            if (IsAuto == true)
            {
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 4, data = true });
            }

        }
        private void DoBtnCommandClose(object param)
        {
            
            if (IsAuto == false)
            {
                //这里等待了... (主线程还是等了..)//合闸
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 7, data = true });
            }


            if (IsAuto == true)
            {
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 8, data = true });
            }

        }
        private void DoBtnCommandOpen(object param)
        {
            if (IsAuto == false)
            {
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 5, data = true });
            }

            if (IsAuto == true)
            {
                //这里等待了... (主线程还是等了..)
                if (queue.Count >= 1)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成", true);
                    return;
                }
                queue.Enqueue(new CommandItem { command = 6, data = true });
            }

        }

        static int IsDaw1;
        static int IsDaw2;
        static int IsDaw3;
        //第一次肯定是升压..
        public bool IsCalcRound1(float TargetVoite)
        {
            
            if (userMainControlModel.PlcHighVoltage < TargetVoite - TargetVoite * 0.1)
            {
                AddRecord("阶段1开始自动升压..", false);
                //先升到目标..
                plc21.Write("M0.3", true);
                //这里需要反向推动一下..
                // 7.0                                                              1400
                //userMainControlModel.PlcHighVoltage = (userMainControlModel.PlcHighVoltage * itemp) / 1000;
                //15000 /200 = 7.5
                float fT = (float)((TargetVoite * 1000) / 200);
                float gaoyadianyabaohu = (float)((userTestControlModel.GaoYaProctectVolte * 1000) / 200);
                float gaoyadianliubaohu = userTestControlModel.DiYaProctectCurrent;
                float temp = 0;
                int IsBreadk = 0;
                while (temp < fT)
                {
                    if (StartScanAndSave == 0 || StartScanAndSave == 2)
                    {
                        return false;
                    }

                    //AddRecord("当前:"+ temp.ToString()+ "目标电压:"+ fT.ToString(), false);
                    byte[]? result2 = plc21.ReadBytes(DataType.DataBlock, 1, 2820, 4);
                    Array.Reverse(result2);
                    temp = BitConverter.ToSingle(result2, 0);
                    if (temp > gaoyadianyabaohu)
                    {
                        AddRecord("第1阶段-严重错误,当前电压高于保护电压,程序停机...", true);
                        return false;
                    }

                    if (userMainControlModel.HighCurrent > gaoyadianliubaohu)
                    {
                        AddRecord("第1阶段-严重错误,当前电流高于保护电流,程序停机...", true);
                        return false;
                    }
                    Thread.Sleep(100);
                    if (IsDaw1 < 10)
                    {
                        IsDaw1++;
                    } 
                    else
                    {
                        IsBreadk++;
                        if (IsBreadk == 5)
                        {
                            //判断是不是真的升压了..
                            if (temp < 0.2)
                            {
                                //这里可能是信号中断了--5秒都过了.. 还不升压..
                                AddRecord("第1阶段-严重错误-5秒后,电压还小于200V,强行终止..-请检查线路是否正常..", true);
                                return false;
                            }
                        }
                        IsDaw1 = 0;
                        DrawPoint(2);
                    }

                    
                }
                plc21.Write("M0.3", false);
                AddRecord("自动升压结束..", false);
                
                return true;
            }
            
            AddRecord("阶段1开始计数"+ userMainControlModel.IsLevel1TimeOver.ToString(), false);
            return true;
        }

        
        public bool IsCalcRound2(float TargetVoite)
        {
            
            float gaoyadianyabaohu = (float)((userTestControlModel.GaoYaProctectVolte * 1000) / 200);
            float gaoyadianliubaohu = userTestControlModel.DiYaProctectCurrent;

            if (userMainControlModel.PlcHighVoltage < TargetVoite)
            {
                AddRecord("第2阶段-开始自动升压..", false);
                //先升到目标..
                plc21.Write("M0.3", true);
                //这里需要反向推动一下..
                // 7.0                                                              1400
                //userMainControlModel.PlcHighVoltage = (userMainControlModel.PlcHighVoltage * itemp) / 1000;
                //15000 /200 = 7.5
                float fT = (float)((TargetVoite * 1000) / 200);
                float temp = (float)((userMainControlModel.PlcHighVoltage * 1000) / 200);
                int IsBreadk = 0;
                float OldHighCurrent = userMainControlModel.PlcHighVoltage;
                while (temp < fT)
                {
                    if (StartScanAndSave == 0 || StartScanAndSave == 2)
                    {
                        return false;
                    }


                    //AddRecord("当前:" + temp.ToString() + "目标电压:" + fT.ToString(), false);
                    byte[]? result2 = plc21.ReadBytes(DataType.DataBlock, 1, 2820, 4);
                    Array.Reverse(result2);
                    temp = BitConverter.ToSingle(result2, 0);
                    if (temp > gaoyadianyabaohu)
                    {
                        AddRecord("第2阶段-严重错误,当前电压高于保护电压,程序停机...", true);
                        return false;
                    }

                    if (userMainControlModel.HighCurrent > gaoyadianliubaohu)
                    {
                        AddRecord("第2阶段-严重错误,当前电流高于保护电流,程序停机...", true);
                        return false;
                    }
                    Thread.Sleep(100);

                    if (IsDaw2 < 10)
                    {
                        IsDaw2++;
                    }
                    else
                    {
                        IsBreadk++;
                        if (IsBreadk == 5)
                        {
                            //判断是不是真的升压了..
                            if (temp == OldHighCurrent)
                            {
                                //这里可能是信号中断了--5秒都过了.. 还不升压..
                                AddRecord("第2阶段-严重错误-电压没变,强行终止..", true);
                                return false;
                            }
                        }
                        IsDaw2 = 0;
                        DrawPoint(2);
                    }
                    //DrawPoint(2);
                }
                AddRecord("第2阶段-自动升压结束..", false);
                plc21.Write("M0.3", false);
                return true;

            }



            if (userMainControlModel.PlcHighVoltage > TargetVoite + TargetVoite * 0.1)
            {
                AddRecord("第2阶段-开始自动降压", false);
                //这里就要去降压--
                plc21.Write("M0.4", true);
                
                //这里需要反向推动一下..
                // 7.0                                                              1400
                //userMainControlModel.PlcHighVoltage = (userMainControlModel.PlcHighVoltage * itemp) / 1000;
                //15000 /200 = 7.5
                float fT1 = (float)((TargetVoite * 1000) / 200);
                float fT2 = (float)((userMainControlModel.PlcHighVoltage * 1000) / 200);
                int IsBreadk = 0;
                float OldHighCurrent = userMainControlModel.PlcHighVoltage;

                while (fT2 > fT1)
                {
                    if (StartScanAndSave == 0 || StartScanAndSave == 2)
                    {
                        return false;
                    }
                    //AddRecord("当前:" + fT2.ToString() + "目标电压:" + fT1.ToString(), false);

                    byte[]? result2 = plc21.ReadBytes(DataType.DataBlock, 1, 2820, 4);
                    Array.Reverse(result2);
                    fT2 = BitConverter.ToSingle(result2, 0);
                    Thread.Sleep(100);
                    if (IsDaw2 < 10)
                    {
                        IsDaw2++;
                    }
                    else
                    {
                        IsBreadk++;
                        if (IsBreadk == 5)
                        {
                            //判断是不是真的升压了..
                            if (fT2 == OldHighCurrent)
                            {
                                //这里可能是信号中断了--5秒都过了.. 还不降压..
                                AddRecord("第2阶段-严重错误-电压没变,强行终止..", true);
                                return false;
                            }
                        }
                        IsDaw2 = 0;
                        DrawPoint(2);
                    }
                }
               
                plc21.Write("M0.4", false);
                AddRecord("第2阶段-降压结束", false);
                return true;
            }
            
            AddRecord("阶段2开始计数" + userMainControlModel.IsLevel2TimeOver.ToString(), false);
            return true;
        }

        public bool IsCalcRound3(float TargetVoite)
        {
            
            float gaoyadianyabaohu = (float)((userTestControlModel.GaoYaProctectVolte * 1000) / 200);
            float gaoyadianliubaohu = userTestControlModel.DiYaProctectCurrent;


            if (userMainControlModel.PlcHighVoltage < TargetVoite)
            {
                AddRecord("第3阶段-开始自动升压..", false);
                //先升到目标..
                plc21.Write("M0.3", true);
                //这里需要反向推动一下..
                // 7.0                                                              1400
                //userMainControlModel.PlcHighVoltage = (userMainControlModel.PlcHighVoltage * itemp) / 1000;
                //15000 /200 = 7.5
                float fT = (float)((TargetVoite * 1000) / 200);
                float temp = (float)((userMainControlModel.PlcHighVoltage * 1000) / 200);
                int IsBreadk = 0;
                float OldHighCurrent = userMainControlModel.PlcHighVoltage;
                while (temp < fT)
                {
                    if (StartScanAndSave == 0 || StartScanAndSave == 2)
                    {
                        return false;
                    }
                    
                    byte[]? result2 = plc21.ReadBytes(DataType.DataBlock, 1, 2820, 4);
                    Array.Reverse(result2);
                    temp = BitConverter.ToSingle(result2, 0);
                    if (temp > gaoyadianyabaohu)
                    {
                        AddRecord("第3阶段-严重错误,当前电压高于保护电压,程序停机...", true);
                        return false;
                    }

                    if (userMainControlModel.HighCurrent > gaoyadianliubaohu)
                    {
                        AddRecord("第3阶段-严重错误,当前电流高于保护电流,程序停机...", true);
                        return false;
                    }
                    Thread.Sleep(100);
                    if (IsDaw3 < 10)
                    {
                        IsDaw3++;
                    }
                    else
                    {
                        IsBreadk++;
                        if (IsBreadk == 5)
                        {
                            //判断是不是真的升压了..
                            if (temp == OldHighCurrent)
                            {
                                //这里可能是信号中断了--5秒都过了.. 还不降压..
                                AddRecord("第3阶段-严重错误-电压没变,强行终止..", true);
                                return false;
                            }
                        }
                        IsDaw3 = 0;
                        DrawPoint(2);
                    }
                }
                AddRecord("第3阶段-自动升压结束..", false);
                plc21.Write("M0.3", false);
                return true;

            }

            if (userMainControlModel.PlcHighVoltage > TargetVoite + TargetVoite * 0.1)
            {
                AddRecord("第3阶段-开始自动降压", false);
                //这里就要去降压--
                plc21.Write("M0.4", true);

                //这里需要反向推动一下..
                // 7.0                                                              1400
                //userMainControlModel.PlcHighVoltage = (userMainControlModel.PlcHighVoltage * itemp) / 1000;
                //15000 /200 = 7.5
                float fT1 = (float)((TargetVoite * 1000) / 200);
                float fT2 = (float)((userMainControlModel.PlcHighVoltage * 1000) / 200);
                int IsBreadk = 0;
                float OldHighCurrent = userMainControlModel.PlcHighVoltage;
                while (fT2 > fT1)
                {
                    if (StartScanAndSave == 0 || StartScanAndSave == 2)
                    {
                        return false;
                    }

                    //AddRecord("当前:" + fT2.ToString() + "目标电压:" + fT1.ToString(), false);

                    byte[]? result2 = plc21.ReadBytes(DataType.DataBlock, 1, 2820, 4);
                    Array.Reverse(result2);
                    fT2 = BitConverter.ToSingle(result2, 0);
                    Thread.Sleep(100);
                    if (IsDaw3 < 10)
                    {
                        IsDaw3++;
                    }
                    else
                    {
                        IsBreadk++;
                        if (IsBreadk == 5)
                        {
                            //判断是不是真的降压了..
                            if (fT2 == OldHighCurrent)
                            {
                                //这里可能是信号中断了--5秒都过了.. 还不降压..
                                AddRecord("第3阶段-严重错误-电压没变,强行终止..", true);
                                return false;
                            }
                        }
                        IsDaw3 = 0;
                        DrawPoint(2);
                    }
                }

                plc21.Write("M0.4", false);
                AddRecord("第3阶段-降压结束", false);
                return true;
            }
            
            
            AddRecord("阶段3开始计数" + userMainControlModel.IsLevel3TimeOver.ToString(), false);
            return true;
        }


        private Task ReAutoTs = null;
        private void DoBtnCommandStopTime(object param)
        {
            /*
            if (ReTs != null)
            {
                if (ReTs.IsCompleted) // 检查任务是否完成后再访问结果或状态
                {
                    //如果已经完成了，那就什么都不干了..
                    AddRecord("自动指令已经结束..", true);
                    return;
                }
                if (ReTs.Status == TaskStatus.Running || ReTs.Status == TaskStatus.WaitingToRun)
                {
                    //如果线程是运行状态
                    cts2.Cancel();
                    cts2.Dispose();
                    //AddRecord("主线程开始等待..", false);
                    // 当前线程会在这里等待直到task完成(主线程停止了，这里很危险..)
                    //这里其实如果需要，还可以去定义一个定时，操时，我想了一下，就算了..
                    //注意这个指令很危险..(主线程等待)
                    ReTs.Wait();
                    AddRecord("自动指令结束..", false);

                    cts2 = new CancellationTokenSource();
                    ReTs = null;
                }
                else
                {
                    AddRecord("工作线程无法判断状态,指令无法执行..", true);
                }
            }
            else
            {
                AddRecord("无法执行指令-因为自动代码没有执行...", true);
            }
            */
            
            if (StartScanAndSave == 2)
            {
                AddRecord("当前正在处理停止指令..", false);
            }
            else
            {
                AddRecord("执行强制停止指令..", true);
                StartScanAndSave = 2;
                
            }
            //queue.Enqueue(new CommandItem { command = 13, data = false });

            //AddRecord("强制执行分闸....", false);
            //plc21.Write("M1.0", true);
            //Thread.Sleep(500);
            //plc21.Write("M1.0", false);
            //AddRecord("分闸完成....", false);
        }


        void DrawPoint(int IsWhere,int level = 0)
        {
            //1和3只是吧数据写入数据库.. 不画点.. -- 只有2才去画点..
            if (IsWhere == 1)
            {
                ServerLineData.Add(new ObservableValue(0));//一个点..
                ServerLineData2.Add(new ObservableValue(0));//一个点..

                //入库.. 和闸..
                record.RecordDateTimer = System.DateTime.Now;
                record.BeginTimer = "Begin";
                //电压
                record.Votil = "0";
                //局放
                record.Partial = "0";
                //开始时间
              

                MyTools.SaveRecordDataToDataBase(record);
                //15 包含了合闸指令..
                plc21.Write("M1.7", true);
                Thread.Sleep(500);
                plc21.Write("M1.7", false);
                return;
            }

            if (IsWhere == 2)
            {
                //if (userMainControlModel.IsTimer >= 200)//TimePoints)
                //{
                    //ServerLineData.RemoveAt(0);
                    //ServerLineData2.RemoveAt(0);
                    //TimePoints.RemoveAt(0);
                 //   AddRecord("点已画满", true);
                //    return;
                //}
                queue.Enqueue(new CommandItem
                {
                    command = 11,
                    data = false,
                    Data1 = userMainControlModel.PartialNumber,
                    Data2 = userMainControlModel.PlcHighVoltage,
                    Data3 = level
                });
                /*
                //画点..
                ServerLineData.Add(new ObservableValue(userMainControlModel.PartialNumber));//一个点..
                ServerLineData2.Add(new ObservableValue(userMainControlModel.PlcHighVoltage));//一个点..
                record.LevelCode = string.Empty;
                record.RecordDateTimer = System.DateTime.Now;
                if (level == 1)
                {
                    record.LevelCode = "Level1";
                }

                if (level == 2)
                {

                    record.LevelCode = "Level2";
                }

                if (level == 3)
                {
                    record.LevelCode = "Level3";
                                     
                }

                record.BeginTimer = userMainControlModel.IsTimer.ToString();
                record.Votil = userMainControlModel.PlcHighVoltage.ToString();
                record.Partial = userMainControlModel.PartialNumber.ToString();
                MyTools.SaveRecordDataToDataBase(record);
                userMainControlModel.IsTimer++; //每次都加1..
                */
                return;
            }

            if (IsWhere == 3)
            {
                ServerLineData.Add(new ObservableValue(0));//一个点..
                ServerLineData2.Add(new ObservableValue(0));//一个点..

                //入库.. 分闸..
                record.RecordDateTimer = System.DateTime.Now;
                record.BeginTimer = "End";
                record.Votil = "0";
                //局放
                record.Partial = "0";


                if (!MyTools.SaveRecordDataToDataBase(record))
                {
                    AddRecord("数据保存失败!", true);
                }
                //包含了分闸指令...
                plc21.Write("M1.0", true);
                Thread.Sleep(200);
                plc21.Write("M1.0", false);

                plc21.Write("M1.5", true);
                Thread.Sleep(1000);
                plc21.Write("M1.5", false);

                return;
            }

        }

        //结束测试 。。
        void OverTest()
        {
            try
            {
                //这里就肯定结束了..
                AddRecord("所有流程完成-开始自动降压....", false);
                //这里就要去降压--
                plc21.Write("M0.4", true);
                //判断零位...
                while (userMainControlModel.Image2 == false)
                {
                    DrawPoint(2);
                    AddRecord("当前电压...." + userMainControlModel.PlcHighVoltage.ToString(), false);
                    Thread.Sleep(1000);
                }
                //DrawPoint(2); //画点--不入库
                plc21.Write("M0.4", false);
                Thread.Sleep(500);
                DrawPoint(3); //分闸..(入库)
                StartScanAndSave = 0;
                AddRecord("全部指令结束.自动化结束...", false);
                return;
            }catch(Exception ex)
            {
                AddRecord("OverTest异常:"+ex.Message, false);
                throw;
            }
        }
        
        private void InitAuto()
        {
            ReAutoTs = Task.Run(async () =>
            {
                try
                {
                    AddRecord("自动化开始..", false);

                    while (!cts2.IsCancellationRequested)
                    {                        
                        if (StartScanAndSave == 1)
                        {
                            Thread.Sleep(1000);                            
                            if (userTestControlModel.Check1 == true)
                            {
                                if (userMainControlModel.IsLevel1TimeOver > 0)
                                {                                    
                                    if (IsCalcRound1(userTestControlModel.TextLevel1Volte) == true)
                                    {
                                        DrawPoint(2, 1);
                                        userMainControlModel.IsLevel1TimeOver--;
                                        continue;
                                    }
                                    else
                                    {
                                        AddRecord("严重错误第一阶段被,强制停止-电流-电压或电流超出..自动化结束", true);
                                        StartScanAndSave = 0;
                                        DrawPoint(3);
                                        continue;
                                    }
                                }

                                if (userTestControlModel.Check2 == true)
                                {
                                    if (userMainControlModel.IsLevel2TimeOver > 0)
                                    {
                                        if (IsCalcRound2(userTestControlModel.TextLevel2Volte) == true)
                                        {
                                            DrawPoint(2, 2);
                                            userMainControlModel.IsLevel2TimeOver--;
                                            continue;
                                        }
                                        else
                                        {
                                            AddRecord("严重错误第2阶段被-强制停止电压或电流超出...自动化结束", true);
                                            StartScanAndSave = 0;
                                            DrawPoint(3);
                                            continue;
                                        }
                                    }

                                    if (userTestControlModel.Check3 == true)
                                    {
                                        if (userMainControlModel.IsLevel3TimeOver > 0)
                                        {
                                            if (IsCalcRound3(userTestControlModel.TextLevel3Volte) == true)
                                            {
                                                DrawPoint(2, 3);
                                                userMainControlModel.IsLevel3TimeOver--;
                                                continue;
                                            }
                                            else
                                            {
                                                AddRecord("严重错误第3阶段被-强制停止-电压或电流超出...自动化结束", true);
                                                StartScanAndSave = 0;
                                                //强行写入END
                                                DrawPoint(3);
                                                continue;
                                            }
                                        }
                                    }
                                }
                                OverTest();
                            }
                        }
                        if (StartScanAndSave == 0)
                        {
                            //这里有2总可能 1.程序自动结束，2程序手动结束..
                            Thread.Sleep(1);
                            continue;
                        }

                        if (StartScanAndSave == 2)
                        {
                            //强制结束测试..
                            //OverTest();
                            //结束测后-恢复为0
                            StartScanAndSave = 0;
                        }
                    }
                    //注意千万不好去AddRecord，为什么呢，因为AddRecord会转主线程-可是..主线程现在是停止状态..
                    AddRecord("自动指令结束..", false);
                }
                catch (Exception ex)
                {
                    AddRecord("自动指令执行中异常..."+ex.Message, true);
                    return;
                }

            });
        }

        private void DoBtnCommandTimer(object param)
        {
            if (IsAuto == false)
            {
                MessageBox.Show("无法开始自动程序,必须先转换到自动模式!");
                return;
            }

                //2次,也就是说，一个点击来2次指令.. 先是CLICK..

            if (ProductNumber == null)
            {
                MessageBox.Show("无法开始自动程序,还没有选择产品");
                return;
            }
            
            if (!userTestControlModel.ValideAll()) return; 



            if (StartScanAndSave == 0)
            {
                userMainControlModel.IsTimer = 0;
                ServerLineData.Clear();
                ServerLineData2.Clear();
                TimePoints.Clear();
                IsDaw3 = 0;
                IsDaw2 = 0;
                IsDaw1 = 0;

                userMainControlModel.IsLevel1TimeOver = userTestControlModel.TextLevel1Time;
                userMainControlModel.IsLevel2TimeOver = userTestControlModel.TextLevel2Time;
                userMainControlModel.IsLevel3TimeOver = userTestControlModel.TextLevel3Time;

                DrawPoint(1);
                AddRecord("自动指令开始执行....", false);
                StartScanAndSave = 1;
            }
            else
            {
                AddRecord("自动指令已在执行中....", false);
            }
            
        }
       

        public void SetCurrentUserInfo(Table_Product table_Product)
        {
            record.ProductName = table_Product.ProductName;
            record.ProductNumber = table_Product.ProductNumber;
            record.ProductType = table_Product.ProductType;
            record.ProductTuhao = table_Product.ProductTuhao;            
            record.ProductStardVotil = table_Product.ProductTestVotil;
            record.ProductStardPartial = table_Product.ProductTestPartial;
            record.ProductParts = table_Product.ProductParts;

            ProductNumber = record.ProductNumber;
            ProductType = record.ProductType;
        }

 
        public void AddRecord(string strdata, bool e)
        {
            //string time = System.DateTime.Now.ToString("HH:mm:ss");
            //string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Insert(0, new Item { Text = strdata, IsRed = e });
                logger.Error(strdata);
            });
        }
    }


    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }

    public class CommandItem
    {
        public int command { get; set; }
        
        public bool data { get; set; }
        public float Data1 { get; set; }
        public float Data2 { get; set; }

        public int Data3 { get; set; }
    }

}
