using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using S7.Net;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;
using S7.Net.Types;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PLCNET5电容塔.Data;
using System.ComponentModel.Design;
using System.Threading;
using System.Collections;

using System.Windows.Controls;
using System.Xml.Linq;
using PLCNET5电容塔升级;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using PLCNET5电容塔升级.Base;
using System.Reflection;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Transactions;



namespace PLCNET5电容塔.ViewModel
{
    //ObservableObject -- ObservableValidator(不能使用和S7冲突了..)
    public class MainViewModel : ObservableObject
    {

        //接法.
        private string userjf = string.Empty;
        public string UserJF { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //串连数.
        private string usercls = string.Empty;
        public string UserCLS { get { return usercls; } set { SetProperty(ref usercls, value); } }

        //并连数.
        private float userbls = 0.0f;
        public float UserBLS { get { return userbls; } set { SetProperty(ref userbls, value); } }


        //预故电压
        private float estimatedfvoltage = 0.0f;
        public float EstimatedVoltage { get { return estimatedfvoltage; } set { SetProperty(ref estimatedfvoltage, value); } }


        //预故电流
        private float estimatedcurrent = 0.0f;
        public float EstimatedCurrent { get { return estimatedcurrent; } set { SetProperty(ref estimatedcurrent, value); } }

        //系统电压.
        private float userxtdy = 0.0f;
        public float UserXTDY { get { return userxtdy; } set { SetProperty(ref userxtdy, value); } }

        //建议投个数
        private string usermcjygs = string.Empty;
        public string UserMCJYGS { get { return usermcjygs; } set { SetProperty(ref usermcjygs, value); } }

        // 显示等待窗口
        private WaitWindow _waitWindow;

        public MainModel mainModel{ get; set;} = new MainModel();
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();

        //主机
        private Plc plc39 = new Plc(CpuType.S71200, "192.168.2.39", 0, 1);
        //从机
        private Plc plc41 = new Plc(CpuType.S71200, "192.168.2.41", 0, 1);

        //为什么不能在这里NEW呢？
        //ProcessWindow? processWindow = null;

        //如果是这样去设置.. 只能页面给我数据.. 我数据无法刷新界面..(如果不使用SetProperty)
        //使用SetProperty -- 就可以刷新数据了.. 只要失去焦点，数据就自动刷新了..
        //[Required(ErrorMessage = "不能为空!")] -- 这里无法设置和S7 冲突了...
        
        private float _userdata1, _userdata2, _userdata3; 
        public float UserData1 { get { return _userdata1; } set { SetProperty(ref _userdata1, value); } }
        public float UserData2 { get { return _userdata2; } set { SetProperty(ref _userdata2, value); } }
        public float UserData3 { get { return _userdata3; } set { SetProperty(ref _userdata3, value); } }

        public ICommand CommandQYZKGHZ { get; set; }

        public ICommand CommandQYZKGFZ { get; set; }

        public ICommand BtnCommandShoudong { get; set; }

        public ICommand BtnCommandZhidong { get; set; }
        public ICommand BtnCommandSendHand1 { get; set; }
        public ICommand BtnCommandSendHand2 { get; set; }
        public ICommand BtnCommandSendHand3 { get; set; }
        public ICommand BtnCommandSendHand4 { get; set; }

        public ICommand BtnCommandSendHandJSOK { get; set; }
        public ICommand BtnCommandSendHandJSCZ { get; set; }


        public ICommand BtnCommandSendStep1 { get; set; }
        public ICommand BtnCommandSendStep2 { get; set; }
        public ICommand BtnCommandSendStep3 { get; set; }

        public AutomaticWin automaticWin { get; set; } = null;

        //为什么不能这样搞呢？因为这样搞，退出莫名奇妙出问题...
        public Step1Window step1Window { get; set; } = null;//new Step1Window();
        public Step2Window step2Window { get; set; } = null;

        public Step3Window step3Window { get; set; } = null;



        private bool IsWork;

        public List<string> ComboBoxItems { get; set; } = new List<string>();

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MainViewModel()
        {

            //注册..
            WeakReferenceMessenger.Default.Register<string, string>(this, "SendPLCCommand", SendPLCCommand);

            //6个按钮
            BtnCommandSendHand1 = new RelayCommand<object>(DoBtnCommandSendHand1);
            BtnCommandSendHand2 = new RelayCommand<object>(DoBtnCommandSendHand2);
            BtnCommandSendHand3 = new RelayCommand<object>(DoBtnCommandSendHand3);
            BtnCommandSendHand4 = new RelayCommand<object>(DoBtnCommandSendHand4);

            BtnCommandSendHandJSOK = new RelayCommand<object>(DoBtnCommandSendHandJSOK);
            BtnCommandSendHandJSCZ = new RelayCommand<object>(DoBtnCommandSendHandJSCZ);

            BtnCommandShoudong = new RelayCommand<object>(DoBtnCommandShoudong);
            BtnCommandZhidong = new RelayCommand<object>(DoCommandZhidong);

            CommandQYZKGHZ = new RelayCommand<object>(DoCommandQYZKGHZ);
            CommandQYZKGFZ = new RelayCommand<object>(DoCommandQYZKGFZ);


            BtnCommandSendStep1 = new RelayCommand<object>(DoBtnCommandSendStep1);
            BtnCommandSendStep2 = new RelayCommand<object>(DoBtnCommandSendStep2);
            BtnCommandSendStep3 = new RelayCommand<object>(DoBtnCommandSendStep3);


            //初始化后台线程...
            InitializeAsync();

            //必须在构造函数里面去NEW -- 不要在
            //step1Window = new Step1Window();
            //step2Window = new Step2Window();
        }

        private Task rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public  Queue<BKCommand> queue { get; set; } = new Queue<BKCommand>();
        //public string TranslationInstructions { get; set; }

        private async void InitializeAsync()
        {
            rotobTaskT = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;

                var myresult3 = await ExePlcCommand(33, true);
                if (!myresult3) { AddRecord("连接PLC主机失败,PLC正常后请重新启动软件!", true); return; }
                AddRecord("连接PLC主机成功", false);
                myresult3 = await ExePlcCommand(34, true);
                if (!myresult3) { AddRecord("连接PLC从机失败,PLC正常后请重新启动软件!", true); return; }
                AddRecord("连接PLC从机成功", false);

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
                        //定时时间到了..
                        if (milliseconds > 1000)
                        {
                            //设置为后台执行..
                            //管理线程要等待执行结果..
                            var myresult1 = await ExePlcCommand(200, true, 1);
                            //必须要判断，要不然 子线程他不等..
                            if (!myresult1)
                            {
                                AddRecord("指令读取PLC执行失败!请检查PLC-然后重新启动软件", true);
                                return;
                            }
                            precurrentTime = currentTime;
                            continue;
                        }
                        //这里我试试不使用管理线程来呢？就直接使用主线程来试试..
                        if (queue.Count <= 0) continue;
                        BKCommand Command = queue.Dequeue();

                        if (Command.Command == 1000)
                        {
                            Thread.Sleep(2000);
                            continue;
                        }

                        WeakReferenceMessenger.Default.Send<string>("开始执行动作"+ Command.CommandDescribe);

                        //其实管理线程这里，正在等待执行结果..
                        var myresult2 = await ExePlcCommand(Command.Command, Command.Data);
                        //不管执行成功或者失败..
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (_waitWindow != null)
                            {
                                _waitWindow.Close();
                                _waitWindow = null;
                            }
                        });
                        WeakReferenceMessenger.Default.Send<string>("结束执行动作" + Command.CommandDescribe);

                        if (myresult2) 
                            AddRecord("指令执行成功!", false);
                        else
                        {
                            AddRecord("指令执行失败!后台退出..", true);
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
        }                    

         void DoPlcCommand(float command,bool data)
        {
            //这里等待了... (主线程还是等了..)
            if (queue.Count >= 1)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成", true);
                return;
            }
            queue.Enqueue(new BKCommand { Command = command ,Data = data});



