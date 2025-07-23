using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using net5_10_14.Base;

namespace net5_10_14.Devcie
{

    public class Robot_PASerialSerial
    {
        private CancellationTokenSource cts = null;
        private Task rotobTaskT = null;

        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //定义委托
        public delegate void RotobCallBack(object sender, Page3_RotobCallBackArg e);
        //声明事件
        public event RotobCallBack RotobCallBackEvent;

        private AutoResetEvent autoEvent = new AutoResetEvent(false);
        private PASerialSerial m_PASerialSerial = null;

        public bool isWork { get; set; }

        private System.Timers.Timer tmr;
        private Queue<RotbotPushArg> queue = new Queue<RotbotPushArg>();


        //我的回调..(继续向上通知UI)
        protected virtual void OnCallbackevent(int currcommand, RotobErrorCode currerrorcode, string currentmessage, float [] dataf = null) 
        {
            //这里要玩一下，就是 串口先回这里，然后 我在打包去解析，解析串口后，在来去回调的...
            Page3_RotobCallBackArg arg = new Page3_RotobCallBackArg();
            arg.currCommand = currcommand;
            arg.currMessage = currentmessage;
            arg.data = dataf;

            //保存了..
            RotobCallBack handler = RotobCallBackEvent;
            handler?.Invoke(this, arg);
        }

        public bool Push(RotbotPushArg comm)
        {
            if (isWork == false)
            {
                queue.Enqueue(comm);
                return true;
            }            
            return false;
        }

        public (string,bool) RobotInit(string comport)
        {
            if (cts != null)
            {
                return ("cts已经有值", false);
            }
            if  (rotobTaskT != null)
            {
                return ("rotobTask已经有值", false);
            }
            m_PASerialSerial = new PASerialSerial();
            m_PASerialSerial.SerialPortCallBackEvent += SerialPortCallBackEvent;
            var item = m_PASerialSerial.OpenPort(comport);
            if (item.Item2 == false)
            {
                return (item.Item1,false);
            }

            /*
             * 内部时钟 先关禁闭..
            tmr = new System.Timers.Timer();    // 无需任何参数
            tmr.Interval = 1000;
            tmr.Elapsed += tmr_Elapsed;  // 使用事件代替委托
            */

            //重置
            cts = new CancellationTokenSource();
            rotobTaskT = null;
            return ("串口打开成功",true);
        }
            
        public void RobotClose()
        {
            //不管怎么样先关闭端口 ...
            if (m_PASerialSerial != null)
                m_PASerialSerial.ClosePort();

            if (rotobTaskT  != null)
            {
                if (rotobTaskT.Status == TaskStatus.Running ||
                    rotobTaskT.Status == TaskStatus.WaitingForActivation)
                {
                    cts.Cancel();
                    while (rotobTaskT.Status != TaskStatus.RanToCompletion)
                        Thread.Sleep(1000);
                    cts = null;
                    rotobTaskT = null;
                }
            }

        }

        void tmr_Elapsed(object sender, EventArgs e)
        {
            //发送读值
            RotbotPushArg listarg1 = new RotbotPushArg();
            listarg1.command = 5;
            Push(listarg1);
        }

        //串口回调..
        void SerialPortCallBackEvent(object sender, PortCallBackArg e)
        {
            autoEvent.Set();//给信号.. 回来了..
            RotbotPushArg comm = new RotbotPushArg();
            comm.command = e.currentcommand;
            comm.data = new byte[e.length];
            Buffer.BlockCopy(e.data, 0, comm.data, 0, e.length);
            comm.length = e.length;
            queue.Enqueue(comm);
        }


