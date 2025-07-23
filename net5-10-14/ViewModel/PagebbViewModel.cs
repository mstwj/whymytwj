using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.Base;
using net5_10_14.Devcie;
using net5_10_14.Table;
using net5_10_14.Table.bbsy;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace net5_10_14.ViewModel
{

    public class PagebbViewModel : INotifyPropertyChanged
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
      
        private bool _isActive1;
        public bool IsActive1
        {
            get { return _isActive1; }
            set
            {
                _isActive1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive1"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource1"));
            }
        }

        public ImageSource ImageSource1
        {
            get
            {
                return IsActive1 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }

        //1.任何实验都要2个表.. 表1产品 表2标准 
        public newproinfo m_newproinfo { get; set; } = null;        
        public experimentstandard_bianbishiyan m_experimentstandard_bianbishiyan { get; set; } = null;
        public bb_report m_bb_report { get; set; } = new bb_report();

        public ObservableCollection<DataRecord> DataRecordList { get; set; } = new ObservableCollection<DataRecord>();

        public DataRecord SelectedDataRecord { get; set; }

        public ICommand BtnCommandSendHand { get; set; } //

        public ICommand BtnCommandSendRead { get; set; } //
        public ICommand BtnCommandSendSet { get; set; } //
        public ICommand BtnCommandSendStart { get; set; } //
        public ICommand BtnCommandSendStop { get; set; } //
        public ICommand BtnCommandOpenprot { get; set; } //
        public ICommand BtnCommandCloseprot { get; set; } //
        public ICommand BtnCommandSaveReport { get; set; } //

        public ICommand BtnCommandSendAuto { get; set; } //


        public ObservableCollection<string> ListBoxData { get; set; } = new ObservableCollection<string>();


        private Robot_BYQBBCSYSerial myRobot;        
        
        //private Plc plc;
        private Button AutoButton = null;
        private int MyDeviceInitPlc = 0;
        private int MyDeviceInitBianbi = 0;


        //这里县城没有亭....
        public async Task<bool> PagebbViewModeinit()
        {

            WeakReferenceMessenger.Default.Send<string, string>("开始初始化", "InitShowProc");
            WeakReferenceMessenger.Default.Send<string, string>("PLC设备检查....", "SetShowProc");
            //1.
            {
                PushCommand(500);
                //如果联机失败怎么办....                
                await Task.Delay(3000);

                if (MyDeviceInitPlc == -1 || MyDeviceInitPlc == 0)
                {
                    string str = "PLC设备连接失败,请看配置是否正确, 线路是否正常,机器是否开启,重新启动软件在测试一次!";                    
                    WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                    return false;
                }
            }


            //1.openprot;
            {
                //变比是第4个设备..
                string portstr = ConfigurationManager.AppSettings["DEVICE_4"];
                var result = myRobot.RobotOpen(portstr);
                if (result.Item2 == false)
                {

                    string str = "串口打开失败,请看配置是否正确, 线路是否正常,重新启动软件在测试一次!";
                    //string str = "串口" + portstr + "错误:失败原因" + result.Item1;
                    WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                    return false;
                }
                MyDebugWrite("串口" + portstr + "打开成功");
            }

            //2.
            {
                PushCommand(1);
                //必须等3秒。。。。。。等设备回来..
                WeakReferenceMessenger.Default.Send<string, string>("变比设备检查....", "SetShowProc");
                await Task.Delay(2000);
                //如果联机失败怎么办....                    
                if (MyDeviceInitBianbi == -1 || MyDeviceInitBianbi == 0)
                {
                    string str = "变比设备连接失败,请看配置是否正确, 线路是否正常,机器是否开启,重新启动软件在测试一次!";                    
                    WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                    return false;
                }
            }

            {                
                using (var context = new MyDbContext())
                {
                    var firstEntity = context.newproinfo.FirstOrDefault(e => e.ProName == App_Config.currendProName);
                    if (firstEntity != null)
                    {
                        m_newproinfo = firstEntity.Clone();
                    }
                    else
                    {                        
                        string str = "数据库严重错误,样品查询失败!";
                        WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                        return false;
                    }

                    //获取单个实体（第一个匹配）
                    var firstEntity2 = context.experimentstandard_bianbishiyan.FirstOrDefault(e => e.Guigexinhao == m_newproinfo.GuigeXinhao && e.Tuhao == m_newproinfo.Tuhao);

                    if (firstEntity2 != null)
                    {
                        m_experimentstandard_bianbishiyan = firstEntity2.Clone();
                    }
                    else
                    {
                        string str = "数据库严重错误,样品没有对比标准，无法判断合格!";
                        WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                        return false;
                    }
                }

                //发布..
                WeakReferenceMessenger.Default.Send<string, string>("BBShowProc", "BBShowProc");

                float tempnumber = (float)(m_newproinfo.GaoyaedingDianya / m_newproinfo.Taoyabili);
                float temnumber1 = (float)(m_newproinfo.GaoyaedingDianya * (1 - m_newproinfo.Taoyabili / 100));
                float tempnumber2 = (float)((m_newproinfo.GaoyaedingDianya * ((2 * m_newproinfo.Taoyabili) / 100)) / (m_newproinfo.DanWeiNumber - 1));
                float temp3;
                float temp4;

                for (int i = 0; i < m_newproinfo.DanWeiNumber; i++)
                {
                    DataRecord temp = new DataRecord();

                    temp.Datagaoyache = (int)(temnumber1 + i * tempnumber2);
                    temp.Datadiyache = (int)m_newproinfo.DiyaedingDianya;

                    temp.Id = i;
                    temp3 = temp.Datagaoyache;
                    temp4 = temp.Datadiyache;

                    temp.Dataelbb = (float)(temp3 / temp4);
                    temp.Datazbfs = m_newproinfo.BiaoHao;
                    int result;
                    if (int.TryParse(m_newproinfo.ZhuHao, out result))
                    {
                        temp.Datadqfj = result;
                    }
                    else  temp.Datadqfj = 0;
                    DataRecordList.Add(temp);
                }
                
                WeakReferenceMessenger.Default.Send<string, string>("全部设备检查完成....", "SetShowProc");
                await Task.Delay(2000);
                WeakReferenceMessenger.Default.Send<string, string>("NULL", "CloseShowProc");

                return true;
            }

        }

        public bool PagebbPlcSwitch()
        {
            /*
            plc.Write("M1.1", true);
            Thread.Sleep(200);
            plc.Write("M1.1", false);
            Thread.Sleep(200);
            */

            PushCommand(10);

            return true;
        }

        public void RotobCallBack(object sender, Page1_RotobCallBackArg e)
        {
            if (e.currErrorcode == RotobErrorCode.Ready)
            {
                MyDebugWrite(e.currMessage);
                return;
            }

            MyDebugWrite(e.currMessage);

            if (e.currErrorcode == RotobErrorCode.ChartMateFail ||
                e.currErrorcode == RotobErrorCode.Error ||
                e.currErrorcode == RotobErrorCode.TimerOver)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (AutoButton != null) AutoButton.IsEnabled = true;
                    MyDeviceInitPlc = -1;
                    MyDeviceInitBianbi = -1;
                });
                
                //MessageBox.Show("运行发生错误,请看机器人日志!");
                return;
            }

            if (e.currErrorcode == RotobErrorCode.Success)
            {
                switch (e.currCommand)
                {
                    case 1: IsActive1 = true; MyDeviceInitBianbi = 1; break;
                    case 2: break;
                    case 3:
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                AutoButton.IsEnabled = true;
                            });
                        }
                        break;
                    case 4: break;
                    case 5: break;
                    case 6:
                        {
                            string tempzbfs = "未知";
                            switch (e.datazbfs)
                            {
                                case 0: tempzbfs = "无"; break;
                                case 1: tempzbfs = "Y/D"; break;
                                case 2: tempzbfs = "D/Y"; break;
                                case 3: tempzbfs = "Y/Y"; break;
                                case 4: tempzbfs = "D/D"; break;
                                case 5: tempzbfs = "Zn/Yn"; break;
                                case 6: tempzbfs = "Zn/D"; break;
                                case 7: tempzbfs = "YN/d"; break;
                                case 8: tempzbfs = "D/yn"; break;
                                case 9: tempzbfs = "YN/y"; break;
                                case 10: tempzbfs = "YN/y"; break;
                                case 11: tempzbfs = "Y/yn"; break;
                                case 12: tempzbfs = "YN/yn"; break;
                            }
                            SelectedDataRecord.Datazbfs = tempzbfs;
                            SelectedDataRecord.Datajx = e.datajx;
                            //SelectedDataRecord.Dataelbb = e.dataelbb;
                            SelectedDataRecord.Datafjjk = e.datafjjk;
                            SelectedDataRecord.DataKAB = e.dataKAB;
                            SelectedDataRecord.DataKBC = e.dataKBC;
                            SelectedDataRecord.DataKCA = e.dataKCA;//KCA相比
                            SelectedDataRecord.DataEAB = e.dataEAB;//EAB误差
                            SelectedDataRecord.DataEBC = e.dataEBC;//EBC误差
                            SelectedDataRecord.DataECA = e.dataECA;  //ECA误差
                            SelectedDataRecord.Dataclfs = e.dataclfs; //测量方式
                            
                            if (Math.Abs(e.dataEAB) > m_experimentstandard_bianbishiyan.Bianbiwucha ||
                                Math.Abs(e.dataEBC) > m_experimentstandard_bianbishiyan.Bianbiwucha ||
                                Math.Abs(e.dataECA) > m_experimentstandard_bianbishiyan.Bianbiwucha)
                            {
                                SelectedDataRecord.DataHGPD  = "不合格";
                            }
                            else
                            {
                                SelectedDataRecord.DataHGPD = "合格";
                            }
                        }
                        break;

                    case 7:
                        DoBtnCommandSaveReport(null);
                        break;
                    case 500:
                        MyDeviceInitPlc = 1;
                        break;

                }
            }
        }


        public PagebbViewModel()
        {
            // 记住Gdtat有自己的datasoushiz。。 要指定来设置..
            IsActive1 = false; 
            MyDebugWrite("启动完成!");

            BtnCommandSendAuto = new RelayCommand<object>(DoBtnCommandSendAtuo);
            BtnCommandSendHand = new RelayCommand<object>(DoBtnCommandSendHand);
            BtnCommandSendRead = new RelayCommand<object>(DoBtnCommandSendRead);
            BtnCommandSendSet = new RelayCommand<object>(DoBtnCommandSendSet);
            BtnCommandSendStart = new RelayCommand<object>(DoBtnCommandSendStart);
            BtnCommandSendStop = new RelayCommand<object>(DoBtnCommandSendStop);
            BtnCommandOpenprot = new RelayCommand<object>(DoBtnCommandOpenprot);
            BtnCommandCloseprot = new RelayCommand<object>(DoBtnCommandCloseprot);
            BtnCommandSaveReport = new RelayCommand<object>(DoBtnCommandSaveReport);


            myRobot = new Robot_BYQBBCSYSerial();
            myRobot.RotobCallBackEvent += RotobCallBack;
            myRobot.Start();
        }


        //默认是单指令..(多指令，第2仓数必须是0)
        RotbotPushArg PushCommand(int Command, int andExe = 1)
        {     
            if (Command == 1)
            {
                IsActive1 = false;
            }

            if (Command == 3000)
            {
                RotbotPushArg sp = new RotbotPushArg();
                sp.command = 3000;
                sp.reserve = andExe.ToString();
                return sp;
            }

            if (Command == 4)
            {
                RotbotPushArg sp = new RotbotPushArg();
                sp.command = 4;
                sp.reserve = "1";
                return sp;
            }

            RotbotPushArg listarg = new RotbotPushArg();
            listarg.command = Command;
            if (andExe == 1)
            {
                if (!myRobot.Push(listarg))
                {
                    MyDebugWrite("机器人忙" + Command.ToString());
                }
            }
            return listarg;
        }

        private void DoBtnCommandSendAtuo(object button)
        {
            //先坚持联机 ..
            if (IsActive1 == false)
            {
                MessageBox.Show("联机失败!请检查设备是否开启,关闭软件重新启动在测试!");
                return;
            }

            Button mybutton = button as Button;

            AutoButton = mybutton;
            mybutton.IsEnabled = false;


            List<RotbotPushArg> listAll = new List<RotbotPushArg>();
            RotbotPushArg retust1 = PushCommand(3000, 5000); //等待5秒..
            RotbotPushArg retust2 = PushCommand(4, 0); //设置3 .
            RotbotPushArg retust3 = PushCommand(3000, 5000); //等待5秒..
            RotbotPushArg retust4 = PushCommand(2,0); //启动 .
            RotbotPushArg retust5 = PushCommand(3000, 15000); //等待15秒..
            RotbotPushArg retust6 = PushCommand(6, 0); //读取
            RotbotPushArg retust7 = PushCommand(3000, 5000); //等待5秒..
            RotbotPushArg retust8 = PushCommand(7, 0); //发信息给主程序 -- 写入数据库.
            RotbotPushArg retust9 = PushCommand(3000, 10000); //等待10秒..
            RotbotPushArg retust10 = PushCommand(3, 0); //停止
            RotbotPushArg retust11 = PushCommand(3000, 10000); //等待10秒..



            listAll.Add(retust1);
            listAll.Add(retust2);
            listAll.Add(retust3);
            listAll.Add(retust4);
            listAll.Add(retust5);
            listAll.Add(retust6);
            listAll.Add(retust7);
            listAll.Add(retust8);
            listAll.Add(retust9);
            listAll.Add(retust10);

            myRobot.Push(listAll);


        }
        

        private void DoBtnCommandSendHand(object button) 
        {
            //SelectedDataRecord.DataKAB = 11.5f;
            PushCommand(1);

            return; 
        }
        private void DoBtnCommandSendRead(object button) 
        {
            PushCommand(6);
        }

        private void DoBtnCommandSendSet(object button)
        {
            PushCommand(4);
            return;
        }
        private void DoBtnCommandSendStart(object button)
        {
            PushCommand(2);
            return;
        }
        private void DoBtnCommandSendStop(object button) 
        {
            PushCommand(3);
            return; 
        }
        private void DoBtnCommandOpenprot(object button)
        {
            //变比是第4个设备..
            string portstr = ConfigurationManager.AppSettings["DEVICE_4"];
            var result = myRobot.RobotOpen(portstr);
            if (result.Item2 == false)
            {
                string str = "串口" + portstr + "错误:失败原因" + result.Item1;
                MessageBox.Show(str);
                return;
            }
            MyDebugWrite("串口" + portstr + "打开成功");
            return;
        }

        private void DoBtnCommandCloseprot(object button) 
        {
            myRobot.RobotClose();

            MyDebugWrite("关闭串口");

            return;
        }

        void DataStatistics()
        {
            //最后一条指令，坑顶是保持...            

            //这里还必须这样搞...(iD设置为0表示，我不提供ID 数据库自己去管理ID..)
            m_bb_report.Id = 0;
            m_bb_report.ProName = m_newproinfo.ProName; //编号
            m_bb_report.Guigexinhao = m_newproinfo.GuigeXinhao;
            m_bb_report.Tuhao = m_newproinfo.Tuhao;

            m_bb_report.Datatimer = DateTime.Now.ToString();
            m_bb_report.Datazbfs = SelectedDataRecord.Datazbfs; //解法..

            int result = 0;
            if (int.TryParse(m_newproinfo.ZhuHao, out result))
            {
                m_bb_report.Datadqfj = result;
            } else m_bb_report.Datadqfj = 0;




            m_bb_report.Datajx = SelectedDataRecord.Datajx;


            m_bb_report.Dataelbb = SelectedDataRecord.Dataelbb;
            m_bb_report.Dataelbb = (double)Math.Round(m_bb_report.Dataelbb, 3);

            m_bb_report.Datafjjk = SelectedDataRecord.Datafjjk;
            m_bb_report.Datafjjk = (double)Math.Round(m_bb_report.Datafjjk, 3);


            m_bb_report.DataKAB = SelectedDataRecord.DataKAB;
            m_bb_report.DataKAB = (double)Math.Round(m_bb_report.DataKAB, 3);

            m_bb_report.DataKBC = SelectedDataRecord.DataKBC;
            m_bb_report.DataKBC = (double)Math.Round(m_bb_report.DataKBC, 3);

            m_bb_report.DataKCA = SelectedDataRecord.DataKCA;
            m_bb_report.DataKCA = (double)Math.Round(m_bb_report.DataKCA, 3);

            m_bb_report.DataEAB = SelectedDataRecord.DataEAB;
            m_bb_report.DataEAB = (double)Math.Round(m_bb_report.DataEAB, 3);

            m_bb_report.DataEBC = SelectedDataRecord.DataEBC;
            m_bb_report.DataEBC = (double)Math.Round(m_bb_report.DataEBC, 3);

            m_bb_report.DataECA = SelectedDataRecord.DataECA;
            m_bb_report.DataECA = (double)Math.Round(m_bb_report.DataECA, 3);

            m_bb_report.Dataclfs = SelectedDataRecord.Dataclfs;

            if (SelectedDataRecord.DataHGPD == null)
            {
                m_bb_report.DataHGPD = "不合格";
            }
            else
            {
                m_bb_report.DataHGPD = SelectedDataRecord.DataHGPD;
            }           
        }

        private void DoBtnCommandSaveReport(object button) 
        {
            DataStatistics();//数据收集.. .
            using (var context = new MyDbContext())
            {
                //验证...
                var myitem = Tools.SafeScanPro("bb_report", m_bb_report);
                if (myitem.Item1 == false)
                {
                    MyDebugWrite($"验证数据失败{myitem.Item2}");
                    return;
                }
                try
                {
                    context.bb_report.Add(m_bb_report);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功
                        MyDebugWrite("添加报告成功");
                    }
                    else
                    {
                        MyDebugWrite("添加报告失败");
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    logger.Error(message);
                    MessageBox.Show("严重错误数据库报错,请检查日志,查看数据库是否正常");
                }
            }

            return; 
        }


        void MyDebugWrite(string strdata)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string str = time + "===>" + strdata;

            Application.Current.Dispatcher.Invoke(() =>
            {
                ListBoxData.Insert(0, str);
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataRecord : INotifyPropertyChanged
    {
        private int _id { get; set; }
        private int  _datagaoyache { get; set; } //高压
        private int _datadiyache { get; set; } //抵押

        private string _datatimer { get; set; } //时间.
        private string _datazbfs { get; set; }  //组别方式.
        private int _datadqfj { get; set; } //当前分接
        private int _datajx { get; set; }   //及性 (0,1,其他)
        private float _dataelbb { get; set; } //额定变比
        private float _datafjjk { get; set; } //分接间距
        private float _dataKAB { get; set; }  //KAB相比
        private float _dataKBC { get; set; }//KBC相比
        private float _dataKCA { get; set; }//KCA相比
        private float _dataEAB { get; set; }//EAB误差
        private float _dataEBC { get; set; }//EBC误差
        private float _dataECA { get; set; } //ECA误差
        private int _dataclfs { get; set; } //测量方式
        private string _datahgpd { get; set; } //合格..


        public int Id { get { return _id; } set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id")); } }

        public int Datagaoyache { get { return _datagaoyache; } set { _datagaoyache = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datagaoyache")); } }

        public int Datadiyache { get { return _datadiyache; } set { _datadiyache = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datadiyache")); } }


        public string Datatimer { get { return _datatimer; } set { _datatimer = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datatimer")); } }
        public string Datazbfs { get { return _datazbfs; } set { _datazbfs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datazbfs")); } }
        public int Datadqfj { get { return _datadqfj; } set { _datadqfj = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datadqfj")); } }
        public int Datajx { get { return _datajx; } set { _datajx = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datajx")); } }
        public float Dataelbb
        {
            get { return _dataelbb; }
            set
            {
                _dataelbb = value;
                _dataelbb = (float)Math.Round(_dataelbb, 3);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dataelbb"));
            }
        }
        public float Datafjjk { get { return _datafjjk; } set { _datafjjk = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datafjjk")); } }
        public float DataKAB { get { return _dataKAB; } set { _dataKAB = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataKAB")); } }
        public float DataKBC { get { return _dataKBC; } set { _dataKBC = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataKBC")); } }
        public float DataKCA
        {
            get
            {
                return _dataKCA;
            }
            set
            {
                _dataKCA = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataKCA"));
            }
        }
        public float DataEAB { get { return _dataEAB; } set { _dataEAB = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataEAB")); } }
        public float DataEBC { get { return _dataEBC; } set { _dataEBC = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataEBC")); } }
        public float DataECA { get { return _dataECA; } set { _dataECA = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataECA")); } }
        public int Dataclfs { get { return _dataclfs; } set { _dataclfs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dataclfs")); } }

       
        public string DataHGPD { get { return _datahgpd; } set { _datahgpd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataHGPD")); } }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
