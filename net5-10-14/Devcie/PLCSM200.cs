using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace net5_10_14.Devcie
{

    //底层PLC回调给机器人..
    public class PLCCallBackArg
    {
        public int result;        
    }

    public class PLCSM200
    {
        private int Reslut;

        //定义委托
        public delegate void PLCCallBack(object sender, PLCCallBackArg e);

        //声明事件
        public event PLCCallBack PLCCallBackEvent;

        public string Ip { get; set; } = null;
        private Plc plc;        

        public PLCSM200(string ip)
        {
            plc = new Plc(CpuType.S7200Smart, ip, 0, 1);
        }


        public async void Connection()
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var timeout = Task.Delay(1000);
            var t = Task.Run(() =>
            {
                try
                {
                    plc.Open();
                    Reslut = 2;
                }
                catch (Exception ex)
                {
                    Reslut = 3;
                }
            },timeoutCancellationTokenSource.Token);
            var completedTask = await Task.WhenAny(timeout, t);

            if (completedTask == timeout)
            {
                Reslut = 1;
                timeoutCancellationTokenSource.Cancel();
            }
            OnCallbackevent();
        }

        public void SetStart()
        {
            plc.Write("M1.5", false);
        }

        void SetStop()
        {
            plc.Write("M1.5", true);
        }

        public bool Write(int address)
        {
            bool value = false;
            switch (address)
            {
                case 0: plc.Write("M0.0", true); Thread.Sleep(200); plc.Write("M0.0", false); Thread.Sleep(200); value = (bool)plc.Read("I0.1");  break;
                case 1: plc.Write("M0.1", true); Thread.Sleep(200); plc.Write("M0.1", false); Thread.Sleep(200); value = (bool)plc.Read("I0.2"); break;
                case 2: plc.Write("M0.2", true); Thread.Sleep(200); plc.Write("M0.2", false); Thread.Sleep(200); value = (bool)plc.Read("I0.3"); break;
                case 3: plc.Write("M0.3", true); Thread.Sleep(200); plc.Write("M0.3", false); Thread.Sleep(200); value = (bool)plc.Read("I0.4"); break;
                case 4: plc.Write("M0.4", true); Thread.Sleep(200); plc.Write("M0.4", false); Thread.Sleep(200); value = (bool)plc.Read("I0.5"); break;
                case 5: plc.Write("M0.5", true); Thread.Sleep(200); plc.Write("M0.5", false); Thread.Sleep(200); value = (bool)plc.Read("I0.6"); break;
                case 6: plc.Write("M0.6", true); Thread.Sleep(200); plc.Write("M0.6", false); Thread.Sleep(200); value = (bool)plc.Read("I0.7"); break;
                case 7: plc.Write("M0.7", true); Thread.Sleep(200); plc.Write("M0.7", false); Thread.Sleep(200); value = (bool)plc.Read("I1.0"); break;
                case 8: plc.Write("M1.0", true); Thread.Sleep(200); plc.Write("M1.0", false); Thread.Sleep(200); value = (bool)plc.Read("I1.1"); break;
                case 9: plc.Write("M1.1", true); Thread.Sleep(200); plc.Write("M1.1", false); Thread.Sleep(200); value = (bool)plc.Read("I1.2");  break; //边币.

            }
            return value;
        }

        //这里回调给机器人..
        protected virtual void OnCallbackevent()
        {
            PLCCallBackArg arg = new PLCCallBackArg();
            arg.result = Reslut;
            PLCCallBack handler = PLCCallBackEvent;
            handler?.Invoke(this, arg);
        }
    }
}
