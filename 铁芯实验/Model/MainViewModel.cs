using System;
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
using System.Windows;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using 铁芯实验.Base;
using System.Windows.Controls;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Modbus.Extensions.Enron;

using System.Windows.Markup;
using System.IO;
using CommunityToolkit.Mvvm.Messaging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using 铁芯实验.View;
using Microsoft.VisualBasic.ApplicationServices;


namespace 铁芯实验.Model
{
    public class MainViewModel : ObservableValidator
    {
        private SerialPort serialPort = new SerialPort();
        private ModbusSerialMaster master = null; //串口-通信（功率测试仪..）

        private TcpClient tcpClient = new TcpClient();
        //private NetworkStream stream; // 网络流，用于与服务器通信
        private ModbusIpMaster masterIp = null;

        private CancellationTokenSource cts = new CancellationTokenSource();
        public Queue<CommandItem> queue { get; set; } = new Queue<CommandItem>();
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();
        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public UserMainModel m_userMainModel { get; set; }
        public UserRecordModel m_userRecordModel{ get; set; }
        public UserInfoModel m_userInfoModel{ get; set; }
        public UserStandardModel m_userStandardModel { get; set; }
        //用户电源状态模块..
        public PowerStateModel m_powerStateModel { get; set; }
        //用户电源设置..
        public UserPowerSetModel m_userPowerSetModel { get; set; }
        public UserTestModel m_userTestModel { get; set; }
        public UserCommunicationModel m_userCommunicationModel { get; set; }

        public ICommand BtnCommandStart { get; set; }
        public ICommand BtnCommandStop { get; set; }
        public ICommand BtnCommandSave { get; set; }


        private bool StartScanSing = false; //开始扫描信号..

        public MainViewModel(UserMainModel userMainModel, 
            UserRecordModel userRecordModel,
            UserInfoModel userInfoModel,
            UserStandardModel userStandardModel,
            PowerStateModel powerStateModel,            
            UserPowerSetModel userPowerSetModel,
            UserTestModel userTestModel,
            UserCommunicationModel userCommunicationModel            
            ) 
        {
            m_userMainModel = userMainModel;
            m_userRecordModel = userRecordModel;
            m_userInfoModel = userInfoModel;
            m_userStandardModel = userStandardModel;
            m_powerStateModel = powerStateModel;
            m_userPowerSetModel = userPowerSetModel;
            m_userTestModel = userTestModel;
            m_userCommunicationModel  = userCommunicationModel;


            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, user) =>
            {
                if (user.Message == "GetQueue") user.obj = queue;
                if (user.Message == "GetMainViewModel") user.obj = this;
                if (user.Message == "GetUserMainModel") user.obj = m_userMainModel;
                if (user.Message == "GetUserRecordModel") user.obj = m_userRecordModel;
                if (user.Message == "GetUserInfoModel") user.obj = m_userInfoModel;
                if (user.Message == "GetUserStandardModel") user.obj = m_userStandardModel;
                if (user.Message == "GetPowerStateModel") user.obj = m_powerStateModel;
                if (user.Message == "GetUserPowerSetModel") user.obj = m_userPowerSetModel;
                if (user.Message == "GetUserTestModel") user.obj = m_userTestModel;
                if (user.Message == "GetUserCommunicationModel") user.obj = m_userCommunicationModel;
            });

            MyTools.SetMainViewModel(this);


            BtnCommandStart = new RelayCommand<object>(DoBtnCommandStart);
            BtnCommandStop = new RelayCommand<object>(DoBtnCommandStop);
            BtnCommandSave = new RelayCommand<object>(DoBtnCommandSave);           

