using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Base
{

    public enum RotobErrorCode
    {
        Success,
        TimerOver,
        ChartMateFail,        
        Error,
        Ready,
    }
     
    //底层串口回调给机器人..
    public class PortCallBackArg
    {
        public int currentcommand;
        public byte[] data;
        public int length;
    }


    public class RotbotPushArg
    {
        public int command; //指令..
        public string reserve;//保留..
        public byte[] data; //数据.
        public int length; //数据长度..
    }

    class MySerialPort
    {
        protected SerialPort serialPort = new SerialPort();

        //当前指令--发送握手--
        protected int currentcomandnumber = 0; //当前指令码..

        public MySerialPort()
        {
            /*
            serialPort.BaudRate = 9800; // 设置波特率
            serialPort.Parity = Parity.None; // 设置奇偶校验
            serialPort.DataBits = 8; // 设置数据位数
            serialPort.StopBits = StopBits.One; // 设置停止位
            serialPort.Handshake = Handshake.None; // 设置握手协议
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); // 接收到数据时的事件
            */
        }

        public virtual void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) { }

        public bool GetPortStatus()
        {
            return serialPort.IsOpen;
        }

        public (string, bool) OpenPort(string comname)
        {
            try
            {
                serialPort.PortName = comname;
                serialPort.Open(); // 打开串口
                return ("", true);
            }
            catch (Exception e)
            {
                return (e.Message, false);
            }
        }

        public void ClosePort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close(); //关闭串口
            }
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
