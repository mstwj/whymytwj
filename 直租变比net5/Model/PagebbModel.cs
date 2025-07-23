using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using S7.Net;
using 直租变比.Model;
using 直租变比net5.Parsing;

namespace 直租变比net5.Model
{
    public class PagebbModel : ObservableObject
    {

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Task? rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<int> queue = new Queue<int>();

        private SerialPort serialPort = new SerialPort();
        //private Plc m_plc = new Plc(CpuType.S7200Smart, "192.168.2.31", 0, 1);
        private Plc m_plc = null;

        private AutoResetEvent autoEvent = new AutoResetEvent(false);
        public newproinfotable? m_mainproinfo { get; set; } = null;

        //16265027138

        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();
        public PagebbModel()
        {
            //InitializeAsync();
        }

        void AddRecord(string strdata, bool e)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });
            });
            WeakReferenceMessenger.Default.Send<string, string>("ScrollEnd", "ScrollEnd");
            logger.Debug(str);
        }

        //InitializeAsync -- 被最顶层窗口调用...--调后就是 切换了..
        public async void InitializeAsync()
        {
            //先打开端口...
            try
            {
                //TCP也OK了...                
                AddRecord("启动完成...", false);

                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议
                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); // 接收到数据时的事件
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_COM_BB"];
                serialPort.Open(); //打开串口                

                AddRecord("打开串口完成", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            rotobTaskT = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;

                //logger.Debug("机器人线程-初始化链接33");
                var myresult3 = await ExePlcCommand(1);

                if (!myresult3) { AddRecord("连接PLC主机失败-后台线程退出,PLC正常后请重新启动软件!", true); return; }
                AddRecord("连接PLC主机成功", false);

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        //这里是需要TRY的。。 我执行代码的时候，遇到了一个问题，就是主线程没有加线程分发.. 导致.. 报错..
                        Thread.Sleep(10);
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();
                        AddRecord("机器人线程-点击", false);
                        var myresult2 = await ExePlcCommand(Command);
                        if (myresult2)
                        {
                            AddRecord("机器人线程-指令执行成功!", false);
                        }
                        else
                        {
                            AddRecord("机器人线程-指令执行失败!-后台线程退出", true);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        AddRecord($"机器人线程异常:-{ex.Message}", true);
                        return;
                    }
                }
            }, cts.Token);
        }

        private async Task<bool> ExePlcCommand(int Command)
        {
            bool Success = false;
            string error = string.Empty;

            AddRecord("请等待,回应!" + Command.ToString(), false);
            var timeout = Task.Delay(8000);
            var t = Task.Run(async () =>
            {
                AddRecord("新线程-开始执行", false);
                try
                {
                    Array.Clear(_buffer, 0, 1000);
                    readlength = 0;
                    switch (Command)
                    {
                        case 1: m_plc.Open(); break;
                        case 2: m_plc.Write("M21.3", true); break;
                        case 3: DataParsing.SendDataHand(serialPort); break; //3握手
                        case 4: DataParsing.SendDataStart(serialPort); break; //4启动
                        case 5: DataParsing.SendDataStop(serialPort); break; //5停止
                        case 6: DataParsing.SendDataSet(serialPort, 2); break; //6设置
                        case 7: DataParsing.SendDataRead(serialPort); break; //7读取
                    }
                    //这里的线程不能直接返回，他要去等待一个信号...                   
                    bool signaled = autoEvent.WaitOne(4000); // 等待4秒
                    if (signaled)
                    {
                        Success = true;
                        AddRecord("新线程-执行完成结束-设备正常返回", false);
                    }
                    else
                    {
                        error = "超时";
                        Success = false;
                        AddRecord("新线程-设备超时:", true);
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    AddRecord("新线程-任务执行异常:" + error, true);
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            //1个是 timerout 超时，1个是t 线程返回..--这里不能去判断t了..
            //要判断
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                AddRecord("机器人线程-超时!", true);
                return false;
            }

            if (Success)
            {

                AddRecord("机器人线程-完成", false);
                return true;
            }
            else
            {
                AddRecord("机器人线程-错误Success==false" + error, true);
                return false;
            }
        }

        private byte[] _buffer = new byte[1000];
        private int readlength = 0;
        private int currentcomandnumber = 0;
        //------------------------------------接受串口数据-------------------
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //串口会开一个新线程...
            try
            {
                //获取接收缓冲区中数据的字节数 -- 
                SerialPort sp = (SerialPort)serialPort;

                //真实读取的字节..(可能返回2次或者3次..)
                readlength = sp.Read(_buffer, readlength, 900);
                readlength += readlength;

                //返回指令...
                if (currentcomandnumber == 3)
                {
                    if (readlength < 7)
                    {
                        AddRecord("串口线程-握手指令长度小于7", true);
                        return;
                    }
                    if (DataParsing.ParsingDataHand(_buffer, readlength))
                    {
                        AddRecord("串口线程-握手正常", false);
                        autoEvent.Set();//给信号.. 回来了..
                    }
                    else
                    {
                        AddRecord("串口线程-握手错误", true);
                    }
                    return;
                }
                if (currentcomandnumber == 4)
                {
                    if (readlength < 7)
                    {
                        AddRecord("串口线程-启动指令长度小于7", true);
                        return;
                    }
                    if (DataParsing.ParsingDataStart(_buffer, readlength))
                    {
                        AddRecord("串口线程-启动正常", false);
                        autoEvent.Set();//给信号.. 回来了..
                    }
                    else
                    {
                        AddRecord("串口线程-启动错误", true);
                    }
                    return;

                }
                if (currentcomandnumber == 5)
                {

                    if (readlength < 5)
                    {
                        AddRecord("串口线程-停止指令长度小于7", true);
                        return;
                    }
                    if (DataParsing.ParsingDataStop(_buffer, readlength))
                    {
                        AddRecord("串口线程-停止正常", false);
                        autoEvent.Set();//给信号.. 回来了..
                    }
                    else
                    {
                        AddRecord("串口线程-停止错误", true);
                    }
                    return;
                }

                if (currentcomandnumber == 6)
                {
                    //设置有可能不返回...
                    AddRecord("串口线程-停止正常", false);
                    autoEvent.Set();//给信号.. 回来了..
                    return;
                }

                if (currentcomandnumber == 7)
                {
                    //读取长度肯定是 >0x27 或者== 0x27
                    if (readlength < 0x27) return;

                    if (DataParsing.ParsingDataRead(_buffer, readlength))
                    {
                        AddRecord("串口线程-读取正常", false);
                        autoEvent.Set();//给信号.. 回来了..
                    }
                    else
                    {
                        AddRecord("串口线程-读取错误", true);
                    }
                    return;
                }

            }
            catch (Exception ex)
            {
                //异常就看日志...                
                AddRecord("串口线程异常:" + ex.Message, false);
            }

        }

    }

    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }

}
