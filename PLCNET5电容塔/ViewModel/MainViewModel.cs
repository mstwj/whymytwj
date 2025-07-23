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



namespace PLCNET5电容塔.ViewModel
{
    //ObservableObject -- ObservableValidator(不能使用和S7冲突了..)
    public class MainViewModel : ObservableObject
    {
        //接法.
        private string userjf = string.Empty;
        public string UserJF { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //并连数.
        private float userbls = 0.0f;
        public float UserBLS { get { return userbls; } set { SetProperty(ref userbls, value); } }


        //预故电压
        private float estimatedfvoltage = 0.0f;
        public float EstimatedVoltage { get { return estimatedfvoltage; } set { SetProperty(ref estimatedfvoltage, value); } }


        //预故电流
        private float estimatedcurrent = 0.0f;
        public float EstimatedCurrent { get { return estimatedcurrent; } set { SetProperty(ref estimatedcurrent, value); } }

        //建议投个数
        private string usermcjygs = string.Empty;
        public string UserMCJYGS { get { return usermcjygs; } set { SetProperty(ref usermcjygs, value); } }

        public MainModel mainModel{ get; set;} = new MainModel();
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();

        //主机
        private Plc plc39 = new Plc(CpuType.S71200, "192.168.2.39", 0, 1);
        //从机
        private Plc plc41 = new Plc(CpuType.S71200, "192.168.2.41", 0, 1);

        //为什么不能在这里NEW呢？
        ProcessWindow? processWindow = null;

        //如果是这样去设置.. 只能页面给我数据.. 我数据无法刷新界面..(如果不使用SetProperty)
        //使用SetProperty -- 就可以刷新数据了.. 只要失去焦点，数据就自动刷新了..
        //[Required(ErrorMessage = "不能为空!")] -- 这里无法设置和S7 冲突了...
        
        private float _userdata1, _userdata2, _userdata3; 
        public float UserData1 { get { return _userdata1; } set { SetProperty(ref _userdata1, value); } }
        public float UserData2 { get { return _userdata2; } set { SetProperty(ref _userdata2, value); } }
        public float UserData3 { get { return _userdata3; } set { SetProperty(ref _userdata3, value); } }

        public ICommand BtnCommandSendHand1 { get; set; }
        public ICommand BtnCommandSendHand2 { get; set; }
        public ICommand BtnCommandSendHand3 { get; set; }
        public ICommand BtnCommandSendHand4 { get; set; }
        public ICommand BtnCommandSendHand5 { get; set; }
        public ICommand BtnCommandSendHand6 { get; set; }

        public ICommand BtnCommandSendHand7 { get; set; }

        public ICommand BtnCommandSendHandJSOK { get; set; }
        public ICommand BtnCommandSendHandJSCZ { get; set; }


        private bool IsWork;

        public List<string> ComboBoxItems { get; set; } = new List<string>();

        public MainViewModel()
        {

            //注册..
            WeakReferenceMessenger.Default.Register<string, string>(this, "SendPLCCommand", SendPLCCommand);




            //6个按钮
            BtnCommandSendHand1 = new RelayCommand<object>(DoBtnCommandSendHand1);
            BtnCommandSendHand2 = new RelayCommand<object>(DoBtnCommandSendHand2);
            BtnCommandSendHand3 = new RelayCommand<object>(DoBtnCommandSendHand3);
            BtnCommandSendHand4 = new RelayCommand<object>(DoBtnCommandSendHand4);
            BtnCommandSendHand5 = new RelayCommand<object>(DoBtnCommandSendHand5);
            BtnCommandSendHand6 = new RelayCommand<object>(DoBtnCommandSendHand6);
            BtnCommandSendHand7 = new RelayCommand<object>(DoBtnCommandSendHand7);

            BtnCommandSendHandJSOK = new RelayCommand<object>(DoBtnCommandSendHandJSOK);
            BtnCommandSendHandJSCZ = new RelayCommand<object>(DoBtnCommandSendHandJSCZ);

            //初始化后台线程...
            InitializeAsync();

        }

        private Task rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<BKCommand> queue = new Queue<BKCommand>();

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
                        if (queue.Count <= 0) continue;
                        BKCommand Command = queue.Dequeue();
                        var myresult2 = await ExePlcCommand(Command.Command, Command.Data);
                        if (myresult2) AddRecord("指令执行成功!", false);
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

        async void DoPlcCommand(float command,bool data)
        {
            //这里等待了... (主线程还是等了..)
            if (queue.Count >= 1)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成", true);
                return;
            }
            queue.Enqueue(new BKCommand { Command = command ,Data = data});
        }

