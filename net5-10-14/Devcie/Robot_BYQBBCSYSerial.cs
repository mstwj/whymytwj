using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using net5_10_14.Base;

namespace net5_10_14.Devcie
{
    class Robot_BYQBBCSYSerial
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
            
        private Task rotobTaskT = null;
        
        public PLCSM200 plc200 { get; set; } = null;

        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //定义委托
        public delegate void RotobCallBack(object sender, Page1_RotobCallBackArg e);
        //声明事件
        public event RotobCallBack RotobCallBackEvent;

        private AutoResetEvent autoEvent = new AutoResetEvent(false);

        private BYQBBCSYSerial m_BYQBBCSYSerial = null;        

        public bool isWork { get; set; }

        private System.Timers.Timer tmr;
        private Queue<RotbotPushArg> queue = new Queue<RotbotPushArg>();

        //我的回调..(继续向上通知机器人)
        protected virtual void OnCallbackevent(int currcommand, RotobErrorCode currerrorcode, string currentmessage,
                                               string datatimer = "0", int datazbfs = 0, int datadqfj = 0,
                                               int datajx = 0, float dataelbb = 0, float datafjjk = 0,
                                               float dataKAB = 0, float dataKBC = 0, float dataKCA = 0,
                                               float dataEAB = 0, float dataEBC = 0, float dataECA = 0,
                                               int dataclfs = 0)

        {

            //这里要玩一下，就是 串口先回这里，然后 我在打包去解析，解析串口后，在来去回调的...
            Page1_RotobCallBackArg arg = new Page1_RotobCallBackArg();
            arg.currCommand = currcommand;
            arg.currErrorcode = currerrorcode;
            arg.currMessage = currentmessage;
            arg.datatimer = datatimer;
            arg.datazbfs = datazbfs;
            arg.datadqfj = datadqfj;
            arg.datajx = datajx;
            arg.dataelbb = dataelbb;
            arg.datafjjk = datafjjk;
            arg.dataKAB = dataKAB;
            arg.dataKBC = dataKBC;
            arg.dataKCA = dataKCA;
            arg.dataEAB = dataEAB;
            arg.dataEBC = dataEBC;
            arg.dataECA = dataECA;
            arg.dataclfs = dataclfs;

            if (arg.currErrorcode == RotobErrorCode.ChartMateFail)
            {
                //端口得到数据,,可是部队 ..
                queue.Clear();
            }

            if (arg.currErrorcode == RotobErrorCode.TimerOver)
            {
                //端口草食了.....
                queue.Clear();
            }

            if (arg.currErrorcode == RotobErrorCode.Error)
            {
                //端口
                queue.Clear();
            }

            //保存了..
            RotobCallBack handler = RotobCallBackEvent;
            handler?.Invoke(this, arg);
        }