        public void Start()
        {           
            try
            {
                rotobTaskT = Task.Run(async () =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        //如果没有取消，我就去做循环..
                        await Task.Delay(1);                        
                        isWork = false;
                        if (queue.Count <= 0) continue;
                        isWork = true;

                        RotbotPushArg Command = queue.Dequeue();
                        ExcCmmand(Command.command, Command.reserve,Command.data,Command.length);
                    }
                }, cts.Token);
            }
            catch (Exception ex)
            {
                //关闭的时候，肯定会异常...
                string message = ex.Message + ex.StackTrace;
                logger.Error(message);
            }
        }

        private bool WaitDevceReturn(int command,string message)
        {
            bool signaled = autoEvent.WaitOne(3000); // 等待3秒
            if (signaled)
                return true;
            else
            {
                OnCallbackevent(command, RotobErrorCode.TimerOver, message);
                return false;
            }
        }

        void ExcCmmand(int command, string currentcommandarg,byte[]data,int length)
        {
            switch (command)
            {
                case 1: m_PASerialSerial.ExeCommand(1, currentcommandarg); WaitDevceReturn(command,"串口超时") ; break;
                case 2: m_PASerialSerial.ExeCommand(2, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 3: m_PASerialSerial.ExeCommand(3, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 4: 
                    {
                        m_PASerialSerial.ExeCommand(4, currentcommandarg);
                        if (WaitDevceReturn(command, "串口超时"))
                        {
                            //没有超时... -- 这里启动辅助计时器.. 1秒推送一个指令..
                            tmr.Start(); //开始计算器工作..
                        }
                        break;
                    }
                case 5:
                    {
                        m_PASerialSerial.ExeCommand(5, currentcommandarg);
                        if (!WaitDevceReturn(command, "串口超时"))
                        {
                            //超时了...
                            tmr.Stop();                            
                        }
                        break;
                    }
                case 6:
                    {
                        m_PASerialSerial.ExeCommand(6, currentcommandarg);
                        WaitDevceReturn(command, "串口超时");
                        //不管超时不超时，都亭子...
                        tmr.Stop();
                        break;
                    }
                case 101: ParsingData1(data,length);  break;
                case 102: ParsingData2(data,length); break;
                case 103: ParsingData3(data,length); break;
                case 104: ParsingData4(data,length); break;
                case 105: ParsingData5(data,length); break;
                case 106: ParsingData6(data,length); break;
            }
        }

        //解析数据
        private void ParsingData1(byte[] data, int length)
        {
            //这里返回的指令需要比较..

            byte[] _buffsuccess = { 0x52, 0x53, 0x03, 0x01, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)            
                OnCallbackevent(101, RotobErrorCode.ChartMateFail, "指令联机-设备返回值错误"); 
            else 
                OnCallbackevent(101, RotobErrorCode.Success, "指令联机-设备正常");
        }

        void ParsingData2(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0x52, 0x53, 0x03, 0x01, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
                OnCallbackevent(102, RotobErrorCode.ChartMateFail, "指令联机-设备返回值错误");
        }

        void ParsingData3(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0x52, 0x53, 0x03, 0x01, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(102, RotobErrorCode.ChartMateFail, "指令联机-设备返回值错误");
            }
        }

        void ParsingData4(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0x52, 0x53, 0x03, 0x13, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(103, RotobErrorCode.ChartMateFail, "指令启动-设备返回值错误");
            }
        }



        void ParsingData5(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0x52, 0x53, 0x03, 0x11, 0x60 };
            int pos = PASerialSerial.FindSubArray(data, _buffsuccess);
            if (pos == -1)
            {
                OnCallbackevent(104, RotobErrorCode.ChartMateFail, "指令读取-没有找到针头");
                return;
            }

            byte[] _buffer = new byte[96];
            Buffer.BlockCopy(data, pos + 6, _buffer, 0, 96);            

            byte[] bdata = new byte[4];
            float floatValue;
            float[] floatb = new float[24];
            for (int i = 0; i < 24; i++)
            {
                Buffer.BlockCopy(_buffer, i * 4, bdata, 0, 4);
                floatValue = BitConverter.ToSingle(bdata, 0);
                
                floatb[i] = floatValue;
            }
            OnCallbackevent(105, RotobErrorCode.ChartMateFail, "读取完成", floatb);
        }

        void ParsingData6(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0x52, 0x53, 0x03, 0x0d, 0x4F, 0x4B, 0xAA };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(106, RotobErrorCode.ChartMateFail, "指令结束-设备返回值错误");
            }
        }

    }
}
