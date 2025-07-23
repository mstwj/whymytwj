using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using System.Windows;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Net;

namespace 电源MODBUS协议_53015.Model
{
    public class MainWinModel : ObservableObject
    {
        private SerialPort serialPort = new SerialPort();
        //private ModbusSerialMaster master = null;
        //private ModbusSerialMaster master = null;

        private ModbusIpMaster master = null;

        private Task rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<int> queue = new Queue<int>();
        public string mySelectItem { get; set; } = string.Empty;
        public List<string> ComboBoxItems { get; set; } = new List<string>();
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MainWinModel()
        {
            ComboBoxItems.Add("COM1");
            ComboBoxItems.Add("COM2");
            ComboBoxItems.Add("COM3");
            ComboBoxItems.Add("COM4");
            ComboBoxItems.Add("COM5");
            ComboBoxItems.Add("COM6");
            ComboBoxItems.Add("COM7");
            ComboBoxItems.Add("COM8");
            ComboBoxItems.Add("COM9");
            ComboBoxItems.Add("COM10");
            ComboBoxItems.Add("COM11");
            ComboBoxItems.Add("COM12");

            mySelectItem = ConfigurationManager.AppSettings["DEVICE_COM"];

            InitializeAsync();
        }

        void AddRecord(string strdata, bool e)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(new Item { Text = str, IsRed = e });
            });            
        }


        public async void InitializeAsync()
        {
            //先打开端口...
            try
            {
                logger.Debug("启动完成");
                AddRecord("启动完成...", false);

                /*
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse("192.168.1.15"), 502);
                master = ModbusIpMaster.CreateIp(tcpClient);
                master.Transport.WriteTimeout = 2000;
                master.Transport.ReadTimeout = 2000;
                master.Transport.WaitToRetryMilliseconds = 500;
                master.Transport.Retries = 3;

                /*
                logger.Debug("启动完成");
                AddRecord("启动完成...", false);
                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议

                master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                master.Transport.ReadTimeout = 1000;// 设置超时时间默认为500毫秒
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_COM"];
                serialPort.Open(); //打开串口                                
                AddRecord("打开串口完成",false);
                */
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
                            logger.Debug("机器人线程-定时器1秒任务");
                            //设置为后台执行..
                            var myresult1 = await ExePlcCommand(2);
                            //必须要判断，要不然 子线程他不等..
                            if (!myresult1)
                            {                                
                                AddRecord("定时器指令读取PLC执行失败-后台线程退出!", true);
                                return;
                            }
                            precurrentTime = currentTime;
                            continue;
                        }
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();
                        logger.Debug("机器人线程-点击" + Command);
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
                        logger.Error("机器人线程报错" + ex.Message);
                        return;
                    }
                }
            }, cts.Token);
        }

        void UpDateUi()
        {

        }

        private ushort[] ReadModbusData(byte slaveId, ushort start, ushort length)
        {
            float[] FloatValue = new float[3];
            try
            {
                //这里我懂了，这里多此一举了.. 本质读取的是WORD 2个字的，可是我自己又转变成了BYTE了...
                ushort[] data = this.master.ReadHoldingRegisters(slaveId, start, length);

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
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void ReadAllPlcData()
        {
            try
            {
                //读表.
                ushort[] data = ReadModbusData(100, 0, 5);
                UpDateUi();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task<bool> ExePlcCommand(int command)
        {
            bool Success = false;
            string error = string.Empty;

            AddRecord("请等待,PLC回应!", false);
            var timeout = Task.Delay(8000);
            var t = Task.Run(async () =>
            {
                logger.Debug("新线程-开始执行" + command);
                try
                {
                    switch (command)
                    {
                        case 1: break;
                        case 2:  break;
                        case 3: break;
                    }
                    Success = true;
                    logger.Debug("新线程-执行完成结束" + command);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    logger.Error("新线程-任务执行异常:" + error);
                    AddRecord("PLC异常:" + error, true);
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                logger.Error("机器人线程-超时" + command);
                AddRecord("PLC超时,请检查PLC是否正常!", true);                
                return false;
            }

            if (Success)
            {
                logger.Debug("机器人线程-完成" + command);
                return true;
            }
            else
            {                
                logger.Error("机器人线程-错误Success==false" + command);
                AddRecord("PLC异常:" + error, true);             
                return false;
            }
        }


    }
    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }
}
