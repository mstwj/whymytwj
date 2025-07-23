using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Modbus.Device;
using System.IO.Ports;
using System.Windows.Markup;
using System.Numerics;
using System.Windows.Media.Imaging;



namespace MSK_36密度变速器表.Model
{    

    public class MainViewModel : ObservableValidator 
    {
        private SerialPort serialPort = new SerialPort();
        //不知道为什么（NMOSBUS是有问题的.. 读是OK的写总是超时..）
        //private ModbusSerialMaster master = null;
        private Task rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<int> queue = new Queue<int>();

        public ICommand CommandWait { get; set; } //
        public ICommand CommandOpen { get; set; } //
        public ICommand CommandClose { get; set; } //
        public ICommand CommandSetDevel { get; set; } //
        public ICommand CommandGetDevel { get; set; } //
        public ObservableCollection<string> ListBoxData { get; set; } = new ObservableCollection<string>();
        public string SelectItem { get; set; } = string.Empty;
        public bool HasError { get; set; }
        public string HasErrorString { get; set; } = string.Empty;
        public List<string> ComboBoxItems { get; set; } = new List<string>();

        /*
        private string _name = string.Empty;
        [Required(ErrorMessage = "Name不能为空")]
        [MinLength(2, ErrorMessage = "name长度不小于2")]
        [MaxLength(30, ErrorMessage = "name长度不大于30")]
        public string Name { get => _name; set { SetProperty(ref _name, value, true); } }
        */
        private int _currentNumber = -1;
        public int CurrentNumber { get => _currentNumber; set { SetProperty(ref _currentNumber, value); } }

        private int _shebehao2 = 0x20;
        public int Shebehao2 { get => _shebehao2; set { SetProperty(ref _shebehao2, value); } }


        private int _shebehao = -1;        
        [Required]
        [CustomValidation(typeof(MainViewModel), nameof(ValidateAge1))]
        public int Shebehao { get => _shebehao;  set { SetProperty(ref _shebehao, value,true); } }

        public static ValidationResult ValidateAge1(int shebehao, ValidationContext context)
        {
            if (shebehao > 0 && shebehao < 239)
            {
                return ValidationResult.Success;
            }
            return new("错误数据只能是1到240之间");
        }

        public MainViewModel()
        {

            CommandOpen = new RelayCommand<object>(DoCommandOpen);
            CommandClose = new RelayCommand<object>(DoCommandClose);
            CommandSetDevel = new RelayCommand<object>(DoCommandSetDevel);
            CommandGetDevel = new RelayCommand<object>(DoCommandGetDevel);
            CommandWait = new RelayCommand<object>(DoCommandWait);

            ComboBoxItems.Add("COM1");
            ComboBoxItems.Add("COM2");
            ComboBoxItems.Add("COM3");
            ComboBoxItems.Add("COM4");
            ComboBoxItems.Add("COM5");
            ComboBoxItems.Add("COM6");
            ComboBoxItems.Add("COM7");
            ComboBoxItems.Add("COM8");
            ComboBoxItems.Add("COM9");

            InitializeAsync();
            AddRecord("启动完成");

            Task ReTs = Task.Run(async () =>
            {
                while (true)
                {
                    //这里肯定会等待..
                    Thread.Sleep(10);
                    //ExePlcCommand();
                    //这里肯定会等待..
                    await Task.Delay(5000);
                    //这里不会去等待..
                    Task retsul = Task.Delay(5000);
                }
            });
        }

        

        private async Task ExePlcCommand()
        {
            await Task.Delay(1000); // 模拟耗时操作
            //return Task.CompletedTask ;
        }

        private async void DoCommandWait(object button)
        {
            //等待2秒，不管是不是主线程....
            await Task.Delay(2000);
            //这里不会去等待..
            var timeout = Task.Delay(2000);

            //MessageBox.Show("123123");
            //var result = await Task.Delay(2000);
        }

        


