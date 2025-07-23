using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net5_10_14.Base;

namespace net5_10_14.Devcie
{

    //不同的设备，就是不能类去继承处理...
    class PASerialSerial:MySerialPort
    {
        private int allreadlength = 0;

        private byte[] _buffer = new byte[1000];

        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //定义委托
        public delegate void SerialPortCallBack(object sender, PortCallBackArg e);

        //声明事件
        public event SerialPortCallBack SerialPortCallBackEvent;


        protected virtual void OnCallbackevent(byte[] data,int length)
        {
            PortCallBackArg arg = new PortCallBackArg();
            arg.currentcommand = currentcomandnumber;
            //所有的指令+100 就是处理...
            arg.currentcommand += 100;
            arg.data = data;
            arg.length = length;
            //保存了..
            SerialPortCallBack handler = SerialPortCallBackEvent;
            handler?.Invoke(this, arg);
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
                        //联机进入通讯模式.
                        currentcommand = "52-53-03-01-00-00-04-00-45-44";
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
                case 2:
                    {
                    }
                    break;
                case 3:
                    {
                        //开始试验,开始后不停推送.
                        currentcommand = "52-53-03-03-02-00-00-00-08-00-45-44";
                        if (cmmandarg == "0")
                        {
                            currentcommand = "52-53-03-03-02-00-00-00-08-00-45-44";
                        }
                        if (cmmandarg == "1")
                        {
                            currentcommand = "52-53-03-03-02-00-01-00-09-00-45-44";
                        }
                        currentcomandnumber = 3;
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
                case 4:
                    {
                        //开始试验,开始后需要发送读结果命令下位机才上传结果
                        currentcommand = "52-53-03-13-02-00-00-00-18-00-45-44";
                        if (cmmandarg == "0")
                        {
                            currentcommand = "52-53-03-13-02-00-00-00-18-00-45-44";
                        }
                        if (cmmandarg == "1")
                        {
                            currentcommand = "52-53-03-13-02-00-01-00-19-00-45-44";
                        }
                        currentcomandnumber = 4;
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;

                case 5:
                    {
                        //读取测试结果
                        currentcommand = "52-53-03-04-00-00-07-00-45-44";
                        currentcomandnumber = 5;
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
                        serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
                    }
                    break;
                case 6:
                    {
                        //结束
                        currentcommand = "52-53-03-0D-00-00-10-00-45-44";
                        currentcomandnumber = 6;
                        currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
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

                    if (allreadlength >= 800) { Array.Clear(_buffer, 0, 1000); allreadlength = 0; readlength = 0; return; }

                    if (currentcomandnumber == 1)
                    {
                        OnCallbackevent(_buffer, allreadlength);
                    }

                    if (currentcomandnumber == 4)
                    {
                        OnCallbackevent(_buffer, allreadlength);
                    }

                    if (currentcomandnumber == 6)
                    {
                        OnCallbackevent(_buffer, allreadlength);
                    }

                    if (currentcomandnumber == 5)
                    {
                        if (allreadlength < 0x6A) return;
                        OnCallbackevent(_buffer, allreadlength);
                    }

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
