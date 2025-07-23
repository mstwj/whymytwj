using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Modbus.Device;
using PLCNET5_11_9.Data;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PLCNET5_11_9.ViewModel
{    
    public class MainViewModel : ObservableObject
    {
        public int Sustaintime { get; set; } = 0; 

        private SerialPort serialPort = new SerialPort();
        private ModbusSerialMaster master = null;
        //private ModbusIpMaster master = null;
        private Task rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<BKCommand> queue = new Queue<BKCommand>();

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //接法.
        private string dqczfs = string.Empty;
        public string DQCZFS { get { return dqczfs; } set { SetProperty(ref dqczfs, value); } }

        private ushort bjz;//中频机的步长..
        public ushort BJZ { get { return bjz; } set { SetProperty(ref bjz, value); } }

        private ushort gpjbc;//工频机的步长..
        public ushort GPJBC { get { return gpjbc; } set { SetProperty(ref gpjbc, value); } }


        private ushort zpj2000dqz;
        public ushort ZPJ2000DQZ { get { return zpj2000dqz; } set { SetProperty(ref zpj2000dqz, value); } }

        private ushort gpj2000dqz ;
        public ushort GPJ2000DQZ { get { return gpj2000dqz; } set { SetProperty(ref gpj2000dqz, value); } }

        //获取资源自动
        public ResourceDictionary resources { get; set; } = null;

        public WaitWindow commandExeWaitWindow = null;

        private bool iswork = false;
        public int BtnWith { get; set; } = 50;
        public int BtnHight { get; set; } = 50;
        public MainModel mainModel { get; set; } = new MainModel();        
        public ObservableCollection<Item> ListBoxData { get; set; } = new ObservableCollection<Item>();

        private Plc plc31 = new Plc(CpuType.S7200Smart, "192.168.2.31", 0, 1);        
        private Plc plc33 = new Plc(CpuType.S7200Smart, "192.168.2.33", 0, 1);
        private Plc plc35 = new Plc(CpuType.S7200Smart, "192.168.2.35", 0, 1);
        private Plc plc37 = new Plc(CpuType.S7200Smart, "192.168.2.37", 0, 1);
        
        public ICommand Command75_1 { get; set; }        
        public ICommand Command75_2 { get; set; }
        public ICommand Command75_3 { get; set; }
        public ICommand Command75_4 { get; set; }
        public ICommand Command75_5 { get; set; }
        public ICommand Command75_6 { get; set; }
        public ICommand Command20_1 { get; set; }
        public ICommand Command20_2 { get; set; }
        public ICommand Command20_3 { get; set; }
        public ICommand Command20_4 { get; set; }
        public ICommand Command20_5 { get; set; }
        public ICommand Command20_6 { get; set; }
        public ICommand Command20T_1 { get; set; }
        public ICommand Command20T_2 { get; set; }
        public ICommand Command20T_3 { get; set; }
        public ICommand Command20T_4 { get; set; }
        public ICommand Command20T_5 { get; set; }
        public ICommand Command20T_6 { get; set; }
        public ICommand Command20T_7 { get; set; }
        public ICommand Command20T_8 { get; set; }
        public ICommand Command30T_8 { get; set; }

        
        public ICommand CommandZB_1 { get; set; }
        public ICommand CommandZB_2 { get; set; }
        public ICommand CommandZCB_1 { get; set; }
        public ICommand CommandZCB_2 { get; set; }
        public ICommand CommandDNBC_1 { get; set; }
        public ICommand CommandDNBC_2 { get; set; }
        public ICommand CommandDNBC_3 { get; set; }
        public ICommand CommandDNBC_4 { get; set; }
        public ICommand CommandDK_1 { get; set; }
        public ICommand CommandDK_2 { get; set; }
        public ICommand CommandQF_1 { get; set; }
        public ICommand CommandQF_2 { get; set; }
        public ICommand CommandQF_3 { get; set; }
        public ICommand CommandQF_4 { get; set; }
        public ICommand CommandQF_5 { get; set; }
        public ICommand CommandQF_6 { get; set; }

        public ICommand CommandGW1_1 { get; set; }
        public ICommand CommandGW1_2 { get; set; }
        public ICommand CommandGW1_3 { get; set; }
        public ICommand CommandGW1_4 { get; set; }
        public ICommand CommandGW2_1 { get; set; }
        public ICommand CommandGW2_2 { get; set; }
        public ICommand CommandGW2_3 { get; set; }
        public ICommand CommandGW2_4 { get; set; }
        public ICommand CommandGW3_1 { get; set; }
        public ICommand CommandGW3_2 { get; set; }
        public ICommand CommandGW3_3 { get; set; }
        public ICommand CommandGW3_4 { get; set; }


        public ICommand Command75_Option1 { get; set; }
        public ICommand Command75_Option3 { get; set; }

        public ICommand Command20_Option1 { get; set; }
        public ICommand Command20_Option3 { get; set; }

        public ICommand Command20T_Option1 { get; set; }
        public ICommand Command20T_Option3 { get; set; }

        public ICommand CommandGPJDY_HZ { get; set; }
        public ICommand CommandGPJDY_FZ { get; set; }

        public ICommand CommandZPJDY_HZ { get; set; }
        public ICommand CommandZPJDY_FZ { get; set; }

        public ICommand CommandTYQ_HZ { get; set; }
        public ICommand CommandTYQ_FZ { get; set; }


        public ICommand CommandMyUp { get; set; }
        public ICommand CommandMyDown { get; set; }

        public MainViewModel()
        {            
            Command75_1 = new RelayCommand<object>(DoCommand75_1);
            Command75_2 = new RelayCommand<object>(DoCommand75_2);
            Command75_3 = new RelayCommand<object>(DoCommand75_3);
            Command75_4 = new RelayCommand<object>(DoCommand75_4);
            Command75_5 = new RelayCommand<object>(DoCommand75_5);
            Command75_6 = new RelayCommand<object>(DoCommand75_6);
            Command20_1 = new RelayCommand<object>(DoCommand20_1);
            Command20_2 = new RelayCommand<object>(DoCommand20_2);
            Command20_3 = new RelayCommand<object>(DoCommand20_3);
            Command20_4 = new RelayCommand<object>(DoCommand20_4);
            Command20_5 = new RelayCommand<object>(DoCommand20_5);
            Command20_6 = new RelayCommand<object>(DoCommand20_6);
            Command20T_1 = new RelayCommand<object>(DoCommand20T_1);
            Command20T_2 = new RelayCommand<object>(DoCommand20T_2);
            Command20T_3 = new RelayCommand<object>(DoCommand20T_3);
            Command20T_4 = new RelayCommand<object>(DoCommand20T_4);
            
            CommandZB_1 = new RelayCommand<object>(DoCommandZB_1);
            CommandZB_2 = new RelayCommand<object>(DoCommandZB_2);
            CommandZCB_1 = new RelayCommand<object>(DoCommandZCB_1);
            CommandZCB_2 = new RelayCommand<object>(DoCommandZCB_2);

            
            CommandDNBC_1 = new RelayCommand<object>(DoCommandDNBC_1);
            CommandDNBC_2 = new RelayCommand<object>(DoCommandDNBC_2);
            CommandDNBC_3 = new RelayCommand<object>(DoCommandDNBC_3);
            CommandDNBC_4 = new RelayCommand<object>(DoCommandDNBC_4);
            CommandDK_1 = new RelayCommand<object>(DoCommandDK_1);
            CommandDK_2 = new RelayCommand<object>(DoCommandDK_2);
           
            
            CommandGW1_1 = new RelayCommand<object>(DoCommandGW1_1);
            CommandGW1_2 = new RelayCommand<object>(DoCommandGW1_2);
            CommandGW1_3 = new RelayCommand<object>(DoCommandGW1_3);
            CommandGW1_4 = new RelayCommand<object>(DoCommandGW1_4);
            CommandGW2_1 = new RelayCommand<object>(DoCommandGW2_1);
            CommandGW2_2 = new RelayCommand<object>(DoCommandGW2_2);
            CommandGW2_3 = new RelayCommand<object>(DoCommandGW2_3);
            CommandGW2_4 = new RelayCommand<object>(DoCommandGW2_4);
            CommandGW3_1 = new RelayCommand<object>(DoCommandGW3_1);
            CommandGW3_2 = new RelayCommand<object>(DoCommandGW3_2);
            CommandGW3_3 = new RelayCommand<object>(DoCommandGW3_3);
            CommandGW3_4 = new RelayCommand<object>(DoCommandGW3_4);

            Command75_Option1 = new RelayCommand<object>(DoCommand75_Option1);
            Command75_Option3 = new RelayCommand<object>(DoCommand75_Option3);

            Command20_Option1 = new RelayCommand<object>(DoCommand20_Option1);
            Command20_Option3 = new RelayCommand<object>(DoCommand20_Option3);

            Command20T_Option1 = new RelayCommand<object>(DoCommand20T_Option1);
            Command20T_Option3 = new RelayCommand<object>(DoCommand20T_Option3);


            CommandGPJDY_HZ = new RelayCommand<object>(DoCommandGPJDY_HZ);
            CommandGPJDY_FZ = new RelayCommand<object>(DoCommandGPJDY_FZ);

            CommandZPJDY_HZ = new RelayCommand<object>(DoCommandZPJDY_HZ);
            CommandZPJDY_FZ = new RelayCommand<object>(DoCommandZPJDY_FZ);

            CommandTYQ_HZ = new RelayCommand<object>(DoCommandTYQ_HZ);
            CommandTYQ_FZ = new RelayCommand<object>(DoCommandTYQ_FZ);

            CommandMyUp = new RelayCommand<object>(DoCommandMyUp);
            CommandMyDown = new RelayCommand<object>(DoCommandMyDown);

            //初始化后台线程...-- 这里不能直接调用，因为resources还是NULL的..
            //InitializeAsync();
            //AddRecord((string)resources["PLCMessage18"], false);
            //AddRecord("启动完成...",false);        
        }

        

        public void MainClose()
        {
            if (rotobTaskT != null)
            {
                cts.Cancel();
            }
        }

        void DoPlcCommand(string command, bool data)
        {
            //这里等待了... (主线程还是等了..)
            if (DQCZFS != "PC")
            {
                MessageBox.Show("无法操作,请切换到PC模式");
                return;
            }

            //if (iswork == true)
            //{
            //  AddRecord((string)resources["PLCMessage17"] + "isword == true", true);
            //  return;
            //}

            //这里等待了... (主线程还是等了..)
            if (queue.Count >= 1)
            {
                AddRecord((string)resources["PLCMessage17"], true);
                //AddRecord("无法执行指令,上一条指令还未执行完成", true);
                return;
            }
            queue.Enqueue(new BKCommand { Command = command, Data = data });

            commandExeWaitWindow = new WaitWindow();
            commandExeWaitWindow.ShowDialog();

        }


        private float[] ReadModbusData(byte slaveId, ushort start, ushort length)
        {
            float[] FloatValue = new float[3];
            try
            {
                //这里我懂了，这里多此一举了.. 本质读取的是WORD 2个字的，可是我自己又转变成了BYTE了...
                //ushort[] data = this.master.ReadHoldingRegisters(slaveId, start, length);

                //这里我懂了，这里多此一举了.. 本质读取的是WORD 2个字的，可是我自己又转变成了BYTE了...
                //ushort[] ushortArray = this.master.ReadHoldingRegisters(1, 0, 6);
                ushort[] ushortArray = this.master.ReadHoldingRegisters(slaveId, start, length);
                
                ushort[] ushortArray1 = new ushort[2];
                ushort[] ushortArray2 = new ushort[2];
                ushort[] ushortArray3 = new ushort[2];
                ushortArray1[0] = ushortArray[0];
                ushortArray1[1] = ushortArray[1];
                ushortArray2[0] = ushortArray[2];
                ushortArray2[1] = ushortArray[3];
                ushortArray3[0] = ushortArray[4];
                ushortArray3[1] = ushortArray[5];

                Array.Reverse(ushortArray1);
                Array.Reverse(ushortArray2);
                Array.Reverse(ushortArray3);
                //Convert.ToSingle(data, Type(float));//因为是 ToSingle所以自动4个 。。

                byte[] byteArray1 = new byte[4];
                Buffer.BlockCopy(ushortArray1, 0, byteArray1, 0, 4);
                float aaa = BitConverter.ToSingle(byteArray1, 0);

                byte[] byteArray2 = new byte[4];
                Buffer.BlockCopy(ushortArray2, 0, byteArray2, 0, 4);
                float bbb = BitConverter.ToSingle(byteArray2, 0);

                byte[] byteArray3 = new byte[4];
                Buffer.BlockCopy(ushortArray3, 0, byteArray3, 0, 4);
                float ccc = BitConverter.ToSingle(byteArray3, 0);

                float roundedNumber1 = aaa;
                //roundedNumber1 = (float)Math.Round(aaa, 2);
                //bbb = bbb / 1000;
                //float roundedNumber2 = (float)Math.Round(bbb, 2);
                float roundedNumber2 = bbb;
                float roundedNumber3 = ccc;

                float[] data = new float[3];
                data[0] = roundedNumber1;
                data[1] = roundedNumber2;
                data[2] = roundedNumber3;

                return data;
            }
            catch (Exception ex)
            {
                throw ;
            }            
        }




        public bool[] alldian_boolinput { get; set; } = new bool[800];

        void ByteToBoolInput(byte[] alldian)
        {
            //Input起始地址100...
            int k = 0;
            //取PLC O-0 2000中频机
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[0], (byte)i);
                alldian_boolinput[k] = value; 
                // 0 -- 7 
            }

            //取PLC O-0 2000中频机
            k = 10;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[1], (byte)i);
                alldian_boolinput[k] = value;
                // 8 -- 15
            }

            //取PLC O-8
            k = 80;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[8], (byte)i);
                alldian_boolinput[k] = value;
                //16 -- 23
            }

            //取PLC O-9
            k = 90;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[9], (byte)i);
                alldian_boolinput[k] = value;
                //24 -- 31
            }

            //取PLC O-12
            k = 120;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[12], (byte)i);
                alldian_boolinput[k] = value;
                //32 -- 39
            }

            //取PLC O-13
            k = 130;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[13], (byte)i);
                alldian_boolinput[k] = value;
                //40 -- 47
            }

            //取PLC O-16
            k = 160;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[16], (byte)i);
                alldian_boolinput[k] = value;
                //48 -- 56
            }

            //取PLC O-17
            k = 170;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[17], (byte)i);
                alldian_boolinput[k] = value;
                //57 -- 64
            }

            //取PLC 20
            k = 200;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[20], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }

            //取PLC 21
            k = 210;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[21], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }


            //取PLC 22
            k = 220;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[22], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }


            //取PLC 23
            k = 230;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[23], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }


            //取PLC 24
            k = 240;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[24], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }

            //取PLC 30
            k = 300;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[30], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }

            //取PLC 31
            k = 310;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[31], (byte)i);
                alldian_boolinput[k] = value;
                //alldian_bool[65] == 
            }


            //取PLC O-40
            k = 400;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[40], (byte)i);
                alldian_boolinput[k] = value;                
                //alldian_bool[65] == 
            }

            //取PLC 41
            k = 410;
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[41], (byte)i);
                alldian_boolinput[k] = value;                
            }

        }

        /*
        void ByteToBoolOutput(byte[] alldian)
        {
            int k = 0;
            //取PLC O-0 2000中频机
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[0], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-0 2000中频机
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[1], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-8
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[8], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-9
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[9], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-12
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[12], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-13
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[13], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-16
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[16], (byte)i);
                alldian_bool[k] = value;
            }

            //取PLC O-17
            for (int i = 0; i < 8; i++, k++)
            {
                bool value = Bit.FromByte(alldian[17], (byte)i);
                alldian_bool[k] = value;
            }

        }
        */

        // 更新数据库中的数据
        void SendDataToDataBase()
        {
            //这里必须是这样，因为数据库是FLOAT，如果直接插入就一大堆小数点.. 这里必须我来保留3位.
            //还有，数据库里面是FLOAT，可是我C#里面必须是double 才能对应..--这里必须 Math.Round 要不然，数据库肯定小数点多...
            //因为我是PC端，所以可以不通过WEB 服务器.. 直接去写数据库..
            my_table temp_info = new my_table();
            Random random = new Random();
            // 例如，在[10, 20]范围内生成浮点数
            float randomFloatInRange1 = (float)(10 + random.NextDouble() * 10);
            float randomFloatInRange2 = (float)(10 + random.NextDouble() * 10);
            float randomFloatInRange3 = (float)(10 + random.NextDouble() * 10);            
            temp_info.ab = (double)Math.Round(randomFloatInRange1, 3);             
            temp_info.bc = (double)Math.Round(randomFloatInRange2, 3);            
            temp_info.ca = (double)Math.Round(randomFloatInRange3, 3);

            
            try
            {
                using (var context = new MyDbContext())
                {
                    //如果ID设置为0 就是添加.. 如果是指定ID就是修改了.
                    //temp_info.id = 0;
                    temp_info.id = 1;
                    //这里不能调用Add --- 如果调用了，就是添加了..
                    //这里必须使用UPDATE-- 更新..
                    context.zhongbianshurudianya_table.Update(temp_info);

                    // 更新已有数据(这里必须先搜索一次吗？--必须先搜索--要不然他不更新..)
                    //var person = context.zhongbianshurudianya_table.Find(temp_info.id);
                    var person = new my_table();

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..(后面的数据给前面的.. )
                        ObjectComparer.CompareObjects(person, temp_info);
                    }
                    //context.SaveChanges();
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        AddRecord((string)resources["MyMessage40"], false);
                    }
                    else
                    {
                        AddRecord((string)resources["MyMessage41"], false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }            
        }

        public bool DKQGZError { get; set; } = false;
        public bool TYQError { get; set; } = false;

        void ReadAllPlcData(bool update = true)
        {
            try
            {
                
                if (update)              
                {
                    //读表.
                    //中变输入电压 。。
                    float[] data1 = ReadModbusData(1, 0, 6);
                    float temp1 = data1[0];
                    float temp2 = data1[0];
                    float temp3 = data1[0];
                    temp1 = temp1 / 1000;
                    temp2 = temp2 / 1000;
                    temp3 = temp3 / 1000;
                    temp1 = (float)Math.Round(temp1, 2);
                    temp2 = (float)Math.Round(temp2, 2);
                    temp3 = (float)Math.Round(temp3, 2);

                    //更新表(6个表)
                    mainModel.Biao4_data1 = temp1;
                    mainModel.Biao4_data2 = temp2;
                    mainModel.Biao4_data3 = temp3;

                    //中变输入电流 。。
                    float[] data2 = ReadModbusData(2, 0, 6);

                    data2[0] = (float)Math.Round(data2[0], 1);
                    data2[1] = (float)Math.Round(data2[1], 1);
                    data2[2] = (float)Math.Round(data2[2], 1);


                    //更新表(6个表)
                    mainModel.Biao5_data1 = data2[0];
                    mainModel.Biao5_data2 = data2[1];
                    mainModel.Biao5_data3 = data2[2];

                    //中变输入频率..
                    float[] data3 = ReadModbusData(3, 0, 6);
                    //更新表(6个表)
                    mainModel.Biao6_data1 = data3[0];
                    //mainModel.Biao6_data2 = data3[1];
                    //mainModel.Biao6_data3 = data3[2];


                    //调压器电流。
                    float[] data4 = ReadModbusData(4, 0, 6);
                    data4[0] = (float)Math.Round(data4[0], 1);
                    data4[1] = (float)Math.Round(data4[1], 1);
                    data4[2] = (float)Math.Round(data4[2], 1);
                    mainModel.Biao1_data1 = data4[0];
                    mainModel.Biao1_data2 = data4[1];
                    mainModel.Biao1_data3 = data4[2];

                    //电容
                    float[] data7 = ReadModbusData(7, 0, 6);
                    data7[0] = (float)Math.Round(data7[0], 1);
                    data7[1] = (float)Math.Round(data7[1], 1);
                    data7[2] = (float)Math.Round(data7[2], 1);
                    mainModel.Biao2_data1 = data7[0];
                    mainModel.Biao2_data2 = data7[1];
                    mainModel.Biao2_data3 = data7[2];

                    //电抗

                    float[] data8 = ReadModbusData(8, 0, 6);
                    data8[0] = (float)Math.Round(data8[0], 1);
                    data8[1] = (float)Math.Round(data8[1], 1);
                    data8[2] = (float)Math.Round(data8[2], 1);
                    mainModel.Biao3_data1 = data8[0];
                    mainModel.Biao3_data2 = data8[1];
                    mainModel.Biao3_data3 = data8[2];


                    //mainModel.Biao2_data1 = data[3];Biao4_data1
                    //mainModel.Biao2_data2 = data[4];
                    // mainModel.Biao2_data3 = data[5];
                }


                //读PLC41 0--1.7(16) -- 一次读取主机所有O点..--为什么是判断输出点呢？
                //ByteToBoolOutput(plc31.ReadBytes(DataType.Output, 0, 0, 18));

                

                byte[] temp37 = plc37.ReadBytes(DataType.Input, 0, 0, 1);
                alldian_boolinput[362] = Bit.FromByte(temp37[0], (byte)5);


                //读PLC 输入点
                ByteToBoolInput(plc31.ReadBytes(DataType.Input, 0, 0, 50));

                byte[] aidian = new byte[4];

                byte[] aidian1 = new byte[2];
                byte[] aidian2 = new byte[2];

                //这里读了 DATABLACK 为什么？--这里是DB块的数据....
                aidian = plc31.ReadBytes(DataType.DataBlock, 2, 4, 4);
                //Array.Reverse(aidian);
                //更新界面上显示的调压数字....

                aidian1[0] = aidian[0];
                aidian1[1] = aidian[1];
                aidian2[0] = aidian[2];
                aidian2[1] = aidian[3];

                Array.Reverse(aidian1);
                Array.Reverse(aidian2);

                GPJ2000DQZ = BitConverter.ToUInt16(aidian1, 0); //因为是ToUint16 所以自动2个..


                ZPJ2000DQZ = BitConverter.ToUInt16(aidian2, 0); //因为是ToUint16 所以自动2个..


                //var m100Bit = plc31.ReadBit(DataType.Memory, 100, 0);
                //if (m100Bit is bool value)
                //{
                //    Console.WriteLine($"M100.0状态：{value}");
                //}

                //读取了M点..
                byte[] dataacb= plc31.ReadBytes(DataType.Memory, 0,  360, 1); //电容补偿AC 电容塔..
                alldian_boolinput[360] = Bit.FromByte(dataacb[0], (byte)0);
                alldian_boolinput[361] = Bit.FromByte(dataacb[0], (byte)1);

                //读取了M点..
                byte[] data365 = plc33.ReadBytes(DataType.Memory, 0, 365, 1);
                alldian_boolinput[365] = Bit.FromByte(data365[0], (byte)4); //中变故障...(读本地..)


                //读取了M点..
                byte[] data370 = plc31.ReadBytes(DataType.Memory, 0, 370, 1);
                alldian_boolinput[370] = Bit.FromByte(data370[0], (byte)0); //2000中频机故障

                //读取了M点..
                byte[] data380 = plc37.ReadBytes(DataType.Memory, 0, 380, 1);
                alldian_boolinput[380] = Bit.FromByte(data380[0], (byte)0); //7500中频机故障

                //读取了M点..
                byte[] data390 = plc37.ReadBytes(DataType.Memory, 0, 390, 1);
                alldian_boolinput[390] = Bit.FromByte(data390[0], (byte)0); //支持变 故障

                //读取了M点..

                //byte[] data400 = plc37.ReadBytes(DataType.Memory, 0, 400, 1);
                //DKQGZError = Bit.FromByte(data400[0], (byte)0); //电抗器变 故障


                //读取了M点..
                byte[] data410 = plc37.ReadBytes(DataType.Memory, 0, 410, 1);
                TYQError = Bit.FromByte(data410[0], (byte)0); //调压器 故障..


                //更新界面--更新门闸..
                if (update)
                {
                    WeakReferenceMessenger.Default.Send<string>("Update");
                }

                //发送数据到数据库(需要吗？)
                //SendDataToDataBase();

            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        //检查是不是断线..
        bool ScanConntion()
        {
            /*
            bool success = true;
            try 
            { 
                plc31.ReadBytes(DataType.Input, 0, 0, 1);
                mainModel.Plc31 = true; //设置绿灯
            }
            catch (Exception ex)
            {
                mainModel.Plc31 = false;//设置黑灯; }
                AddRecord((string)resources["PLCMessage12"], true);
                success = false;
            }

            try
            {
                plc33.ReadBytes(DataType.Input, 0, 0, 1);
                mainModel.Plc33 = true; //设置绿灯
                success = false;
            }
            catch (Exception ex)
            {
                mainModel.Plc33 = false;//设置黑灯; }
                AddRecord((string)resources["PLCMessage13"], true);
                success = false;

            }


            try
            {
                plc35.ReadBytes(DataType.Input, 0, 0, 1);
                mainModel.Plc35 = true; //设置绿灯
            }
            catch (Exception ex)
            {
                mainModel.Plc35 = false;//设置黑灯; }
                AddRecord((string)resources["PLCMessage14"], true);
                success = false;
            }

            try
            {
                plc37.ReadBytes(DataType.Input, 0, 0, 1);
                mainModel.Plc37 = true; //设置绿灯
            }
            catch (Exception ex)
            {
                mainModel.Plc37 = false;//设置黑灯; }
                AddRecord((string)resources["PLCMessage15"], true);
                success = false;
            }
            */

            return true;            
        }

        private void DoCommand75_1(object button) { AddRecord("指令:7500工频机输出合闸", false);  DoPlcCommand("DoCommand75_1", true);  return;}        
        private void DoCommand75_2(object button) { AddRecord("指令:7500工频机输出分闸", false);  DoPlcCommand("DoCommand75_2", true);  return;}        
        private void DoCommand75_3(object button) { AddRecord("指令:7500工频机励磁合闸", false);  DoPlcCommand("DoCommand75_3", true);   return; }
        private void DoCommand75_4(object button) { AddRecord("指令:7500工频机励磁分闸", false);  DoPlcCommand("DoCommand75_4", true);  return; }
        private void DoCommand75_5(object button) { AddRecord("指令:7500工频机升", false);  DoPlcCommand("DoCommand75_5", true);   return; }
        private void DoCommand75_6(object button) { AddRecord("指令:7500工频机降", false);  DoPlcCommand("DoCommand75_6", true);  return; }
        private void DoCommand20_1(object button) { AddRecord("指令:2000中频机输出合闸", false);  DoPlcCommand("DoCommand20_1", true);   return; }
        private void DoCommand20_2(object button) { AddRecord("指令:2000中频机输出分闸", false);  DoPlcCommand("DoCommand20_2", true);   return; }
        private void DoCommand20_3(object button) { AddRecord("指令:2000中频机励磁合闸", false);  DoPlcCommand("DoCommand20_3", true);   return; }
        private void DoCommand20_4(object button) { AddRecord("指令:2000中频机励磁分闸", false);  DoPlcCommand("DoCommand20_4", true);   return; }
        private void DoCommand20_5(object button) { AddRecord("指令:2000工频机升", false);  DoPlcCommand("DoCommand20_5", true);  return; }

        private void DoCommand20_6(object button) { AddRecord("指令:2000工频机降", false);  DoPlcCommand("DoCommand20_6", true);  return; }
        private void DoCommand20T_1(object button) { AddRecord("指令:调压器输出合闸", false);  DoPlcCommand("DoCommand20T_1", true);   return; }
        private void DoCommand20T_2(object button) { AddRecord("指令:调压器输出分闸", false);  DoPlcCommand("DoCommand20T_2", true);  return; }
        private void DoCommand20T_3(object button) { AddRecord("指令:调压器励磁合闸", false);  DoPlcCommand("DoCommand20T_3", true);   return; }
        private void DoCommand20T_4(object button) { AddRecord("指令:调压器励磁分闸", false);  DoPlcCommand("DoCommand20T_4", true);  return; }
      
        public void DoCommand20T_5_UP() { DoPlcCommand("DoCommand20T_5_UP", true);   return; }
        public void DoCommand20T_5_UPSTOP() { DoPlcCommand("DoCommand20T_5_UPSTOP", true);   return; }        
        public void DoCommand20T_6_UP() { DoPlcCommand("DoCommand20T_6_UP", true);   return; }
        public void DoCommand20T_6_UPSTOP() { DoPlcCommand("DoCommand20T_6_UPSTOP", true);  return; }


        private void DoCommandZB_1(object button) { AddRecord("指令:中变输入合闸", false);  DoPlcCommand("DoCommandZB_1", true);  return; }
        private void DoCommandZB_2(object button) { AddRecord("指令:中变输入分闸", false);  DoPlcCommand("DoCommandZB_2", true);  return; }

        private void DoCommandZCB_1(object button) { AddRecord("指令:支撑变输入合闸", false);  DoPlcCommand("DoCommandZCB_1", true);  return; }
        private void DoCommandZCB_2(object button) { AddRecord("指令:支撑变输入分闸", false);  DoPlcCommand("DoCommandZCB_2", false);  return; }

        private void DoCommandDNBC_1(object button) { AddRecord("指令:电容补偿AC合闸", false);  DoPlcCommand("DoCommandDNBC_1", true);   return; }
        private void DoCommandDNBC_2(object button) { AddRecord("指令:电容补偿AC分闸", false);  DoPlcCommand("DoCommandDNBC_2", true);   return; }
        private void DoCommandDNBC_3(object button) { AddRecord("指令:电容补偿B合闸", false);  DoPlcCommand("DoCommandDNBC_3", true);   return; }
        private void DoCommandDNBC_4(object button) { AddRecord("指令:电容补偿B分闸", false);  DoPlcCommand("DoCommandDNBC_4", true);   return; }

        private void DoCommandDK_1(object button) { AddRecord("指令:电抗补偿合闸", false);  DoPlcCommand("DoCommandDK_1", true);   return; }
        private void DoCommandDK_2(object button) { AddRecord("指令:电抗补偿分闸", false); DoPlcCommand("DoCommandDK_2", true);   return; }
        

        private void DoCommandGW1_1(object button) { DoPlcCommand("DoCommandGW1_1", true);  return; }
        private void DoCommandGW1_2(object button) { DoPlcCommand("DoCommandGW1_2", true);   return; }
        private void DoCommandGW1_3(object button) { DoPlcCommand("DoCommandGW1_3", true);  return; }
        private void DoCommandGW1_4(object button) { DoPlcCommand("DoCommandGW1_4", true);  return; }
        private void DoCommandGW2_1(object button) { DoPlcCommand("DoCommandGW2_1", true);  return; }
        private void DoCommandGW2_2(object button) { DoPlcCommand("DoCommandGW2_2", true);  return; }
        private void DoCommandGW2_3(object button) { DoPlcCommand("DoCommandGW2_3", true);return; }
        private void DoCommandGW2_4(object button) { DoPlcCommand("DoCommandGW2_4", true); return; }
        private void DoCommandGW3_1(object button) { DoPlcCommand("DoCommandGW3_1", true);  return; }
        private void DoCommandGW3_2(object button) { DoPlcCommand("DoCommandGW3_2", true);   return; }
        private void DoCommandGW3_3(object button) { DoPlcCommand("DoCommandGW3_3", true); return; }
        private void DoCommandGW3_4(object button) { DoPlcCommand("DoCommandGW3_4", true);  return; }

        private void DoCommand75_Option1(object button) { AddRecord("指令:7500工频机单相合闸", false);  DoPlcCommand("DoCommand75_Option1", true);   return; }
        private void DoCommand75_Option3(object button) { AddRecord("指令:7500工频机三相合闸", false);  DoPlcCommand("DoCommand75_Option3", true);  return; }
        private void DoCommand20_Option1(object button) { AddRecord("指令:2000工频机单相合闸", false);  DoPlcCommand("DoCommand20_Option1", true);   return; }
        private void DoCommand20_Option3(object button) { AddRecord("指令:2000工频机三相合闸", false);  DoPlcCommand("DoCommand20_Option3", true);   return; }
        private void DoCommand20T_Option1(object button) { AddRecord("指令:调压器单相合闸", false);  DoPlcCommand("DoCommand20T_Option1", true);  return; }
        private void DoCommand20T_Option3(object button) { AddRecord("指令:调压器三相合闸", false);  DoPlcCommand("DoCommand20T_Option3", true);  return; }

        private void DoCommandGPJDY_HZ(object button) { AddRecord("指令:工频机电源合闸指令", false);  DoPlcCommand("DoCommandGPJDY_HZ", true);   return; }

        private void DoCommandGPJDY_FZ(object button) { AddRecord("指令:工频机电源分闸", false);  DoPlcCommand("DoCommandGPJDY_FZ", true);   return; }
        private void DoCommandZPJDY_HZ(object button) { AddRecord("指令:中频机电源合闸", false);  DoPlcCommand("DoCommandZPJDY_HZ", true);   return; }
        private void DoCommandZPJDY_FZ(object button) { AddRecord("指令:中频机电源分闸", false);  DoPlcCommand("DoCommandZPJDY_FZ", true);  return; }
        private void DoCommandTYQ_HZ(object button) { AddRecord("指令:调压器电源合闸", false); DoPlcCommand("DoCommandTYQ_HZ", true);  return; }
        private void DoCommandTYQ_FZ(object button) { AddRecord("指令:调压器电源分闸", false);  DoPlcCommand("DoCommandTYQ_FZ", true);   return; }

        private void DoCommandMyUp(object button)
        {
            AddRecord("指令:调压器升压", false);

            //MessageBox.Show(Sustaintime.ToString());

            DoPlcCommand("DoCommandMyUp", true); 
            return; 
        }
        private void DoCommandMyDown(object button) { AddRecord("指令:调压器降压", false); DoPlcCommand("DoCommandMyDown", true); return; }


        private async Task<bool> ExePlcCommand(string command, bool data, int isBackCall = 0)
        {                    
            bool Success = false;
            string error = string.Empty;
            //iswork = true;

            //var dynamicBrush = (SolidColorBrush)this.FindResource("DynamicBrush");
            //if (isBackCall == 0) AddRecord("请等待,PLC回应!", false);
            if (isBackCall == 0) AddRecord((string)resources["PLCMessage1"], false);
            var timeout = Task.Delay(20000);
            var t = Task.Run(async () =>
            {
                logger.Debug("新线程-开始执行" + command);
                try
                {
                    if (command != "33" && command != "34" && command != "35" && command != "36")
                    {
                        if (!ScanConntion()) 
                        {
                            AddRecord("请注意设备断线,请检查设备是否正常!", true);
                            //断线设备无法执行任何指令..
                            Success = false;
                            return;
                        }
                    }
                    if (command == "33")
                    {
                        AddRecord("准备连接PLC31", false);
                        plc31.Open();
                        mainModel.Plc31 = true; //设置绿灯
                        AddRecord("连接PLC31完成", false);
                    }
                    if (command == "34")
                    {
                        AddRecord("准备连接PLC33", false);
                        plc33.Open();
                        mainModel.Plc33 = true; //设置绿灯
                        AddRecord("连接PLC33完成", false);
                    }
                    if (command == "35")
                    {
                        AddRecord("准备连接PLC35", false);
                        plc35.Open();
                        mainModel.Plc35 = true; //设置绿灯
                        AddRecord("连接PLC35完成", false);
                    }

                    if (command == "36")
                    {
                        AddRecord("准备连接PLC37", false);
                        plc37.Open();
                        mainModel.Plc37 = true; //设置绿灯
                        AddRecord("连接PLC37完成", false);
                    }
                        
                    if (command == "500") ReadAllPlcData();

                    int IntOutTimer = 0;

                    switch (command)
                    {
                        case "DoCommand75_Option1": //7500单项合闸 -- 图G5
                            {
                                plc31.Write("M21.2", true);
                                Thread.Sleep(300);
                                plc31.Write("M21.2", false);
                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[211] == true)
                                    {
                                        AddRecord("7500工频机单项合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("7500工频机单项无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommand75_Option3": //7500三项合闸-- 图G5
                            {
                                plc31.Write("M21.1", true);
                                Thread.Sleep(300);
                                plc31.Write("M21.1", false);
                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[210] == true)
                                    {
                                        AddRecord("7500工频机三项合闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("7500工频机三项合闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommand20_Option1"://2000 单相合闸 -- G7 图
                            {
                                plc31.Write("M20.2", true);
                                Thread.Sleep(300);
                                plc31.Write("M20.2", false);
                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[205] == true)
                                    {
                                        AddRecord("2000中频机单相合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("2000中频机单相合闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;

                        case "DoCommand20_Option3": //2000 三相合闸 G7 图
                            {
                                plc31.Write("M20.1", true);
                                Thread.Sleep(300);
                                plc31.Write("M20.1", false);
                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[204] == true)
                                    {
                                        AddRecord("2000中频机三相合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("2000中频机三相合闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;

                        case "DoCommand20T_Option1": //图7-1 中的图1
                            {
                                plc31.Write("M22.7", false);
                                Thread.Sleep(300);                                
                                alldian_boolinput[501] = true;
                                AddRecord("调压器单项选择完成", false);
                            }
                            break;

                        case "DoCommand20T_Option3"://图7-1 中的图1
                            {
                                plc31.Write("M22.7", true);
                                alldian_boolinput[501] = false;
                                AddRecord("调压器三项选择完成", false);
                            }
                            break;

                        case "DoCommandGPJDY_HZ": //图7-2
                            {
                                plc31.Write("M30.2", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.2", false);

                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[304] == true)
                                    {
                                        AddRecord("工频机电源合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("工频机电源合闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommandGPJDY_FZ":  //图7-2
                            {
                                plc31.Write("M30.3", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.3", false);

                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    Thread.Sleep(1000);
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    
                                    if (alldian_boolinput[304] == false)
                                    {
                                        AddRecord("工频机电源分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("工频机电源分闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommandZPJDY_HZ": // //图7-3
                            {
                                plc31.Write("M30.0", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.0", false);

                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    Thread.Sleep(1000);
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    
                                    if (alldian_boolinput[303] == true)
                                    {
                                        AddRecord("中频机电源合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中频机电源合闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommandZPJDY_FZ": //图7-3s
                            {
                                plc31.Write("M30.1", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.1", false);

                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    Thread.Sleep(1000);
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    
                                    if (alldian_boolinput[303] == true)
                                    {
                                        AddRecord("中频机电源分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中频机电源分闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommandTYQ_HZ": //7-1 图3
                            {
                                plc31.Write("M30.4", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.4", false);

                                Thread.Sleep(300);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[301] == true)
                                    {
                                        AddRecord("调压器合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("调压器合闸无法完成-请检查是否有互锁", true);
                                }


                            }
                            break;

                        case "DoCommandTYQ_FZ"://7-1 图3
                            {


                                plc31.Write("M30.5", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.5", false);

                                Thread.Sleep(300);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[301] == false)
                                    {
                                        AddRecord("调压器分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("调压器分闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;

                        case "DoCommand75_1"://Command75_1   7.5MVA   工频机组(输出) 合闸 G6图
                            {
                                plc31.Write("M21.3", true);
                                Thread.Sleep(300);
                                plc31.Write("M21.3", false);

                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[212] == true)
                                    {
                                        AddRecord("工频机组(输出)合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("工频机组(输出)合闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;

                        case "DoCommand75_2": //Command75_2   7.5MVA   工频机组(输出) 分闸 G6图
                            {
                                plc31.Write("M21.4", true); 
                                Thread.Sleep(300);
                                plc31.Write("M21.4", false);

                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[212] == false)
                                    {
                                        AddRecord("工频机组(输出)分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("工频机组(输出)分闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;
                        case "DoCommand75_3": //Command75_3   7.5MVA   工频机组（力磁） 分闸 D4图
                            {
                                plc31.Write("M21.5", true); 
                                Thread.Sleep(300); 
                                plc31.Write("M21.5", false);
                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[213] == false)
                                    {
                                        AddRecord("工频机组励磁分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("工频机组励磁分闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;
                        case "DoCommand75_4": //Command75_4   7.5MVA   工频机组（力磁） 合闸    D4图
                            {
                                plc31.Write("M21.5", true);
                                Thread.Sleep(300);
                                plc31.Write("M21.5", false);
                                Thread.Sleep(300);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    Thread.Sleep(1000);
                                    if (alldian_boolinput[213] == true)
                                    {
                                        AddRecord("工频机组励磁合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("工频机组励磁合闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;
                        case "DoCommand75_5"://Command75_5   7.5MVA   工频机组（升)   无图  对应旋钮
                            {
                                if (GPJBC == 0)
                                {
                                    AddRecord("步长值不合法!步长不能为0", true);
                                    break;
                                }

                                if (GPJBC > 50)
                                {
                                    AddRecord("步长不能大于50!", true);
                                    break;
                                }

                                if (GPJ2000DQZ + GPJBC > 999)
                                {
                                    AddRecord("无法在升值,当前值+步长已超过最大值!", true);
                                    break;
                                }
                                //这里读了 DATABLACK 为什么？--这里是DB块的数据....
                                ushort ushortValue = (ushort)(GPJ2000DQZ + GPJBC);
                                byte[] byteArray = BitConverter.GetBytes(ushortValue);
                                Array.Reverse(byteArray);

                                plc31.WriteBytes(DataType.DataBlock, 2, 4, byteArray);
                                AddRecord("写入PLC数据完成", false);
                            }
                            break;
                        case "DoCommand75_6": //Command75_6   7.5MVA   工频机组（降)无图
                            {
                                if (GPJBC == 0)
                                {
                                    AddRecord("步长值不合法!步长不能为0", true);
                                    break;
                                }
                                if (GPJBC > 50)
                                {
                                    AddRecord("步长值不能大于50!", true);
                                    break;
                                }

                                if (GPJ2000DQZ - GPJBC < 0)
                                {
                                    AddRecord("无法在降,当前值+步长已超过最小值!",true);
                                    break;
                                }
                                //这里读了 DATABLACK 为什么？--这里是DB块的数据....
                                ushort ushortValue = (ushort)(GPJ2000DQZ - GPJBC);
                                byte[] byteArray = BitConverter.GetBytes(ushortValue);
                                Array.Reverse(byteArray);

                                plc31.WriteBytes(DataType.DataBlock, 2, 4, byteArray);
                                AddRecord("写入PLC数据完成", false);
                            }
                            break;
                        case "DoCommand20_1": //Command20_1   2MVA   中频机组(输出) 合闸   图G8
                            {
                                plc31.Write("M20.3", true);
                                Thread.Sleep(300);
                                plc31.Write("M20.3", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[206] == true)
                                    {
                                        AddRecord("中频机组(输出)合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中频机组(输出)合闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;
                        case "DoCommand20_2": //Command20_2   2MVA   中频机组(输出) 分闸 图G8
                            {
                                plc31.Write("M20.4", true); 
                                Thread.Sleep(300); 
                                plc31.Write("M20.4", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[206] == false)
                                    {
                                        AddRecord("中频机组(输出)分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中频机组(输出)分闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;
                        case "DoCommand20_3"://Command20_3   2MVA   中频机组（力磁） 分闸     图D7
                            {
                                plc31.Write("M20.5", true);
                                Thread.Sleep(300);
                                plc31.Write("M20.5", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[207] == true)
                                    {
                                        AddRecord("中频机组励磁合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中频机组励磁合闸无法完成-请检查是否有互锁", true);
                                }
                            }
                                break;
                        case "DoCommand20_4"://Command20_4   2MVA   中频机组（磁力） 合闸 图D7
                            {
                                plc31.Write("M20.6", true);
                                Thread.Sleep(300); 
                                plc31.Write("M20.6", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[207] == false)
                                    {
                                        AddRecord("中频机组励磁分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中频机组励磁分闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;
                        case "DoCommandZeroclearing":
                            {
                                //这里读了 DATABLACK 为什么？--这里是DB块的数据....
                                ushort ushortValue = 0;
                                byte[] byteArray = BitConverter.GetBytes(ushortValue);
                                Array.Reverse(byteArray);

                                plc31.WriteBytes(DataType.DataBlock, 2, 0, byteArray);
                            }
                            break;
                        case "DoCommand20_5": //  2MVA   中频机组（升)  按下 无图 旋钮                                            
                            {
                                if (BJZ == 0)
                                {
                                    AddRecord("设置步长值不合法!步长为0", true);
                                    break;
                                }

                                if (BJZ > 50)
                                {
                                    AddRecord("设置步长值不能大于50!", true);
                                    break;
                                }



                                if (ZPJ2000DQZ + BJZ > 999)
                                {
                                    AddRecord("无法在升值,当前值+步长值超过最大值!", true);
                                    break;
                                }    
                                //这里读了 DATABLACK 为什么？--这里是DB块的数据....
                                ushort ushortValue = (ushort)(ZPJ2000DQZ + BJZ);
                                byte[] byteArray = BitConverter.GetBytes(ushortValue);
                                Array.Reverse(byteArray);

                                plc31.WriteBytes(DataType.DataBlock, 2, 6, byteArray);
                                AddRecord("写入PLC数据完成", false);

                            }
                            break;
                     

                        case "DoCommand20_6": //  2MVA   中频机组（降) 无图 旋钮
                            {
                                //这里读了 DATABLACK 为什么？--这里是DB块的数据....
                                if (BJZ == 0)
                                {
                                    AddRecord("设置步长值不合法!步长不能为0", true);
                                    break;
                                }

                                if (BJZ > 50)
                                {
                                    AddRecord("设置步长值不能大于50!", true);
                                    break;
                                }

                                if (ZPJ2000DQZ - BJZ < 0)
                                {
                                    AddRecord("无法在降,当前值-步长值小于0", true);
                                    break;
                                }
                                ushort ushortValue = (ushort)(ZPJ2000DQZ - BJZ);
                                byte[] byteArray = BitConverter.GetBytes(ushortValue);
                                Array.Reverse(byteArray);
                                plc31.WriteBytes(DataType.DataBlock, 2, 6, byteArray);
                                AddRecord("写入PLC数据完成", false);
                            }
                            break;
                        case "DoCommand20T_1"://调压器(输入)  合闸  图3-2
                            {
                                plc31.Write("M22.0", true);
                                Thread.Sleep(300);
                                plc31.Write("M22.0", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[214] == true)
                                    {
                                        AddRecord("调压器(输入)合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("调压器(输入)合闸无法完成-请检查是否有互锁", true);
                                }

                            } break;
                        case "DoCommand20T_2"://调压器(输入)  分闸 图3-2
                            {
                                plc31.Write("M22.1", true); 
                                Thread.Sleep(300);
                                plc31.Write("M22.1", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[214] == false)
                                    {
                                        AddRecord("调压器(输入)分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("调压器(输入)分闸无法完成-请检查是否有互锁", true);
                                }

                            }
                            break;
                        case "DoCommand20T_3"://调压器(输出)  合闸  7-1 2图
                            {
                                plc31.Write("M22.2", true);
                                Thread.Sleep(300);
                                plc31.Write("M22.2", false);
                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[215] == true)
                                    {
                                        AddRecord("调压器(输出)合闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("调压器(输出)合闸无法完成-请检查是否有互锁-或者设备有问题", true);
                                }

                            }
                            break;
                        case "DoCommand20T_4"://调压器(输出)  分闸  7-1 2图
                            {
                                plc31.Write("M22.3", true);
                                Thread.Sleep(300); 
                                plc31.Write("M22.3", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[215] == false)
                                    {
                                        AddRecord("调压器(输出)分闸完成", false);
                                        break;
                                    }
                                }
                                if (IntOutTimer == 10)
                                {
                                    AddRecord("调压器(输出)分闸无法完成-请检查是否有互锁-或者设备有问题", true);
                                }

                            }
                            break;
   

                        case "DoCommandMyUp":
                            {
                                plc31.Write("M22.4", true);
                                Thread.Sleep(500);
                                if (Sustaintime == 0) Sustaintime = Sustaintime * 1000;
                                if (Sustaintime == 1) Sustaintime = Sustaintime * 1000;
                                if (Sustaintime == 2) Sustaintime = Sustaintime * 1000;
                                if (Sustaintime == 3) Sustaintime = Sustaintime * 1500;
                                if (Sustaintime == 4) Sustaintime = Sustaintime * 2000;
                                
                                Thread.Sleep(Sustaintime);
                                plc31.Write("M22.4", false);
                                AddRecord("调压器升压结束", false);
                            }
                            break;
                            
                        case "DoCommandMyDown":
                            {
                                plc31.Write("M22.5", true);
                                Thread.Sleep(500);
                                if (Sustaintime == 0) Sustaintime = Sustaintime * 1000;
                                if (Sustaintime == 1) Sustaintime = Sustaintime * 1000;
                                if (Sustaintime == 2) Sustaintime = Sustaintime * 1000;
                                if (Sustaintime == 3) Sustaintime = Sustaintime * 1500;
                                if (Sustaintime == 4) Sustaintime = Sustaintime * 2000;
                                Thread.Sleep(Sustaintime);

                                plc31.Write("M22.5", false);
                                AddRecord("调压器降压结束", false);
                            }
                            break;
                       
                        case "DoCommandZB_1": //中变输入合闸 
                            {
                                plc31.Write("M30.6", true);
                                Thread.Sleep(300);
                                plc31.Write("M30.6", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[300] == true)
                                    {
                                        AddRecord("中变输入合闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中变输入合闸无法完成-请检查是否有互锁-或者设备有问题", true);
                                }

                            }
                            break;
                        case "DoCommandZB_2": //中变输入分闸
                            {
                                plc31.Write("M30.7", true);
                                Thread.Sleep(300); 
                                plc31.Write("M30.7", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[300] == false)
                                    {
                                        AddRecord("中变输入分闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("中变输入分闸无法完成-请检查是否有互锁-或者设备有问题", true);
                                }
                            }
                            break;
                        case "DoCommandZCB_1": //支撑变输入合闸
                            {
                                plc31.Write("M32.4", true);
                                Thread.Sleep(300);
                                plc31.Write("M32.4", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[306] == true)
                                    {
                                        AddRecord("支撑变合闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("支撑变合闸无法完成-请检查是否有互锁-或者设备有问题", true);
                                }
                            }
                            break;
                        case "DoCommandZCB_2"://支撑变输入分闸
                            {
                                plc31.Write("M32.5", true);
                                Thread.Sleep(300);
                                plc31.Write("M32.5", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[306] == false)
                                    {
                                        AddRecord("支撑变分闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("支撑变分闸无法完成-请检查是否有互锁-或者设备有问题", true);
                                }
                            }
                            break;
                        case "DoCommandDNBC_1": //电容补偿AC合闸
                            {
                                plc31.Write("M16.0", true); 
                                Thread.Sleep(300); 
                                plc31.Write("M16.0", false);
                                AddRecord("指令已发送..", false);

                            }
                            break;
                        case "DoCommandDNBC_2": //电容补偿AC分闸
                            {
                                plc31.Write("M16.1", true); 
                                Thread.Sleep(300);
                                plc31.Write("M16.1", false);
                                AddRecord("指令已发送..", false);
                            }
                            break;

                        case "DoCommandDNBC_3": //电容补偿b合
                            {
                                plc31.Write("M16.2", true);
                                Thread.Sleep(300);
                                plc31.Write("M16.2", false);
                                AddRecord("指令已发送..", false);
                            }
                            break;
                        case "DoCommandDNBC_4": //电容补偿B分
                            {
                                plc31.Write("M16.3", true); 
                                Thread.Sleep(300);
                                plc31.Write("M16.3", false);
                                AddRecord("指令已发送..", false);
                            }
                            break;
                        case "DoCommandDK_1": //电抗调节 合闸
                            {
                                plc31.Write("M31.0", true);
                                Thread.Sleep(300); 
                                plc31.Write("M31.0", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    Thread.Sleep(1000);
                                    //等待了15秒后，来检查信号。。
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[311] == true)
                                    {
                                        AddRecord("电抗补偿合闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("电抗补偿合闸完成无法完成-请检查是否有互锁-或者设备有问题", true);
                                }
                            }
                            break;
                        case "DoCommandDK_2": //电抗调节 分闸
                            {
                                plc31.Write("M31.1", true); 
                                Thread.Sleep(300); 
                                plc31.Write("M31.1", false);

                                for (IntOutTimer = 0; IntOutTimer < 10; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[311] == false)
                                    {
                                        AddRecord("电抗补偿分闸完成", false);
                                        break;
                                    }
                                }

                                if (IntOutTimer == 10)
                                {
                                    AddRecord("电抗调节分闸无法完成-请检查是否有互锁", true);
                                }
                            }
                            break;
                        case "DoCommandGW1_1"://工位1 AC合
                            {
                                if (alldian_boolinput[402] == false && alldian_boolinput[403] == false &&
                                    alldian_boolinput[404] == false && alldian_boolinput[405] == false)
                                {

                                    plc31.Write("M40.0", true);
                                    Thread.Sleep(500);
                                    plc31.Write("M40.0", false);
                                    Thread.Sleep(500);
                                    //合玩了要去判断会信号...
                                    for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                    {
                                        //等待了15秒后，来检查信号。。
                                        Thread.Sleep(1000);
                                        ReadAllPlcData(false);
                                        if (alldian_boolinput[400] == true)
                                        {
                                            AddRecord("工位1刀闸AC和闸完成", false);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    AddRecord("无法发送指令给工位1刀闸AC合.请检查其他工位是否已经合闸了", true);
                                }
                            }
                            break; 
                        case "DoCommandGW1_2"://工位1 AC分
                            {
                                plc31.Write("M40.1", true);
                                Thread.Sleep(500); 
                                plc31.Write("M40.1", false);
                                Thread.Sleep(500);
                                //合玩了要去判断会信号...
                                for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[406] == true)
                                    {
                                        AddRecord("工位1刀闸AC分闸完成", false);
                                        break;
                                    }
                                }
                            }
                            break;
                        case "DoCommandGW1_4"://工位1 B合
                            {
                                if (alldian_boolinput[402] == false && alldian_boolinput[403] == false &&
                                    alldian_boolinput[404] == false && alldian_boolinput[405] == false)
                                {

                                    plc31.Write("M40.2", true);
                                    Thread.Sleep(500);
                                    plc31.Write("M40.2", false);
                                    Thread.Sleep(500);
                                    //判断信号..
                                    for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                    {
                                        //等待了15秒后，来检查信号。。
                                        Thread.Sleep(1000);
                                        ReadAllPlcData(false);
                                        if (alldian_boolinput[401] == true)
                                        {
                                            AddRecord("工位1刀闸B合闸完成", false);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    AddRecord("工位1刀闸B合闸无法完成-请检查其他工位是否合闸", true);
                                }

                            }
                            break; 
                        case "DoCommandGW1_3"://工位1 B分
                            {
                                plc31.Write("M40.3", true); 
                                Thread.Sleep(500); 
                                plc31.Write("M40.3", false);
                                Thread.Sleep(500);
                                for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[407] == true)
                                    {
                                        AddRecord("工位1刀闸B分闸完成", false);
                                        break;
                                    }
                                }

                            }
                            break;

                        case "DoCommandGW2_1"://工位2 AC合
                            {
                                if (alldian_boolinput[400] == false && alldian_boolinput[401] == false &&
                                    alldian_boolinput[404] == false && alldian_boolinput[405] == false)
                                {

                                    plc31.Write("M40.4", true);
                                    Thread.Sleep(500);
                                    plc31.Write("M40.4", false);
                                    Thread.Sleep(500);
                                    for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                    {
                                        //等待了15秒后，来检查信号。。
                                        Thread.Sleep(1000);
                                        ReadAllPlcData(false);
                                        if (alldian_boolinput[402] == true)
                                        {
                                            AddRecord("工位2刀闸AC合闸完成", false);
                                            break;
                                        }
                                    }
                                } 
                                else
                                {
                                    AddRecord("工位2刀闸AC合闸无法完成-请检查其他工位是否合闸", true);
                                }
                            }
                            break; 
                        case "DoCommandGW2_2"://工位2 AC分
                            {
                                plc31.Write("M40.5", true); 
                                Thread.Sleep(500); 
                                plc31.Write("M40.5", false);
                                Thread.Sleep(500);
                                for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[410] == true)
                                    {
                                        AddRecord("工位2刀闸AC分闸完成", false);
                                        break;
                                    }
                                }
                            }
                            break; 
                        case "DoCommandGW2_4": //工位2 B合
                            {
                                if (alldian_boolinput[400] == false && alldian_boolinput[401] == false &&
                                    alldian_boolinput[404] == false && alldian_boolinput[405] == false)
                                {
                                    plc31.Write("M40.6", true);
                                    Thread.Sleep(500);
                                    plc31.Write("M40.6", false);
                                    Thread.Sleep(500);
                                    for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                    {
                                        //等待了15秒后，来检查信号。。
                                        Thread.Sleep(1000);
                                        ReadAllPlcData(false);
                                        if (alldian_boolinput[403] == true)
                                        {
                                            AddRecord("工位2刀闸B合闸完成", false);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    AddRecord("工位2刀闸B合闸无法完成-请检查其他工位是否合闸", true);
                                }
                            } 
                            break;
                        case "DoCommandGW2_3"://工位2 B分
                            {
                                plc31.Write("M40.7", true); 
                                Thread.Sleep(500); 
                                plc31.Write("M40.7", false);
                                Thread.Sleep(500);

                                for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[411] == true)
                                    {
                                        AddRecord("工位2刀闸B分闸完成", false);
                                        break;
                                    }
                                }

                            }
                            break;
                        case "DoCommandGW3_1":// 工位3 AC合
                            {
                                if (alldian_boolinput[400] == false && alldian_boolinput[401] == false &&
                                    alldian_boolinput[402] == false && alldian_boolinput[403] == false )
                                {

                                    plc31.Write("M41.0", true);
                                    Thread.Sleep(500);
                                    plc31.Write("M41.0", false);
                                    Thread.Sleep(500);

                                    for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                    {
                                        Thread.Sleep(1000);
                                        //等待了15秒后，来检查信号。。
                                        ReadAllPlcData(false);
                                        if (alldian_boolinput[404] == true)
                                        {
                                            AddRecord("工位3刀闸 AC合闸完成", false);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    AddRecord("无法发送指令给工位3刀闸AC合.请检查其他工位是否已经合闸了", true);
                                }
                            }
                            break; 
                        case "DoCommandGW3_2": // 工位3 AC分
                            {
                                plc31.Write("M41.1", true);
                                Thread.Sleep(500);
                                plc31.Write("M41.1", false);
                                Thread.Sleep(500);

                                for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[412] == true)
                                    {
                                        AddRecord("工位3刀闸AC分闸完成", false);
                                        break;
                                    }
                                }
                            }
                            break;
                        case "DoCommandGW3_3":// 工位3 B分
                            {
                                plc31.Write("M41.3", true);
                                Thread.Sleep(500);
                                plc31.Write("M41.3", false);
                                Thread.Sleep(500);
                                for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                {
                                    //等待了15秒后，来检查信号。。
                                    Thread.Sleep(1000);
                                    ReadAllPlcData(false);
                                    if (alldian_boolinput[413] == true)
                                    {
                                        AddRecord("工位3刀闸B分闸完成", false);
                                        break;
                                    }
                                }

                            }
                            break; 
                        case "DoCommandGW3_4":// 工位3 B合
                            {

                                if (alldian_boolinput[400] == false && alldian_boolinput[401] == false &&
                                    alldian_boolinput[402] == false && alldian_boolinput[403] == false)
                                {
                                    plc31.Write("M41.2", true);
                                    Thread.Sleep(500);
                                    plc31.Write("M41.2", false);
                                    Thread.Sleep(500);
                                    for (IntOutTimer = 0; IntOutTimer < 15; IntOutTimer++)
                                    {
                                        //等待了15秒后，来检查信号。。
                                        Thread.Sleep(1000);
                                        ReadAllPlcData(false);
                                        if (alldian_boolinput[405] == true)
                                        {
                                            AddRecord("工位3刀闸B合闸完成", false);
                                            break;
                                        }
                                    }                                    
                                }
                                else
                                {
                                    AddRecord("无法发送指令给工位3刀闸B合.请检查其他工位是否已经合闸了",true);
                                }


                            }
                            break; 
                    }
                    Success = true;
                    logger.Debug("新线程-执行完成结束" + command);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    AddRecord((string)resources["PLCMessage2"] + error, true);
                    logger.Error("新线程-任务执行异常:" + error);
                    //AddRecord("PLC异常:" + error, true);
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                AddRecord((string)resources["PLCMessage3"], true);
                logger.Error("执行线程-超时" + command);
                //AddRecord("PLC超时,请检查PLC是否正常!", true);                
                return false;
            }

            if (Success)
            {
                //AddRecord("指令成功执行完成", true);
                logger.Debug("执行线程-完成" + command);
                return true;
            }
            else
            {
                AddRecord((string)resources["PLCMessage2"], true);
                logger.Error("执行线程-错误Success==false" + command);
                //AddRecord("PLC异常:" + error, true);             
                return false;
            }
        }
            
        

        void AddRecord(string strdata,bool e)
        {            
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string str = time + "=>" + strdata;
            Application.Current.Dispatcher.Invoke(() =>
            {                
                ListBoxData.Add(new Item { Text = str, IsRed = e });
            });
            //WeakReferenceMessenger.Default.Send<string, string>("ScrollEnd", "ScrollEnd");            
        }

        public async void InitializeAsync()
        {
            //先打开端口...
            try
            {
                //TCP也OK了...
                /*
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 502);
                master=ModbusIpMaster.CreateIp(tcpClient);
                master.Transport.WriteTimeout = 2000;
                master.Transport.ReadTimeout = 2000;
                master.Transport.WaitToRetryMilliseconds = 500;
                master.Transport.Retries = 3;
                */
                
                //笔记本使用，PC
                
                AddRecord((string)resources["PLCMessage18"], false);
                logger.Debug("启动完成");

                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议

                master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                master.Transport.ReadTimeout = 500;// 设置超时时间默认为500毫秒
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_COM"];
                serialPort.Open(); //打开串口                
                AddRecord((string)resources["PLCMessage4"], false);
                
                
                //AddRecord("打开串口完成",false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            rotobTaskT = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;

                logger.Debug("机器人线程-初始化链接33");
                var myresult3 = await ExePlcCommand("33", true);
                //if (!myresult3) { AddRecord("连接PLC主机失败-后台线程退出,PLC正常后请重新启动软件!", true); return; }
                if (!myresult3) { AddRecord((string)resources["PLCMessage5"], true); return; }
                //AddRecord("连接PLC主机成功", false);
                AddRecord((string)resources["PLCMessage6"], false);

                logger.Debug("机器人线程-初始化链接34");
                myresult3 = await ExePlcCommand("34", true);
                //if (!myresult3) { AddRecord("连接PLC从机失败-后台线程退出,PLC正常后请重新启动软件!", true); return; }
                if (!myresult3) { AddRecord((string)resources["PLCMessage7"], true); return; }
                //AddRecord("连接PLC从机成功", false);
                AddRecord((string)resources["PLCMessage8"], false);

                logger.Debug("机器人线程-初始化链接35");
                myresult3 = await ExePlcCommand("35", true);
                //if (!myresult3) { AddRecord("连接PLC主机失败-后台线程退出,PLC正常后请重新启动软件!", true); return; }
                if (!myresult3) { AddRecord((string)resources["PLCMessage7"], true); return; }
                //AddRecord("连接PLC从机成功", false);
                AddRecord((string)resources["PLCMessage8"], false);

                logger.Debug("机器人线程-初始化链接36");
                myresult3 = await ExePlcCommand("36", true);
                //if (!myresult3) { AddRecord("连接PLC从机失败-后台线程退出,PLC正常后请重新启动软件!", true); return; }
                if (!myresult3) { AddRecord((string)resources["PLCMessage7"], true); return; }
                //AddRecord("连接PLC从机成功", false);
                AddRecord((string)resources["PLCMessage8"], false);

                //var myresult1 = await ExePlcCommand("500", true, 1);

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        //iswork = false;
                        //这里是需要TRY的。。 我执行代码的时候，遇到了一个问题，就是主线程没有加线程分发.. 导致.. 报错..
                        Thread.Sleep(10);
                        System.DateTime currentTime = System.DateTime.UtcNow;

                        TimeSpan ts = (TimeSpan)(currentTime - precurrentTime);
                        //得到毫秒数...
                        double milliseconds = ts.TotalMilliseconds;
                        
                        //定时时间到了..
                        
                        if (milliseconds > 1000)
                        {
                            logger.Debug("机器人线程-定时器1秒任务");
                            //设置为后台执行..
                            var myresult1 = await ExePlcCommand("500", true,1 );
                            //必须要判断，要不然 子线程他不等..
                            if (!myresult1)
                            {
                                AddRecord((string)resources["PLCMessage9"], true);
                                //AddRecord("定时器指令读取PLC执行失败-后台线程退出!", true);
                                return;
                            }
                            precurrentTime = currentTime;
                            continue;
                        }
                        
                        if (queue.Count <= 0) continue;
                        BKCommand Command = queue.Dequeue();
                        logger.Debug("机器人线程-点击" + Command.Command);
                        var myresult2 = await ExePlcCommand(Command.Command, Command.Data);


                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (commandExeWaitWindow != null)
                            {
                                commandExeWaitWindow.Close();
                                commandExeWaitWindow = null;
                            }                                
                        });
                        
                        

                        if (myresult2)
                        {
                            //AddRecord((string)resources["PLCMessage10"], false);
                            //AddRecord("指令执行成功!", false);
                        }
                        else
                        {
                            AddRecord((string)resources["PLCMessage11"], true);
                            //AddRecord("指令执行失败!-后台线程退出", true);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        AddRecord($"{ex.Message}", true);
                        logger.Error("机器人线程报错"+ ex.Message);
                        return;
                    }
                }
            }, cts.Token);
        }


    }

    public class Item
    {
        public string Text { get; set; }
        public bool IsRed { get; set; }
    }

    public class BKCommand
    {
        public string Command { get; set; } = string.Empty;
        public bool Data { get; set; } = false;
    }
}
