using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net5_10_14.Base;

namespace net5_10_14.Devcie
{
    class BYQBBCSYSerial : MySerialPort
    {
        private int allreadlength = 0;

        private byte[] _buffer = new byte[1000];

        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //定义委托
        public delegate void SerialPortCallBack(object sender, PortCallBackArg e);

        //声明事件
        public event SerialPortCallBack SerialPortCallBackEvent;

        //这里回调给机器人..
        protected virtual void OnCallbackevent(byte[] data, int length)
        {
            PortCallBackArg arg = new PortCallBackArg();
            arg.currentcommand = currentcomandnumber;            
            arg.data = data;
            arg.length = length;
            //保存了..
            SerialPortCallBack handler = SerialPortCallBackEvent;
            handler?.Invoke(this, arg);
        }

        public BYQBBCSYSerial()
        {

            serialPort.BaudRate = 9600; // 设置波特率
            serialPort.Parity = Parity.None; // 设置奇偶校验
            serialPort.DataBits = 8; // 设置数据位数
            serialPort.StopBits = StopBits.One; // 设置停止位
            serialPort.Handshake = Handshake.None; // 设置握手协议
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); // 接收到数据时的事件


        }

        public bool ExeCommand(int command, string cmmandarg)
        {
            if (!serialPort.IsOpen)
            {
                return false;
            }
            byte[] currentcommandbytes; //当前指令的byte
            string currentcommand;
            currentcomandnumber = command;
            switch (command)
            {
                case 1:
                    {

                        //发送握手..
                        currentcommand = "C8-D9-00-B0-01";
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        byte[] crcdata = CRCCalc(currentcommandbytes);
                        currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
                case 2:
                    {
                        //发送启动..
                        currentcommand = "C8-D9-00-B0-05-00-38";
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        byte[] crcdata = CRCCalc(currentcommandbytes);
                        currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
                case 3:
                    {
                        //发送停止..
                        currentcommand = "C8-D9-00-B0-08";
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        byte[] crcdata = CRCCalc(currentcommandbytes);
                        currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
                case 4:
                    {
                        currentcommand = "0";
                        //发送设置..                        
                        if (cmmandarg == "0")
                        {
                            currentcommand = "C8-D9-00-B0-06-00-39-00-01-02-00-01";
                        }
                        if (cmmandarg == "1")
                        {
                            currentcommand = "C8-D9-00-B0-06-00-39-00-01-02-00-02";
                        }
                        if (cmmandarg == "2")
                        {
                            currentcommand = "C8-D9-00-B0-06-00-39-00-01-02-00-03";
                        }
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        byte[] crcdata = CRCCalc(currentcommandbytes);
                        currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;

                case 5:
                    {
                        //本来应该有5的，可是 设置的时候，是不对的...
                    }
                    break;
                case 6:
                    {                        
                        //动发送读取测量数据. 
                        currentcommand = "C8-D9-00-B0-03-00-04-00-0E";//这里数据怎么写呢?
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        byte[] crcdata = CRCCalc(currentcommandbytes);
                        currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
            }
            return true;
        }

        public override void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int readlength = 0;

            //这里工作线程执行..
            try
            {
                if (serialPort.IsOpen)
                {
                    //获取接收缓冲区中数据的字节数 -- 
                    SerialPort sp = (SerialPort)serialPort;

                    //真实读取的字节..
                    readlength = sp.Read(_buffer, allreadlength, 800);
                    allreadlength += readlength;

                   // if (allreadlength >= 800) { Array.Clear(_buffer, 0, 1000); allreadlength = 0; readlength = 0; return; }

                    //返回指令...
                    if (currentcomandnumber != 6)
                    {
                        OnCallbackevent(_buffer, allreadlength);
                        currentcomandnumber = 0;
                        Array.Clear(_buffer, 0, 1000);
                        allreadlength = 0;
                        readlength = 0;
                        return;
                    }

                    //指令是6..
                    if (allreadlength < 0x33) 
                        return;
                    OnCallbackevent(_buffer, allreadlength);
                    currentcomandnumber = 0;
                    Array.Clear(_buffer, 0, 1000);
                    allreadlength = 0;
                    readlength = 0;
                }
            }
            catch (Exception ex)
            {
                //异常就看日志...
                string message = ex.Message + ex.StackTrace + allreadlength.ToString() + readlength.ToString();
                logger.Error(message);
            }
        }




    }
}
