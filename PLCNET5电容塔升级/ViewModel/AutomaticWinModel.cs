using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using PLCNET5电容塔;
using PLCNET5电容塔升级.Base;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace PLCNET5电容塔升级.ViewModel
{
    public class AutomaticWinModel : ObservableObject
    {
        //第一层状态..
        public bool CheckBoxLevel_1_1 { get; set; }
        public bool CheckBoxLevel_1_2 { get; set; }
        public bool CheckBoxLevel_1_3 { get; set; }
        public bool CheckBoxLevel_1_4 { get; set; }
        public bool CheckBoxLevel_1_5 { get; set; }
        public bool CheckBoxLevel_1_6 { get; set; }



        //这里自动化还是比较麻烦，需要知道所有按钮对象的状态..
        public Plc plc39 { get; set; }

        //默认补偿相数为0
        public int BtnComBoxListBCXSIndex { get; set; } = 0;
        public ObservableCollection<string> BtnComBoxListBCXS { get; set; } = new ObservableCollection<string>();


        //用户选择接法.
        public int BtnComBoxListUserXZJFIndex { get; set; } = 0;
        public ObservableCollection<string> BtnComBoxListUserXZJF { get; set; } = new ObservableCollection<string>();

        //用户选择 并联方式..
        public int BtnComBoxListUserBLFSIndex { get; set; }
        public ObservableCollection<string> BtnComBoxListUserBLFS { get; set; } = new ObservableCollection<string>();

        //用户选择工位..
        public int BtnComBoxListSCGWIndex { get; set; } = 0;
        //工位
        public ObservableCollection<string> BtnComBoxListSCGW { get; set; } = new ObservableCollection<string>();

        //接法.
        private string userjf = string.Empty;
        public string UserJF { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //串连数.
        private string usercls = string.Empty;
        public string UserCLS { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //并连数.
        private float userbls = 0.0f;
        public float UserBLS { get { return userbls; } set { SetProperty(ref userbls, value); } }


        //系统电压.
        private float userxtdy = 0.0f;
        public float UserXTDY { get { return userxtdy; } set { SetProperty(ref userxtdy, value); } }

        //建议投个数
        private string usermcjygs = string.Empty;
        public string UserMCJYGS  { get { return usermcjygs; } set { SetProperty(ref usermcjygs, value); } }

        //每层实际投入数量..
        private float mcsjtrsl = 0.0f;
        public float McsjTrsl { get { return mcsjtrsl; } set { SetProperty(ref mcsjtrsl, value); } }

        //反推实验电流
        private float ftsydl = 0.0f;
        public float Ftsydl { get { return ftsydl; } set { SetProperty(ref ftsydl, value); } }

        //偏差值
        private float pcz = 0.0f;
        public float Pcz { get { return pcz; } set { SetProperty(ref pcz, value); } }


        //开始执行-这里流程就开始了，喀喀喀.. 动作..
        public ICommand CommandBtnExe { get; set; }

        //开始计算仿真的数据.
        public ICommand BtnCommandStartCount { get; set; }
        
        //执行了..仿真计算..
        public ICommand BtnCommandExeBCXS { get; set; }

        

        //显示执行流程..
        private string exerecord = string.Empty;
        public string Exerecord { get { return exerecord; } set { SetProperty(ref exerecord, value); } }

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private bool mytoken = false;
        
        //预故电压
        private float estimatedfvoltage = 0.0f;
        public float EstimatedVoltage { get { return estimatedfvoltage; } set { SetProperty(ref estimatedfvoltage, value); } }

        //预故电流
        private float estimatedcurrent = 0.0f;
        public float EstimatedCurrent { get { return estimatedcurrent; } set { SetProperty(ref estimatedcurrent, value); } }

        public AutomaticWinModel()
        {
            BtnComBoxListBCXS.Add("三相");
            BtnComBoxListBCXS.Add("单相");

            BtnComBoxListUserXZJF.Add("Y接法");
            BtnComBoxListUserXZJF.Add("D接法");
            BtnComBoxListUserXZJF.Add("单相");

            BtnComBoxListSCGW.Add("第1工位");
            BtnComBoxListSCGW.Add("第2工位");


            myBCXSItems.Add(new MyBCXSItem() { Vold = 78f,   Message = "Y接法-6串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 65f,   Message = "Y接法-5串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 52f,   Message = "Y接法-4串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 39f,   Message = "Y接法-3串2并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 26f,   Message = "Y接法-2串3并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 13f,   Message = "Y接法-1串6并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 45f,   Message = "D接法-6串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 37.5f, Message = "D接法-5串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 30f,   Message = "D接法-4串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 22.5f, Message = "D接法-3串2并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 15f,   Message = "D接法-2串3并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 7.5f,  Message = "D接法-1串6并" });

            myBCXSItemsDx.Add(new MyBCXSItem() { Vold = 22.5f, Message = "单相-3串6并" });
            myBCXSItemsDx.Add(new MyBCXSItem() { Vold = 45f, Message = "单相-6串3并" });
            myBCXSItemsDx.Add(new MyBCXSItem() { Vold = 67.5f, Message = "单相-9串2并" });

            //以下代码会异常，为什么，因为无法排序..
            //myBCXSItems.Sort();
            //以下代码正常.. 从小到大，按照电压排序..
            myBCXSItems = myBCXSItems.OrderBy(n => n.Vold).ToList(); // 升序排序

            myBCXSItemsDx = myBCXSItemsDx.OrderBy(n => n.Vold).ToList(); // 升序排序

            CommandBtnExe = new RelayCommand<object>(DoCommandBtnExe);

            BtnCommandStartCount = new RelayCommand<object>(DoBtnCommandStartCount);
            
            BtnCommandExeBCXS = new RelayCommand<object>(DoBtnCommandExeBCXS);            


        }

        public void SetAllUserBtn(object self)
        {
            //StackPanel? stackPanel = sender as StackPanel;
            object? buttons = VisualTreeHelpers.ScanButtonFromStackPanel(self, "myButtonGA");
            //Button? buttont = VisualTreeHelpers.ScanButtonFromStackPanel(self, "BCommandStop");
            return;
        }

        public List<string> AllGateF = new List<string>() {
            "AC_F", "B_F" ,  "G1_F" ,
            "G2_F", "G3_F",  "G4_F",
            "G5_F", "1G1_F", "1G2_F",
            "1G3_F","1G4_F", "1G5_F",
            "1G6_F","K1_F","K2_F"};

        public List<string> AutoCommandList = new List<string>();


        
        public bool InspectExc(string Command)
        {
            switch (Command)
            {
                case "AC":  return MyTools.myButtonGAC.myButtonModel.Data; //"总输出AC和闸"
                case "AC_F": return MyTools.myButtonGAC.myButtonModel.Data; //"总输出AC分闸"

                case "B":  return MyTools.myButtonGB.myButtonModel.Data; //"总输出B和闸"
                case "B_F":  return MyTools.myButtonGB.myButtonModel.Data;  //"总输出B分闸"

                case "G1": return MyTools.myButtonG1.myButtonModel.Data; //"G1合闸"
                case "G1_F": return MyTools.myButtonG1.myButtonModel.Data; //"G1分闸"

                case "G2": return MyTools.myButtonG2.myButtonModel.Data; //"G2合闸"
                case "G2_F": return MyTools.myButtonG2.myButtonModel.Data; //"G2分闸"

                case "G3": return MyTools.myButtonG3.myButtonModel.Data; //"G3合闸"
                case "G3_F": return MyTools.myButtonG3.myButtonModel.Data; //"G3分闸"

                case "G4": return MyTools.myButtonG4.myButtonModel.Data; //"G4合闸"
                case "G4_F": return MyTools.myButtonG4.myButtonModel.Data; //"G4分闸"

                case "G5": return MyTools.myButtonG5.myButtonModel.Data; //"G5合闸"
                case "G5_F": return MyTools.myButtonG5.myButtonModel.Data; //"G5分闸"

                case "1G1": return MyTools.myButton1G1.myButtonModel.Data; //"1G1合闸"
                case "1G1_F": return MyTools.myButton1G1.myButtonModel.Data; //"1G1分闸"

                case "1G2": return MyTools.myButton1G2.myButtonModel.Data; //"1G2合闸"
                case "1G2_F": return MyTools.myButton1G2.myButtonModel.Data; //"1G2分闸"

                case "1G3": return MyTools.myButton1G3.myButtonModel.Data; //"1G3合闸"
                case "1G3_F": return MyTools.myButton1G3.myButtonModel.Data;  //"1G3分闸"

                case "1G4": return MyTools.myButton1G4.myButtonModel.Data;  //"1G4合闸"
                case "1G4_F": return MyTools.myButton1G4.myButtonModel.Data;  //"1G4分闸"

                case "1G5": return MyTools.myButton1G5.myButtonModel.Data;  //"1G5合闸"
                case "1G5_F": return MyTools.myButton1G5.myButtonModel.Data;  //"1G5分闸"

                case "1G6": return MyTools.myButton1G6.myButtonModel.Data;  //"1G6合闸"
                case "1G6_F": return MyTools.myButton1G6.myButtonModel.Data;  //"1G6分闸"

                case "K1": return MyTools.myButtonK1.myButtonModel.Data;  //"K1合闸"
                case "K1_F": return MyTools.myButtonK1.myButtonModel.Data;  //"K1分闸"

                case "K2": return MyTools.myButtonK2.myButtonModel.Data;  //"K2合闸"
                case "K2_F": return MyTools.myButtonK2.myButtonModel.Data;  //"K2分闸"            
            }
            throw new Exception("没有一个选中!");
        }
        

        void AutoExe(string Command)
        {
            switch (Command)
            {
                case "AC": { plc39.Write("M100.0", true); Thread.Sleep(300); plc39.Write("M100.0", false); break; }//"总输出AC和闸"
                case "AC_F": { plc39.Write("M100.1", true); Thread.Sleep(300); plc39.Write("M100.1", false); break; }//"总输出AC分闸"

                case "B": { plc39.Write("M100.2", true); Thread.Sleep(300); plc39.Write("M100.2", false); break; }//"总输出B和闸"
                case "B_F": { plc39.Write("M100.3", true); Thread.Sleep(300); plc39.Write("M100.3", false); break; }//"总输出B分闸"

                case "G1": { plc39.Write("M100.4", true); Thread.Sleep(300); plc39.Write("M100.4", false); break; }//"G1合闸"
                case "G1_F": { plc39.Write("M100.5", true); Thread.Sleep(300); plc39.Write("M100.5", false); break; }//"G1分闸"

                case "G2": { plc39.Write("M100.6", true); Thread.Sleep(300); plc39.Write("M100.6", false); break; }//"G2合闸"
                case "G2_F": { plc39.Write("M100.7", true); Thread.Sleep(300); plc39.Write("M100.7", false); break; }//"G2分闸"

                case "G3": { plc39.Write("M101.0", true); Thread.Sleep(300); plc39.Write("M101.0", false); break; }//"G3合闸"
                case "G3_F": { plc39.Write("M101.1", true); Thread.Sleep(300); plc39.Write("M101.1", false); break; }//"G3分闸"

                case "G4": { plc39.Write("M101.2", true); Thread.Sleep(300); plc39.Write("M101.2", false); break; }//"G4合闸"
                case "G4_F": { plc39.Write("M101.3", true); Thread.Sleep(300); plc39.Write("M101.3", false); break; }//"G4分闸"

                case "G5": { plc39.Write("M101.4", true); Thread.Sleep(300); plc39.Write("M101.4", false); break; }//"G5合闸"
                case "G5_F": { plc39.Write("M101.5", true); Thread.Sleep(300); plc39.Write("M101.5", false); break; }//"G5分闸"

                case "1G1": { plc39.Write("M101.6", true); Thread.Sleep(300); plc39.Write("M101.6", false); break; }//"1G1合闸"
                case "1G1_F": { plc39.Write("M101.7", true); Thread.Sleep(300); plc39.Write("M101.7", false); break; }//"1G1分闸"

                case "1G2": { plc39.Write("M102.0", true); Thread.Sleep(300); plc39.Write("M102.0", false); break; }//"1G2合闸"
                case "1G2_F": { plc39.Write("M102.1", true); Thread.Sleep(300); plc39.Write("M102.1", false); break; }//"1G2分闸"

                case "1G3": { plc39.Write("M102.2", true); Thread.Sleep(300); plc39.Write("M102.2", false); break; }//"1G3合闸"
                case "1G3_F": { plc39.Write("M102.3", true); Thread.Sleep(300); plc39.Write("M102.3", false); break; }//"1G3分闸"

                case "1G4": { plc39.Write("M102.4", true); Thread.Sleep(300); plc39.Write("M102.4", false); break; }//"1G4合闸"
                case "1G4_F": { plc39.Write("M102.5", true); Thread.Sleep(300); plc39.Write("M102.5", false); break; }//"1G4分闸"

                case "1G5": { plc39.Write("M102.6", true); Thread.Sleep(300); plc39.Write("M102.6", false); break; }//"1G5合闸"
                case "1G5_F": { plc39.Write("M102.7", true); Thread.Sleep(300); plc39.Write("M102.7", false); break; }//"1G5分闸"

                case "1G6": { plc39.Write("M103.0", true); Thread.Sleep(300); plc39.Write("M103.0", false); break; }//"1G6合闸"
                case "1G6_F": { plc39.Write("M103.1", true); Thread.Sleep(300); plc39.Write("M103.1", false); break; }//"1G6分闸"

                case "K1": { plc39.Write("M103.2", true); Thread.Sleep(300); plc39.Write("M103.2", false); break; }//"K1合闸"
                case "K1_F": { plc39.Write("M103.3", true); Thread.Sleep(300); plc39.Write("M103.3", false); break; }//"K1分闸"

                case "K2": { plc39.Write("M103.4", true); Thread.Sleep(300); plc39.Write("M103.4", false); break; }//"K2合闸"
                case "K2_F": { plc39.Write("M103.5", true); Thread.Sleep(300); plc39.Write("M103.5", false); break; }//"K2分闸"            

            }
        }

        //开始计算仿真的数据..
        private async void DoBtnCommandStartCount(object param)
        {
            float count = 0.0f;
            //这里我只计算，第一层...
            if (CheckBoxLevel_1_1 == true) count += 0.25f;
            if (CheckBoxLevel_1_2 == true) count += 0.5f;
            if (CheckBoxLevel_1_3 == true) count += 1f;
            if (CheckBoxLevel_1_4 == true) count += 2f;
            if (CheckBoxLevel_1_5 == true) count += 4f;
            if (CheckBoxLevel_1_6 == true) count += 8f;

            //这里是实际投..
            McsjTrsl = count;
            // 反推试验电流 = 实际投 * 单个电容 * 并联数 (预估电压 / 系统电压)
            Ftsydl = count * 53.3f * UserBLS*(EstimatedVoltage / UserXTDY);
            //计算偏差值
            Pcz = EstimatedCurrent - Ftsydl;
            //保留两位小数
            Pcz = (float)Math.Round(Pcz, 2);
            //在取一个绝对值.
            Pcz = Math.Abs(Pcz);

        }

        //开始执行计算..
        public bool StarExeCount()
        {
            if (BtnComBoxListUserXZJFIndex == -1)
            {
                MessageBox.Show("接法还没有选择!");
                return false;
            }

            if (BtnComBoxListUserBLFSIndex == -1)
            {
                MessageBox.Show("并联方式还没有选择!");
                return false;
            }

            //选择接法..
            if (BtnComBoxListUserXZJFIndex == 0) UserJF = "Y";
            if (BtnComBoxListUserXZJFIndex == 1) UserJF = "D";
            if (BtnComBoxListUserXZJFIndex == 2) UserJF = "单相";

            if (BtnComBoxListUserXZJFIndex == 0)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    UserCLS = "6"; UserBLS = 1; UserXTDY = 78;
                }
                if (BtnComBoxListUserBLFSIndex == 1)
                {
                    UserCLS = "5"; UserBLS = 1; UserXTDY = 65;
                }
                if (BtnComBoxListUserBLFSIndex == 2)
                {
                    UserCLS = "4"; UserBLS = 1; UserXTDY = 52;
                }
                if (BtnComBoxListUserBLFSIndex == 3)
                {
                    UserCLS = "3"; UserBLS = 2; UserXTDY = 39;
                }
                if (BtnComBoxListUserBLFSIndex == 4)
                {
                    UserCLS = "2"; UserBLS = 3; UserXTDY = 26;
                }

                if (BtnComBoxListUserBLFSIndex == 5)
                {
                    UserCLS = "1"; UserBLS = 6; UserXTDY = 13;
                }
            }

            if (BtnComBoxListUserXZJFIndex == 1)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    UserCLS = "6"; UserBLS = 1; UserXTDY = 45;
                }

                if (BtnComBoxListUserBLFSIndex == 1)
                {
                    UserCLS = "5"; UserBLS = 1; UserXTDY = 37.5f;
                }

                if (BtnComBoxListUserBLFSIndex == 2)
                {
                    UserCLS = "4"; UserBLS = 1; UserXTDY = 30;
                }
                if (BtnComBoxListUserBLFSIndex == 3)
                {
                    UserCLS = "3"; UserBLS = 2; UserXTDY = 22.5f;
                }

                if (BtnComBoxListUserBLFSIndex == 4)
                {
                    UserCLS = "2"; UserBLS = 3; UserXTDY = 15;
                }
                if (BtnComBoxListUserBLFSIndex == 5)
                {
                    UserCLS = "1"; UserBLS = 6; UserXTDY = 7.5f;
                }
            }

            if (BtnComBoxListUserXZJFIndex == 2)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    UserCLS = "3"; UserBLS = 6; UserXTDY = 22.5f;
                }
                if (BtnComBoxListUserBLFSIndex == 1)
                {
                    UserCLS = "6"; UserBLS = 3; UserXTDY = 45;
                }

                if (BtnComBoxListUserBLFSIndex == 2)
                {
                    UserCLS = "9"; UserBLS = 2; UserXTDY = 67.5f;
                }
            }

            if (EstimatedVoltage == 0)
            {
                MessageBox.Show("预估电压不能为0!");
                return false;
            }

            if(EstimatedCurrent == 0)
            {
                MessageBox.Show("预估电流不能为0!");
                return false;
            }

            if (UserXTDY < EstimatedVoltage)
            {
                MessageBox.Show("系统额定电压小于预估电压!");
                return false;
            }


            if (EstimatedVoltage != 0 && EstimatedCurrent != 0 && UserXTDY != 0 && UserBLS != 0)
            {
                //预估试验电流(Us/UN） 1023.5 --  630 / (24/39)
                //这里不要这样直接去除法，要不然 这个就不对的..
                float data = EstimatedVoltage / UserXTDY;
                float ygsydl = EstimatedCurrent / data;
                //预估试验电流/并联数 == 每层需要的电流..
                float mcxydl = ygsydl / UserBLS;
                //等于每层电流值/额定单元电容电流（系统预先设置53.33）投的个数.
                string? dg = ConfigurationManager.AppSettings["BASEDATA"].ToString();
                float.TryParse(dg, out float x);
                float tbgs = mcxydl / x;
                tbgs = (float)Math.Round(tbgs, 2); // 保留两位小数
                UserMCJYGS = tbgs.ToString();

                return true;
            }
            return false;

        }


        //确定电压..
        private void DoCommandBtnExe(object param)
        {

            if (BtnComBoxListUserXZJFIndex == -1)
            {
                MessageBox.Show("接法还没有选择!");
                return;
            }

            if (BtnComBoxListUserBLFSIndex == -1)
            {
                MessageBox.Show("并联方式还没有选择!");
                return;
            }


            //选择接法..
            if (BtnComBoxListUserXZJFIndex == 0) UserJF = "Y";
            if (BtnComBoxListUserXZJFIndex == 1) UserJF = "D";
            if (BtnComBoxListUserXZJFIndex == 2) UserJF = "单相";

            if (BtnComBoxListUserXZJFIndex == 0)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0) 
                {
                    UserCLS = "6"; UserBLS = 1; UserXTDY = 78;
                    //(6串1并)0 . G1 G4 1G2 K1 
                    AutoCommandList.Add("G1");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("K1");
                }
                if (BtnComBoxListUserBLFSIndex == 1) 
                {
                    UserCLS = "5"; UserBLS = 1; UserXTDY = 65;
                    AutoCommandList.Add("G1");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("K2");
                }
                if (BtnComBoxListUserBLFSIndex == 2) 
                {
                    UserCLS = "4"; UserBLS = 1; UserXTDY = 52;
                    AutoCommandList.Add("G1");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("K1");
                }
                if (BtnComBoxListUserBLFSIndex == 3) 
                {
                    UserCLS = "3"; UserBLS = 2; UserXTDY = 39;
                    AutoCommandList.Add("G1");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G3");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("K2");
                }
                if (BtnComBoxListUserBLFSIndex == 4) 
                {
                    UserCLS = "2"; UserBLS = 3; UserXTDY = 26;
                    AutoCommandList.Add("G1");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("1G6");
                    AutoCommandList.Add("K1");
                }

                if (BtnComBoxListUserBLFSIndex == 5) 
                {
                    UserCLS = "1"; UserBLS = 6; UserXTDY = 13;
                    AutoCommandList.Add("G1");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("1G3");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("1G6");
                    AutoCommandList.Add("K2");
                }
            }

            if (BtnComBoxListUserXZJFIndex == 1)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0) 
                {
                    UserCLS = "6"; UserBLS = 1; UserXTDY = 45;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G3");
                    AutoCommandList.Add("G5");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("K1");
                }

                if (BtnComBoxListUserBLFSIndex == 1) 
                {
                    UserCLS = "5"; UserBLS = 1; UserXTDY = 37.5f;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G3");
                    AutoCommandList.Add("G5");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("K2");
                }

                if (BtnComBoxListUserBLFSIndex == 2) 
                {
                    UserCLS = "4"; UserBLS = 1; UserXTDY = 30;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G3");
                    AutoCommandList.Add("G5");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("K1");

                }
                if (BtnComBoxListUserBLFSIndex == 3) 
                {
                    UserCLS = "3"; UserBLS = 2; UserXTDY = 22.5f;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G3");
                    AutoCommandList.Add("G5");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G3");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("K2");
                }

                if (BtnComBoxListUserBLFSIndex == 4) 
                {
                    UserCLS = "2"; UserBLS = 3; UserXTDY = 15;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G3");
                    AutoCommandList.Add("G5");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("1G6");
                    AutoCommandList.Add("K1");

                }
                if (BtnComBoxListUserBLFSIndex == 5) 
                {
                    UserCLS = "1"; UserBLS = 6; UserXTDY = 7.5f;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G3");
                    AutoCommandList.Add("G5");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("1G3");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("1G6");
                    AutoCommandList.Add("K2");
                }
            }

            if (BtnComBoxListUserXZJFIndex == 2)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    UserCLS = "3"; UserBLS = 6; UserXTDY = 22.5f;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("1G3");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("1G6");
                    AutoCommandList.Add("K2");
                }
                if (BtnComBoxListUserBLFSIndex == 1) 
                {
                    UserCLS = "6"; UserBLS = 3; UserXTDY = 45;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G2");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("1G6");
                    AutoCommandList.Add("K1");
                }

                if (BtnComBoxListUserBLFSIndex == 2)                 
                {
                    UserCLS = "9"; UserBLS = 2; UserXTDY = 67.5f;
                    AutoCommandList.Add("G2");
                    AutoCommandList.Add("G4");
                    AutoCommandList.Add("1G1");
                    AutoCommandList.Add("1G3");
                    AutoCommandList.Add("1G4");
                    AutoCommandList.Add("1G5");
                    AutoCommandList.Add("K2");
                }
            }

            if (UserXTDY < EstimatedVoltage)
            {
                MessageBox.Show("系统额定电压小于预估电压!");
                return;
            }



            if (EstimatedVoltage != 0 && EstimatedCurrent != 0 && UserXTDY != 0 && UserBLS != 0)
            {
                //预估试验电流(Us/UN） 1023.5 --  630 / (24/39)
                //这里不要这样直接去除法，要不然 这个就不对的..
                float data = EstimatedVoltage / UserXTDY;
                float ygsydl = EstimatedCurrent / data;
                //预估试验电流/并联数 == 每层需要的电流..
                float mcxydl = ygsydl / UserBLS;
                //等于每层电流值/额定单元电容电流（系统预先设置53.33）投的个数.
                string? dg = ConfigurationManager.AppSettings["BASEDATA"].ToString();
                float.TryParse(dg, out float x);
                float tbgs = mcxydl / x;
                tbgs = (float)Math.Round(tbgs, 2); // 保留两位小数
                UserMCJYGS = tbgs.ToString();
                
            }

        }

        //取消动作.电压..
        private void DoBtnCommandExeSetVCancel(object param)
        {
            mytoken = false;            
        }

        //下拉选择接法..
        public void DoCommandXZJF()
        {
            if (BtnComBoxListUserXZJFIndex == 0)
            {
                BtnComBoxListUserBLFS.Clear();
                BtnComBoxListUserBLFS.Add("6串1并");
                BtnComBoxListUserBLFS.Add("5串1并");
                BtnComBoxListUserBLFS.Add("4串1并");
                BtnComBoxListUserBLFS.Add("3串2并");
                BtnComBoxListUserBLFS.Add("2串3并");
                BtnComBoxListUserBLFS.Add("1串6并");

            }

            if (BtnComBoxListUserXZJFIndex == 1)
            {
                BtnComBoxListUserBLFS.Clear();
                BtnComBoxListUserBLFS.Add("6串1并");
                BtnComBoxListUserBLFS.Add("5串1并");
                BtnComBoxListUserBLFS.Add("4串1并");
                BtnComBoxListUserBLFS.Add("3串2并");
                BtnComBoxListUserBLFS.Add("2串3并");
                BtnComBoxListUserBLFS.Add("1串6并");

            }

            if (BtnComBoxListUserXZJFIndex == 2)
            {
                BtnComBoxListUserBLFS.Clear();
                BtnComBoxListUserBLFS.Add("3串6并");
                BtnComBoxListUserBLFS.Add("6串3并");
                BtnComBoxListUserBLFS.Add("9串2并");
            }            

        }

        //已经被排序了..
        private List<MyBCXSItem> myBCXSItems = new List<MyBCXSItem>();
        private List<MyBCXSItem> myBCXSItemsDx = new List<MyBCXSItem>();
        //计算补偿橡树
        private void DoBtnCommandExeBCXS(object param)
        {
            if (BtnComBoxListBCXSIndex == 0)
            {
                bool isfind = false;
                foreach (var item in myBCXSItems)
                {
                    if (item.Vold > EstimatedVoltage)
                    {
                        MessageBox.Show("推荐组合:" + item.Message);
                        isfind = true;
                        break;
                    }
                }
                if (isfind == false)
                {
                    MessageBox.Show("没有一个组合符合要求,请检查电压输入是否合适");
                    return;
                }
            }

            if (BtnComBoxListBCXSIndex == 1)
            {
                bool isfind = false;
                foreach (var item in myBCXSItemsDx)
                {
                    if (item.Vold > EstimatedVoltage)
                    {
                        MessageBox.Show("推荐组合:" + item.Message);
                        isfind = true;
                        break;
                    }
                }
                if (isfind == false)
                {
                    MessageBox.Show("没有一个单相组合符合要求,请检查电压输入是否合适");
                    return;
                }
            }

        }

    }

    public class MyBCXSItem
    {
        public float Vold { get; set; } = 0.0f;
        public string Message { get; set; } = string.Empty;
    }
}