            _waitWindow = new WaitWindow();
            _waitWindow.ShowDialog();

        }

        

        private void SendPLCCommand(object recipient, string message)
        {
            if (message == "myButtonGA_He") { DoPlcCommand(100.0f, true); }
            if (message == "myButtonGB_He") { DoPlcCommand(100.2f, true);  }

            if (message == "myButtonG1_He") { DoPlcCommand(100.4f, true);  }
            if (message == "myButtonG2_He") { DoPlcCommand(100.6f, true);  }
            if (message == "myButtonG3_He") { DoPlcCommand(101.0f, true); }
            if (message == "myButtonG4_He") { DoPlcCommand(101.2f, true);  }
            if (message == "myButtonG5_He") { DoPlcCommand(101.4f, true);  }

            if (message == "myButton1G1_He") { DoPlcCommand(101.6f, true); }
            if (message == "myButton1G2_He") { DoPlcCommand(102.0f, true); }
            if (message == "myButton1G3_He") { DoPlcCommand(102.2f, true); }
            if (message == "myButton1G4_He") { DoPlcCommand(102.4f, true); }
            if (message == "myButton1G5_He") { DoPlcCommand(102.6f, true); }
            if (message == "myButton1G6_He") { DoPlcCommand(103.0f, true); }

            if (message == "myButton1K1_He") { DoPlcCommand(103.2f, true); }
            if (message == "myButton1K2_He") { DoPlcCommand(103.4f, true);}

            if (message == "myButtonQ1_1_He") { DoPlcCommand(103.6f, true);}
            if (message == "myButtonQ1_2_He") { DoPlcCommand(103.7f, true); }
            if (message == "myButtonQ1_3_He") { DoPlcCommand(104.0f, true); }
            if (message == "myButtonQ1_4_He") { DoPlcCommand(104.1f, true); }
            if (message == "myButtonQ1_5_He") { DoPlcCommand(104.2f, true); }
            if (message == "myButtonQ1_6_He") { DoPlcCommand(104.3f, true); }

            if (message == "myButtonQ2_1_He") { DoPlcCommand(104.4f, true); }
            if (message == "myButtonQ2_2_He") { DoPlcCommand(104.5f, true); }
            if (message == "myButtonQ2_3_He") { DoPlcCommand(104.6f, true); }
            if (message == "myButtonQ2_4_He") { DoPlcCommand(104.7f, true); }
            if (message == "myButtonQ2_5_He") { DoPlcCommand(105.0f, true); }
            if (message == "myButtonQ2_6_He") { DoPlcCommand(105.1f, true); }

            if (message == "myButtonQ3_1_He") { DoPlcCommand(105.2f, true); }
            if (message == "myButtonQ3_2_He") { DoPlcCommand(105.3f, true); }
            if (message == "myButtonQ3_3_He") { DoPlcCommand(105.4f, true); }
            if (message == "myButtonQ3_4_He") { DoPlcCommand(105.5f, true); }
            if (message == "myButtonQ3_5_He") { DoPlcCommand(105.6f, true); }
            if (message == "myButtonQ3_6_He") { DoPlcCommand(105.7f, true); }

            if (message == "myButtonQ4_1_He") { DoPlcCommand(106.0f, true); }
            if (message == "myButtonQ4_2_He") { DoPlcCommand(106.1f, true); }
            if (message == "myButtonQ4_3_He") { DoPlcCommand(106.2f, true);  }
            if (message == "myButtonQ4_4_He") { DoPlcCommand(106.3f, true);  }
            if (message == "myButtonQ4_5_He") { DoPlcCommand(106.4f, true); }
            if (message == "myButtonQ4_6_He") { DoPlcCommand(106.5f, true); }

            if (message == "myButtonQ5_1_He") { DoPlcCommand(106.6f, true); }
            if (message == "myButtonQ5_2_He") { DoPlcCommand(106.7f, true); }
            if (message == "myButtonQ5_3_He") { DoPlcCommand(107.0f, true); }
            if (message == "myButtonQ5_4_He") { DoPlcCommand(107.1f, true); }
            if (message == "myButtonQ5_5_He") { DoPlcCommand(107.2f, true); }
            if (message == "myButtonQ5_6_He") { DoPlcCommand(107.3f, true); }

            if (message == "myButtonQ6_1_He") { DoPlcCommand(107.4f, true);  }
            if (message == "myButtonQ6_2_He") { DoPlcCommand(107.5f, true); }
            if (message == "myButtonQ6_3_He") { DoPlcCommand(107.6f, true); }
            if (message == "myButtonQ6_4_He") { DoPlcCommand(107.7f, true); }
            if (message == "myButtonQ6_5_He") { DoPlcCommand(108.0f, true); }
            if (message == "myButtonQ6_6_He") { DoPlcCommand(108.1f, true); }


            if (message == "myButtonQ1_1_Fen") { DoPlcCommand(103.6f, false);}
            if (message == "myButtonQ1_2_Fen") { DoPlcCommand(103.7f, false);}
            if (message == "myButtonQ1_3_Fen") { DoPlcCommand(104.0f, false);}
            if (message == "myButtonQ1_4_Fen") { DoPlcCommand(104.1f, false);}
            if (message == "myButtonQ1_5_Fen") { DoPlcCommand(104.2f, false);}
            if (message == "myButtonQ1_6_Fen") { DoPlcCommand(104.3f, false);}

            if (message == "myButtonQ2_1_Fen") { DoPlcCommand(104.4f, false);}
            if (message == "myButtonQ2_2_Fen") { DoPlcCommand(104.5f, false);}
            if (message == "myButtonQ2_3_Fen") { DoPlcCommand(104.6f, false);}
            if (message == "myButtonQ2_4_Fen") { DoPlcCommand(104.7f, false);}
            if (message == "myButtonQ2_5_Fen") { DoPlcCommand(105.0f, false);}
            if (message == "myButtonQ2_6_Fen") { DoPlcCommand(105.1f, false); }

            if (message == "myButtonQ3_1_Fen") { DoPlcCommand(105.2f, false);}
            if (message == "myButtonQ3_2_Fen") { DoPlcCommand(105.3f, false);}
            if (message == "myButtonQ3_3_Fen") { DoPlcCommand(105.4f, false);}
            if (message == "myButtonQ3_4_Fen") { DoPlcCommand(105.5f, false);}
            if (message == "myButtonQ3_5_Fen") { DoPlcCommand(105.6f, false);}
            if (message == "myButtonQ3_6_Fen") { DoPlcCommand(105.7f, false);}

            if (message == "myButtonQ4_1_Fen") { DoPlcCommand(106.0f, false);}
            if (message == "myButtonQ4_2_Fen") { DoPlcCommand(106.1f, false);}
            if (message == "myButtonQ4_3_Fen") { DoPlcCommand(106.2f, false);}
            if (message == "myButtonQ4_4_Fen") { DoPlcCommand(106.3f, false);}
            if (message == "myButtonQ4_5_Fen") { DoPlcCommand(106.4f, false);}
            if (message == "myButtonQ4_6_Fen") { DoPlcCommand(106.5f, false);}

            if (message == "myButtonQ5_1_Fen") { DoPlcCommand(106.6f, false);}
            if (message == "myButtonQ5_2_Fen") { DoPlcCommand(106.7f, false);}
            if (message == "myButtonQ5_3_Fen") { DoPlcCommand(107.0f, false);}
            if (message == "myButtonQ5_4_Fen") { DoPlcCommand(107.1f, false);}
            if (message == "myButtonQ5_5_Fen") { DoPlcCommand(107.2f, false);}
            if (message == "myButtonQ5_6_Fen") { DoPlcCommand(107.3f, false); }

            if (message == "myButtonQ6_1_Fen") { DoPlcCommand(107.4f, false);}
            if (message == "myButtonQ6_2_Fen") { DoPlcCommand(107.5f, false);}
            if (message == "myButtonQ6_3_Fen") { DoPlcCommand(107.6f, false);}
            if (message == "myButtonQ6_4_Fen") { DoPlcCommand(107.7f, false);}
            if (message == "myButtonQ6_5_Fen") { DoPlcCommand(108.0f, false);}
            if (message == "myButtonQ6_6_Fen") { DoPlcCommand(108.1f, false);}

            if (message == "myButtonGA_Fen") { DoPlcCommand(100.1f, true); }
            if (message == "myButtonGB_Fen") { DoPlcCommand(100.3f, true); }
            // if (message == "myButtonGC_Fen") DoPlcCommand(100.1f, true);

            if (message == "myButtonG1_Fen") { DoPlcCommand(100.5f, true); }
            if (message == "myButtonG2_Fen") { DoPlcCommand(100.7f, true); }
            if (message == "myButtonG3_Fen") { DoPlcCommand(101.1f, true); }
            if (message == "myButtonG4_Fen") { DoPlcCommand(101.3f, true); }
            if (message == "myButtonG5_Fen") { DoPlcCommand(101.5f, true); }

            if (message == "myButton1G1_Fen") { DoPlcCommand(101.7f, true);}
            if (message == "myButton1G2_Fen") { DoPlcCommand(102.1f, true);}
            if (message == "myButton1G3_Fen") { DoPlcCommand(102.3f, true);}
            if (message == "myButton1G4_Fen") { DoPlcCommand(102.5f, true); }
            if (message == "myButton1G5_Fen") { DoPlcCommand(102.7f, true);}
            if (message == "myButton1G6_Fen") { DoPlcCommand(103.1f, true); }

            if (message == "myButton1K1_Fen") { DoPlcCommand(103.3f, true);}
            if (message == "myButton1K2_Fen") { DoPlcCommand(103.5f, true); }


        }