        private void SendPLCCommand(object recipient, string message)
        {
            
            if (message == "myButtonGA_He") DoPlcCommand(100.0f, true); 
            if (message == "myButtonGB_He") DoPlcCommand(100.2f, true);
            if (message == "myButtonGC_He") DoPlcCommand(100.0f, true);

            if (message == "myButtonG1_He") DoPlcCommand(100.4f, true);

            if (message == "myButtonG2_He") DoPlcCommand(100.6f, true);
            if (message == "myButtonG3_He") DoPlcCommand(101.0f, true);
            if (message == "myButtonG4_He") DoPlcCommand(101.2f, true);
            if (message == "myButtonG5_He") DoPlcCommand(101.4f, true);

            if (message == "myButton1G1_He") DoPlcCommand(101.6f, true);
            if (message == "myButton1G2_He") DoPlcCommand(102.0f, true);
            if (message == "myButton1G3_He") DoPlcCommand(102.2f, true);
            if (message == "myButton1G4_He") DoPlcCommand(102.4f, true);
            if (message == "myButton1G5_He") DoPlcCommand(102.6f, true);
            if (message == "myButton1G6_He") DoPlcCommand(103.0f, true);

            if (message == "myButton1K1_He") DoPlcCommand(103.2f, true);
            if (message == "myButton1K2_He") DoPlcCommand(103.4f, true);


            if (message == "myButtonQ1_1_He") DoPlcCommand(103.6f, true);
            if (message == "myButtonQ1_2_He") DoPlcCommand(103.7f, true);

            if (message == "myButtonQ1_3_He") DoPlcCommand(104.0f, true);

            if (message == "myButtonQ1_4_He") DoPlcCommand(104.1f, true);
            if (message == "myButtonQ1_5_He") DoPlcCommand(104.2f, true);
            if (message == "myButtonQ1_6_He") DoPlcCommand(104.3f, true);

            if (message == "myButtonQ2_1_He") DoPlcCommand(104.4f, true);
            if (message == "myButtonQ2_2_He") DoPlcCommand(104.5f, true);
            if (message == "myButtonQ2_3_He") DoPlcCommand(104.6f, true);
            if (message == "myButtonQ2_4_He") DoPlcCommand(104.7f, true);

            if (message == "myButtonQ2_5_He") DoPlcCommand(105.0f, true);
            if (message == "myButtonQ2_6_He") DoPlcCommand(105.1f, true);
            if (message == "myButtonQ3_1_He") DoPlcCommand(105.2f, true);
            if (message == "myButtonQ3_2_He") DoPlcCommand(105.3f, true);
            if (message == "myButtonQ3_3_He") DoPlcCommand(105.4f, true);
            if (message == "myButtonQ3_4_He") DoPlcCommand(105.5f, true);
            if (message == "myButtonQ3_5_He") DoPlcCommand(105.6f, true);
            if (message == "myButtonQ3_6_He") DoPlcCommand(105.7f, true);

            if (message == "myButtonQ4_1_He") DoPlcCommand(106.0f, true);
            if (message == "myButtonQ4_2_He") DoPlcCommand(106.1f, true);
            if (message == "myButtonQ4_3_He") DoPlcCommand(106.2f, true);
            if (message == "myButtonQ4_4_He") DoPlcCommand(106.3f, true);
            if (message == "myButtonQ4_5_He") DoPlcCommand(106.4f, true);
            if (message == "myButtonQ4_6_He") DoPlcCommand(106.5f, true);
            if (message == "myButtonQ5_1_He") DoPlcCommand(106.6f, true);
            if (message == "myButtonQ5_2_He") DoPlcCommand(106.7f, true);

            if (message == "myButtonQ5_3_He") DoPlcCommand(107.0f, true);
            if (message == "myButtonQ5_4_He") DoPlcCommand(107.1f, true);
            if (message == "myButtonQ5_5_He") DoPlcCommand(107.2f, true);
            if (message == "myButtonQ5_6_He") DoPlcCommand(107.3f, true);

            if (message == "myButtonQ6_1_He") DoPlcCommand(107.4f, true);
            if (message == "myButtonQ6_2_He") DoPlcCommand(107.5f, true);
            if (message == "myButtonQ6_3_He") DoPlcCommand(107.6f, true);
            if (message == "myButtonQ6_4_He") DoPlcCommand(107.7f, true);
            if (message == "myButtonQ6_5_He") DoPlcCommand(108.0f, true);
            if (message == "myButtonQ6_6_He") DoPlcCommand(108.1f, true);


            if (message == "myButtonQ1_1_Fen") DoPlcCommand(103.6f, false);
            if (message == "myButtonQ1_2_Fen") DoPlcCommand(103.7f, false);
            if (message == "myButtonQ1_3_Fen") DoPlcCommand(104.0f, false);
            if (message == "myButtonQ1_4_Fen") DoPlcCommand(104.1f, false);
            if (message == "myButtonQ1_5_Fen") DoPlcCommand(104.2f, false);
            if (message == "myButtonQ1_6_Fen") DoPlcCommand(104.3f, false);
            if (message == "myButtonQ2_1_Fen") DoPlcCommand(104.4f, false);
            if (message == "myButtonQ2_2_Fen") DoPlcCommand(104.5f, false);
            if (message == "myButtonQ2_3_Fen") DoPlcCommand(104.6f, false);
            if (message == "myButtonQ2_4_Fen") DoPlcCommand(104.7f, false);
            if (message == "myButtonQ2_5_Fen") DoPlcCommand(105.0f, false);
            if (message == "myButtonQ2_6_Fen") DoPlcCommand(105.1f, false);
            if (message == "myButtonQ3_1_Fen") DoPlcCommand(105.2f, false);
            if (message == "myButtonQ3_2_Fen") DoPlcCommand(105.3f, false);
            if (message == "myButtonQ3_3_Fen") DoPlcCommand(105.4f, false);
            if (message == "myButtonQ3_4_Fen") DoPlcCommand(105.5f, false);
            if (message == "myButtonQ3_5_Fen") DoPlcCommand(105.6f, false);
            if (message == "myButtonQ3_6_Fen") DoPlcCommand(105.7f, false);

            if (message == "myButtonQ4_1_Fen") DoPlcCommand(106.0f, false);
            if (message == "myButtonQ4_2_Fen") DoPlcCommand(106.1f, false);
            if (message == "myButtonQ4_3_Fen") DoPlcCommand(106.2f, false);
            if (message == "myButtonQ4_4_Fen") DoPlcCommand(106.3f, false);
            if (message == "myButtonQ4_5_Fen") DoPlcCommand(106.4f, false);
            if (message == "myButtonQ4_6_Fen") DoPlcCommand(106.5f, false);
            if (message == "myButtonQ5_1_Fen") DoPlcCommand(106.6f, false);
            if (message == "myButtonQ5_2_Fen") DoPlcCommand(106.7f, false);

            if (message == "myButtonQ5_3_Fen") DoPlcCommand(107.0f, false);
            if (message == "myButtonQ5_4_Fen") DoPlcCommand(107.1f, false);
            if (message == "myButtonQ5_5_Fen") DoPlcCommand(107.2f, false);
            if (message == "myButtonQ5_6_Fen") DoPlcCommand(107.3f, false);
            if (message == "myButtonQ6_1_Fen") DoPlcCommand(107.4f, false);
            if (message == "myButtonQ6_2_Fen") DoPlcCommand(107.5f, false);
            if (message == "myButtonQ6_3_Fen") DoPlcCommand(107.6f, false);
            if (message == "myButtonQ6_4_Fen") DoPlcCommand(107.7f, false);

            if (message == "myButtonQ6_5_Fen") DoPlcCommand(108.0f, false);
            if (message == "myButtonQ6_6_Fen") DoPlcCommand(108.1f, false);


            if (message == "myButtonGA_Fen") DoPlcCommand(100.1f, true);
            if (message == "myButtonGB_Fen") DoPlcCommand(100.3f, true);
            if (message == "myButtonGC_Fen") DoPlcCommand(100.1f, true);

            if (message == "myButtonG1_Fen") DoPlcCommand(100.5f, true);
            if (message == "myButtonG2_Fen") DoPlcCommand(100.7f, true);
            if (message == "myButtonG3_Fen") DoPlcCommand(101.1f, true);
            if (message == "myButtonG4_Fen") DoPlcCommand(101.3f, true);
            if (message == "myButtonG5_Fen") DoPlcCommand(101.5f, true);

            if (message == "myButton1G1_Fen") DoPlcCommand(101.7f, true);
            if (message == "myButton1G2_Fen") DoPlcCommand(102.1f, true);
            if (message == "myButton1G3_Fen") DoPlcCommand(102.3f, true);
            if (message == "myButton1G4_Fen") DoPlcCommand(102.5f, true);
            if (message == "myButton1G5_Fen") DoPlcCommand(102.7f, true);
            if (message == "myButton1G6_Fen") DoPlcCommand(103.1f, true);

            if (message == "myButton1K1_Fen") DoPlcCommand(103.3f, true);
            if (message == "myButton1K2_Fen") DoPlcCommand(103.5f, true);
        }

