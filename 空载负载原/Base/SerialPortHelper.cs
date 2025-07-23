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
using 空载_负载.Model;
using System.Diagnostics;

namespace 空载_负载.Base
{
    public class SerialPortHelper
    {
        private SerialPort _serialPort;

        public int cuucentcommand { get; set; } = 0; //当前指令..
        
        public SerialPortHelper(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _serialPort.ReadBufferSize = 500; // 设置接收缓冲区大小--很主要，这里解决了2次发送和接收的问题..
            _serialPort.WriteBufferSize = 500; // 设置发送缓冲区大小--很主要，这里解决了2次发送和接收的问题..
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


        public void Write(byte[] buffer, int offset, int count)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(buffer,offset,count);
            }
        }

        private int readlength = 0;
        private byte[] _buffer = new byte[500];
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;

            //真实读取的字节..
            int length = sp.Read(_buffer, readlength, 500 - readlength);
            readlength = readlength + length;
            
            Debug.WriteLine(length.ToString() + "-----"+ readlength.ToString());

            if (cuucentcommand == 0)
            {
                readlength = 0;
                Array.Clear(_buffer, 0, 500);                
            }

            if (cuucentcommand == 1) //联机..
            {
                if (readlength >= 10)
                {
                    byte[] arraySource = new byte[readlength];
                    Array.Copy(_buffer, arraySource, readlength);
                    byte[] arrayConnect = { 0x52, 0x53, 0x03, 0x01, 0x4F, 0x4B, 0x9E, 0x00, 0x45, 0x44 }; //联机OK...

                    int lengthToCompare = 10; // 比较前6个字节
                    bool areEqual = arraySource.Take(lengthToCompare).SequenceEqual(arrayConnect.Take(lengthToCompare));
                    if (areEqual)
                    {
                        MessageInit myMessage = new MessageInit();
                        myMessage.Message = "PortMessage_Connect";
                        WeakReferenceMessenger.Default.Send(myMessage);
                        readlength = 0;
                        Array.Clear(_buffer, 0, 500);
                        cuucentcommand = 0;
                        return;

                    }
                }
            }

            if (cuucentcommand == 2) //空载测试..
            {
                if (readlength >= 10)
                {
                    byte[] arraySource = new byte[readlength];
                    Array.Copy(_buffer, arraySource, readlength);
                    byte[] arrayComeKongZai = { 0x52, 0x53, 0x03, 0x013, 0x4F, 0x4B, 0xB0, 0x00, 0x45, 0x44 }; //进入空载测试..
                    int lengthToCompare = 10; // 比较前10个字节
                    bool areEqual = arraySource.Take(lengthToCompare).SequenceEqual(arrayComeKongZai.Take(lengthToCompare));
                    if (areEqual)
                    {
                        MessageInit myMessage = new MessageInit();
                        myMessage.Message = "PortMessage_ComeTestState";
                        WeakReferenceMessenger.Default.Send(myMessage);
                        readlength = 0;
                        Array.Clear(_buffer, 0, 500);
                        cuucentcommand = 0;
                        return;

                    }
                }                
            }

            if (cuucentcommand == 3) //读取数据
            {
                if (readlength >= 0x6A)
                {
                    //肯定是返回的数据..
                    byte[] array1 = new byte[readlength];
                    byte[] array2 = { 0x52, 0x53, 0x03, 0x11, 0x60, 0x00 };
                    Array.Copy(_buffer, array1, readlength);

                    int lengthToCompare = 6; // 比较前6个字节
                    bool areEqual1 = array1.Take(lengthToCompare).SequenceEqual(array2.Take(lengthToCompare));
                    if (areEqual1 == true)
                    {
                        //跳过前6个字符..
                        byte[] newArray = array1.Skip(6).ToArray();

                        AnalysisData analysisData = new AnalysisData(newArray);

                        MessageInit myMessage = new MessageInit();
                        myMessage.Message = "PortMessage_Data";
                        myMessage.Data = analysisData;
                        WeakReferenceMessenger.Default.Send(myMessage);
                        readlength = 0;
                        Array.Clear(_buffer, 0, 500);
                        cuucentcommand = 0;
                        return;
                    }
                }
            }