        public void MainClose()
        {
            if (rotobTaskT !=null)
            {
                cts.Cancel();                
            }
            if (automaticWin != null)
                automaticWin.Close();
        }


        ~MainViewModel()
        {
            if (plc39.IsConnected)
            {
                plc39.Close();                       
            }

            if (plc41.IsConnected)
            {
                plc41.Close();
            }
            
        }



        void InitPLCReadData()
        {
            // 位读取，也就是读取布尔值
            /*
            try
            {
                // 写
                //plc39.Write("M10.1", true);  // 写入布尔值
                //这里报错是应为.. PLC没有设置.GET PUT允许..
                byte[] result = plc39.ReadBytes(DataType.Memory, 0, 100, 1);
                for (int i = 0; i < 7; i++)
                {
                    bool value = Bit.FromByte(result[0], (byte)i);
                }
            }
            catch (Exception ex )
            {
                string message = ex.Message;
                AddRecord("PLC读异常:"+ message, false);
            }
            */
        }

        public void AddRecord(string strdata, bool e)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str =  strdata + "<=" + time ;
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });
                logger.Error(str);
            });
            WeakReferenceMessenger.Default.Send<string, string>("ScrollEnd", "ScrollEnd");
        }

        private async void DoBtnCommandSendStep3(object sender)
        {
            //显示第2步的对话框..
            /*
            if (step3Window == null)
            {
                step3Window = new Step3Window();
                step3Window.model.plc39 = plc39;
                step3Window.ShowDialog();
                step3Window = null;
            }
            */

            queue.Enqueue(new BKCommand { Command = 200, Data = true });

        }

        private async void DoBtnCommandSendStep2(object sender)
        {
            //显示第2步的对话框..
            if (step2Window == null)
            {
                step2Window = new Step2Window();
                step2Window.model.queue = queue;
                step2Window.model.UserCLS = UserCLS;
                step2Window.model.UserBLS = UserBLS;

                step2Window.model.UserXTDY = UserXTDY;
                step2Window.model.EstimatedVoltage = EstimatedVoltage;
                step2Window.model.EstimatedCurrent = EstimatedCurrent;
                step2Window.model.UserMCJYGS = UserMCJYGS;

                step2Window.model.SetTextShow();
                step2Window.ShowDialog();
                step2Window = null;

            }

        }

        private async void DoBtnCommandSendStep1(object sender)
        {
            //显示第1步的对话框..
            if (step1Window == null)
            {
                step1Window = new Step1Window();
                step1Window.model.queue = queue;
                step1Window.ShowDialog();
                
                UserCLS = step1Window.model.UserCLS;
                UserBLS = step1Window.model.UserBLS;
                UserJF = step1Window.model.UserJF + "接:" +UserCLS+"串"+ UserBLS.ToString()+"并";

                UserXTDY = step1Window.model.UserXTDY;
                EstimatedVoltage = step1Window.model.EstimatedVoltage;
                EstimatedCurrent = step1Window.model.EstimatedCurrent;
                UserMCJYGS = step1Window.model.UserMCJYGS;

                step1Window = null;
            }
            
        }


        private async void DoCommandZhidong(object sender)
        {
            //记住 对话框，不能在前面去NEW，，这样主线程退出的时候，对话框是不会跟着主线程退出的..
            if (automaticWin == null)
            {
                automaticWin = new AutomaticWin();
                automaticWin.model.plc39 = plc39;
                automaticWin.Show();
            }
            return;
        }

        private void DoCommandQYZKGHZ(object sender)
        {
            DoPlcCommand(201f, true); 
            return;
        }

        private void DoCommandQYZKGFZ(object sender)
        {
            DoPlcCommand(202f, true);
            return;
        }


        private async void DoBtnCommandShoudong(object sender)
        {
            if (automaticWin != null)
            {
                automaticWin.Close();
                automaticWin = null;
            }
            return;
        }



        bool[]? alldian = null;

        public void SetAllDian(bool[] datad)
        {
            alldian = datad;
            //mainModel.SetAllDian(alldian);
        }

        //更新大框左边..
        void ReadAllPlcData(bool updateshow = true)
        {
            try
            {
                Array.Clear(alldian, 0, alldian.Length);
                
                int k = 0;


                byte[]? result0 = plc39.ReadBytes(DataType.Input, 0, 0, 30);

                //读PLC39 21
                //39 21.0 = 21.7 k =0
                byte result1 = result0[21];
                for (int i = 0; i < 8; i++) 
                {
                    bool value = Bit.FromByte(result1, (byte)i); 
                    alldian[k++] = value; 
                }

                //39 24.0 = 24.7 k = 8
                //39 25.0 = 25.7 k = 16
                byte[] result2 = new byte[2];
                result2[0] = result0[24]; 
                result2[1] = result0[25];
                for (int j =  0; j < 2; j++) 
                    for (int i = 0; i < 8; i++) 
                    { 
                        bool value = Bit.FromByte(result2[j], (byte)i); 
                        alldian[k++] = value;
                    }


                //读PLC41 0--1.7(16)
                //Array.Clear(alldian, 0, alldian.Length);
                // k = 16 -- 16 
                result0 = plc41.ReadBytes(DataType.Input, 0, 0, 30);

                byte[] result3 = new byte[2];
                result3[0] = result0[0];
                result3[1] = result0[1];                
                for (int j = 0; j < 2; j++) 
                    for (int i = 0; i < 8; i++)
                    { 
                        bool value = Bit.FromByte(result3[j], (byte)i); 
                        alldian[k++] = value; 
                    }

                //读PLC41 8.0--8.7(16)
                // k = 32 -- 16 
                byte[] result4 = new byte[2];
                result4[0] = result0[8];
                result4[1] = result0[9];

                for (int j = 0; j < 2; j++) 
                    for (int i = 0; i < 8; i++) 
                    {
                        bool value = Bit.FromByte(result4[j], (byte)i); 
                        alldian[k++] = value; 
                    }

                //读PLC41 12.0--12.7(16)
                // k = 48 -- 16 
                byte[] result5 = new byte[2];
                result5[0] = result0[12];
                result5[1] = result0[13];
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result5[j], (byte)i);
                        alldian[k++] = value; 
                    }

                //读PLC41 16.0--16.7(16)
                //k = 64 -- 16 
                byte[] result6 = new byte[2];
                result6[0] = result0[16];
                result6[1] = result0[17];
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result6[j], (byte)i); 
                        alldian[k++] = value; 
                    }

                //读PLC41 20.0--21.7(16)
                //k = 80 -- 16 
                byte[] result7 = new byte[2];
                result7[0] = result0[20];
                result7[1] = result0[21];
                for (int j = 0; j < 2; j++) 
                    for (int i = 0; i < 8; i++) 
                    { 
                        bool value = Bit.FromByte(result7[j], (byte)i); 
                        alldian[k++] = value; 
                    }

                //读PLC41 24.0--25.7(16)
                ////k = 96 -- 16 
                byte[] result8 = plc41.ReadBytes(DataType.Input, 0, 24, 2);
                for (int j = 0; j < 2; j++) for (int i = 0; i < 8; i++) { bool value = Bit.FromByte(result8[j], (byte)i); alldian[k++] = value; }

                k = 120;

                //读PLC39 0--1.7(16)
                //k- 120 -- 16
                byte[] result9 = plc39.ReadBytes(DataType.Input, 0, 0, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result9[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 8.0--9.7(16)
                //137 -- 16    142..
                byte[] result10 = plc39.ReadBytes(DataType.Input, 0, 8, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result10[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 12.0--13.7(16)
                //153   -- 16
                byte[] result11 = plc39.ReadBytes(DataType.Input, 0, 12, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result11[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 16.0--17.7(16)
                //169 -- 16
                byte[] result12 = plc39.ReadBytes(DataType.Input, 0, 16, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result12[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 20.0(8)
                //185 -- 8
                byte[] result13 = plc39.ReadBytes(DataType.Input, 0, 20, 1);
                //byte result13 = result0[20];
                for (int i = 0; i < 8; i++)
                {
                    bool value = Bit.FromByte(result13[0], (byte)i);
                    alldian[k++] = value;
                }

                if (updateshow == true)
                {
                    //更新界面(主界面 - 子界面)，下面的红点...
                    WeakReferenceMessenger.Default.Send<string, string>("ShowAllPlcDian", "SWaitchXZ");
                }

                //更新界面1（主界面）(状态上面.)，下面的红点...
                //mainModel.UpdateUIUp();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        bool ScanConntion()
        {

            if (!plc39.IsConnected)
            {
                AddRecord("PLC-31主机断线,请检查PLC链接是否正常...", true);
                mainModel.IsPlc39 = true;//设置红灯
                return false;
            }
            if (!plc41.IsConnected)
            {
                AddRecord("PLC-33从机断线,请检查PLC链接是否正常...", true);
                mainModel.IsPlc41 = true; //设置红灯
                return false;
            }
            mainModel.IsPlc39 = false; //设置绿灯
            mainModel.IsPlc41 = false;//设置绿灯
            return true;
        }


        private async Task<bool> ExePlcCommand(float command, bool data,int isBackCall = 0)
        {            
            if (IsWork)
            {
                if (isBackCall == 0) AddRecord("请等待上一个任务完成!",true);
                return true;
            }
            IsWork = true;
            
            
            bool Success = false;
            string error = string.Empty;
            if (isBackCall == 0) WeakReferenceMessenger.Default.Send<string, string>("Dn", "EnableAll");
            if (isBackCall == 0) AddRecord("请等待,PLC回应!", false);
            var timeout = Task.Delay(30000);

            var t = Task.Run(async () =>
            {
                try
                {
                    if (command != 33 && command != 34) 
                    {
                        if (!ScanConntion()) 
                        {
                            Success = false; 
                            return;
                        }
                    }

                    int knifeSleep = 0;
                    

                    switch (command)

                    {
                        case 201f:
                            {
                                plc39.Write("M20.7", true);
                                Thread.Sleep(1000);
                                plc39.Write("M20.7", false);
                                AddRecord("气源开关和闸", false);
                                break;
                            }

                        case 202f:
                            {
                                plc39.Write("M20.6", true);
                                Thread.Sleep(1000);
                                plc39.Write("M20.6", false);
                                AddRecord("气源开关分闸", false);
                                break;
                            }

                        //必须先判断一下状态.. 
                        case 100.0f:
                            {
                                plc39.Write("M100.0", true); 
                                Thread.Sleep(5000);
                                plc39.Write("M100.0", false);
                           
                                for (knifeSleep = 0; knifeSleep < 15; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);

                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[120] == true)
                                    {
                                        AddRecord("PLC的总输出AC和闸完成", false);
                                        break;
                                    }
                                }
                                
                                if (knifeSleep == 15)
                                {
                                    AddRecord("严重错误:过了15秒,还没有收到PLC的总输出AC和闸信号", true);
                                }
                                break; 
                            }//"总输出AC和闸"
                        case 100.1f: 
                            {
                                plc39.Write("M100.1", true);
                                Thread.Sleep(5000); 
                                plc39.Write("M100.1", false);
                                for (knifeSleep = 0; knifeSleep < 15; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[121] == true)
                                    {
                                        AddRecord("PLC的总输出AC分闸完成", false);
                                        break;
                                    }

                                }
                                if (knifeSleep == 15)
                                {
                                    AddRecord("严重错误:过了15秒,还没有收到PLC的总输出AC分闸信号", true);
                                }
                                break;
                            }//"总输出AC分闸"
                        
                        case 100.2f: 
                            {
                                plc39.Write("M100.2", true);
                                Thread.Sleep(3000);
                                plc39.Write("M100.2", false);
                                for (knifeSleep = 0; knifeSleep < 15; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[122] == true)
                                    {
                                        AddRecord("PLC的总输出B和闸完成", false);
                                        break;
                                    }

                                }
                                if (knifeSleep == 15)
                                {
                                    AddRecord("严重错误:过了10秒,还没有收到PLC的总输出B和闸信号", true);
                                }
                                break;
                            }//"总输出B和闸"

                        case 100.3f:
                            {
                                plc39.Write("M100.3", true); 
                                Thread.Sleep(3000);
                                plc39.Write("M100.3", false);
                                for (knifeSleep = 0; knifeSleep < 15; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[123] == true)
                                    {
                                        AddRecord("PLC的总输出B分闸完成", true);
                                        break;
                                    }

                                }
                                if (knifeSleep == 15)
                                {
                                    AddRecord("严重错误:过了10秒,还没有收到PLC的总输出B分闸信号", true);
                                }
                                break; 
                            }//"总输出B分闸"

                        case 100.4f: 
                            {
                                plc39.Write("M100.4", true); 
                                Thread.Sleep(3000);
                                plc39.Write("M100.4", false);
                                
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[128] == true)
                                    {
                                        AddRecord("PLC的G1合闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G1合闸信号", true);
                                }
                                break;
                            }//"G1合闸"
                            
                        case 100.5f:
                            { 
                                plc39.Write("M100.5", true);
                                Thread.Sleep(5000);
                                plc39.Write("M100.5", false);
                                for (knifeSleep = 0; knifeSleep < 15; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[129] == true)
                                    {
                                        AddRecord("PLC的G1分闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G1分闸信号", true);
                                }
                                break;
                            }//"G1分闸"

                        case 100.6f: 
                            {
                                plc39.Write("M100.6", true); 
                                Thread.Sleep(3000);
                                plc39.Write("M100.6", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[130] == true)
                                    {
                                        AddRecord("PLC的G2合闸信号", false);
                                        break;
                                    }
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G2合闸信号", true);
                                }

                                break;
                            }//"G2合闸"

                        case 100.7f: 
                            {
                                plc39.Write("M100.7", true); 
                                Thread.Sleep(5000);
                                plc39.Write("M100.7", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[131] == true)
                                    {
                                        AddRecord("收到PLC的G2分闸信号", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G2分闸信号", true);
                                }

                                break; 
                            }//"G2分闸"
                            
                        case 101.0f: 
                            {
                                plc39.Write("M101.0", true);
                                Thread.Sleep(3000);
                                plc39.Write("M101.0", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[132] == true)
                                    {
                                        AddRecord("收到PLC的G3合闸信号", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G3合闸信号", true);
                                }

                                break;
                            }//"G3合闸"

                        case 101.1f:
                            {
                                plc39.Write("M101.1", true);
                                Thread.Sleep(3000);
                                plc39.Write("M101.1", false);
                                for (knifeSleep = 0; knifeSleep < 15; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[133] == false)
                                    {
                                        AddRecord("收到PLC的G3分闸信号", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G3分闸信号", true);
                                }

                                break; 
                            }//"G3分闸"


                        case 101.2f:
                            {
                                plc39.Write("M101.2", true);
                                Thread.Sleep(3000);
                                plc39.Write("M101.2", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[136] == true)
                                    {
                                        AddRecord("严重错误:过了8秒,还没有收到PLC的G4合闸信号", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G4合闸信号", true);
                                }

                                break;
                            }//"G4合闸"

                        case 101.3f: 
                            {
                                plc39.Write("M101.3", true);
                                Thread.Sleep(3000);
                                plc39.Write("M101.3", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[137] == true)
                                    {
                                        AddRecord("收到PLC的G4分闸信号", false);
                                        break;
                                    }
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G4分闸信号", true);
                                }
                                break;
                            }//"G4分闸"

                        case 101.4f: 
                            {
                                plc39.Write("M101.4", true);
                                Thread.Sleep(3000);
                                plc39.Write("M101.4", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[138] == true)
                                    {
                                        AddRecord("收到PLC的G5合闸信号", false);
                                        break;
                                    }
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G5合闸信号", true);
                                }
                                break; 
                            }//"G5合闸"
                        
                        case 101.5f: 
                            {
                                plc39.Write("M101.5", true); 
                                Thread.Sleep(3000); 
                                plc39.Write("M101.5", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[139] == true)
                                    {
                                        AddRecord("收到PLC的G5分闸信号", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的G5分闸信号", true);
                                }

                                break;
                            }//"G5分闸"

                        case 101.6f: 
                            {
                                plc39.Write("M101.6", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M101.6", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[140] == true && alldian[152] == true && alldian[164] == true)
                                    {
                                        AddRecord("1G1,2G1,3G1合闸完成", false);
                                        break;
                                    }
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G1,2G1,3G1合闸信号", true);
                                }
                                break; 
                            }//"1G1,2G1,3G1合闸"
                        
                        case 101.7f:
                            {
                                plc39.Write("M101.7", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M101.7", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[141] == true && alldian[153] == true && alldian[165] == true)
                                    {
                                        AddRecord("1G1,2G1,3G1分闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G1,2G1,3G1分闸信号", true);
                                }

                                break; 
                            }//"1G1,2G1,3G1分闸"

                        case 102.0f: 
                            {
                                plc39.Write("M102.0", true);
                                Thread.Sleep(5000); 
                                plc39.Write("M102.0", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[142] == true && alldian[154] == true && alldian[166] == true)
                                    {
                                        AddRecord("1G2,2G2,3G2合闸完成", false);
                                        break;
                                    }
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G2,2G2,3G2合闸信号", true);
                                }
                                break; 
                            }//"1G2,2G2,3G2合闸"
                        
                        case 102.1f: 
                            {
                                plc39.Write("M102.1", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M102.1", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[143] == true && alldian[155] == true && alldian[167] == true)
                                    {
                                        AddRecord("1G2,2G2,3G2分闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G2,2G2,3G2分闸信号", true);
                                }

                                break; 
                            }//"1G2,2G2,3G2分闸"

                        case 102.2f: 
                            {
                                plc39.Write("M102.2", true);
                                Thread.Sleep(5000); 
                                plc39.Write("M102.2", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);

                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[144] == true && alldian[156] == true && alldian[168] == true)
                                    {
                                        AddRecord("1G3,2G3,3G3合闸完成", false);
                                        break;
                                    }
                                 }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了15秒,还没有收到PLC的1G3,2G3,3G3合闸信号", true);
                                }
                                break; 
                            }//"1G3,2G3,3G3合闸"
                        
                        case 102.3f: 
                            {
                                plc39.Write("M102.3", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M102.3", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[145] == true && alldian[157] == true && alldian[169] == true)
                                    {
                                        AddRecord("1G3,2G3,3G3分闸完成", false);
                                        break;
                                    }
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G3,2G3,3G3分闸信号", true);
                                }
                                break; 
                            }//"1G3,2G3,3G3分闸"

                        case 102.4f: 
                            {
                                plc39.Write("M102.4", true); 
                                Thread.Sleep(5000);
                                plc39.Write("M102.4", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[146] == true && alldian[158] == true && alldian[170] == true)
                                    {
                                        AddRecord("1G4,2G4,3G4合闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G4,2G4,3G4合闸信号", true);
                                }

                                break;
                            }//"1G4,2G4,3G4合闸"
                        
                        case 102.5f: 
                            {
                                plc39.Write("M102.5", true);
                                Thread.Sleep(5000); 
                                plc39.Write("M102.5", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[147] == true && alldian[159] == true && alldian[171] == true)
                                    {
                                        AddRecord("1G4,2G4,3G4分闸完成", false);
                                        break;
                                    }
                              
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G4,2G4,3G4分闸信号", true);
                                }
                                break;
                            }//"1G4,2G4,3G4分闸"

                        case 102.6f:
                            {
                                plc39.Write("M102.6", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M102.6", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[148] == true && alldian[160] == true && alldian[172] == true)
                                    {
                                        AddRecord("1G5,2G5,3G5合闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G5,2G5,3G5合闸信号", true);
                                }

                                break; 
                            }//"1G5,2G5,3G5合闸"
                        
                        case 102.7f: 
                            {
                                plc39.Write("M102.7", true);
                                Thread.Sleep(5000); 
                                plc39.Write("M102.7", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[149] == true && alldian[161] == true && alldian[173] == true)
                                    {
                                        AddRecord("1G5,2G5,3G5分闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G5,2G5,3G5分闸信号", true);
                                }

                                break; 
                            }//"1G5,2G5,3G5分闸"

                        case 103.0f:
                            {
                                plc39.Write("M103.0", true); 
                                Thread.Sleep(5000);
                                plc39.Write("M103.0", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[150] == true && alldian[162] == true && alldian[174] == true)
                                    {
                                        AddRecord("1G6,2G6,3G6合闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G6,2G6,3G6合闸信号", true);
                                }

                                break;
                            }//"1G6,2G6,3G6合闸"
                        
                        case 103.1f: 
                            {
                                plc39.Write("M103.1", true);
                                Thread.Sleep(5000); 
                                plc39.Write("M103.1", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++) 
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[151] == true && alldian[163] == true && alldian[175] == true)
                                    {
                                        AddRecord("1G6,2G6,3G6分闸完成", false);
                                        break;
                                    }
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的1G6,2G6,3G6分闸信号", true);
                                }
                                break;
                            }//"1G6,2G6,3G6分闸"

                        case 103.2f: 
                            {
                                plc39.Write("M103.2", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M103.2", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[176] == true && alldian[180] == true && alldian[186] == true)
                                    {
                                        AddRecord("K1,K3,K5合闸完成", false);
                                        break;
                                    }
                               
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的K1,K3,K5合闸信号", true);
                                }
                                break; 
                            }//"K1,K3,K5合闸"
                        
                        case 103.3f: 
                            {
                                plc39.Write("M103.3", true);
                                Thread.Sleep(5000);
                                plc39.Write("M103.3", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[177] == true && alldian[181] == true && alldian[187] == true)
                                    {
                                        AddRecord("K1,K3,K5分闸完成", false);
                                        break;
                                    }
                            
                                }

                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的K1,K3,K5分闸信号", true);
                                }
                                break; 
                            }//"K1,K3,K5分闸"

                        case 103.4f: 
                            {
                                plc39.Write("M103.4", true);
                                Thread.Sleep(5000);
                                plc39.Write("M103.4", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[178] == true && alldian[184] == true && alldian[188] == true)
                                    {
                                        AddRecord("K2,K4,K6合闸完成", false);
                                        break;
                                    }
                             
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的K2,K4,K6合闸信号", true);
                                }
                                break;
                            }//"K2,K4,K6合闸"
                        
                        case 103.5f:
                            {
                                plc39.Write("M103.5", true); 
                                Thread.Sleep(5000); 
                                plc39.Write("M103.5", false);
                                for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                {
                                    Thread.Sleep(1000);
                                    //这里必须判断了..
                                    ReadAllPlcData(false);
                                    //如果过了15秒，刀闸信号还不回来.. 就说明 设备有问题..
                                    if (alldian[179] == true && alldian[185] == true && alldian[189] == true)
                                    {
                                        AddRecord("PLC的K2,K4,K6分闸完成", false);
                                        break;
                                    }
                            
                                }
                                if (knifeSleep == 10)
                                {
                                    AddRecord("严重错误:过了8秒,还没有收到PLC的K2,K4,K6分闸信号", true);
                                }
                                break; 
                            }//"K2,K4,K6分闸"

                        case 103.6f: 
                            {
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M103.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M103.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[0] == true && alldian[40] == true && alldian[80] == true)
                                        {
                                            AddRecord("Q1-1合闸完成", false);
                                            break;
                                        }
                                
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M203.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M203.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[0] == false && alldian[40] == false && alldian[80] == false)
                                        {
                                            AddRecord("Q1-1分闸完成", false);
                                            break;
                                        }                                
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1分闸信号-未返回.请检查设备", true);
                                    }
                                }

                                break;
                            }//"Q1-1合闸"
                        case 103.7f: 
                            {
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M103.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M103.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-2 BQ1-2 CQ1-2
                                        ReadAllPlcData(false);
                                        if (alldian[1] == true && alldian[41] == true && alldian[81] == true)
                                        {
                                            AddRecord("Q1-2合闸完成", false);
                                            break;
                                        }
                               
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-2合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M203.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M203.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[1] == false && alldian[41] == false && alldian[81] == false)
                                        {
                                            AddRecord("Q1-2分闸完成", false);
                                            break;
                                        }
                                      

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-2分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q1-2合闸"

                        case 104.0f: 
                            {
                                //plc39.Write("M104.0", data);

                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-3 BQ1-3 CQ1-3
                                        ReadAllPlcData(false);
                                        if (alldian[2] == true && alldian[42] == true && alldian[82] == true)
                                        {
                                            AddRecord("Q1-3合闸完成", false);
                                            break;
                                        }
                                
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-3合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[2] == false && alldian[42] == false && alldian[82] == false)
                                        {
                                            AddRecord("Q1-3分闸完成", false);
                                            break;
                                        }
                           
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-3分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q1-3合闸"

                        case 104.1f: 
                            {
                                //plc39.Write("M104.1", data);
                                if (data == true)
                                {

                                    //合闸..
                                    plc39.Write("M104.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-4 BQ1-4 CQ1-4
                                        ReadAllPlcData(false);
                                        if (alldian[3] == true && alldian[43] == true && alldian[83] == true)
                                        {
                                            AddRecord("Q1-4合闸完成", false);
                                            break;
                                        }
                                       

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-4合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[3] == false && alldian[43] == false && alldian[83] == false)
                                        {
                                            AddRecord("Q1-4分闸完成", false);
                                            break;
                                        }
                                
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-4分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q1-4合闸"                                     
                        case 104.2f:
                            {
                                //plc39.Write("M104.2", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-5 BQ1-5 CQ1-5
                                        ReadAllPlcData(false);
                                        if (alldian[4] == true && alldian[44] == true && alldian[84] == true)
                                        {
                                            AddRecord("Q1-5合闸完成", false);
                                            break;
                                        }
                                
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-5合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[4] == false && alldian[44] == false && alldian[84] == false)
                                        {
                                            AddRecord("Q1-5分闸完成", false);
                                            break;
                                        }
                                    
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-5分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q1-5合闸"
                        case 104.3f:
                            {
                                //plc39.Write("M104.3", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-6 BQ1-6 CQ1-6
                                        ReadAllPlcData(false);
                                        if (alldian[5] == true && alldian[45] == true && alldian[85] == true)
                                        {
                                            AddRecord("Q1-6合闸完成", false);
                                            break;
                                        }
                                      
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-6合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[5] == false && alldian[45] == false && alldian[85] == false)
                                        {
                                            AddRecord("Q1-6分闸完成", false);
                                            break;
                                        }
                                        
                                    }

                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-6分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q1-6合闸"

                        case 104.4f:
                            {
                                //plc39.Write("M104.4", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ2-1 BQ2-1 CQ2-1
                                        ReadAllPlcData(false);
                                        if (alldian[6] == true && alldian[46] == true && alldian[88] == true)
                                        {
                                            AddRecord("Q2-1合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[6] == false && alldian[46] == false && alldian[88] == false)
                                        {
                                            AddRecord("Q2-1分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-1分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q2-1合闸"
                        case 104.5f: 
                            {
                                //plc39.Write("M104.5", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ2-2 BQ2-2 CQ2-2
                                        ReadAllPlcData(false);
                                        if (alldian[7] == true && alldian[47] == true && alldian[89] == true)
                                        {
                                            AddRecord("Q2-2合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-2合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[7] == false && alldian[47] == false && alldian[89] == false)
                                        {
                                            AddRecord("Q2-2分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-2分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q2-2合闸"
                        case 104.6f: 
                            {
                                //plc39.Write("M104.6", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ2-3 BQ2-3 CQ2-3
                                        ReadAllPlcData(false);
                                        if (alldian[8] == true && alldian[48] == true && alldian[90] == true)
                                        {
                                            AddRecord("Q2-3合闸完成", false);
                                            break;
                                        }
                                     
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-3合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[8] == false && alldian[48] == false && alldian[90] == false)
                                        {
                                            AddRecord("Q2-3分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-3分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q2-3合闸"
                        case 104.7f: 
                            {
                                //plc39.Write("M104.7", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M104.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M104.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ2-4 BQ2-4 CQ2-4
                                        ReadAllPlcData(false);
                                        if (alldian[9] == true && alldian[49] == true && alldian[91] == true)
                                        {
                                            AddRecord("Q2-4合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-4合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M204.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M204.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[9] == false && alldian[49] == false && alldian[91] == false)
                                        {
                                            AddRecord("Q2-4分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-4分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q2-4合闸"
                        case 105.0f: 
                            {
                                //plc39.Write("M105.0", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ2-5 BQ2-5 CQ2-5
                                        ReadAllPlcData(false);
                                        if (alldian[10] == true && alldian[50] == true && alldian[92] == true)
                                        {
                                            AddRecord("Q2-5合闸完成", false);
                                            break;
                                        }

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-5合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[10] == false && alldian[50] == false && alldian[92] == false)
                                        {
                                            AddRecord("Q2-5分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-5分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q2-5合闸"
                        case 105.1f: 
                            {
                                //plc39.Write("M105.1", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ2-6 BQ2-6 CQ2-6
                                        ReadAllPlcData(false);
                                        if (alldian[11] == true && alldian[51] == true && alldian[93] == true)
                                        {
                                            AddRecord("Q2-6合闸完成", false);
                                            break;
                                        }

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-6合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[11] == false && alldian[51] == false && alldian[93] == false)
                                        {
                                            AddRecord("Q2-6分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q2-6分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q2-6合闸"

                        case 105.2f:
                            {
                                //plc39.Write("M105.2", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ3-1 BQ3-1 CQ3-1
                                        ReadAllPlcData(false);
                                        if (alldian[12] == true && alldian[52] == true && alldian[94] == true)
                                        {
                                            AddRecord("Q3-1合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[12] == false && alldian[52] == false && alldian[94] == false)
                                        {
                                            AddRecord("Q3-1分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-1分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q3-1合闸"
                        case 105.3f:
                            {
                                //plc39.Write("M105.3", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ3-2 BQ3-2 CQ3-2
                                        ReadAllPlcData(false);
                                        if (alldian[13] == true && alldian[53] == true && alldian[95] == true)
                                        {
                                            AddRecord("Q3-2合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-2合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[13] == false && alldian[53] == false && alldian[95] == false)
                                        {
                                            AddRecord("Q3-2分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-2分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q3-2合闸"
                        case 105.4f: 
                            {
                                //plc39.Write("M105.4", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ3-3 BQ3-3 CQ3-3
                                        ReadAllPlcData(false);
                                        if (alldian[14] == true && alldian[54] == true && alldian[96] == true)
                                        {
                                            AddRecord("Q3-3合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[14] == false && alldian[54] == false && alldian[96] == false)
                                        {
                                            AddRecord("Q3-3分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-3分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q3-3合闸"
                        case 105.5f:
                            { 
                                //plc39.Write("M105.5", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ3-4 BQ3-4 CQ3-4
                                        ReadAllPlcData(false);
                                        if (alldian[15] == true && alldian[55] == true && alldian[97] == true)
                                        {
                                            AddRecord("Q3-4合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-4合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[15] == false && alldian[55] == false && alldian[97] == false)
                                        {
                                            AddRecord("Q3-4分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-4分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q3-4合闸"
                        case 105.6f:
                            {
                                //plc39.Write("M105.6", data);

                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ3-5 BQ3-5 CQ3-5
                                        ReadAllPlcData(false);
                                        if (alldian[16] == true && alldian[56] == true && alldian[98] == true)
                                        {
                                            AddRecord("Q3-5合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-5合闸信号-未返回.请检查设备", true);
                                    }

                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[16] == false && alldian[56] == false && alldian[98] == false)
                                        {
                                            AddRecord("Q3-5分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-5分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q3-5合闸"
                        case 105.7f:
                            {
                                //plc39.Write("M105.7", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M105.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M105.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ3-6 BQ3-6 CQ3-6
                                        ReadAllPlcData(false);
                                        if (alldian[17] == true && alldian[57] == true && alldian[99] == true)
                                        {
                                            AddRecord("Q3-6合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-6合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M205.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M205.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[17] == false && alldian[57] == false && alldian[99] == false)
                                        {
                                            AddRecord("Q3-6分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q3-6分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q3-6合闸"

                        case 106.0f:
                            {
                                //plc39.Write("M106.0", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ4-1 BQ4-1 CQ4-1
                                        ReadAllPlcData(false);
                                        if (alldian[18] == true && alldian[58] == true && alldian[100] == true)
                                        {
                                            AddRecord("Q4-1合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[18] == false && alldian[58] == false && alldian[100] == false)
                                        {
                                            AddRecord("Q4-1分闸完成", false);
                                            break;
                                        }
                                       
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-1分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q4-1合闸"
                        case 106.1f:
                            { 
                                //plc39.Write("M106.1", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ4-2 BQ4-2 CQ4-2
                                        ReadAllPlcData(false);
                                        if (alldian[19] == true && alldian[59] == true && alldian[101] == true)
                                        {
                                            AddRecord("Q4-2合闸完成", false);
                                            break;
                                        }
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[19] == false && alldian[59] == false && alldian[101] == false)
                                        {
                                            AddRecord("Q4-2分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-2分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q4-2合闸"
                        case 106.2f: 
                            {
                                //plc39.Write("M106.2", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ4-3 BQ4-3 CQ4-3
                                        ReadAllPlcData(false);
                                        if (alldian[20] == true && alldian[60] == true && alldian[102] == true)
                                        {
                                            AddRecord("Q4-3合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-3合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[20] == false && alldian[60] == false && alldian[102] == false)
                                        {
                                            AddRecord("Q4-3分闸完成", false);
                                            break;
                                        }
                                    
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-3分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q4-3合闸"
                        case 106.3f:
                            {
                                //plc39.Write("M106.3", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ4-4 BQ4-4 CQ4-4
                                        ReadAllPlcData(false);
                                        if (alldian[21] == true && alldian[61] == true && alldian[103] == true)
                                        {
                                            AddRecord("Q4-4合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-4合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[21] == false && alldian[61] == false && alldian[103] == false)
                                        {
                                            AddRecord("Q4-4分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-4分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q4-4合闸"
                        case 106.4f: 
                            {
                                //plc39.Write("M106.4", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ4-5 BQ4-5 CQ4-5
                                        ReadAllPlcData(false);
                                        if (alldian[22] == true && alldian[64] == true && alldian[104] == true)
                                        {
                                            AddRecord("Q4-5合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[22] == false && alldian[64] == false && alldian[104] == false)
                                        {
                                            AddRecord("Q4-5分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q4-5合闸"
                        case 106.5f:
                            {
                                //plc39.Write("M106.5", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ4-6 BQ4-6 CQ4-6
                                        ReadAllPlcData(false);
                                        if (alldian[23] == true && alldian[65] == true && alldian[105] == true)
                                        {
                                            AddRecord("Q4-6合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-6合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[23] == false && alldian[65] == false && alldian[105] == false)
                                        {
                                            AddRecord("Q4-6分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q4-6分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q4-6合闸"

                        case 106.6f:
                            {
                                //plc39.Write("M106.6", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-1 BQ5-1 CQ5-1
                                        ReadAllPlcData(false);
                                        if (alldian[24] == true && alldian[66] == true && alldian[106] == true)
                                        {
                                            AddRecord("Q5-1合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[24] == false && alldian[66] == false && alldian[106] == false)
                                        {
                                            AddRecord("Q5-1分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-1分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q5-1合闸"
                        case 106.7f: 
                            {
                                //plc39.Write("M106.7", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M106.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M106.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-2 BQ5-2 CQ5-2
                                        ReadAllPlcData(false);
                                        if (alldian[25] == true && alldian[67] == true && alldian[107] == true)
                                        {
                                            AddRecord("Q5-2合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-2合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M206.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M206.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[25] == false && alldian[67] == false && alldian[107] == false)
                                        {
                                            AddRecord("Q5-2分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-2分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q5-2合闸"
                        case 107.0f:
                            {
                                //plc39.Write("M107.0", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-3 BQ5-3 CQ5-3
                                        ReadAllPlcData(false);
                                        if (alldian[26] == true && alldian[68] == true && alldian[108] == true)
                                        {
                                            AddRecord("Q5-3合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1合闸信号-未返回.请检查设备", true);
                                    }

                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[26] == false && alldian[68] == false && alldian[108] == false)
                                        {
                                            AddRecord("Q5-3分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-3分闸信号-未返回.请检查设备", true);
                                    }
                                }

                                break;
                            }//"Q5-3合闸"
                        case 107.1f: 
                            {
                                //plc39.Write("M107.1", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-4 BQ5-4 CQ5-4
                                        ReadAllPlcData(false);
                                        if (alldian[27] == true && alldian[69] == true && alldian[109] == true)
                                        {
                                            AddRecord("Q5-4合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q1-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[27] == false && alldian[69] == false && alldian[109] == false)
                                        {
                                            AddRecord("Q5-4分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-4分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q5-4合闸"
                        case 107.2f:
                            { 
                                //plc39.Write("M107.2", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-5 BQ5-5 CQ5-5
                                        ReadAllPlcData(false);
                                        if (alldian[28] == true && alldian[70] == true && alldian[110] == true)
                                        {
                                            AddRecord("Q5-5合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-5合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.2", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.2", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[28] == false && alldian[70] == false && alldian[110] == false)
                                        {
                                            AddRecord("Q5-5分闸完成", false);
                                            break;
                                        }
                                      
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-5分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break; 
                            }//"Q5-5合闸"
                        case 107.3f:
                            {
                                //plc39.Write("M107.3", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-6 BQ5-6 CQ5-6
                                        ReadAllPlcData(false);
                                        if (alldian[29] == true && alldian[71] == true && alldian[111] == true)
                                        {
                                            AddRecord("Q5-6合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-6合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.3", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.3", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ5-6 BQ5-6 CQ5-6
                                        ReadAllPlcData(false);
                                        if (alldian[29] == false && alldian[71] == false && alldian[111] == false)
                                        {
                                            AddRecord("Q5-6分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q5-6分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q5-6合闸"

                        case 107.4f: 
                            {
                                //plc39.Write("M107.4", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-1 BQ6-1 CQ6-1
                                        ReadAllPlcData(false);
                                        if (alldian[32] == true && alldian[72] == true && alldian[113] == true)
                                        {
                                            AddRecord("Q6-1合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-1合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.4", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.4", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[32] == false && alldian[72] == false && alldian[113] == false)
                                        {
                                            AddRecord("Q6-1分闸完成", false);
                                            break;
                                        }
                                       

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-1分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q6-1合闸"
                        case 107.5f:
                            {
                                //plc39.Write("M107.5", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-2 BQ6-2 CQ6-2
                                        ReadAllPlcData(false);
                                        if (alldian[33] == true && alldian[73] == true && alldian[114] == true)
                                        {
                                            AddRecord("Q6-2合闸完成", false);
                                            break;
                                        }
                                      

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-2合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.5", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.5", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[33] == false && alldian[73] == false && alldian[114] == false)
                                        {
                                            AddRecord("Q6-2分闸完成", false);
                                            break;
                                        }
                                  

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-2分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q6-2合闸"
                        case 107.6f:
                            {
                                //plc39.Write("M107.6", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M107.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-3 BQ6-3 CQ6-3
                                        ReadAllPlcData(false);
                                        if (alldian[34] == true && alldian[74] == true && alldian[115] == true)
                                        {
                                            AddRecord("Q6-3合闸完成", false);
                                            break;
                                        }
                                       
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-3合闸信号-未返回.请检查设备", true);
                                    }
                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.6", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.6", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[34] == false && alldian[74] == false && alldian[115] == false)
                                        {
                                            AddRecord("Q6-3分闸完成", false);
                                            break;
                                        }
                                        

                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-3分闸信号-未返回.请检查设备", true);
                                    }

                                }

                                break;
                            }//"Q6-3合闸"
                        case 107.7f:
                            {
                                //plc39.Write("M107.7", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M107.7", true);
                                    Thread.Sleep(5000);
                                    plc39.Write("M107.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-4 BQ6-4 CQ6-4
                                        ReadAllPlcData(false);
                                        if (alldian[35] == true && alldian[75] == true && alldian[116] == true)
                                        {
                                            AddRecord("Q6-4合闸完成", false);
                                            break;
                                        }
                                       
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了15秒,Q6-4合闸信号-未返回.请检查设备", true);
                                    }

                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M207.7", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M207.7", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ1-1 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[35] == false && alldian[75] == false && alldian[116] == false)
                                        {
                                            AddRecord("Q6-4分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-4分闸信号-未返回.请检查设备", true);
                                    }


                                }

                                break;
                            }//"Q6-4合闸"
                        case 108.0f: 
                            {
                                //plc39.Write("M108.0", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M108.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M108.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-5 BQ6-5 CQ6-5
                                        ReadAllPlcData(false);
                                        if (alldian[36] == true && alldian[76] == true && alldian[117] == true)
                                        {
                                            AddRecord("Q6-5合闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-5合闸信号-未返回.请检查设备", true);
                                    }

                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M208.0", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M208.0", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-5 BQ1-1 CQ1-1
                                        ReadAllPlcData(false);
                                        if (alldian[36] == false && alldian[76] == false && alldian[117] == false)
                                        {
                                            AddRecord("Q6-5分闸完成", false);
                                            break;
                                        }
                                       
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-5分闸信号-未返回.请检查设备", true);
                                    }


                                }

                                break;
                            }//"Q6-5合闸"
                        case 108.1f:
                            {
                                //plc39.Write("M108.1", data);
                                if (data == true)
                                {
                                    //合闸..
                                    plc39.Write("M108.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M108.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-6 BQ6-6 CQ6-6
                                        ReadAllPlcData(false);
                                        if (alldian[37] == true && alldian[77] == true && alldian[118] == true)
                                        {
                                            AddRecord("Q6-6合闸完成", false);
                                            break;
                                        }
                                       
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-6合闸信号-未返回.请检查设备", true);
                                    }

                                }
                                else
                                {
                                    //分闸..
                                    plc39.Write("M208.1", true);
                                    Thread.Sleep(2000);
                                    plc39.Write("M208.1", false);
                                    for (knifeSleep = 0; knifeSleep < 10; knifeSleep++)
                                    {
                                        Thread.Sleep(1000);
                                        //这里要去检查3个信号，AQ6-6 BQ6-6 CQ6-6
                                        ReadAllPlcData(false);
                                        if (alldian[37] == false && alldian[77] == false && alldian[118] == false)
                                        {
                                            AddRecord("Q6-6分闸完成", false);
                                            break;
                                        }
                                        
                                    }
                                    if (knifeSleep == 10)
                                    {
                                        AddRecord("严重错误:过了8秒,Q6-6分闸信号-未返回.请检查设备", true);
                                    }
                                }

                                break;
                            }//"Q6-6合闸"
                        case 200f: ReadAllPlcData(); break;
                        case 33f: plc39.Open(); break;
                        case 34f: plc41.Open(); break;

                    }
                    Success = true;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    AddRecord("PLC异常:"+error, true);
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            //主线程和管理线程这里，都应该是返回了，主线程干啥去了？
            //管理线程，返回难道又执行代码去了吗？
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                AddRecord("PLC超时,请检查PLC是否正常!", true);
                if (isBackCall == 0) WeakReferenceMessenger.Default.Send<string, string>("En", "EnableAll");
                IsWork = false;
                return false;
            }

            if (Success)
            {
                if (isBackCall == 0) WeakReferenceMessenger.Default.Send<string, string>("En", "EnableAll");
                IsWork = false;
                return true;
            }
            else
            {
                AddRecord("PLC异常:" + error, true);
                if (isBackCall == 0) WeakReferenceMessenger.Default.Send<string, string>("En", "EnableAll");
                IsWork = false;
                return false;
            }

        }

        private async void DoBtnCommandSendHandJSOK(object sender)
        {
            //这里如果是绑定了，直接就红了...(点击确定后，使用的是过去的数值..)
            _ = UserData3;
            return;
        }

        private async void DoBtnCommandSendHandJSCZ(object sender)
        {
            return;
        }


        private async void DoBtnCommandSendHand1(object button)
        {
            //YanPingWindow dialog = new YanPingWindow(); return dialog.ShowDialog() == true; 
            /*
            processWindow = new ProcessWindow();
            processWindow.Topmost = true;
            processWindow.Show();
            /*
            //到这里已经被赋值...(开始是false,到这里true)
            bool Orgdata = !Data1;
            var myresult = await ExePlcCommand(1, Data1);
            if (!myresult)
                Data1 = Orgdata;
            return;
            */
            WeakReferenceMessenger.Default.Send<string, string>("1", "SWaitchXZ");
        }

        private async void DoBtnCommandSendHand2(object button)
        {
            //到这里已经被赋值...(开始是false,到这里true)
            /*
             * IsActive1 = true;
            bool Orgdata = !Data2;
            var myresult = await ExePlcCommand(2, Data2);
            if (!myresult)
                Data2 = Orgdata;
            */
            WeakReferenceMessenger.Default.Send<string, string>("2", "SWaitchXZ");
            return;
        }

        private async void DoBtnCommandSendHand3(object button)
        {
            //IsActive1 = false;
            WeakReferenceMessenger.Default.Send<string, string>("3", "SWaitchXZ");
            return;
        }

        private async void DoBtnCommandSendHand4(object button)
        {
            //到这里已经被赋值...(开始是false,到这里true)
            //WeakReferenceMessenger.Default.Send<string, string>("4", "SWaitchXZ");
            return;
        }


    }

    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }

    public class BKCommand
    {
        public float Command { get; set; }
        public bool Data { get; set; }
        public string CommandDescribe { get; set; }
    }
}
