using System;
using System.IO.Ports;
using System.Linq;

namespace 直租变比net5.Parsing
{
    public class DataParsing
    {
        public static void SendDataHand(SerialPort serialPort)
        {
            //发送握手..
            string currentcommand = "C8-D9-00-B0-01";
            byte[] currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
        }

        public static void SendDataStart(SerialPort serialPort)
        {
            //发送启动..
            string currentcommand = "C8-D9-00-B0-05-00-38";
            byte[] currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
        }

        public static void SendDataStop(SerialPort serialPort)
        {
            //发送停止..
            string currentcommand = "C8-D9-00-B0-08";
            byte[] currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
        }


        public static void SendDataSet(SerialPort serialPort, int setint)
        {
            string currentcommand = string.Empty;
            //发送设置..                        
            if (setint == 0)
            {
                currentcommand = "C8-D9-00-B0-06-00-39-00-01-02-00-01";
            }
            if (setint == 1)
            {
                currentcommand = "C8-D9-00-B0-06-00-39-00-01-02-00-02";
            }
            if (setint == 2)
            {
                currentcommand = "C8-D9-00-B0-06-00-39-00-01-02-00-03";
            }
            byte[] currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
        }


        public static void SendDataRead(SerialPort serialPort)
        {
            //动发送读取测量数据. 
            string currentcommand = "C8-D9-00-B0-03-00-04-00-0E";
            byte[] currentcommandbytes = currentcommand.Split('-').AsParallel().Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] crcdata = CRCCalc(currentcommandbytes);
            currentcommandbytes = currentcommandbytes.Concat(crcdata).ToArray();
            serialPort.Write(currentcommandbytes, 0, currentcommandbytes.Length);
        }

        //解析数据
        public static bool ParsingDataHand(byte[] data, int length)
        {
            //握手
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x01, 0x4F, 0x4B };
            if (FindSubArray(data, _buffsuccess) == -1)
            {
                //OnCallbackevent(1, RotobErrorCode.ChartMateFail, "<===握手-设备返回值错误");
                return false;
            }
            else
            {
                //OnCallbackevent(1, RotobErrorCode.Success, "<===握手-完成");
                return true;
            }

        }

        public static bool ParsingDataStart(byte[] data, int length)
        {
            //启动
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x05, 0x00, 0x38 };
            if (FindSubArray(data, _buffsuccess) == -1)
            {
                //OnCallbackevent(2, RotobErrorCode.ChartMateFail, "<===启动-设备返回值错误");
                return false;
            }
            else
            {
                //OnCallbackevent(2, RotobErrorCode.Success, "<===启动-完成");
                return true;
            }
        }

        public static bool ParsingDataStop(byte[] data, int length)
        {
            //停止
            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x08 };
            if (FindSubArray(data, _buffsuccess) == -1)
            {
                //OnCallbackevent(3, RotobErrorCode.ChartMateFail, "<===停止-设备返回值错误");
                return false;
            }
            else
            {
                //OnCallbackevent(3, RotobErrorCode.Success, "<===停止-完成");
                return true;
            }
        }

        public static bool ParsingDataSet(byte[] data, int length)
        {
            //这里返回的指令需要比较..
            //OnCallbackevent(4, RotobErrorCode.Success, "<===设备数据4返回!");
            return true;
        }


        public static bool ParsingDataRead(byte[] data, int length)
        {
            //这里返回的指令需要比较..

            byte[] _buffsuccess = { 0xC8, 0xD9, 0x00, 0xB0, 0x03, 0x34 };
            if (FindSubArray(data, _buffsuccess) == -1)
            {
                //OnCallbackevent(6, RotobErrorCode.ChartMateFail, "<===数据读取指令返回值错误");
                return false;
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

            //OnCallbackevent(6, RotobErrorCode.Success, "<===数据读取完成", datatimer, datazbfs, datadqfj, datajx, dataelbb,
            //datafjjk, dataKAB, dataKBC, dataKCA, dataEAB, dataEBC, dataECA, dataclfs);
            return true;
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


        public static int FindSubArray(byte[] array, byte[] subArray)
        {
            int foundAt = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array.Length - i < subArray.Length)
                {
                    break;
                }
                else
                {
                    byte[] temp = new byte[subArray.Length];
                    Array.Copy(array, i, temp, 0, subArray.Length);
                    if (temp.SequenceEqual(subArray))
                    {
                        foundAt = i;
                        break;
                    }
                }
            }
            return foundAt;
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
            crc16[0] = (byte)((crc >> 8) & 0xff);
            //CRC寄存器的低8位变成高8位
            crc16[1] = (byte)(crc & 0xff);
            return crc16;
        }
    }
}
