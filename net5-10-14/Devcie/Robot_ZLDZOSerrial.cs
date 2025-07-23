using S7.Net;
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
    public class Robot_ZLDZOSerrial
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Task rotobTaskT = null;

        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //定义委托
        public delegate void RotobCallBack(object sender, Page2_RotobCallBackArg e);
        //声明事件
        public event RotobCallBack RotobCallBackEvent;

        private AutoResetEvent autoEvent = new AutoResetEvent(false);

        private ZLDZOSerrial m_ZLDZOSerrial_DY = null;
        private ZLDZOSerrial m_ZLDZOSerrial_GY = null;

        public PLCSM200 plc200 { get; set; } = null;

        public bool isWork { get; set; }

        private System.Timers.Timer tmr;
        private Queue<RotbotPushArg> queue = new Queue<RotbotPushArg>();

        //我的回调..(继续向上通知UI)
        protected virtual void OnCallbackevent(int currcommand, RotobErrorCode currerrorcode, string currentmessage,
                                               float EAB = 0, float EBC = 0, float ECA = 0)
        {

            //这里要玩一下，就是 串口先回这里，然后 我在打包去解析，解析串口后，在来去回调的...
            Page2_RotobCallBackArg arg = new Page2_RotobCallBackArg();
            arg.currCommand = currcommand;
            arg.currErrorcode = currerrorcode;
            arg.currMessage = currentmessage;
            arg.EAB = EAB;
            arg.EBC = EBC;
            arg.ECA = ECA;

            //保存了..
            RotobCallBack handler = RotobCallBackEvent;
            handler?.Invoke(this, arg);

            if (currerrorcode == RotobErrorCode.Ready) return;
            if (currerrorcode != RotobErrorCode.Success)
            {
                //如果一条指令没有成功 .
                queue.Clear(); 
            }

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

        public void RobotClose()
        {
            //不管怎么样先关闭端口 ...
            if (m_ZLDZOSerrial_GY != null)
                m_ZLDZOSerrial_GY.ClosePort();

            if (m_ZLDZOSerrial_DY != null)
                m_ZLDZOSerrial_DY.ClosePort();

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
        void SerialPortCallBackEvent_GY(object sender, PortCallBackArg e)
        {
            if (e.currentcommand == 0) return;  //可能第2次发送数据....
            autoEvent.Set();//给信号.. 回来了..
           
            //这里不能亚入了，不能让机器人去解析，要不然，自动执行，无法搞...
            switch (e.currentcommand)
            {
                case 1: ParsingData1_GY(e.data, e.length); break;
                case 2: ParsingData2_GY(e.data, e.length); break;
                case 3: ParsingData3_GY(e.data, e.length); break;
                case 4: ParsingData4_GY(e.data, e.length); break;
                case 5: ParsingData5_GY(e.data, e.length); break;
            }
        }

        void SerialPortCallBackEvent_DY(object sender, PortCallBackArg e)
        {
            if (e.currentcommand == 0) return;  //可能第2次发送数据....
            autoEvent.Set();//给信号.. 回来了..
            
            switch (e.currentcommand)
            {
                case 1: ParsingData1_DY(e.data, e.length); break;
                case 2: ParsingData2_DY(e.data, e.length); break;
                case 3: ParsingData3_DY(e.data, e.length); break;
                case 4: ParsingData4_DY(e.data, e.length); break;
                case 5: ParsingData5_DY(e.data, e.length); break;
            }
        }

        public (string, bool) RobotOpen_D(string comport)
        {
            var item = m_ZLDZOSerrial_DY.OpenPort(comport);
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

        public (string, bool) RobotOpen_G(string comport)
        {
            var item = m_ZLDZOSerrial_GY.OpenPort(comport);
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

        string CommandToChinese(int comm)
        {
            string str;
            switch (comm)
            {

                case 1: str = "PLC-AB"; break;
                case 2: str = "PLC-BC"; break;
                case 3: str = "PLC-CA"; break;
                case 4: str = "PLC-ab"; break;
                case 5: str = "PLC-bc"; break;
                case 6: str = "PLC-ca"; break;
                case 7: str = "PLC-an"; break;
                case 8: str = "PLC-bn"; break;
                case 9: str = "PLC-cn"; break;
                case 10: str = "PLC-变比"; break;
                case 500: str = "PLC-连接"; break;

                case 201: str = "低压-握手"; break;
                case 202: str = "低压-启动"; break;
                case 203: str = "低压-停止"; break;
                case 204: str = "低压-读取"; break;

                case 301: str = "低压-分析握手"; break;
                case 302: str = "低压-分析启动"; break;
                case 303: str = "低压-分析停止"; break;
                case 304: str = "低压-分析读取"; break;

                case 231: str = "高压-握手"; break;
                case 232: str = "高压-启动"; break;
                case 233: str = "高压-停止"; break;
                case 234: str = "高压-读取"; break;

                case 331: str = "高压-分析握手"; break;
                case 332: str = "高压-分析启动"; break;
                case 333: str = "高压-分析停止"; break;
                case 334: str = "高压-分析读取"; break;
                case 3000: str = "机器人延时"; break;
                case 501: str = "保存加计算"; break;
                default: str = "未知"; break;
            }

            return str;

        }

        public void Start()
        {

            m_ZLDZOSerrial_DY = new ZLDZOSerrial();
            m_ZLDZOSerrial_DY.SerialPortCallBackEvent += SerialPortCallBackEvent_DY;

            m_ZLDZOSerrial_GY = new ZLDZOSerrial();
            m_ZLDZOSerrial_GY.SerialPortCallBackEvent += SerialPortCallBackEvent_GY;

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

        void PLCCallBackEvent(object sender, PLCCallBackArg e)
        {
            switch (e.result)
            {
                case 1: OnCallbackevent(500, RotobErrorCode.TimerOver, "PLC连接失败!"); break;
                case 2: OnCallbackevent(500, RotobErrorCode.Success, "PLC连接成功"); break;
                case 3: OnCallbackevent(500, RotobErrorCode.Error, "PLC异常"); break;
            }
        }

        void ExcCmmand(int command, string currentcommandarg, byte[] data, int length)
        {
            if (command != 500)
            {
                if (plc200 == null)
                {
                    OnCallbackevent(500, RotobErrorCode.Error, "PLC必须连接");
                    return;
                }
            }

            switch (command)
            {

                case 1: { if (plc200.Write(0)) { OnCallbackevent(1, RotobErrorCode.Success, "PLC完成-AB"); } else { OnCallbackevent(1, RotobErrorCode.Error, "PLC失败-AB"); } } break;
                case 2: { if (plc200.Write(1)) { OnCallbackevent(2, RotobErrorCode.Success, "PLC完成-BC"); } else { OnCallbackevent(2, RotobErrorCode.Error, "PLC失败-BC"); } } break;
                case 3: { if (plc200.Write(2)) { OnCallbackevent(3, RotobErrorCode.Success, "PLC完成-CA"); } else { OnCallbackevent(3, RotobErrorCode.Error, "PLC失败-CA"); } } break;
                case 4: { if (plc200.Write(3)) { OnCallbackevent(4, RotobErrorCode.Success, "PLC完成-ab"); } else { OnCallbackevent(4, RotobErrorCode.Error, "PLC失败-ab"); } } break;
                case 5: { if (plc200.Write(4)) { OnCallbackevent(5, RotobErrorCode.Success, "PLC完成-bc"); } else { OnCallbackevent(5, RotobErrorCode.Error, "PLC失败-bc"); } } break;
                case 6: { if (plc200.Write(5)) { OnCallbackevent(6, RotobErrorCode.Success, "PLC完成-ca"); } else { OnCallbackevent(6, RotobErrorCode.Error, "PLC失败-ca"); } } break;
                case 7: { if (plc200.Write(6)) { OnCallbackevent(7, RotobErrorCode.Success, "PLC完成-an"); } else { OnCallbackevent(7, RotobErrorCode.Error, "PLC失败-an"); } } break;
                case 8: { if (plc200.Write(7)) { OnCallbackevent(8, RotobErrorCode.Success, "PLC完成-bn"); } else { OnCallbackevent(8, RotobErrorCode.Error, "PLC失败-bn"); } } break;
                case 9: { if (plc200.Write(8)) { OnCallbackevent(9, RotobErrorCode.Success, "PLC完成-cn"); } else { OnCallbackevent(9, RotobErrorCode.Error, "PLC失败-cn"); } } break;
                case 10: { if (plc200.Write(9)) { OnCallbackevent(10, RotobErrorCode.Success, "PLC完成-变比"); } else { OnCallbackevent(10, RotobErrorCode.Error, "PLC失败-变比"); } } break;

                case 500:
                    {
                        string strip = ConfigurationManager.AppSettings["DEVICE_1"];
                        //WriteList("PLC连接"+strip);
                        plc200 = new PLCSM200(strip);
                        plc200.PLCCallBackEvent += PLCCallBackEvent;
                        //Task<bool> task = plc200.Connection();
                        plc200.Connection();
                    }
                    break;

                case 201: m_ZLDZOSerrial_DY.ExeCommand(1, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 202: m_ZLDZOSerrial_DY.ExeCommand(2, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 203: m_ZLDZOSerrial_DY.ExeCommand(3, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 204: m_ZLDZOSerrial_DY.ExeCommand(4, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 205: m_ZLDZOSerrial_DY.ExeCommand(5, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;

                case 231: m_ZLDZOSerrial_GY.ExeCommand(1, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 232: m_ZLDZOSerrial_GY.ExeCommand(2, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 233: m_ZLDZOSerrial_GY.ExeCommand(3, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 234: m_ZLDZOSerrial_GY.ExeCommand(4, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                case 235: m_ZLDZOSerrial_GY.ExeCommand(5, currentcommandarg); WaitDevceReturn(command, "串口超时"); break;
                
                case 3000: { if (int.TryParse(currentcommandarg, out int result)) { Thread.Sleep(result); }break;}
                case 501: OnCallbackevent(501, RotobErrorCode.Success, "保存加计算"); break; 
            }
        }

        //解析数据
        void ParsingData1_GY(byte[] data, int length)
        {
            //握手
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x01, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(231, RotobErrorCode.ChartMateFail, "高压指令联机-设备返回值错误");
            }
            else
            {
                OnCallbackevent(231, RotobErrorCode.Success, "高压握手-完成");
            }

        }

        void ParsingData2_GY(byte[] data, int length)
        {
            //启动
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x05, 0x00, 0x05 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(232, RotobErrorCode.ChartMateFail, "高压指令联机-设备返回值错误");
            }
            else
            {
                OnCallbackevent(232, RotobErrorCode.Success, "高压启动-完成");
            }
        }

        void ParsingData3_GY(byte[] data, int length)
        {
            //停止
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x0A };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(233, RotobErrorCode.ChartMateFail, "高压指令停止-设备返回值错误");
            }
            else
            {
                OnCallbackevent(233, RotobErrorCode.Success, "高压停止-完成");
            }
        }

        void ParsingData4_GY(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x03, 0x1F };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(234, RotobErrorCode.ChartMateFail, "高压读取数据-设备返回错误!");
            }
            else
            {
                var result = GetValues(data);
                float EAB = result.Item1;//EAB误差 -- 9.75(A)
                float EBC = result.Item2;//EBC误差 -- 0.556(o)
                float ECA = result.Item3; //ECA误差 -- 0.664(o)
                //EAB *= 1000; //这里需要X1000 -- K偶...
                //EBC *= 1000;
                //ECA *= 1000;               
                //ECA = ECA/1000;
                //if (ECA == 0)
                //{
                //这里有问题..
                //throw new Exception("数据读取成功-值为0...");
                //}
                OnCallbackevent(234, RotobErrorCode.Success, "高压数据读取成功", EAB, EBC, ECA);
            }
        }

        void ParsingData5_GY(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x06, 0x00, 0x04 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(235, RotobErrorCode.ChartMateFail, "高压指令设置返回值错误");
            }
            else
            {
                OnCallbackevent(235, RotobErrorCode.Success, "高压指令设置-设备完成");
            }
        }



        void ParsingData1_DY(byte[] data, int length)
        {
            //握手
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x01, 0x4F, 0x4B };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(201, RotobErrorCode.ChartMateFail, "低压指令联机-设备返回值错误");
            }
            else
            {
                OnCallbackevent(201, RotobErrorCode.Success, "低压握手-完成");
            }

        }

        void ParsingData2_DY(byte[] data, int length)
        {
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x05, 0x00, 0x05 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(202, RotobErrorCode.ChartMateFail, "低压指令启动-设备返回值错误");
            }
            else
            {
                OnCallbackevent(202, RotobErrorCode.Success, "低压启动-完成");
            }
        }

        void ParsingData3_DY(byte[] data, int length)
        {
            //停止
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x0A };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(203, RotobErrorCode.ChartMateFail, "低压指令停止-设备返回值错误");
            }
            else
            {
                OnCallbackevent(203, RotobErrorCode.Success, "低压停止-完成");
            }
        }

        void ParsingData4_DY(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x03, 0x1F };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(204, RotobErrorCode.ChartMateFail, "低压读取数据-设备返回错误!");
            }
            else
            {
                var result = GetValues(data);
                float EAB = result.Item1;//EAB误差
                float EBC = result.Item2;//EBC误差
                float ECA = result.Item3; //ECA误差

                //if (ECA != 0) ECA = ECA; //返回为豪哦，变成O。。                
                                            //EAB /= 1000; //这里需要X1000 -- K偶...
                                            //EBC /= 1000;                
                                            //if (ECA == 0)
                                            //{
                                            //这里有问题..
                                            //throw new Exception("数据读取成功-值为0...");
                                            //}
                OnCallbackevent(204, RotobErrorCode.Success, "低压数据读取成功", EAB, EBC, ECA);
            }
        }

        void ParsingData5_DY(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0x38, 0x06, 0x00, 0x04 };
            if (PASerialSerial.FindSubArray(data, _buffsuccess) == -1)
            {
                OnCallbackevent(205, RotobErrorCode.ChartMateFail, "低压指令设置返回值错误");
            }
            else
            {
                OnCallbackevent(205, RotobErrorCode.Success, "低压指令设置-设备完成");
            }
        }


        static public (float, float, float) GetValues(byte[] data)
        {
            byte[] _cbuffer = new byte[4];
            float floatValue1;
            float floatValue2;
            float floatValue3;

            Buffer.BlockCopy(data, 20, _cbuffer, 0, 4);
            //数值翻转得到大端模式...
            Array.Reverse(_cbuffer);
            floatValue1 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 24, _cbuffer, 0, 4);
            //数值翻转得到大端模式...
            Array.Reverse(_cbuffer);
            floatValue2 = BitConverter.ToSingle(_cbuffer, 0);

            Buffer.BlockCopy(data, 28, _cbuffer, 0, 4);
            //数值翻转得到大端模式...
            Array.Reverse(_cbuffer);
            floatValue3 = BitConverter.ToSingle(_cbuffer, 0);


            return (floatValue1, floatValue2, floatValue3);
        }
    }
}

