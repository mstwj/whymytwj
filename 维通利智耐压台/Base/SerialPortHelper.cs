using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading;
using System.Timers;

namespace 维通利智耐压台.Base
{
    public class SerialPortHelper
    {
        private SerialPort _serialPort;

        //public System.Timers.Timer timer { get; set; } = new System.Timers.Timer(1000); // 设置间隔时间为1000毫秒（1秒）
        
        public SerialPortHelper(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
           // timer.Elapsed += OnTimedEvent; // 注册事件处理程序
           // timer.AutoReset = true; // 设置定时器为自动重置模式（如果需要只执行一次，则设置为false）
           // timer.Enabled = false; // 不启动定时器
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void Write(string data)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(data);
            }
        }

       /* public int intSValue1 { get; set; } = 27000;
       // public int intSValue2 { get; set; } = 2560;
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            //A5 5A 00 00 01 11 01 13 A9 00 00 00 38 FA 0D 0A
            
            MyMessage myMessage = new MyMessage();
            myMessage.Message = "UpdataJuFan";
            myMessage.obj1 = intSValue1; //局放.
            myMessage.obj2 = intSValue2; //电压.
            WeakReferenceMessenger.Default.Send(myMessage);
        }
       */

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            byte[] _buffer = new byte[1000];
            //真实读取的字节..
            int readlength = sp.Read(_buffer, 0, 1000);
            //int readlength = 16;
            //byte[] _buffer = new byte[16] {
            //   0xA5, 0x5A, 0x00, 0x00, 0x01, 0x11, 0x01, 0x13,
            //   0xA9, 0x00, 0x00, 0x00, 0x38, 0xFA, 0x0D, 0x0A,
            //    };
            float fValue1 = 0.0f, fValue2 = 0.0f;
            if (readlength == 16)
            {
                int intValue1= 0, intValue2 = 0;
                if (_buffer[0] == 0xA5 && _buffer[1] == 0x5A)
                {
                    byte[] byteArray1 = { _buffer[5], _buffer[4], _buffer[3], _buffer[2] };
                    intValue1 = BitConverter.ToInt32(byteArray1, 0);
                    fValue1 = intValue1;
                    fValue1 = fValue1 / 100;

                   

                    if (_buffer[14] == 0x0D && _buffer[15] == 0x0A)
                    {
                        byte[] byteArray2 = { _buffer[12], _buffer[11], _buffer[10], _buffer[9] };
                        intValue2 = BitConverter.ToInt32(byteArray2, 0);
                        fValue2 = intValue2;
                        fValue2 = fValue2 / 10;
                    }
                }
                //A5 5A 00 00 01 11 01 13 A9 00 00 00 38 FA 0D 0A
                MyMessage myMessage = new MyMessage();
                myMessage.Message = "UpdataJuFan";
                myMessage.fobj1 = fValue1; //局放.
                myMessage.fobj2 = fValue2; //电压.
                WeakReferenceMessenger.Default.Send(myMessage);

            }
        }
    }
}
