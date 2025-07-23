using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.Base;
using net5_10_14.Devcie;
using net5_10_14.Table;
using net5_10_14.Table.bbsy;
using net5_10_14.Table.zzsy;
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

    public class PagezzViewModel : INotifyPropertyChanged
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private float _dataab, _databc, _dataca, _abbccaxjc ,_dataan, _databn, _datacn, _anbncnxjc;
        private string _datahgqd;

        public float Dataab { get { return _dataab; } set { _dataab = value; OnPropertyChanged(nameof(Dataab)); } }
        public float Databc { get { return _databc; } set { _databc = value; OnPropertyChanged(nameof(Databc)); } }
        public float Dataca { get { return _dataca; } set { _dataca = value; OnPropertyChanged(nameof(Dataca)); } }
        public float Dabbccaxjc { get { return _abbccaxjc; } set { _abbccaxjc = value; OnPropertyChanged(nameof(Dabbccaxjc)); } }
        public float Dataan { get { return _dataan; } set { _dataan = value; OnPropertyChanged(nameof(Dataan)); } }
        public float Databn { get { return _databn; } set { _databn = value; OnPropertyChanged(nameof(Databn)); } }
        public float Datacn { get { return _datacn; } set { _datacn = value; OnPropertyChanged(nameof(Datacn)); } }
        public float Dnbncnxjc { get { return _anbncnxjc; } set { _anbncnxjc = value; OnPropertyChanged(nameof(Dnbncnxjc)); } }
        public string Datahgqd { get { return _datahgqd; } set { _datahgqd = value; OnPropertyChanged(nameof(Datahgqd)); } }

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

        private bool _isActive2;
        public bool IsActive2
        {
            get { return _isActive2; }
            set
            {
                _isActive2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive2"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource2"));
            }
        }

        public ImageSource ImageSource2
        {
            get
            {
                return IsActive2 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive3;
        public bool IsActive3
        {
            get { return _isActive3; }
            set
            {
                _isActive3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive3"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource3"));
            }
        }

        public ImageSource ImageSource3
        {
            get
            {
                return IsActive3 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive4;
        public bool IsActive4
        {
            get { return _isActive4; }
            set
            {
                _isActive4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive4"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource4"));
            }
        }

        public ImageSource ImageSource4
        {
            get
            {
                return IsActive4 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive5;
        public bool IsActive5
        {
            get { return _isActive5; }
            set
            {
                _isActive5 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive5"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource5"));
            }
        }

        public ImageSource ImageSource5
        {
            get
            {
                return IsActive5 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive6;
        public bool IsActive6
        {
            get { return _isActive6; }
            set
            {
                _isActive6 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive6"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource6"));
            }
        }

        public ImageSource ImageSource6
        {
            get
            {
                return IsActive6 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive7;
        public bool IsActive7
        {
            get { return _isActive7; }
            set
            {
                _isActive7 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive7"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource7"));
            }
        }

        public ImageSource ImageSource7
        {
            get
            {
                return IsActive7 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive8;
        public bool IsActive8
        {
            get { return _isActive8; }
            set
            {
                _isActive8 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive8"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource8"));
            }
        }

        public ImageSource ImageSource8
        {
            get
            {
                return IsActive8 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive9;
        public bool IsActive9
        {
            get { return _isActive9; }
            set
            {
                _isActive9 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive9"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource9"));
            }
        }

        public ImageSource ImageSource9
        {
            get
            {
                return IsActive9 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive10;
        public bool IsActive10
        {
            get { return _isActive10; }
            set
            {
                _isActive10 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive10"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource10"));
            }
        }

        public ImageSource ImageSource10
        {
            get
            {
                return IsActive10 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive11;
        public bool IsActive11
        {
            get { return _isActive11; }
            set
            {
                _isActive11 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive11"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource11"));
            }
        }

        public ImageSource ImageSource11
        {
            get
            {
                return IsActive11 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive12;
        public bool IsActive12
        {
            get { return _isActive12; }
            set
            {
                _isActive12 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive12"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource12"));
            }
        }

        public ImageSource ImageSource12
        {
            get
            {
                return IsActive12 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private bool _isActive13;
        public bool IsActive13
        {
            get { return _isActive13; }
            set
            {
                _isActive13 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive13"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource13"));
            }
        }

        public ImageSource ImageSource13
        {
            get
            {
                return IsActive13 ? new BitmapImage(new Uri(@"pack://application:,,,/Asset/1.png"))
                               : new BitmapImage(new Uri(@"pack://application:,,,/Asset/2.png"));
            }
        }


        private int PresCommand = 0; //记录上一条指令...
        private Button AutoButton = null;

        //1.任何实验都要2个表.. 表1产品 表2标准 
        public newproinfo m_newproinfo { get; set; } = null;        
        public experimentstandard_ziliudianzushiyan m_experimentstandard_ziliudianzushiyan { get; set; } = null;
        public zz_report m_zz_report { get; set; } = new zz_report();

        public ObservableCollection<ZZDataRecord> DataRecordList { get; set; } = new ObservableCollection<ZZDataRecord>();

        public ZZDataRecord SelectedDataRecord { get; set; }        

        public ICommand BtnCommandSendHand_G { get; set; }
        public ICommand BtnCommandSendRead_G { get; set; }
        public ICommand BtnCommandSendSet_G { get; set; }
        public ICommand BtnCommandSendStart_G { get; set; }
        public ICommand BtnCommandSendStop_G { get; set; }
        public ICommand BtnCommandOpenprot_G { get; set; }
        public ICommand BtnCommandSendSave_G { get; set; }
        

        public ICommand BtnCommandCloseprot_G { get; set; }
        public ICommand BtnCommandSendHand_D { get; set; }
        public ICommand BtnCommandSendRead_D { get; set; }
        public ICommand BtnCommandSendStart_D { get; set; }
        public ICommand BtnCommandSendStop_D { get; set; }
        public ICommand BtnCommandSendAtuo_D { get; set; }
        

        public ICommand BtnCommandOpenprot_D { get; set; }
        public ICommand BtnCommandCloseprot_D { get; set; }

        public ICommand BtnCommandSet_M00 { get; set; }
        public ICommand BtnCommandSet_M01 { get; set; }
        public ICommand BtnCommandSet_M02 { get; set; }
        public ICommand BtnCommandSet_M03 { get; set; }
        public ICommand BtnCommandSet_M04 { get; set; }
        public ICommand BtnCommandSet_M05 { get; set; }
        public ICommand BtnCommandSet_M06 { get; set; }
        public ICommand BtnCommandSet_M07 { get; set; }
        public ICommand BtnCommandSet_M08 { get; set; }
        public ICommand BtnCommandSet_M09 { get; set; }
        public ICommand BtnCommandSet_M10 { get; set; }

        public ICommand BtnCommandPLC { get; set; }


        private int MyDeviceInitPlc = 0;
        private int MyDeviceInitGaoYa = 0;
        private int MyDeviceInitDiYa = 0;

        private Robot_ZLDZOSerrial myRobot;

        public ObservableCollection<string> ListBoxData { get; set; } = new ObservableCollection<string>();

        public PagezzViewModel()
        {
            // 记住Gdtat有自己的datasoushiz。。 要指定来设置..
            IsActive1 = false;
            IsActive2 = false;
            IsActive3 = false;
            IsActive4 = false;
            IsActive5 = false;
            IsActive6 = false;
            IsActive7 = false;
            IsActive8 = false;
            IsActive9 = false;
            IsActive10 = false;
            IsActive11 = false;
            IsActive12 = false;
            IsActive13 = false;
            

            MyDebugWrite("启动完成!");

            BtnCommandPLC = new RelayCommand<object>(DoBtnCommandPLC);


            BtnCommandSendHand_G = new RelayCommand<object>(DoBtnCommandSendHand_G);

            BtnCommandSendRead_G = new RelayCommand<object>(DoBtnCommandSendRead_G);            

            BtnCommandSendStart_G = new RelayCommand<object>(DoBtnCommandSendStart_G);

            BtnCommandSendStop_G = new RelayCommand<object>(DoBtnCommandSendStop_G);

            BtnCommandOpenprot_G = new RelayCommand<object>(DoBtnCommandOpenprot_G);

            BtnCommandCloseprot_G = new RelayCommand<object>(DoBtnCommandCloseprot_G);

            BtnCommandSendHand_D = new RelayCommand<object>(DoBtnCommandSendHand_D);

            BtnCommandSendRead_D = new RelayCommand<object>(DoBtnCommandSendRead_D);

            BtnCommandSendStart_D = new RelayCommand<object>(DoBtnCommandSendStart_D);

            BtnCommandSendStop_D = new RelayCommand<object>(DoBtnCommandSendStop_D);

            BtnCommandOpenprot_D = new RelayCommand<object>(DoBtnCommandOpenprot_D);

            BtnCommandCloseprot_D = new RelayCommand<object>(DoBtnCommandCloseprot_D);

            BtnCommandSendAtuo_D = new RelayCommand<object>(DoBtnCommandSendAtuo_D);
            BtnCommandSendSave_G = new RelayCommand<object>(DoBtnCommandSendSave_G);

            BtnCommandSet_M00 = new RelayCommand<object> (DoBtnCommandSet_M00);
            BtnCommandSet_M01 = new RelayCommand<object> (DoBtnCommandSet_M01);
            BtnCommandSet_M02 = new RelayCommand<object> (DoBtnCommandSet_M02);
            BtnCommandSet_M03 = new RelayCommand<object> (DoBtnCommandSet_M03);
            BtnCommandSet_M04 = new RelayCommand<object> (DoBtnCommandSet_M04);
            BtnCommandSet_M05 = new RelayCommand<object> (DoBtnCommandSet_M05);
            BtnCommandSet_M06 = new RelayCommand<object> (DoBtnCommandSet_M06);
            BtnCommandSet_M07 = new RelayCommand<object> (DoBtnCommandSet_M07);
            BtnCommandSet_M08 = new RelayCommand<object> (DoBtnCommandSet_M08);
            BtnCommandSet_M09 = new RelayCommand<object> (DoBtnCommandSet_M09);
            
            for (int i = 1;i < 10;i++)
            {
                ZZDataRecord temp = new ZZDataRecord();
                temp.Id = i;
                DataRecordList.Add(temp);
            }

            myRobot = new Robot_ZLDZOSerrial();
            myRobot.RotobCallBackEvent += RotobCallBack;
            myRobot.Start();

        }

        private void DoBtnCommandSet_M00(object param) { PushCommand(1); } //AB        
        private void DoBtnCommandSet_M01(object param) { PushCommand(2); } //BC
        private void DoBtnCommandSet_M02(object param) { PushCommand(3); } //CA

        private void DoBtnCommandSet_M03(object param) { PushCommand(4); } //ab
        private void DoBtnCommandSet_M04(object param) { PushCommand(5); } //bc
        private void DoBtnCommandSet_M05(object param) { PushCommand(6); } //ca

        private void DoBtnCommandSet_M06(object param) { PushCommand(7); } //an

        private void DoBtnCommandSet_M07(object param) { PushCommand(8); } //bn

        private void DoBtnCommandSet_M08(object param) { PushCommand(9); } //cn
        private void DoBtnCommandSet_M09(object param) { PushCommand(10); } //bianbi
               

        private void DoBtnCommandSendHand_D(object param) { PushCommand(201);  }
        private void DoBtnCommandSendHand_G(object param) { PushCommand(231); }

        private void DoBtnCommandSendRead_D(object param) { PushCommand(204); }
        private void DoBtnCommandSendRead_G(object param) { PushCommand(234); }

        private void DoBtnCommandSendStart_D(object param) { PushCommand(202);}
        private void DoBtnCommandSendStart_G(object param) { PushCommand(232);}

        private void DoBtnCommandSendStop_D(object param) { PushCommand(203);}
        private void DoBtnCommandSendStop_G(object param){ PushCommand(233);}

        private void DoBtnCommandPLC(object param) { PushCommand(500); }
        

        private void DoBtnCommandOpenprot_D(object param)
        {
            string portstr = ConfigurationManager.AppSettings["DEVICE_6"];
            var result = myRobot.RobotOpen_D(portstr);
            if (result.Item2 == false)
            {
                string str = "串口" + portstr + "错误:失败原因" + result.Item1;
                MessageBox.Show(str);
                MyDeviceInitDiYa = -1;
                return;
            }

            MyDebugWrite("串口" + portstr + "打开成功");
        }

        private void DoBtnCommandOpenprot_G(object param)
        {
            string portstr = ConfigurationManager.AppSettings["DEVICE_7"];
            var result = myRobot.RobotOpen_G(portstr);
            if (result.Item2 == false)
            {
                string str = "串口" + portstr + "错误:失败原因" + result.Item1;
                MessageBox.Show(str);
                MyDeviceInitGaoYa = -1;
                return;
            }

            MyDebugWrite("串口" + portstr + "打开成功");

        }


        private void DoBtnCommandCloseprot_D(object param)
        {
            myRobot.RobotClose();
            MyDebugWrite("串口低压关闭");
        }

        void DataStatistics()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AutoButton.IsEnabled = true;
            });

            //这里还必须这样搞...(iD设置为0表示，我不提供ID 数据库自己去管理ID..)
            m_zz_report.Id = 0;
            m_zz_report.ProName = m_newproinfo.ProName; //编号
            m_zz_report.Guigexinhao = m_newproinfo.GuigeXinhao;
            m_zz_report.Tuhao = m_newproinfo.Tuhao;


            m_zz_report.Datatimer = DateTime.Now.ToString();
            
            m_zz_report.DataAB = SelectedDataRecord.DataAB;
            m_zz_report.DataAB = (double)Math.Round(m_zz_report.DataAB, 3);

            m_zz_report.DataBC = SelectedDataRecord.DataBC;
            m_zz_report.DataBC = (double)Math.Round(m_zz_report.DataBC, 3);

            m_zz_report.DataCA = SelectedDataRecord.DataCA;
            m_zz_report.DataCA = (double)Math.Round(m_zz_report.DataCA, 3);

            m_zz_report.DataABBCCAXJC = SelectedDataRecord.DataABBCCAXJC;
            m_zz_report.DataABBCCAXJC = (double)Math.Round(m_zz_report.DataABBCCAXJC, 3);

            m_zz_report.DataHGPD = SelectedDataRecord.DataHGPD;

            m_zz_report.Dataabx = Dataab;
            m_zz_report.Dataabx = (double)Math.Round(m_zz_report.Dataabx, 3);

            m_zz_report.Databcx = Databc;
            m_zz_report.Databcx = (double)Math.Round(m_zz_report.Databcx, 3);

            m_zz_report.Datacax = Dataca;
            m_zz_report.Datacax = (double)Math.Round(m_zz_report.Datacax, 3);

            m_zz_report.Dabbccaxjcx = Dabbccaxjc;
            m_zz_report.Dabbccaxjcx = (double)Math.Round(m_zz_report.Dabbccaxjcx, 3);

            m_zz_report.Dataanx = Dataan;
            m_zz_report.Dataanx = (double)Math.Round(m_zz_report.Dataanx, 3);

            m_zz_report.Databnx = Databn;
            m_zz_report.Databnx = (double)Math.Round(m_zz_report.Databnx, 3);

            m_zz_report.Datacnx = Datacn;
            m_zz_report.Datacnx = (double)Math.Round(m_zz_report.Datacnx, 3);

            m_zz_report.Dnbncnxjcx = Dnbncnxjc;
            m_zz_report.Dnbncnxjcx = (double)Math.Round(m_zz_report.Dnbncnxjcx, 3);

            m_zz_report.Datahgqdx = Datahgqd;
        
                        
            if (SelectedDataRecord.DataHGPD == null)
            {
                m_zz_report.DataHGPD = "不合格";
            }

            if (Datahgqd == null)
            {
                m_zz_report.Datahgqdx = "不合格";                
            }

        }

        List<RotbotPushArg> AutoAction(int command,int arg)
        {

            List<RotbotPushArg> list = new List<RotbotPushArg>();
            RotbotPushArg retust1 = null;
            RotbotPushArg retust2 = null;
            RotbotPushArg retust3 = null;
            RotbotPushArg retust4 = null;
            RotbotPushArg retust5 = null;
            RotbotPushArg retust6 = null;
            RotbotPushArg retust7 = null;
            RotbotPushArg retust8 = null;
            

            retust1 = PushCommand(arg, 0); //设置AB                                                       
            retust2 = PushCommand(3000, 2000); //等待2秒..

            if (command == 1)
            {
                retust3 = PushCommand(232, 0); //启动高压 .
                retust4 = PushCommand(3000, 15000); //等待15秒..

                retust5 = PushCommand(234, 0); //度曲高压 .
                retust6 = PushCommand(3000, 2000); //等待2秒..

                retust7 = PushCommand(233, 0); //停止高压 .
                retust8 = PushCommand(3000, 10000); //等待10秒..
            }

            if (command == 2)
            {
                retust3 = PushCommand(202, 0); //启动地压 .
                retust4 = PushCommand(3000, 12000); //等待12秒..

                retust5 = PushCommand(204, 0); //度曲地压 .
                retust6 = PushCommand(3000, 2000); //等待2秒..

                retust7 = PushCommand(203, 0); //停止地压 .
                retust8 = PushCommand(3000, 12000); //等待12秒..
            }

            list.Add(retust1);
            list.Add(retust2);
            list.Add(retust3);
            list.Add(retust4);
            list.Add(retust5);
            list.Add(retust6);
            list.Add(retust7);
            list.Add(retust8);
            

            return list;
        }

        private void DoBtnCommandSendAtuo_D(object param)
        {
            //先坚持联机 ..
            if (IsActive1 == false)
            {
                MessageBox.Show("请先PLC联机!");
                return;
            }
            if (IsActive2 == false)
            {
                MessageBox.Show("请先低压设备联机!");
                return;
            }
            if (IsActive3 == false)
            {
                MessageBox.Show("请先高压设备联机!");
                return;
            }

            Button mybutton = param as Button;
            AutoButton = mybutton;
            mybutton.IsEnabled = false;



            List<RotbotPushArg> listAll = new List<RotbotPushArg>();

            listAll.AddRange(AutoAction(1,1));

            listAll.AddRange(AutoAction(1,2));

            listAll.AddRange(AutoAction(1,3));

            listAll.AddRange(AutoAction(2, 4));
            listAll.AddRange(AutoAction(2, 5));
            listAll.AddRange(AutoAction(2, 6));

            RotbotPushArg retust1 = PushCommand(3000, 1000); //等待5秒..
            RotbotPushArg retust2 = PushCommand(501, 0); //发信息给主程序 -- 写入数据库.

            listAll.Add(retust1);
            listAll.Add(retust2);

            myRobot.Push(listAll);           

        }

        private void DoBtnCommandSendSave_G(object param)
        {
            DataStatistics();//数据收集.. .
            using (var context = new MyDbContext())
            {
                //验证...
                var myitem = Tools.SafeScanPro("zz_report", m_zz_report);
                if (myitem.Item1 == false)
                {
                    MyDebugWrite($"验证数据失败{myitem.Item2}");
                    return;
                }
                try
                {
                    context.zz_report.Add(m_zz_report);
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

        private void DoBtnCommandCloseprot_G(object param)
        {
            myRobot.RobotClose();
            MyDebugWrite("串口高压关闭");
        }

        RotbotPushArg PushCommand(int Command, int andExe = 1)
        {
            if (Command < 20)
            {
                switch (Command)
                {
                    case 1: IsActive4 = false; break;
                    case 2: IsActive5 = false; break;
                    case 3: IsActive6 = false; break;
                    case 4: IsActive7 = false; break;
                    case 5: IsActive8 = false; break;
                    case 6: IsActive9 = false; break;
                    case 7: IsActive10 = false; break;
                    case 8: IsActive11 = false; break;
                    case 9: IsActive12 = false; break;
                    case 10: IsActive13 = false; break;
                }
            }

            if (Command == 500)
            {
                string strip = ConfigurationManager.AppSettings["DEVICE_1"];
                MyDebugWrite("PLC连接" + strip);
                IsActive1 = false;
            }

            if (Command == 201)
            {
                IsActive2 = false;
            }

            if (Command == 231)
            {
                IsActive3 = false;
            }

            if (Command == 3000)
            {
                RotbotPushArg sp = new RotbotPushArg();
                sp.command = 3000;
                sp.reserve = andExe.ToString();
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


        public async Task<bool> PagezzViewModeinit()
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

            //2
            DoBtnCommandOpenprot_G(null);
            if (MyDeviceInitGaoYa == -1)
            {
                string str = "串口高压打开失败,请看配置是否正确, 线路是否正常,重新启动软件在测试一次!"; 
                WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                return false;
            }

            //3
            DoBtnCommandOpenprot_D(null);
            if (MyDeviceInitDiYa == -1)
            {
                string str = "串口低压打开失败,请看配置是否正确, 线路是否正常,重新启动软件在测试一次!";                
                WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                return false;
            }


            PushCommand(201);
            WeakReferenceMessenger.Default.Send<string, string>("低压设备检查....", "SetShowProc");
            await Task.Delay(2000);
            //如果联机失败怎么办....
            
            if (MyDeviceInitDiYa == -1 || MyDeviceInitDiYa == 0)
            {

                string str = "低压设备连接失败,请看配置是否正确, 线路是否正常,机器是否开启,重新启动软件在测试一次!";                
                WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                return false;
            }
            
            PushCommand(231);
            WeakReferenceMessenger.Default.Send<string, string>("高压设备检查....", "SetShowProc");
            await Task.Delay(2000);

            //如果联机失败怎么办....                
            if (MyDeviceInitGaoYa == -1 || MyDeviceInitGaoYa ==0)
            {
                string str = "高压设备连接失败,请看配置是否正确, 线路是否正常,机器是否开启,重新启动软件在测试一次!";
                WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                return false;
            }

            using (var context = new MyDbContext())
            {
                var firstEntity = context.newproinfo.FirstOrDefault(e => e.ProName == App_Config.currendProName);
                if (firstEntity != null)
                {
                    m_newproinfo = firstEntity.Clone();
                }
                else
                {
                    string str = "数据库样品查询失败,请看数据库配置是否正确!";
                    WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");
                    return false;
                }

                // 获取单个实体（第一个匹配）
                var firstEntity2 = context.experimentstandard_ziliudianzushiyan.FirstOrDefault(e => e.Guigexinhao == m_newproinfo.GuigeXinhao && e.Tuhao == m_newproinfo.Tuhao);

                if (firstEntity2 != null)
                {
                    m_experimentstandard_ziliudianzushiyan = firstEntity2.Clone();
                }
                else
                {
                    string str = "数据库样品有对比标准,请看数据库配置是否正确!";                    
                    WeakReferenceMessenger.Default.Send<string, string>(str, "SetShowProc");

                    return false ;
                }
            }

            //发布..
            WeakReferenceMessenger.Default.Send<string, string>("ZZShowProc", "ZZShowProc");
            //发布..
            WeakReferenceMessenger.Default.Send<string, string>("全部设备检查完成....", "SetShowProc");
            

            await Task.Delay(2000);
            WeakReferenceMessenger.Default.Send<string, string>("NULL", "CloseShowProc");
            return true;
        }


        void SetPresLen()
        {
            switch (PresCommand)
            {
                case 1: IsActive4 = false; break;
                case 2: IsActive5 = false; break;
                case 3: IsActive6 = false; break;
                case 4: IsActive7 = false; break;
                case 5: IsActive8 = false; break;
                case 6: IsActive9 = false; break;
                case 7: IsActive10 = false; break;
                case 8: IsActive11 = false; break;
                case 9: IsActive12 = false; break;
                case 10: IsActive13 = false; break;
            }
        }

        
        //这里的机器人回到.. 不是只有平流层
        public void RotobCallBack(object sender, Page2_RotobCallBackArg e)
        {
            if (e.currErrorcode == RotobErrorCode.Ready)
            {
                MyDebugWrite(e.currMessage);
                return;
            }

            if (e.currErrorcode == RotobErrorCode.ChartMateFail ||
                        e.currErrorcode == RotobErrorCode.Error ||
                        e.currErrorcode == RotobErrorCode.TimerOver)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (AutoButton != null) AutoButton.IsEnabled = true;
                    MyDeviceInitPlc = -1;
                    MyDeviceInitGaoYa = -1;
                    MyDeviceInitDiYa = -1;
                });

                //MessageBox.Show("运行发生错误,请看机器人日志!");
                return;
            }

            if (e.currErrorcode == RotobErrorCode.Success)
            {
                if (e.currCommand < 20) SetPresLen();

                //这里是 机器人回调...            
                switch (e.currCommand)
                {
                    case 1: MyDebugWrite(e.currMessage); IsActive4 = true; PresCommand = 1; break;
                    case 2: MyDebugWrite(e.currMessage); IsActive5 = true; PresCommand = 2; break;
                    case 3: MyDebugWrite(e.currMessage); IsActive6 = true; PresCommand = 3; break;
                    case 4: MyDebugWrite(e.currMessage); IsActive7 = true; PresCommand = 4; break;
                    case 5: MyDebugWrite(e.currMessage); IsActive8 = true; PresCommand = 5; break;
                    case 6: MyDebugWrite(e.currMessage); IsActive9 = true; PresCommand = 6; break;
                    case 7: MyDebugWrite(e.currMessage); IsActive10 = true; PresCommand = 7; break;
                    case 8: MyDebugWrite(e.currMessage); IsActive11 = true; PresCommand = 8; break;
                    case 9: MyDebugWrite(e.currMessage); IsActive12 = true; PresCommand = 9; break;
                    case 10: MyDebugWrite(e.currMessage); IsActive13 = true; PresCommand = 10; break;

                    case 201: MyDebugWrite(e.currMessage);IsActive2 = true; MyDeviceInitDiYa = 1; break;
                        
                    case 202: MyDebugWrite(e.currMessage); break;
                    case 203: MyDebugWrite(e.currMessage); break;
                    case 204:
                        {
                            MyDebugWrite(e.currMessage);
                            if (e.currErrorcode == RotobErrorCode.Success)
                            {
                                //变成嚎哦...
                                e.ECA *= 1000;
                                e.ECA = (float)Math.Round(e.ECA, 3);
                                if (PresCommand == 4) Dataab = e.ECA; //ab
                                if (PresCommand == 5) Databc = e.ECA; //bc
                                if (PresCommand == 6) Dataca = e.ECA; //ca

                                if (PresCommand == 7) Dataan = e.ECA; //an
                                if (PresCommand == 8) Databn = e.ECA; //bn
                                if (PresCommand == 9) Datacn = e.ECA; //cn
                            }
                        }
                        break;

                    case 210: break;
                    case 211: break;

                    case 231:
                        MyDebugWrite(e.currMessage);
                        {
                            IsActive3 = true;
                            MyDeviceInitGaoYa = 1;
                            break;
                        }

                    case 232: MyDebugWrite(e.currMessage); break;
                    case 233: MyDebugWrite(e.currMessage); break;
                    case 234:
                        {
                            MyDebugWrite(e.currMessage);
                            if (e.currErrorcode == RotobErrorCode.Success)
                            {
                                //数据过来的时候，是哦。。。
                                //变成嚎哦...
                                e.ECA *= 1000;
                                e.ECA = (float)Math.Round(e.ECA, 3);
                                if (PresCommand == 1) SelectedDataRecord.DataAB = e.ECA;
                                if (PresCommand == 2) SelectedDataRecord.DataBC = e.ECA;
                                if (PresCommand == 3) SelectedDataRecord.DataCA = e.ECA;
                            }
                        }
                        break;

                    case 240: break;
                    case 241: break;

                    case 500:
                        {
                            MyDebugWrite(e.currMessage);
                            IsActive1 = true;
                            MyDeviceInitPlc = 1;
                        }   
                        break;

                    case 501:
                        {

                            //存储前计算....
                            if (SelectedDataRecord.DataAB != 0 &&
                                SelectedDataRecord.DataBC != 0 &&
                                SelectedDataRecord.DataCA != 0)
                            {
                                SelectedDataRecord.DataABBCCAXJC = CalacData(SelectedDataRecord.DataAB, SelectedDataRecord.DataBC, SelectedDataRecord.DataCA);
                                SelectedDataRecord.DataABBCCAXJC = (float)Math.Round(SelectedDataRecord.DataABBCCAXJC, 3);
                                if (Math.Abs(SelectedDataRecord.DataABBCCAXJC) < m_experimentstandard_ziliudianzushiyan.Xiandianzhupinghen)
                                {
                                    SelectedDataRecord.DataHGPD = "合格";
                                }
                                else
                                {
                                    SelectedDataRecord.DataHGPD = "不合格";
                                }
                            }
                            else
                            {
                                SelectedDataRecord.DataHGPD = "不合格";
                                SelectedDataRecord.DataABBCCAXJC = 0;
                            }

                            if (Dataab != 0 &&
                                Databc != 0 &&
                                Dataca != 0)
                            {
                                Dabbccaxjc = CalacData(Dataab, Databc, Dataca);
                                Dabbccaxjc = (float)Math.Round(Dabbccaxjc, 3);
                            }
                            else
                                Dabbccaxjc = 0;

                            if (Dataan != 0 &&
                                Databn != 0 &&
                                Datacn != 0)
                                Dnbncnxjc = CalacData(Dataan, Databn, Datacn);
                            else
                                Dnbncnxjc = 0;

                            if (Dabbccaxjc != 0 && Dnbncnxjc != 0)
                            {
                                if (Math.Abs(Dabbccaxjc) < m_experimentstandard_ziliudianzushiyan.Xiandianzhupinghen &&
                                    Math.Abs(Dnbncnxjc) < m_experimentstandard_ziliudianzushiyan.Xdianzhupinghen)
                                {
                                    Datahgqd = "合格";
                                }
                                else
                                {
                                    Datahgqd = "不合格";
                                }

                            }
                            else
                            {
                                Datahgqd = "不合格";
                            }

                            DoBtnCommandSendSave_G(null);
                        }
                        break;
                }
            }
        }

        public float CalacData(float num1, float num2, float num3)
        {            
            float max = Math.Max(num1, Math.Max(num2, num3));
            float min = Math.Min(num1, Math.Min(num2, num3));            
            return max - min;
        }


        private void DobtnCommand1(object button) 
        {            
            //发布..
            //WeakReferenceMessenger.Default.Send<string, string>("SetGTTextLocation", location);
            return; 
        }
        private void DobtnCommand2(object button) { return; }
        private void DobtnCommand3(object button) { return; }

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

    public class ZZDataRecord : INotifyPropertyChanged
    {
        private int _id;
        private float  _dataAB, _dataBC, _dataCA, _dataABBCCAXJC;
        private string _dataHGPD;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        public float DataAB { get { return _dataAB; } set { _dataAB = value; OnPropertyChanged(nameof(DataAB)); }}
        public float DataBC { get { return _dataBC; } set { _dataBC = value; OnPropertyChanged(nameof(DataBC)); } }
        public float DataCA { get { return _dataCA; } set { _dataCA = value; OnPropertyChanged(nameof(DataCA)); } }
        public float DataABBCCAXJC { get { return _dataABBCCAXJC; } set { _dataABBCCAXJC = value; OnPropertyChanged(nameof(DataABBCCAXJC)); } }
        public string DataHGPD { get { return _dataHGPD; } set { _dataHGPD = value; OnPropertyChanged(nameof(DataHGPD)); } }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
