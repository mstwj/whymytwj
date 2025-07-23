using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using PLCNET5电容塔.ViewModel;
using PLCNET5电容塔升级.Base;
using S7.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace PLCNET5电容塔升级.ViewModel
{
    public class Step1WindowModel : ObservableValidator
    {
        //这里自动化还是比较麻烦，需要知道所有按钮对象的状态..
        //public Plc plc39 { get; set; }

        public Queue<BKCommand> queue { get; set; }

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        //默认补偿相数为0
        public int BtnComBoxListBCXSIndex { get; set; } = 0;
        public ObservableCollection<string> BtnComBoxListBCXS { get; set; } = new ObservableCollection<string>();


        //用户选择接法.
        public int BtnComBoxListUserXZJFIndex { get; set; }
        public ObservableCollection<string> BtnComBoxListUserXZJF { get; set; } = new ObservableCollection<string>();

        //用户选择 并联方式..
        public int BtnComBoxListUserBLFSIndex { get; set; }
        public ObservableCollection<string> BtnComBoxListUserBLFS { get; set; } = new ObservableCollection<string>();

        //接法.
        private string userjf = string.Empty;
        public string UserJF { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //串连数.
        private string usercls = string.Empty;
        public string UserCLS { get { return usercls; } set { SetProperty(ref usercls, value); } }

        //并连数.
        private float userbls = 0.0f;
        public float UserBLS { get { return userbls; } set { SetProperty(ref userbls, value); } }

        
        //预故电压
        private float estimatedfvoltage = 0.0f;
        [Required(ErrorMessage = "设置错误:必须输入预故电压")]
        [MyValidate(1000, 5, ErrorMessage = "设置错误:预故电压最大1000(KV)-最小5(KV)")]
        public float EstimatedVoltage { get { return estimatedfvoltage; } set { SetProperty(ref estimatedfvoltage, value); } }

        
        //预故电流
        private float estimatedcurrent = 0.0f;
        [Required(ErrorMessage = "设置错误:必须输入预故电流")]
        [MyValidate(10000, 1, ErrorMessage = "设置错误:预故电流最大10000(A)-最小1(A)")]
        public float EstimatedCurrent { get { return estimatedcurrent; } set { SetProperty(ref estimatedcurrent, value); } }

        //系统电压.
        private float userxtdy = 0.0f;
        public float UserXTDY { get { return userxtdy; } set { SetProperty(ref userxtdy, value); } }

        //建议投个数
        private string usermcjygs = string.Empty;
        public string UserMCJYGS { get { return usermcjygs; } set { SetProperty(ref usermcjygs, value); } }

        //显示执行流程..
        private string exerecord = string.Empty;
        public string Exerecord { get { return exerecord; } set { SetProperty(ref exerecord, value); } }


        //点击智能推算电压..
        public ICommand BtnCommandExeBCXS { get; set; }
        //点击开始计算投补个数..
        public ICommand BtnStarExeCount { get; set; }

        //开始执行动作了..
        public ICommand BtnCommandBtnExe { get; set; }

        public ICommand BtnCommandBtnCansel { get; set; }


        //自动执行的指令 ...
        public List<BKCommand> AutoCommandList = new List<BKCommand>();


        public Step1WindowModel()
        {
            //一般要习惯使用事件的方法.. 不要使用 什么自定义的事件..c# 本身就给了很好的解决方法..
            //今天理解了事件..
            //比如CAN 这个协议，或者是驱动，如果没有C#的 怎么办，只能 直接吧DLL 加入进来，然后，非托管的方法来使用了..
            BtnComBoxListBCXS.Add("三相");
            BtnComBoxListBCXS.Add("单相");

            BtnComBoxListUserXZJF.Add("Y接法");
            BtnComBoxListUserXZJF.Add("D接法");
            BtnComBoxListUserXZJF.Add("单相");

            BtnCommandExeBCXS = new RelayCommand<object>(DoBtnCommandExeBCXS);
            BtnStarExeCount = new RelayCommand<object>(DoBtnStarExeCount);
            BtnCommandBtnExe = new RelayCommand<object>(DoBtnCommandBtnExe);
            BtnCommandBtnCansel = new RelayCommand<object>(DoBtnCommandBtnCansel);

            //以下是推荐选择..(计算使用，用户不管..)
            myBCXSItems.Add(new MyBCXSItem() { Vold = 78f, Message = "Y接法-6串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 65f, Message = "Y接法-5串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 52f, Message = "Y接法-4串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 39f, Message = "Y接法-3串2并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 26f, Message = "Y接法-2串3并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 13f, Message = "Y接法-1串6并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 45f, Message = "D接法-6串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 37.5f, Message = "D接法-5串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 30f, Message = "D接法-4串1并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 22.5f, Message = "D接法-3串2并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 15f, Message = "D接法-2串3并" });
            myBCXSItems.Add(new MyBCXSItem() { Vold = 7.5f, Message = "D接法-1串6并" });

            myBCXSItemsDx.Add(new MyBCXSItem() { Vold = 22.5f, Message = "单相-3串6并" });
            myBCXSItemsDx.Add(new MyBCXSItem() { Vold = 45f, Message = "单相-6串3并" });
            myBCXSItemsDx.Add(new MyBCXSItem() { Vold = 67.5f, Message = "单相-9串2并" });

            myBCXSItems = myBCXSItems.OrderBy(n => n.Vold).ToList(); // 升序排序
            myBCXSItemsDx = myBCXSItemsDx.OrderBy(n => n.Vold).ToList(); // 升序排序

            WeakReferenceMessenger.Default.Register<string>(this, (r, user) =>
            {
                if (user == "结束执行动作全部指令完成")
                {
                    my_task = false;
                }
                Exerecord = user;
            });

        }

        public void OnMyClose()
        {
            WeakReferenceMessenger.Default.Unregister<string>(this);
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

            //启动.频率..       
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }

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


        //开始执行计算..
        private void DoBtnStarExeCount(object param)
        {
            //启动.频率..       
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }

            if (BtnComBoxListUserXZJFIndex == -1)
            {
                MessageBox.Show("接法还没有选择!");
                return ;
            }

            if (BtnComBoxListUserBLFSIndex == -1)
            {
                MessageBox.Show("并联方式还没有选择!");
                return ;
            }

            //选择接法..
            if (BtnComBoxListUserXZJFIndex == 0) UserJF = "Y";
            if (BtnComBoxListUserXZJFIndex == 1) UserJF = "D";
            if (BtnComBoxListUserXZJFIndex == 2) UserJF = "单相";

            if (BtnComBoxListUserXZJFIndex == 0)
            {
                //选择并联数....
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



            if (UserXTDY < EstimatedVoltage)
            {
                MessageBox.Show("系统额定电压小于预估电压!");
                return ;
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

                return ;
            }
            return ;
        }

        private bool my_task = false;
        //private Task m_task = null;

        private void DoBtnCommandBtnCansel(object param)
        {
            queue.Clear();
            AutoCommandList.Clear();
            my_task = false;
        }

        //开始合闸..
        private async void DoBtnCommandBtnExe(object param)
        {
            if (my_task == true)
            {
                MessageBox.Show("正在执行合闸动作");
                return ;
            }
            queue.Clear();
            AutoCommandList.Clear();

            if (BtnComBoxListUserBLFSIndex == -1)
            {
                MessageBox.Show("无法执行动作，请选择方式");
                return;
            }

            if (BtnComBoxListUserXZJFIndex == 0)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    //(6串1并)0 . G1 G4 1G2 K1 
                    //AutoCommandList.Add("G1");
                    AutoCommandList.Add(new BKCommand { Command = 100.4f, Data = true, CommandDescribe = "G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }
                if (BtnComBoxListUserBLFSIndex == 1)
                {
                    //AutoCommandList.Add("G1");
                    AutoCommandList.Add(new BKCommand { Command = 100.4f, Data = true, CommandDescribe = "G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                }
                if (BtnComBoxListUserBLFSIndex == 2)
                {
                    //AutoCommandList.Add("G1");
                    AutoCommandList.Add(new BKCommand { Command = 100.4f, Data = true, CommandDescribe = "G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }
                if (BtnComBoxListUserBLFSIndex == 3)
                {
                    //AutoCommandList.Add("G1");
                    AutoCommandList.Add(new BKCommand { Command = 100.4f, Data = true, CommandDescribe = "G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G3");
                    AutoCommandList.Add(new BKCommand { Command = 102.2f, Data = true, CommandDescribe = "1G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }
                if (BtnComBoxListUserBLFSIndex == 4)
                {
                    UserCLS = "2"; UserBLS = 3; UserXTDY = 26;
                    //AutoCommandList.Add("G1");
                    AutoCommandList.Add(new BKCommand { Command = 100.4f, Data = true, CommandDescribe = "G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G6");
                    AutoCommandList.Add(new BKCommand { Command = 103.0f, Data = true, CommandDescribe = "1G6和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }

                if (BtnComBoxListUserBLFSIndex == 5)
                {
                    //AutoCommandList.Add("G1");
                    AutoCommandList.Add(new BKCommand { Command = 100.4f, Data = true, CommandDescribe = "G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000  });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G3");
                    AutoCommandList.Add(new BKCommand { Command = 102.2f, Data = true, CommandDescribe = "1G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                    //AutoCommandList.Add("1G6");
                    AutoCommandList.Add(new BKCommand { Command = 103.0f, Data = true, CommandDescribe = "1G6和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }
            }

            if (BtnComBoxListUserXZJFIndex == 1)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G3");
                    AutoCommandList.Add(new BKCommand { Command = 101.0f, Data = true, CommandDescribe = "G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G5");
                    AutoCommandList.Add(new BKCommand { Command = 101.4f, Data = true, CommandDescribe = "G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }

                if (BtnComBoxListUserBLFSIndex == 1)
                {
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G3");
                    AutoCommandList.Add(new BKCommand { Command = 101.0f, Data = true, CommandDescribe = "G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G5");
                    AutoCommandList.Add(new BKCommand { Command = 101.4f, Data = true, CommandDescribe = "G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                }

                if (BtnComBoxListUserBLFSIndex == 2)
                {
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G3");
                    AutoCommandList.Add(new BKCommand { Command = 101.0f, Data = true, CommandDescribe = "G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G5");
                    AutoCommandList.Add(new BKCommand { Command = 101.4f, Data = true, CommandDescribe = "G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                }
                if (BtnComBoxListUserBLFSIndex == 3)
                {
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G3");
                    AutoCommandList.Add(new BKCommand { Command = 101.0f, Data = true, CommandDescribe = "G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G5");
                    AutoCommandList.Add(new BKCommand { Command = 101.4f, Data = true, CommandDescribe = "G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G3");
                    AutoCommandList.Add(new BKCommand { Command = 102.2f, Data = true, CommandDescribe = "1G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 105.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                }

                if (BtnComBoxListUserBLFSIndex == 4)
                {
                    UserCLS = "2"; UserBLS = 3; UserXTDY = 15;
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G3");
                    AutoCommandList.Add(new BKCommand { Command = 101.0f, Data = true, CommandDescribe = "G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G5");
                    AutoCommandList.Add(new BKCommand { Command = 101.4f, Data = true, CommandDescribe = "G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G6");
                    AutoCommandList.Add(new BKCommand { Command = 103.0f, Data = true, CommandDescribe = "1G6和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                }
                if (BtnComBoxListUserBLFSIndex == 5)
                {
                    UserCLS = "1"; UserBLS = 6; UserXTDY = 7.5f;
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G3");
                    AutoCommandList.Add(new BKCommand { Command = 101.0f, Data = true, CommandDescribe = "G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G5");
                    AutoCommandList.Add(new BKCommand { Command = 101.4f, Data = true, CommandDescribe = "G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G3");
                    AutoCommandList.Add(new BKCommand { Command = 102.2f, Data = true, CommandDescribe = "1G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G6");
                    AutoCommandList.Add(new BKCommand { Command = 103.0f, Data = true, CommandDescribe = "1G6和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }
            }

            if (BtnComBoxListUserXZJFIndex == 2)
            {
                //选择接法..
                if (BtnComBoxListUserBLFSIndex == 0)
                {
                    UserCLS = "3"; UserBLS = 6; UserXTDY = 22.5f;
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G3");
                    AutoCommandList.Add(new BKCommand { Command = 102.2f, Data = true, CommandDescribe = "1G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G6");
                    AutoCommandList.Add(new BKCommand { Command = 103.0f, Data = true, CommandDescribe = "1G6和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                }
                if (BtnComBoxListUserBLFSIndex == 1)
                {
                    UserCLS = "6"; UserBLS = 3; UserXTDY = 45;
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G2");
                    AutoCommandList.Add(new BKCommand { Command = 102.0f, Data = true, CommandDescribe = "1G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G6");
                    AutoCommandList.Add(new BKCommand { Command = 103.0f, Data = true, CommandDescribe = "1G6和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("K1");
                    //AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    //AutoCommandList.Add("K2");
                    AutoCommandList.Add(new BKCommand { Command = 103.4f, Data = true, CommandDescribe = "K2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }

                if (BtnComBoxListUserBLFSIndex == 2)
                {
                    UserCLS = "9"; UserBLS = 2; UserXTDY = 67.5f;
                    //AutoCommandList.Add("G2");
                    AutoCommandList.Add(new BKCommand { Command = 100.6f, Data = true, CommandDescribe = "G2和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("G4");
                    AutoCommandList.Add(new BKCommand { Command = 101.2f, Data = true, CommandDescribe = "G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G1");
                    AutoCommandList.Add(new BKCommand { Command = 101.6f, Data = true, CommandDescribe = "1G1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G3");
                    AutoCommandList.Add(new BKCommand { Command = 102.2f, Data = true, CommandDescribe = "1G3和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G4");
                    AutoCommandList.Add(new BKCommand { Command = 102.4f, Data = true, CommandDescribe = "1G4和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                    //AutoCommandList.Add("1G5");
                    AutoCommandList.Add(new BKCommand { Command = 102.6f, Data = true, CommandDescribe = "1G5和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });

                    //AutoCommandList.Add("K1");
                    AutoCommandList.Add(new BKCommand { Command = 103.2f, Data = true, CommandDescribe = "K1和闸" });
                    AutoCommandList.Add(new BKCommand { Command = 1000 });
                }
            }

           
            
            //1.先打开所有闸..

            queue.Enqueue(new BKCommand { Command = 100.5f, Data = true, CommandDescribe = "G1分闸" });

            queue.Enqueue(new BKCommand { Command = 100.7f, Data = true, CommandDescribe = "G2分闸" });
            queue.Enqueue(new BKCommand { Command = 101.1f, Data = true, CommandDescribe = "G3分闸" });
            queue.Enqueue(new BKCommand { Command = 101.3f, Data = true, CommandDescribe = "G4分闸" });
            queue.Enqueue(new BKCommand { Command = 101.5f, Data = true, CommandDescribe = "G5分闸" });

            queue.Enqueue(new BKCommand { Command = 101.7f, Data = true, CommandDescribe = "1G1分闸" });
            queue.Enqueue(new BKCommand { Command = 102.1f, Data = true, CommandDescribe = "1G2分闸" });
            queue.Enqueue(new BKCommand { Command = 102.3f, Data = true, CommandDescribe = "1G3分闸" });
            queue.Enqueue(new BKCommand { Command = 102.5f, Data = true, CommandDescribe = "1G4分闸" });
            queue.Enqueue(new BKCommand { Command = 102.7f, Data = true, CommandDescribe = "1G5分闸" });
            queue.Enqueue(new BKCommand { Command = 103.1f, Data = true, CommandDescribe = "1G6分闸" });

            queue.Enqueue(new BKCommand { Command = 103.3f, Data = true, CommandDescribe = "K1分闸" });
            queue.Enqueue(new BKCommand { Command = 103.5f, Data = true, CommandDescribe = "K2分闸" });

            queue.Enqueue(new BKCommand { Command = 99999, Data = true, CommandDescribe = "全部分闸动作完成..." });
            queue.Enqueue(new BKCommand { Command = 1000 });
            //这里主线程，会不会等待呢？不会等待。。如果是
            //var t = Task.Run(async () => 注意，上面没有await.. 主线程看到这个代码，是返回。
            //这里有个问题，就是加了 = await 居然报错..注意这里加了= 为什么加个等于就报错呢？因为这里加了= 就有问题了，因为主线程返回了，那么这个=就应该=
            //所以必须使用其他方法。。比如 这样。。 await Task.Run(async () => -- 不能这样直接写..


            AutoCommandList.Add(new BKCommand { Command = 99999, Data = true, CommandDescribe = "全部指令完成" });

            //这里肯定等待..
            foreach (var item in AutoCommandList)
            {
                queue.Enqueue(item);
            }
            
            my_task = true;
            

        }


    }



}