        public bool Push(List<RotbotPushArg> list)
        {
            if (isWork == false)
            {
                foreach (RotbotPushArg item in list)
                {
                    queue.Enqueue(item);
                }
                return true;
            }

            return false;
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

        public (string, bool) RobotOpen(string comport)
        {
            var item = m_BYQBBCSYSerial.OpenPort(comport);
            if (item.Item2 == false)
            {
                return (item.Item1, false);
            }

            /*
             * 内部时钟 先关禁闭..
            tmr = new System.Timers.Timer();    // 无需任何参数
            tmr.Interval = 1000;
            tmr.Elapsed += tmr_Elapsed;  // 使用事件代替委托
            */

            //重置
            return ("串口打开成功", true);
        }

        public void RobotClose()
        {
            //不管怎么样先关闭端口 ...
            if (m_BYQBBCSYSerial != null)
                m_BYQBBCSYSerial.ClosePort();

            /*
            if (rotobTaskT != null)
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
            */

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
            //这里必须写上面，信号只能设置1次.....
            if (e.currentcommand == 0) return;

            autoEvent.Set();//给信号.. 回来了..
            
            //这里不能亚入了，不能让机器人去解析，要不然，自动执行，无法搞...
            switch (e.currentcommand)
            {
                case 1: ParsingData1(e.data, e.length); break;
                case 2: ParsingData2(e.data, e.length); break;
                case 3: ParsingData3(e.data, e.length); break;
                case 4: ParsingData4(e.data, e.length); break;
                case 5: ParsingData5(e.data, e.length); break;
                case 6: ParsingData6(e.data, e.length); break;
            }
        }

        string CommandToChinese(int comm)
        {
            string str;
            switch (comm)
            {

                case 1: str = "握手"; break;
                case 2: str = "启动"; break;
                case 3: str = "停止"; break;
                case 4: str = "设置"; break;
                case 5: str = "未知55"; break;
                case 6: str = "读取"; break;
                case 7: str = "写入数据库"; break;
                case 10: str = "PLC"; break;
                case 3000: str = "机器人等待"; break;
                default: str = "未知"; break;
            }

            return str;

        }


        public void Start()
        {
            m_BYQBBCSYSerial = new BYQBBCSYSerial();
            m_BYQBBCSYSerial.SerialPortCallBackEvent += SerialPortCallBackEvent;

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
                        OnCallbackevent(0, RotobErrorCode.Ready, "指令" + CommandToChinese(Command.command) + "开始执行");
                        ExcCmmand(Command.command, Command.reserve, Command.data, Command.length);
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

        private bool WaitDevceReturn(int command, string message)
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

        void ExcCmmand(int command, string currentcommandarg, byte[] data, int length)
        {

            /*
            if (!m_BYQBBCSYSerial.GetPortStatus())
            {
                OnCallbackevent(command, RotobErrorCode.Error, "端口没打开");
                return;
            }
            */

            switch (command)
            {
                case 1: m_BYQBBCSYSerial.ExeCommand(1, currentcommandarg); WaitDevceReturn(command, "串口超时");break;
                case 2: m_BYQBBCSYSerial.ExeCommand(2, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 3: m_BYQBBCSYSerial.ExeCommand(3, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 4: m_BYQBBCSYSerial.ExeCommand(4, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 5: m_BYQBBCSYSerial.ExeCommand(5, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 6: 
                    m_BYQBBCSYSerial.ExeCommand(6, currentcommandarg);
                    WaitDevceReturn(command, "串口超时"); 
                    break;
                case 7:
                    {
                        OnCallbackevent(7, RotobErrorCode.Success, "保存数据到数据库");
                    }
                    break;

                case 10: { if (plc200.Write(9)) { OnCallbackevent(10, RotobErrorCode.Success, "PLC完成-变比"); } else { OnCallbackevent(10, RotobErrorCode.Error, "PLC失败-变比"); } } break;


                case 500:
                    {
                        string strip = ConfigurationManager.AppSettings["DEVICE_1"];
                        plc200 = new PLCSM200(strip);
                        plc200.PLCCallBackEvent += PLCCallBackEvent; 
                        plc200.Connection();
                    }
                    break;
                case 3000: { if (int.TryParse(currentcommandarg, out int result)) { Thread.Sleep(result); } break; }

            }
        }

        void PLCCallBackEvent(object sender, PLCCallBackArg e)
        {
            switch (e.result)
            {
                case 1: OnCallbackevent(500, RotobErrorCode.TimerOver, "PLC连接失败!"); break;
                case 2: OnCallbackevent(500, RotobErrorCode.Success, "PLC连接成功"); break;
                case 3: OnCallbackevent(500, RotobErrorCode.Error, "PLC异常"); break;
            }
        }


        //解析数据
        private void ParsingData1(byte[] data, int length)
        {
            //握手

            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x01, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(1, RotobErrorCode.ChartMateFail, "<===握手-设备返回值错误");
            }
            else
            {
                OnCallbackevent(1, RotobErrorCode.Success, "<===握手-完成");
            }

        }

        void ParsingData2(byte[] data, int length)
        {
            //启动
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x05, 0x00, 0x38 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(2, RotobErrorCode.ChartMateFail, "<===启动-设备返回值错误");
            }
            else
            {
                OnCallbackevent(2, RotobErrorCode.Success, "<===启动-完成");
            }
        }

        void ParsingData3(byte[] data, int length)
        {
            //停止
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x08 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(3, RotobErrorCode.ChartMateFail, "<===停止-设备返回值错误");
            }
            else
            {
                OnCallbackevent(3, RotobErrorCode.Success, "<===停止-完成");
            }
        }

        void ParsingData4(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            OnCallbackevent(4, RotobErrorCode.Success, "<===设备数据4返回!");
        }

        void ParsingData5(byte[] data, int length)
        {
        }

        void ParsingData6(byte[] data, int length)
        {
            //这里返回的指令需要比较..
                                    
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x03, 0x34 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(6, RotobErrorCode.ChartMateFail, "<===数据读取指令返回值错误");
                return;
            }
            //成功..
            //如果这里报错，就报错吧...(这里很有可能不是buffer数据错误..)
            var result = GetValues(data);
            //上报事件 -- 设备设置成功
            //如果是自动模式，就不要这样上报了..                           
            //orgdata data = new orgdata();
            //data.showmode = 0;
            string datatimer = result.Item1; //时间.
            int datazbfs = result.Item2;  //组别方式.
            int datadqfj = result.Item3; //当前分接
            int datajx = result.Item4;   //及性 (0,1,其他)
            float dataelbb = result.Item5; //额定变比
            float datafjjk = result.Item6; //分接间距
            float dataKAB = result.Item7;  //KAB相比
            float dataKBC = result.Item8;//KBC相比
            float dataKCA = result.Item9;//KCA相比
            float dataEAB = result.Item10;//EAB误差
            float dataEBC = result.Item11;//EBC误差
            float dataECA = result.Item12; //ECA误差
                                            //int dataclfs = result.Item13; //测量方式(原来的值被弃用..)
            int dataclfs = result.Item14; //测量方式(这里这个值，测量方式..我不知道是干啥的，修改为 组别号)

            OnCallbackevent(6, RotobErrorCode.Success, "<===数据读取完成", datatimer, datazbfs, datadqfj, datajx, dataelbb,
                datafjjk, dataKAB, dataKBC, dataKCA, dataEAB, dataEBC, dataECA, dataclfs);

        }

        static public (string, int, int, int, float, float, float, float, float, float, float, float, int, int) GetValues(byte[] data)
        {
            //时间需要显示吗?
            int _buffchar1 = (int)data[6];
            int _buffchar2 = (int)data[7];
            int _buffchar3 = (int)data[8];
            int _buffchar4 = (int)data[9];
            int _buffchar5 = (int)data[10];
            int _buffchar6 = (int)data[11];
            int _buffchar7 = (int)data[12];

            string datetimer = _buffchar1.ToString() + _buffchar2.ToString() + "-" + _buffchar3.ToString() + "-" + _buffchar4.ToString() + "|" +
                _buffchar5.ToString() + ":" + _buffchar6.ToString() + ":" + _buffchar7.ToString();

            //组别方式 。。(bejdd)
            int intValue1 = -1;

            //这里还要判断一下高4位...
            byte highNibble = (byte)(data[13] >> 4); // 右移4位，然后转换成byte
            intValue1 = highNibble; //组别方式..

            //组别号.(临时.)
            int temp1 = (int)(data[13] & 0x0F); //高4位清0



            //当前分接..
            int intValue2 = (int)data[14];

            //当前及性..
            int intValue3 = (int)data[15];


            byte[] _cbuffer = new byte[4];


            Buffer.BlockCopy(data, 16, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue1 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 20, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue2 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 24, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue3 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 28, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue4 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 32, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue5 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 36, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue6 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 40, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue7 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 44, _cbuffer, 0, 4);
            Array.Reverse(_cbuffer);
            float floatValue8 = BitConverter.ToSingle(_cbuffer, 0);

            //当前及性..
            int intValue4 = (int)data[48];

            return (datetimer, intValue1, intValue2, intValue3,
                floatValue1, floatValue2, floatValue3, floatValue4,
                floatValue5, floatValue6, floatValue7, floatValue8,
                intValue4, temp1);
        }


    }
}