        private void DoCommandGetDevel(object button)
        {
            /*
            if (queue.Count >0)
            {
                AddRecord("无法执行指令,上一条指令还未执行完成");
                return;
            }
            queue.Enqueue(2);
            */

            //这里我懂了，这里多此一举了.. 本质读取的是WORD 2个字的，可是我自己又转变成了BYTE了...
            ushort[] ushortArray = this.master.ReadHoldingRegisters(1, 0, 6);
            ushort[] ushortArray1 = new ushort[2];
            ushort[] ushortArray2 = new ushort[2];
            ushort[] ushortArray3 = new ushort[2];
            ushortArray1[0] = ushortArray[0];
            ushortArray1[1] = ushortArray[1];
            ushortArray2[0] = ushortArray[2];
            ushortArray2[1] = ushortArray[3];
            ushortArray3[0] = ushortArray[4];
            ushortArray3[1] = ushortArray[5];

            Array.Reverse(ushortArray1);
            Array.Reverse(ushortArray2);
            Array.Reverse(ushortArray3);
            //Convert.ToSingle(data, Type(float));//因为是 ToSingle所以自动4个 。。
            
            byte[] byteArray1 = new byte[4];
            Buffer.BlockCopy(ushortArray1, 0, byteArray1, 0, 4);
            float aaa = BitConverter.ToSingle(byteArray1, 0);

            byte[] byteArray2 = new byte[4];
            Buffer.BlockCopy(ushortArray2, 0, byteArray2, 0, 4);
            float bbb = BitConverter.ToSingle(byteArray2, 0);

            byte[] byteArray3 = new byte[4];
            Buffer.BlockCopy(ushortArray3, 0, byteArray3, 0, 4);
            float ccc = BitConverter.ToSingle(byteArray3, 0);

            aaa = aaa / 1000;
            float roundedNumber1 = (float)Math.Round(aaa, 2);
            bbb = bbb / 1000;
            float roundedNumber2 = (float)Math.Round(bbb, 2);
            ccc = ccc / 1000;
            float roundedNumber3 = (float)Math.Round(ccc, 2);

          
        }

        private void DoCommandSetDevel(object button)
        {
            //WeakReferenceMessenger.Default.Send<string, string>("ValideAll", "ValideAll");            
            ValidateAllProperties(); 
            if (HasErrors)
            {                
                // Handle validation errors
                // For example, inform the user to correct the errors
                MessageBox.Show("验证没有通过-请看错误提示!");
            }
            else
            {                
                if (queue.Count > 0)
                {
                    AddRecord("无法执行指令,上一条指令还未执行完成");
                    return;
                }
                queue.Enqueue(1);
            }

            return;
        }

        private ModbusSerialMaster master = null;

        private void DoCommandOpen(object button)
        {
            serialPort.BaudRate = 9600; // 设置波特率
            serialPort.Parity = Parity.None; // 设置奇偶校验
            serialPort.DataBits = 8; // 设置数据位数
            serialPort.StopBits = StopBits.One; // 设置停止位
            serialPort.Handshake = Handshake.None; // 设置握手协议

            master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
            master.Transport.ReadTimeout = 500;// 设置超时时间默认为500毫秒
            serialPort.PortName = "COM4";
            serialPort.Open(); //打开串口                

            /*
            //先打开端口...
            try
            {
                AddRecord("准备打开串口");

                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议(这里如果是 FULUKE就是硬件..)

                //master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                //master.Transport.ReadTimeout = 1000;// 设置超时时间默认为1000毫秒
                serialPort.PortName = SelectItem;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); // 接收到数据时的事件                
                serialPort.Open(); //打开串口
                AddRecord("打开串口成功!");

            }
            catch (Exception ex)
            {
                AddRecord("打开串口失败!" + ex.Message);
                return;
            }
            */



            return;
        }





        
        int currentcomandnumber = 0;
        private AutoResetEvent autoEvent = new AutoResetEvent(false);

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {


            byte[] _buffer = new byte[100];
            try
            {
                //获取接收缓冲区中数据的字节数 -- 
                SerialPort sp = (SerialPort)serialPort;

                //真实读取的字节..
                int readlength = sp.Read(_buffer, 0, 80);
                if (readlength < 5) return;
                autoEvent.Set();//给信号.. 回来了..

                //返回指令...
                if (currentcomandnumber == 1)
                {
                    CurrentNumber = _buffer[5];
                    return;
                }

                if (currentcomandnumber == 2)
                {
                    //读..
                    CurrentNumber = _buffer[4];
                    return;
                }
            }
            catch (Exception ex)
            {
                //异常就看日志...
                AddRecord("系统串口线程异常:"+ex.Message);                
            }
        }