        public void MainClose()
        {
            if (rotobTaskT !=null)
            {
                cts.Cancel();                
            }
            if (processWindow != null)
            {
                processWindow.Close();
            }
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

        void AddRecord(string strdata, bool e)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str =  strdata + "<=" + time ;
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });                
            });
            WeakReferenceMessenger.Default.Send<string, string>("ScrollEnd", "ScrollEnd");
        }

        bool[]? alldian = null;

        public void SetAllDian(bool[] datad)
        {
            alldian = datad;
            //mainModel.SetAllDian(alldian);
        }

        //更新大框左边..
        void ReadAllPlcData()
        {
            try
            {                
                int k = 0;


                byte[]? result0 = plc39.ReadBytes(DataType.Input, 0, 0, 30);

                byte result1 = result0[21];
                for (int i = 0; i < 8; i++) 
                {
                    bool value = Bit.FromByte(result1, (byte)i); 
                    alldian[k++] = value; 
                }

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
                byte[] result8 = plc41.ReadBytes(DataType.Input, 0, 24, 2);
                for (int j = 0; j < 2; j++) for (int i = 0; i < 8; i++) { bool value = Bit.FromByte(result8[j], (byte)i); alldian[k++] = value; }

                k = 120;

                //读PLC39 0--1.7(16)
                byte[] result9 = plc39.ReadBytes(DataType.Input, 0, 0, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result9[j], (byte)i);
                        alldian[k++] = value;
                    }    

                //读PLC39 8.0--9.7(16)
                byte[] result10 = plc39.ReadBytes(DataType.Input, 0, 8, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result10[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 12.0--13.7(16)
                byte[] result11 = plc39.ReadBytes(DataType.Input, 0, 12, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result11[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 16.0--17.7(16)
                byte[] result12 = plc39.ReadBytes(DataType.Input, 0, 16, 2);
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        bool value = Bit.FromByte(result12[j], (byte)i);
                        alldian[k++] = value;
                    }

                //读PLC39 20.0(8)
                byte result13 = result0[20];
                for (int i = 0; i < 8; i++)
                {
                    bool value = Bit.FromByte(result13, (byte)i);
                    alldian[k++] = value;
                }

                //更新界面(主界面 - 子界面)，下面的红点...
                WeakReferenceMessenger.Default.Send<string, string>("ShowAllPlcDian", "SWaitchXZ");

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
                mainModel.IsPlc39 = false;//设置红灯
                return false;
            }
            if (!plc41.IsConnected)
            {
                AddRecord("PLC-33从机断线,请检查PLC链接是否正常...", true);
                mainModel.IsPlc41 = false; //设置红灯
                return false;
            }
            mainModel.IsPlc39 = true; //设置绿灯
            mainModel.IsPlc41 = true;//设置绿灯
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
            var timeout = Task.Delay(8000);
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
                    
                    switch (command)
                    {                        
                        case 100.0f: { plc39.Write("M100.0", true); Thread.Sleep(300); plc39.Write("M100.0", false); break; }//"总输出AC和闸"
                        case 100.1f: { plc39.Write("M100.1", true); Thread.Sleep(300); plc39.Write("M100.1", false); break; }//"总输出AC分闸"
                        
                        case 100.2f: { plc39.Write("M100.2", true); Thread.Sleep(300); plc39.Write("M100.2", false); break; }//"总输出B和闸"
                        case 100.3f: { plc39.Write("M100.3", true); Thread.Sleep(300); plc39.Write("M100.3", false); break; }//"总输出B分闸"

                        case 100.4f: { plc39.Write("M100.4", true); Thread.Sleep(300); plc39.Write("M100.4", false); break; }//"G1合闸"
                        case 100.5f: { plc39.Write("M100.5", true); Thread.Sleep(300); plc39.Write("M100.5", false); break; }//"G1分闸"

                        case 100.6f: { plc39.Write("M100.6", true); Thread.Sleep(300); plc39.Write("M100.6", false); break; }//"G2合闸"
                        case 100.7f: { plc39.Write("M100.7", true); Thread.Sleep(300); plc39.Write("M100.7", false); break; }//"G2分闸"
                            
                        case 101.0f: { plc39.Write("M101.0", true); Thread.Sleep(300); plc39.Write("M101.0", false); break; }//"G3合闸"
                        case 101.1f: { plc39.Write("M101.1", true); Thread.Sleep(300); plc39.Write("M101.1", false); break; }//"G3分闸"

                        case 101.2f: { plc39.Write("M101.2", true); Thread.Sleep(300); plc39.Write("M101.2", false); break; }//"G4合闸"
                        case 101.3f: { plc39.Write("M101.3", true); Thread.Sleep(300); plc39.Write("M101.3", false); break; }//"G4分闸"

                        case 101.4f: { plc39.Write("M101.4", true); Thread.Sleep(300); plc39.Write("M101.4", false); break; }//"G5合闸"
                        case 101.5f: { plc39.Write("M101.5", true); Thread.Sleep(300); plc39.Write("M101.5", false); break; }//"G5分闸"

                        case 101.6f: { plc39.Write("M101.6", true); Thread.Sleep(300); plc39.Write("M101.6", false); break; }//"1G1合闸"
                        case 101.7f: { plc39.Write("M101.7", true); Thread.Sleep(300); plc39.Write("M101.7", false); break; }//"1G1分闸"

                        case 102.0f: { plc39.Write("M102.0", true); Thread.Sleep(300); plc39.Write("M102.0", false); break; }//"1G2合闸"
                        case 102.1f: { plc39.Write("M102.1", true); Thread.Sleep(300); plc39.Write("M102.1", false); break; }//"1G2分闸"

                        case 102.2f: { plc39.Write("M102.2", true); Thread.Sleep(300); plc39.Write("M102.2", false); break; }//"1G3合闸"
                        case 102.3f: { plc39.Write("M102.3", true); Thread.Sleep(300); plc39.Write("M102.3", false); break; }//"1G3分闸"

                        case 102.4f: { plc39.Write("M102.4", true); Thread.Sleep(300); plc39.Write("M102.4", false); break; }//"1G4合闸"
                        case 102.5f: { plc39.Write("M102.5", true); Thread.Sleep(300); plc39.Write("M102.5", false); break; }//"1G4分闸"

                        case 102.6f: { plc39.Write("M102.6", true); Thread.Sleep(300); plc39.Write("M102.6", false); break; }//"1G5合闸"
                        case 102.7f: { plc39.Write("M102.7", true); Thread.Sleep(300); plc39.Write("M102.7", false); break; }//"1G5分闸"

                        case 103.0f: { plc39.Write("M103.0", true); Thread.Sleep(300); plc39.Write("M103.0", false); break; }//"1G6合闸"
                        case 103.1f: { plc39.Write("M103.1", true); Thread.Sleep(300); plc39.Write("M103.1", false); break; }//"1G6分闸"

                        case 103.2f: { plc39.Write("M103.2", true); Thread.Sleep(300); plc39.Write("M103.2", false); break; }//"K1合闸"
                        case 103.3f: { plc39.Write("M103.3", true); Thread.Sleep(300); plc39.Write("M103.3", false); break; }//"K1分闸"

                        case 103.4f: { plc39.Write("M103.4", true); Thread.Sleep(300); plc39.Write("M103.4", false); break; }//"K2合闸"
                        case 103.5f: { plc39.Write("M103.5", true); Thread.Sleep(300); plc39.Write("M103.5", false); break; }//"K2分闸"

                        case 103.6f: { plc39.Write("M103.6", data ); break; }//"Q1-1合闸"
                        case 103.7f: { plc39.Write("M103.7", data); break; }//"Q1-2合闸"

                        case 104.0f: { plc39.Write("M104.0", data); break; }//"Q1-3合闸"

                        case 104.1f: { plc39.Write("M104.1", data); break; }//"Q1-4合闸"                                     
                        case 104.2f: { plc39.Write("M104.2", data); break; }//"Q1-5合闸"
                        case 104.3f: { plc39.Write("M104.3", data); break; }//"Q1-6合闸"

                        case 104.4f: { plc39.Write("M104.4", data); break; }//"Q2-1合闸"
                        case 104.5f: { plc39.Write("M104.5", data); break; }//"Q2-2合闸"
                        case 104.6f: { plc39.Write("M104.6", data); break; }//"Q2-3合闸"
                        case 104.7f: { plc39.Write("M104.7", data); break; }//"Q2-4合闸"
                        case 105.0f: { plc39.Write("M105.0", data); break; }//"Q2-5合闸"
                        case 105.1f: { plc39.Write("M105.1", data); break; }//"Q2-6合闸"

                        case 105.2f: { plc39.Write("M105.2", data); break; }//"Q3-1合闸"
                        case 105.3f: { plc39.Write("M105.3", data); break; }//"Q3-2合闸"
                        case 105.4f: { plc39.Write("M105.4", data); break; }//"Q3-3合闸"
                        case 105.5f: { plc39.Write("M105.5", data); break; }//"Q3-4合闸"
                        case 105.6f: { plc39.Write("M105.6", data); break; }//"Q3-5合闸"
                        case 105.7f: { plc39.Write("M105.7", data); break; }//"Q3-6合闸"

                        case 106.0f: { plc39.Write("M106.0", data); break; }//"Q4-1合闸"
                        case 106.1f: { plc39.Write("M106.1", data); break; }//"Q4-2合闸"
                        case 106.2f: { plc39.Write("M106.2", data); break; }//"Q4-3合闸"
                        case 106.3f: { plc39.Write("M106.3", data); break; }//"Q4-4合闸"
                        case 106.4f: { plc39.Write("M106.4", data); break; }//"Q4-5合闸"
                        case 106.5f: { plc39.Write("M106.5", data); break; }//"Q4-6合闸"

                        case 106.6f: { plc39.Write("M106.6", data); break; }//"Q5-1合闸"
                        case 106.7f: { plc39.Write("M106.7", data); break; }//"Q5-2合闸"
                        case 107.0f: { plc39.Write("M107.0", data); break; }//"Q5-3合闸"
                        case 107.1f: { plc39.Write("M107.1", data); break; }//"Q5-4合闸"
                        case 107.2f: { plc39.Write("M107.2", data); break; }//"Q5-5合闸"
                        case 107.3f: { plc39.Write("M107.3", data); break; }//"Q5-6合闸"

                        case 107.4f: { plc39.Write("M107.4", data); break; }//"Q6-1合闸"
                        case 107.5f: { plc39.Write("M107.5", data); break; }//"Q6-2合闸"
                        case 107.6f: { plc39.Write("M107.6", data); break; }//"Q6-3合闸"
                        case 107.7f: { plc39.Write("M107.7", data); break; }//"Q6-4合闸"
                        case 108.0f: { plc39.Write("M108.0", data); break; }//"Q6-5合闸"
                        case 108.1f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"

                        case 1f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 2f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 3f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 4f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 5f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 6f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 7f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 8f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 9f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 10f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 11f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 12f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 13f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 14f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"
                        case 15f: { plc39.Write("M108.1", data); break; }//"Q6-6合闸"

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

        private async void DoBtnCommandSendHand5(object button)
        {
            WeakReferenceMessenger.Default.Send<string, string>("1", "SWaitchXZ");
            return;
        }

        private async void DoBtnCommandSendHand6(object button)
        {
            WeakReferenceMessenger.Default.Send<string, string>("2", "SWaitchXZ");
            return;
        }

        private async void DoBtnCommandSendHand7(object button)
        {
            WeakReferenceMessenger.Default.Send<string, string>("3", "SWaitchXZ");
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
    }
}