            AddRecord("开始初始化",false);
            //启动的时候，就打开串口，如果失败了. 就要重新启动软件...
            InitializationLink();            
        }

        private void DoBtnCommandStart(object param)
        {

            if (m_userTestModel.Shijiazhashu_data == 0)
            {
                System.Windows.MessageBox.Show("无法启动,施加电压为0");
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
            if (MyTools.SaveDataToDataBase(m_userRecordModel,m_userInfoModel))
            {
                AddRecord("添加数据成功!",false);
            }else
            {
                AddRecord("添加数据失败!",true);
            }
        }

        
        //设置用户信息..
        public bool SetCurrentUserInfo()
        {
            if (MainWindow.Current_ProductNumber == null)
            {
                return false;
            }
            if (MyTools.GetCurrentUserInfoAndStandard(m_userInfoModel, m_userStandardModel))
            {
                m_userTestModel.ShowOneOrThree = m_userInfoModel.PhaseNumber;
                //施加电压设置为0;
                m_userTestModel.Shijiazhashu_data = 0;
                return true;
            }

            return false;
        }

        public async void InitializationLink()
        {                  
            
            //先打开端口...
            try
            {             
                AddRecord("设置串口", false);
                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议
                master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                master.Transport.ReadTimeout = 1000;// 设置超时时间默认为1000毫秒
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_GLFXY"];
                serialPort.Open(); //打开串口                                
                AddRecord("设置串口完成", false);

                AddRecord("设置网口", false);
                //这里变频电源，自动吧我给踢了... 我。。。。不是我的问题，是他服务端的问题..
                tcpClient = new TcpClient();                
                //tcpClient.Connect(IPAddress.Parse(ConfigurationManager.AppSettings["DEVICE_BPDY"]), 502);
                await tcpClient.ConnectAsync(IPAddress.Parse(ConfigurationManager.AppSettings["DEVICE_BPDY"]), 502);

                masterIp = ModbusIpMaster.CreateIp(tcpClient);
                //masterIp.Transport.WriteTimeout = 2000;
                //masterIp.Transport.ReadTimeout = 2000;
                //masterIp.Transport.WaitToRetryMilliseconds = 250; //// 设置重试间隔时间为250毫秒
                //masterIp.Transport.Retries = 3; //// 设置重试次数为3次
                AddRecord("设置网口完成", false);
                //这里的TCP有问题，算了.. 不研究了.. 反正有问题..
                
                // 异步连接到指定的服务器IP和端口
                //await tcpClient.ConnectAsync(sIp, 502);

                // 获取与服务器通信的网络流
                //stream = tcpClient.GetStream();

                // 启动一个异步任务接收来自服务器的消息
                //ReceiveMessages();
            }
            catch (Exception ex)
            {
                AddRecord("串口或网口异常"+ex.Message, true);
                MessageBox.Show("初始化失败!异常:"+ex.Message);
                return ;
            }

            //可以等一下管家线程..
            Task ReTs = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;
                AddRecord("管理线程启动...", false);

                //开始先去链接电源..
                var initresult = await ExePlcCommand(10, 0);
                if (!initresult)
                {
                    AddRecord("管理线程init指令执行失败!", true);
                    MessageBox.Show("管理线程-开始连接电源失败，请检查设备!");
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
                            if (StartScanSing == true)
                            {
                                //var myresult1 = await ExePlcCommand(999999, 0);                                 
                                var myresult1 = await ExePlcCommand(1, 0); 
                                if (!myresult1)
                                {
                                    AddRecord("管理线程退出-定时器指令执行失败", true);
                                    return;
                                }
                            }
                            //这里就是一个心跳包，有可能执行失败...
                            var myresult = await ExePlcCommand(13, 0);
                            if (!myresult)
                                AddRecord("心跳包执行失败", true);
                                
                            
                            precurrentTime = currentTime;
                            continue;
                        }
                        if (queue.Count <= 0) continue;
                        CommandItem Command = queue.Dequeue();
                        if (Command.command == 1000) 
                        {
                            AddRecord("管理等待..",false);
                            Thread.Sleep(Command.regaddressdata);                             
                            AddRecord("管理等待完成..", false);
                            continue;
                        }
                        AddRecord("管理员-指令开始执行!:"+Command.command.ToString(), false);
                        var myresult2 = await ExePlcCommand(Command.command,Command.regaddress, Command.regaddressdata);
                        if (myresult2)
                        {
                            AddRecord("管理员-指令执行成功!", false);
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
            }, cts.Token);

            //这里我等一下..
            // 等待任务完成(不要这样主线程不动了..)await task;
            //Thread.Sleep(100);
            //WeakReferenceMessenger.Default.Send<string, string>("All", "EanableControl");
            AddRecord("初始化完成....", false);            
            return;
        }

        private void WriteIpDelectSiglePowerData(ushort address, ushort addressdata)
        {
            try
            {
                if (tcpClient.Connected)
                {
                    //电源-- 100 设备地址定死了是100,1表示长度就是1..                
                    masterIp.WriteSingleRegister(100, address, addressdata);
                }
                else
                {
                    AddRecord("和设备链接断开!", true);
                }

            }
            catch (Exception ex) { throw; }
        }

        private void WriteIpDelectMultiplePowerData(ushort address, ushort[] addressdata)
        {
            try
            {                
                if (tcpClient.Connected)
                {
                    //电源-- 100 设备地址定死了是100,1表示长度就是1..                
                    masterIp.WriteMultipleRegisters(100, address, addressdata);                    
                } else
                {
                    AddRecord("和设备链接断开!", true);
                }

            } catch (Exception ex) { throw; }
        }

        private void ReadIpDelectPowerData(ushort address)
        {
            try
            {
                //电源-- 100 设备地址定死了是100,1表示长度就是1..
                ushort[] data = masterIp.ReadHoldingRegisters(100, address, 17);                
                ushort datac = data[0];
                switch (datac)
                {
                    
                    case 0: { m_powerStateModel.Powerstate = "待机"; } break;
                    case 1: { m_powerStateModel.Powerstate = "启动"; } break;
                    case 2: { m_powerStateModel.Powerstate = "设置"; } break;
                    case 3: { m_powerStateModel.Powerstate = "短路"; } break;
                    case 4: { m_powerStateModel.Powerstate = "过热"; } break;
                    case 5: { m_powerStateModel.Powerstate = "过载"; } break;
                    
                }
                m_userPowerSetModel.Hz_data = (data[1] / 10).ToString(); //数据1 频率.. 要除以10
                //A相输出电压 UINT    0.1V R   0x0002
                _ = data[2].ToString("0.0");
                ///B相输出电压 UINT    0.1V R   0x0003
                _ = data[3].ToString("0.0");
                //C相输出电压 UINT    0.1V R   0x0004
                _ = data[4].ToString("0.0");
                //A相输出电流 UINT    0.1A R   0x0005
                _ = data[5].ToString("0.0");
                //if (m_userTestModel.ProtectCurrent < data[5])
                //{
                    //严重错误.. -- 电流过大了，要烧东西..
                //    AddRecord("A相输出电流过大了,请断电-请断电-请断电!" + data[5].ToString(), true);
                    //直接下指令停止电源..
                //    WriteIpDelectSiglePowerData(17, 0);
                //}
                
                //B相输出电流 UINT    0.1A R   0x0006
                _ = data[6].ToString("0.0");
                //if (m_userTestModel.ProtectCurrent < data[6])
                //{
                    //严重错误.. -- 电流过大了，要烧东西..
                //    AddRecord("B相输出电流过大了,请断电-请断电-请断电!" + data[6].ToString(), true);
                    //直接下指令停止电源..
                //    WriteIpDelectSiglePowerData(17, 0);
                //}

                //C相输出电流 UINT    0.1A R   0x0007
                _ = data[7].ToString("0.0");
               // if (m_userTestModel.ProtectCurrent < data[7])
                //{
                    //严重错误.. -- 电流过大了，要烧东西..
                 //   AddRecord("C相输出电流过大了,请断电-请断电-请断电!" + data[7].ToString(), true);
                    //直接下指令停止电源..
                 //   WriteIpDelectSiglePowerData(17, 0);
                //}

                //A相输出有功功率 UINT    0.01KW R   0x0008
                _ = data[8].ToString("0.0");
                //B相输出有功功率 UINT    0.01KW R   0x0009
                _ = data[9].ToString("0.0");
                //C相输出有功功率 UINT    0.01KW R   0x000A
                _ = data[10].ToString("0.0");
                //高低档状态 UINT        R   0x000B
                _ = data[11].ToString("0.0");
                datac = data[11];
                switch (datac)
                {
                    case 0: { m_powerStateModel.Powergaodi = "低档"; } break;
                    case 1: { m_powerStateModel.Powergaodi = "高档"; } break;
                }

                //设置频率 UINT    0.1Hz R/ W 0x000C 
                m_userPowerSetModel.Hz_data = (data[12] / 10).ToString();
                //UINT    0.1V R/ W 0x000D
                float fdata1 = data[13];
                float fdata2 = fdata1 / 10;
                m_userPowerSetModel.Adainya_data = fdata2.ToString("0.0");

                fdata1 = data[14];
                fdata2 = fdata1 / 10;
                m_userPowerSetModel.Bdainya_data = fdata2.ToString("0.0");
                //UINT    0.1V R/ W 0x000F

                fdata1 = data[15];
                fdata1 = fdata1 / 10;
                m_userPowerSetModel.Cdainya_data = fdata2.ToString("0.0");
                //设置三相电压 UINT    0.1V R/ W 0x0010
                //Dianya_data = data[16] / 10;
            }
            catch (Exception ex) 
            {
                throw; 
            }
        }

        void setLinkBreakOrConnection(int command,bool myvalue)
        {
            if (command == 1) { m_userCommunicationModel.Image1 = myvalue; }
            if (command == 2) { m_userCommunicationModel.Image1 = myvalue; }
            if (command == 10) { m_userCommunicationModel.Image2 = myvalue; }
            if (command == 11) { m_userCommunicationModel.Image2 = myvalue; }
            if (command == 12) { m_userCommunicationModel.Image2 = myvalue; }
        }

      
        private async Task<bool> ExePlcCommand(int command, ushort regaddress, ushort regaddressdata = 0)
        {
            bool Success = false;
            string error = string.Empty;

            //AddRecord("管理线程-请等待,设备回应!", false);
            var timeout = Task.Delay(12000);
            var t = Task.Run(async () =>
            {                
                try
                {
                    switch (command)
                    {
                        case 1: ReadModbusData(); break;
                        case 2:
                            {
                                //写入      (92)
                                //01  10    00 5C    00 02     04   00 00 00 01    37 06 --（监视得到的数据）--(软件模拟的)         
                                //(设备回的.) 01 10 00 5C 00 02 81 DA
                                //01  10    00 92    00 02     04   00 00 00 01 
                                ushort[] data = new ushort[2]{ 0, 0 };
                                //这里可以不封装了..只要返回不超时就是OK的..
                                //1 设备 146 92 data 设置..
                                data[1] = regaddressdata;
                                this.master.WriteMultipleRegisters(1, regaddress, data);
                            }
                            break;
                        case 3: break; //设置功率分析
                        case 4: break;                            
                        case 10: ReadIpDelectPowerData(regaddress); break;//10开始是电源指令类..
                        case 11:
                            {
                                ushort[] sdata = new ushort[5];
                                //[12] 设置频率 UINT    0.1Hz R/ W 0x000C
                                //[13]UINT    0.1V R/ W 0x000D
                                //[14]UINT    0.1V R/ W 0x000E
                                //[15]UINT    0.1V R/ W 0x000F
                                //[16]设置三相电压 UINT    0.1V R/ W 0x0010
                                ushort temp1,temp2;
                                if (!ushort.TryParse(m_userPowerSetModel.Hz_data, out temp1)) 
                                { AddRecord("转换频率失败!", true); return; }

                                if (!ushort.TryParse(m_userPowerSetModel.Dianya_data, out temp2))
                                { AddRecord("转电压失败!", true); return; }

                                int temp11 = temp2 % 3;
                                int temp12 = temp2 / 3;

                                if (m_userTestModel.ShowOneOrThree == "三相")
                                {
                                    //第一步
                                    {
                                        AddRecord("第1次加压,请等待...",false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)((temp11 * 10) / 1.7321);
                                        sdata[2] = (ushort)((temp11 * 10) / 1.7321);
                                        sdata[3] = (ushort)((temp11 * 10) / 1.7321);
                                        sdata[4] = (ushort)((temp11 * 10) / 1.7321);
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                    }
                                    Thread.Sleep(3000);
                                    //第2步
                                    {
                                        AddRecord("第2次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)(((temp11 + temp12) * 10) / 1.7321);
                                        sdata[2] = (ushort)(((temp11 + temp12) * 10) / 1.7321);
                                        sdata[3] = (ushort)(((temp11 + temp12) * 10) / 1.7321);
                                        sdata[4] = (ushort)(((temp11 + temp12) * 10) / 1.7321);
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                    }
                                    Thread.Sleep(2000);
                                    //第3步
                                    {
                                        AddRecord("第3次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)(((temp11 + temp12 + temp12) * 10) / 1.7321);
                                        sdata[2] = (ushort)(((temp11 + temp12 + temp12) * 10) / 1.7321);
                                        sdata[3] = (ushort)(((temp11 + temp12 + temp12) * 10) / 1.7321);
                                        sdata[4] = (ushort)(((temp11 + temp12 + temp12) * 10) / 1.7321);
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                    }
                                    Thread.Sleep(2000);
                                    //第4步
                                    {
                                        AddRecord("第4次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)(((temp11 + temp12 + temp12+ temp12) * 10) / 1.7321);
                                        sdata[2] = (ushort)(((temp11 + temp12 + temp12+ temp12) * 10) / 1.7321);
                                        sdata[3] = (ushort)(((temp11 + temp12 + temp12+ temp12) * 10) / 1.7321);
                                        sdata[4] = (ushort)(((temp11 + temp12 + temp12+ temp12) * 10) / 1.7321);
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                        AddRecord("加压完成.....", false);
                                    }
                                }
                                if (m_userTestModel.ShowOneOrThree == "单相")
                                {
                                    /*
                                    sdata[0] = (ushort)(temp1 * 10);
                                    sdata[1] = (ushort)(temp2 * 10);
                                    sdata[2] = (ushort)(temp2 * 10);
                                    sdata[3] = (ushort)(temp2 * 10);
                                    sdata[4] = (ushort)(temp2 * 10);
                                    WriteIpDelectMultiplePowerData(regaddress, sdata);
                                    Thread.Sleep(500);
                                    ReadIpDelectPowerData(0);
                                    */
                                    //第一步
                                    {
                                        AddRecord("第1次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)((temp11 * 10));
                                        sdata[2] = (ushort)((temp11 * 10));
                                        sdata[3] = (ushort)((temp11 * 10));
                                        sdata[4] = (ushort)((temp11 * 10));
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                    }
                                    Thread.Sleep(3000);
                                    //第2步
                                    {
                                        AddRecord("第2次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)(((temp11 + temp12) * 10));
                                        sdata[2] = (ushort)(((temp11 + temp12) * 10));
                                        sdata[3] = (ushort)(((temp11 + temp12) * 10));
                                        sdata[4] = (ushort)(((temp11 + temp12) * 10));
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                    }
                                    Thread.Sleep(2000);
                                    //第3步
                                    {
                                        AddRecord("第3次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)(((temp11 + temp12 + temp12) * 10));
                                        sdata[2] = (ushort)(((temp11 + temp12 + temp12) * 10));
                                        sdata[3] = (ushort)(((temp11 + temp12 + temp12) * 10));
                                        sdata[4] = (ushort)(((temp11 + temp12 + temp12) * 10));
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                    }
                                    Thread.Sleep(2000);
                                    //第4步
                                    {
                                        AddRecord("第4次加压,请等待...", false);
                                        sdata[0] = (ushort)(temp1 * 10);
                                        sdata[1] = (ushort)(((temp11 + temp12 + temp12 + temp12) * 10));
                                        sdata[2] = (ushort)(((temp11 + temp12 + temp12 + temp12) * 10));
                                        sdata[3] = (ushort)(((temp11 + temp12 + temp12 + temp12) * 10));
                                        sdata[4] = (ushort)(((temp11 + temp12 + temp12 + temp12) * 10));
                                        WriteIpDelectMultiplePowerData(regaddress, sdata);
                                        Thread.Sleep(500);
                                        ReadIpDelectPowerData(0);
                                        AddRecord("加压完成.....", false);
                                    }
                                }

                            }
                            break;

                        case 12:
                            {
                                WriteIpDelectSiglePowerData(regaddress, regaddressdata);
                                Thread.Sleep(500);
                                ReadIpDelectPowerData(0);
                            }
                            break;
                        case 13: 
                            {
                                ushort[] data = masterIp.ReadHoldingRegisters(100, 0, 1);
                            }
                            break;
                        case 999999: Testsimulation(); break; //仿真数据.
                    }
                    Success = true;
                    //AddRecord("工作线程-执行结束" + command.ToString(), false);                    
                }
                catch (Exception ex)
                {
                    //总异常:
                    error = ex.Message;
                    AddRecord("工作线程-执行异常:" + error + "当前指令:"+command.ToString(), true);
                    Success = false;
                    return;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                setLinkBreakOrConnection(command, false);
                AddRecord("管理线程-超时,请检查设备是否正常!" + command.ToString(), true);
                return false;
            }

            if (Success)
            {
                setLinkBreakOrConnection(command,true);
                return true;
            }
            else
            {
                setLinkBreakOrConnection(command, false);
                //The operation is not allowed on non-connected sockets.
                AddRecord("管理线程-工作线程异常:" + error, true);
                if (error.Contains("non-connected sockets"))
                {
                    AddRecord("管理线程-被服务器踢掉:" + error, true);
                    try
                    {
                        //尝试重新连接
                        AddRecord("管理线程-重新链接服务器:", false);
                        tcpClient = new TcpClient();
                        tcpClient.Connect(IPAddress.Parse(ConfigurationManager.AppSettings["DEVICE_BPDY"]), 502);
                        masterIp = ModbusIpMaster.CreateIp(tcpClient);
                    }
                    catch (Exception ex)
                    {
                        //如果补救链接也失败--那就没办法了..
                        AddRecord("管理线程-补救异常:" + error, true);
                        return false;
                    }
                }
                return false;
            }
        }

        public void AddRecord(string strdata, bool e)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            //string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Insert(0,new Item { Text = strdata, IsRed = e });
                logger.Error(strdata);
            });
        }

        private void ShowDevclDataToUi(float[]sdata)
        {            
            m_userMainModel.Umn_ab = (float)(sdata[0] * 1.732);
            m_userMainModel.Umn_ab = (float)Math.Round(m_userMainModel.Umn_ab, 3);

            m_userMainModel.Umn_bc = (float)(sdata[1] * 1.732);
            m_userMainModel.Umn_bc = (float)Math.Round(m_userMainModel.Umn_bc, 3);

            m_userMainModel.Umn_ca = (float)(sdata[2] * 1.732);
            m_userMainModel.Umn_ca = (float)Math.Round(m_userMainModel.Umn_ca, 3);

            m_userMainModel.Urms_ab  = (float)(sdata[3] * 1.732);
            m_userMainModel.Urms_ab = (float)Math.Round(m_userMainModel.Urms_ab, 3);

            m_userMainModel.Urms_bc  = (float)(sdata[4] * 1.732);
            m_userMainModel.Urms_bc = (float)Math.Round(m_userMainModel.Urms_bc, 3);

            m_userMainModel.Urms_ca = (float)(sdata[5] * 1.732); 
            m_userMainModel.Urms_ca = (float)Math.Round(m_userMainModel.Urms_ca, 3);

            m_userMainModel.Irms_ab  = sdata[6];
            m_userMainModel.Irms_ab = (float)Math.Round(m_userMainModel.Irms_ab, 3);

            if (m_userTestModel.ProtectCurrent < m_userMainModel.Irms_ab)
            {
                //严重错误.. -- 电流过大了，要烧东西..
                AddRecord("A相输出电流过大了,请断电-请断电-请断电!" , true);
                //直接下指令停止电源..
                WriteIpDelectSiglePowerData(17, 0);
            }

            m_userMainModel.Irms_bc  = sdata[7];
            m_userMainModel.Irms_bc = (float)Math.Round(m_userMainModel.Irms_bc, 3);

            m_userMainModel.Irms_ca = sdata[8];
            m_userMainModel.Irms_ca = (float)Math.Round(m_userMainModel.Irms_ca, 3);

            m_userMainModel.P_ab  = sdata[9];
            m_userMainModel.P_ab = (float)Math.Round(m_userMainModel.P_ab, 3);

            m_userMainModel.P_bc  = sdata[10];
            m_userMainModel.P_bc = (float)Math.Round(m_userMainModel.P_bc, 3);

            m_userMainModel.P_ca = sdata[11];
            m_userMainModel.P_ca = (float)Math.Round(m_userMainModel.P_ca, 3);

            /*
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
            */

            

            //计算结果..(计算是工作线程计算的..)(以下为 三相显示..)
            if (m_userTestModel.ShowOneOrThree == "三相")
            {
                
                //ATPVoltage 三相电压算术平均值
                m_userRecordModel.ATPVoltage = (m_userMainModel.Umn_ab + m_userMainModel.Umn_bc + m_userMainModel.Umn_ca) / 3;
                m_userRecordModel.ATPVoltage = (float)Math.Round(m_userRecordModel.ATPVoltage, 3);
                m_userRecordModel.RMSvalue = (m_userMainModel.Urms_ab + m_userMainModel.Urms_bc + m_userMainModel.Urms_ca) / 3;
                m_userRecordModel.RMSvalue = (float)Math.Round(m_userRecordModel.RMSvalue, 3);
                //IRMSvalue 三相电流算术平均值
                m_userRecordModel.IRMSvalue = (m_userMainModel.Irms_ab + m_userMainModel.Irms_bc + m_userMainModel.Irms_ca) / 3;
                m_userRecordModel.IRMSvalue = (float)Math.Round(m_userRecordModel.IRMSvalue, 3);

                //计算空载电流
                m_userRecordModel.NoloadCurrent2 = MyTools.GetNoloadCurrentThree(m_userRecordModel.IRMSvalue, m_userRecordModel.ATPVoltage, m_userTestModel.Shijiazhashu_data);

                //计算 电流%.(显示 电流%)
                m_userRecordModel.PercentageNoloadCurrent = MyTools.GetCalcRatedCurrentPercentageThree(m_userRecordModel.NoloadCurrent2, m_userRecordModel.NoloadCurrent);
                m_userRecordModel.PercentageNoloadCurrent = (float)Math.Round(m_userRecordModel.PercentageNoloadCurrent, 3);

                //计算 空载损耗.
                m_userRecordModel.NoloadLoss = MyTools.GetCalcNoloadlossThree(m_userMainModel.P_ab + m_userMainModel.P_bc + m_userMainModel.P_ca,
                    m_userRecordModel.ATPVoltage, m_userTestModel.Shijiazhashu_data);
                m_userRecordModel.NoloadLoss = (float)Math.Round(m_userRecordModel.NoloadLoss, 3);                        

                //1.空载损耗.  --- 2.施加电压. -- 3额定电压.. --4 空载%   5空载标准
                //6空载标准(上) 7 空载标准(下) 8 电流标准  9  电流标准（上）10 电流标准（下）
                int result = MyTools.GetResultThree(m_userRecordModel.NoloadLoss,//float
                    m_userRecordModel.PercentageNoloadCurrent,
                    m_userStandardModel.ProductStandard, //string
                    m_userStandardModel.ProductStandardUpperimit,//string
                    m_userStandardModel.ProductStandardDownimit,//string
                    m_userStandardModel.ProductCurrentStandard,//string
                    m_userStandardModel.ProductCurrentStandardUpperrimit,//string
                    m_userStandardModel.ProductCurrentStandardDownimit);//string


                if (result == 1) m_userRecordModel.QualifiedJudgment = "合格";
                if (result == 0) m_userRecordModel.QualifiedJudgment = "不合格";
                if (result == -1) AddRecord("计算结果转换失败!", true);

            }
            //计算结果..(计算是工作线程计算的..)(以下为 单相显示..)
            if (m_userTestModel.ShowOneOrThree == "单相")
            {
                m_userRecordModel.ATPVoltage = (m_userMainModel.Umn_ab + m_userMainModel.Umn_bc + m_userMainModel.Umn_ca) / 3;
                m_userRecordModel.RMSvalue = (m_userMainModel.Urms_ab + m_userMainModel.Urms_bc + m_userMainModel.Urms_ca) / 3;
                m_userRecordModel.IRMSvalue = (m_userMainModel.Irms_ab + m_userMainModel.Irms_bc + m_userMainModel.Irms_ca) / 3;

                //计算空载电流
                m_userRecordModel.NoloadCurrent2 = MyTools.GetNoloadCurrentThree(m_userRecordModel.IRMSvalue, m_userRecordModel.ATPVoltage, m_userTestModel.Shijiazhashu_data);

                //计算 电流%.(显示 电流%)
                m_userRecordModel.PercentageNoloadCurrent = MyTools.GetCalcRatedCurrentPercentageThree(m_userRecordModel.NoloadCurrent2, m_userRecordModel.NoloadCurrent);
                m_userRecordModel.PercentageNoloadCurrent = (float)Math.Round(m_userRecordModel.PercentageNoloadCurrent, 3);

                //计算 空载损耗.
                m_userRecordModel.NoloadLoss = MyTools.GetCalcNoloadlossThree(m_userMainModel.P_ab + m_userMainModel.P_bc + m_userMainModel.P_ca,
                    m_userRecordModel.ATPVoltage, m_userTestModel.Shijiazhashu_data);
                m_userRecordModel.NoloadLoss = (float)Math.Round(m_userRecordModel.NoloadLoss, 3);

                int result = MyTools.GetResultThree(m_userRecordModel.NoloadLoss,//float
                        m_userRecordModel.PercentageNoloadCurrent,
                        m_userStandardModel.ProductStandard, //string
                        m_userStandardModel.ProductStandardUpperimit,//string
                        m_userStandardModel.ProductStandardDownimit,//string
                        m_userStandardModel.ProductCurrentStandard,//string
                        m_userStandardModel.ProductCurrentStandardUpperrimit,//string
                        m_userStandardModel.ProductCurrentStandardDownimit);//string

                if (result == 1) m_userRecordModel.QualifiedJudgment = "合格";
                if (result == 0) m_userRecordModel.QualifiedJudgment = "不合格";
                if (result == -1) AddRecord("计算结果转换失败!", true);

            }

        }

        void Testsimulation()
        {
            //仿真数据...(设备功率分析)
            float[] sdata = new float[12];
            for (int i = 0;i<12;i++)
            {
                Random ran = new Random();
                int n = ran.Next(100, 10000);
                sdata[i] = n;
            }

            ShowDevclDataToUi(sdata);

        }


        private void ReadModbusData()
        {
            float[] FloatValue = new float[12];
            try
            {
                //第一单元
                ushort[] data1 = this.master.ReadHoldingRegisters(1, 0x100, 18);
                float[] fdata1 = MyTools.ushortToFloat(data1);

                FloatValue[0] = (float)(fdata1[0] * 0.998); //过来的数据都是
                FloatValue[3] = fdata1[0];
                FloatValue[6] = fdata1[1];
                FloatValue[9] = fdata1[2];

                //第2单元
                ushort[] data2 = this.master.ReadHoldingRegisters(1, 0x160, 18);
                float[] fdata2 = MyTools.ushortToFloat(data2);
                FloatValue[1] = (float)(fdata2[0] * 0.998);
                FloatValue[4] = fdata2[0];
                FloatValue[7] = fdata2[1];
                FloatValue[10] = fdata2[2];

                //第3单元
                ushort[] data3 = this.master.ReadHoldingRegisters(1, 0x1C0, 18);
                float[] fdata3 = MyTools.ushortToFloat(data3);
                FloatValue[2] = (float)(fdata3[0] * 0.998);
                FloatValue[5] = fdata3[0];
                FloatValue[8] = fdata3[1];
                FloatValue[11] = fdata3[2];

                ShowDevclDataToUi(FloatValue);

                return;
            }
            catch(Exception ex)
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

    public class CommandItem
    {
        public int command { get; set; }
        public ushort regaddress { get; set; }
        public ushort regaddressdata { get; set; }
    }
}