        private void DoCommandClose(object button)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    AddRecord("关闭串口完成!");
                }
            }
            catch(Exception ex)
            {
                AddRecord("关闭串口错误!"+ex.Message);
            }
            return;
        }


        void AddRecord(string strdata)
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Add(str);
            });
        }

        public async void InitializeAsync()
        {
            rotobTaskT = Task.Run(async () =>
            {
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        Thread.Sleep(10);
                        System.DateTime currentTime = System.DateTime.UtcNow;
             
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();
                        AddRecord("机器人-开始执行指令");
                        var result = await ExePlcCommand(Command);

                        if (result)                        
                            AddRecord("机器人-执行指令完成");
                        else                       
                            AddRecord("机器人-指令执行错误");                        
                    }
                    catch (Exception ex)
                    {
                        AddRecord("机器人-指令执行异常"+ex.Message);
                        return;
                    }
                }
            }, cts.Token);
        }

        private bool WriteModbusData(byte slaveId, ushort start, ushort value)
        {
            byte[] currentcommandbytes; //当前指令的byte
            string currentcommand;
            
            int totalLength = 2; // 假设我们需要的字符串长度是6
            char paddingChar = '0'; // 指定用'0'来填充左边

            //这里需要吧10转16..
            string hexString = value.ToString("X");            
            string svalue = hexString.ToString().PadLeft(totalLength, paddingChar);            

            //修改数据..
            //FA 03 00 30 00 01 91 8e            
            currentcommand = "FA-06-00-30-00-"+svalue;
            currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
            bool signaled = autoEvent.WaitOne(2000); // 等2秒

            if (signaled) return true;
            else return false;


            //这里异常了..
            //await master.WriteSingleRegisterAsync(slaveId, start, value); // 1是从站地址，registerAddress是寄存器地址，value是要写入的值
            //response.Wait();
            //这里反正就超时了..
            //this.master.WriteSingleRegister(slaveId, start, value);             
        }
        private bool ReadModbusData(byte slaveId, ushort start, ushort length)
        {
            byte[] currentcommandbytes; //当前指令的byte
            string currentcommand;

            //发送握手..
            //FA 03 00 30 00 01 91 8e
            currentcommand = "FA-03-00-30-00-01";
            currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
            bool signaled = autoEvent.WaitOne(2000); // 等2秒

            if (signaled) return true;
            else return false;
            /*
            try
            {        
                ushort[] data = this.master.ReadHoldingRegisters(slaveId, start, length);
                return data;
            }
            catch (Exception ex)
            {
                AddRecord("函数ReadHoldingRegisters失败"+ex.Message);
                throw;
            }
            */            
        }

        private async Task<bool> ExePlcCommand(int command)
        {
            bool Success = false;
            string error = string.Empty;

            var timeout = Task.Delay(5000);
            var t = Task.Run(async () =>
            {                
                try
                {
                    switch (command)
                    {
                        case 1:
                            {
                                currentcomandnumber = 1;
                                //写设备..
                                if (WriteModbusData((byte)Shebehao2, 0x30, (ushort)Shebehao))
                                {
                                    AddRecord("修改设备ID成功");
                                }
                                else
                                {
                                    AddRecord("修改设备ID超时");
                                }
                            }
                            break;
                        case 2:
                            {
                                currentcomandnumber = 2;
                                //读表.
                                if (ReadModbusData((byte)Shebehao2, 0x30, 1))
                                {
                                    AddRecord("读取设备成功");                                    
                                }
                                else
                                {
                                    AddRecord("读取设备超时");
                                    Success = false;
                                    return;
                                }
                            }
                            break;
                    }
                    Success = true;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    AddRecord("指令执行异常:" + error);
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                AddRecord("执行指令超时");
                return false;
            }

            if (Success)
            {               
                return true;
            }
            else
            {
                AddRecord("执行指令错误");                
                return false;
            }
        }

        public static byte[] CRCCalc(byte[] data)
        {
            //1.预置1个16位的寄存器为十六进制FFFF(即全为1); 称此寄存器为CRC寄存器;
            //crc计算赋初始值
            var crc = 0xffff;
            for (var i = 0; i < data.Length; i++)
            {
                //2.把第一个8位二进制数据(既通讯信息帧的第一个字节)与16位的CRC寄存器的低8位相异或，把结果放于CRC寄存器;
                crc = crc ^ data[i]; //将八位数据与crc寄存器异或 ,异或的算法就是，两个二进制数的每一位进行比较，如果相同则为0，不同则为1

                //3.把CRC寄存器的内容右移一位(朝低位)用0填补最高位，并检查右移后的移出位;
                //4.如果移出位为0:重复第3步(再次右移一位); 如果移出位为1:CRC寄存器与多项式A001(1010 0000 0000 0001)进行异或;
                //5.重复步骤3和4，直到右移8次，这样整个8位数据全部进行了处理;
                for (var j = 0; j < 8; j++)
                {
                    int temp;
                    temp = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (temp == 1) crc = crc ^ 0xa001;
                    crc = crc & 0xffff;
                }
            }

            //CRC寄存器的高低位进行互换
            var crc16 = new byte[2];
            //CRC寄存器的高8位变成低8位，
            crc16[1] = (byte)((crc >> 8) & 0xff);
            //CRC寄存器的低8位变成高8位
            crc16[0] = (byte)(crc & 0xff);
            return crc16;
        }



    }
}