            if (cuucentcommand == 4) //退出..
            {
                if (readlength >= 10)
                {                                        
                    byte[] arraySource = new byte[readlength];
                    Array.Copy(_buffer, arraySource, readlength);
                    byte[] arrayStop = { 0x52, 0x53, 0x03, 0x0D, 0x4F, 0x4b, 0xAA, 0x00, 0x45, 0x44 }; //停止..
                    int lengthToCompare = 10; // 比较前10个字节
                    bool areEqual = arraySource.Take(lengthToCompare).SequenceEqual(arrayStop.Take(lengthToCompare));
                    if (areEqual)
                    {
                        MessageInit myMessage = new MessageInit();
                        myMessage.Message = "PortMessage_Stop";
                        WeakReferenceMessenger.Default.Send(myMessage);
                        readlength = 0;
                        Array.Clear(_buffer, 0, 500);
                        cuucentcommand = 0;
                        return;

                    }
                }


                //bool areEqualConnect = ContainsBytes(arraySource, arrayConnect);
                //bool areEqualComeKongZai = ContainsBytes(arraySource, arrayComeKongZai);
                //bool areEqualStop = ContainsBytes(arraySource, arrayStop);
                //bool areEqualError = ContainsBytes(arraySource, arrayError);
            }

        }


        public static bool ContainsBytes(byte[] source, byte[] search)
        {
            if (source == null || search == null || search.Length == 0 || source.Length < search.Length)
                return false;

            for (int i = 0; i <= source.Length - search.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < search.Length; j++)
                {
                    if (source[i + j] != search[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return true;
            }
            return false;
        }

    }

    public class AnalysisData
    {

        public AnalysisData(byte[] source)
        {
            byte[] destination = new byte[4];
            Array.ConstrainedCopy(source, 0, destination, 0, 4);            
            fltVoltage[0] = BitConverter.ToSingle(destination, 0);
            fltVoltage[0] = (float)Math.Round(fltVoltage[0], 3); // 保留3位小数

            Array.ConstrainedCopy(source, 4, destination, 0, 4);            
            fltVoltage[1] = BitConverter.ToSingle(destination, 0);
            fltVoltage[1] = (float)Math.Round(fltVoltage[1], 3); // 保留3位小数

            Array.ConstrainedCopy(source, 8, destination, 0, 4);            
            fltVoltage[2] = BitConverter.ToSingle(destination, 0);
            fltVoltage[2] = (float)Math.Round(fltVoltage[2], 3); // 保留3位小数


            Array.ConstrainedCopy(source, 12, destination, 0, 4);            
            fltMeanVoltage[0] = BitConverter.ToSingle(destination, 0);
            fltMeanVoltage[0] = (float)Math.Round(fltMeanVoltage[0], 3); // 保留3位小数

            Array.ConstrainedCopy(source, 16, destination, 0, 4);            
            fltMeanVoltage[1] = BitConverter.ToSingle(destination, 0);
            fltMeanVoltage[1] = (float)Math.Round(fltMeanVoltage[1], 3); // 保留3位小数

            Array.ConstrainedCopy(source, 20, destination, 0, 4);
            fltMeanVoltage[2] = BitConverter.ToSingle(destination, 0);
            fltMeanVoltage[2] = (float)Math.Round(fltMeanVoltage[2], 3); // 保留3位小数


            Array.ConstrainedCopy(source, 24, destination, 0, 4);            
            fltCurrent[0] = BitConverter.ToSingle(destination, 0);
            fltCurrent[0] = (float)Math.Round(fltCurrent[0], 3); // 保留3位小数

            Array.ConstrainedCopy(source, 28, destination, 0, 4);            
            fltCurrent[1] = BitConverter.ToSingle(destination, 0);
            fltCurrent[1] = (float)Math.Round(fltCurrent[1], 3); // 保留3位小数

            Array.ConstrainedCopy(source, 32, destination, 0, 4);            
            fltCurrent[2] = BitConverter.ToSingle(destination, 0);
            fltCurrent[2] = (float)Math.Round(fltCurrent[2], 3); // 保留3位小数


            //平均电压..
            Array.ConstrainedCopy(source, 36, destination, 0, 4);            
            fltAverageVoltage = BitConverter.ToSingle(destination, 0);
            fltAverageVoltage = (float)Math.Round(fltAverageVoltage, 3);

            //平均电流
            Array.ConstrainedCopy(source, 40, destination, 0, 4);            
            fltAverageCurrent = BitConverter.ToSingle(destination, 0);
            fltAverageCurrent = (float)Math.Round(fltAverageCurrent, 3);

            //Float	fltVoltageWaveD;	//电压波形畸变率
            Array.ConstrainedCopy(source, 44, destination, 0, 4);            
            fltVoltageWaveD = BitConverter.ToSingle(destination, 0);

            //Float	fltPower[3];		//三相功率
            Array.ConstrainedCopy(source, 48, destination, 0, 4);            
            fltPower[0] = BitConverter.ToSingle(destination, 0);
            fltPower[0] = (float)Math.Round(fltPower[0], 3);

            Array.ConstrainedCopy(source, 52, destination, 0, 4);            
            fltPower[1] = BitConverter.ToSingle(destination, 0);
            fltPower[1] = (float)Math.Round(fltPower[1], 3);

            Array.ConstrainedCopy(source, 56, destination, 0, 4);            
            fltPower[2] = BitConverter.ToSingle(destination, 0);
            fltPower[2] = (float)Math.Round(fltPower[2], 3);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //Float	fltCosQ;		//功率因数
            Array.ConstrainedCopy(source, 60, destination, 0, 4);            
            fltCosQ = BitConverter.ToSingle(destination, 0);            

            //Float	fltFreq；		//频率
            Array.ConstrainedCopy(source, 64, destination, 0, 4);            
            fltFreq = BitConverter.ToSingle(destination, 0);            

            //Float	fltMeasureSumPower;	//实测损耗(实测空载损耗/负载损耗)
            Array.ConstrainedCopy(source, 68, destination, 0, 4);            
            fltMeasureSumPower = BitConverter.ToSingle(destination, 0);
            fltMeasureSumPower = (float)Math.Round(fltMeasureSumPower, 3);

            //Float	fltAdjustSumPower;	//校正到额定条件下的损耗
            Array.ConstrainedCopy(source, 72, destination, 0, 4);            
            fltAdjustSumPower = BitConverter.ToSingle(destination, 0);

            //Float	fltUkIoPercent;		//Io%（空载时）或Uk%（负载时）
            Array.ConstrainedCopy(source, 76, destination, 0, 4);            
            fltUkIoPercent = BitConverter.ToSingle(destination, 0);

            //Float	fltTempAdjustPower[2];	//校正到设定温度的负载损耗，
            Array.ConstrainedCopy(source, 80, destination, 0, 4);            
            fltTempAdjustPower[0] = BitConverter.ToSingle(destination, 0);

            Array.ConstrainedCopy(source, 84, destination, 0, 4);            
            fltTempAdjustPower[1] = BitConverter.ToSingle(destination, 0);

            //	Float	fltUkt;	//校正到设定温度的Uk%,只有负载损耗才有意义,空载时忽略
            Array.ConstrainedCopy(source, 88, destination, 0, 4);            
            fltUkt = BitConverter.ToSingle(destination, 0);

            //Float	fltZt;	//校正到设定温度的Zt, 只有负载损耗才有意义,空载时忽略
            Array.ConstrainedCopy(source, 92, destination, 0, 4);            
            fltZt = BitConverter.ToSingle(destination, 0);

        }

        public float[] fltVoltage = new float[3];    //三相电压
        public float[] fltMeanVoltage = new float[3];  //三相平均电压
        public float[] fltCurrent = new float[3];	  //三相电流

        public float fltAverageVoltage; //平均电压
        public float fltAverageCurrent;    //平均电流
        public float fltVoltageWaveD;  //电压波形畸变率
        public float[] fltPower = new float[3];      //三相功率

        public float fltCosQ;      //功率因数
        public float fltFreq;		//频率


        public float fltMeasureSumPower;   //实测损耗(实测空载损耗/负载损耗)
        public float fltAdjustSumPower;    //校正到额定条件下的损耗（空载损耗/负载损耗）
        public float fltUkIoPercent;       //Io%（空载时）或Uk%（负载时）
        public float[] fltTempAdjustPower = new float[2];    //校正到设定温度的负载损耗，
                                               //温度系数法、国标公式法两种结果
                                               //只有负载损耗才有意义,空载时忽略
        public float fltUkt;   //校正到设定温度的Uk%,只有负载损耗才有意义,空载时忽略
        public float fltZt;	//校正到设定温度的Zt, 只有负载损耗才有意义,空载时忽略

    }
}
