using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using PLCNET5电容塔.ViewModel;
using PLCNET5电容塔升级.Base;
using S7.Net;
using System;
using System.Collections.Generic;
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
    public class Step2WindowModel : ObservableObject
    {

        private string levelshowtext = "每层实投补电容数";
        public string LevelShowText { get { return levelshowtext; } set { SetProperty(ref levelshowtext, value); } }

        //public Plc plc39 { get; set; }

        public Queue<BKCommand> queue { get; set; }

        //注意2个文件必须拷贝到EXE文件目录下..
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        //接法.
        private string userjf = string.Empty;
        public string UserJF { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //串连数.
        private string usercls = string.Empty;
        public string UserCLS { get { return userjf; } set { SetProperty(ref userjf, value); } }

        //并连数.
        private float userbls = 0.0f;
        public float UserBLS { get { return userbls; } set { SetProperty(ref userbls, value); } }


        //预故电压
        private float estimatedfvoltage = 0.0f;
        public float EstimatedVoltage { get { return estimatedfvoltage; } set { SetProperty(ref estimatedfvoltage, value); } }


        //预故电流
        private float estimatedcurrent = 0.0f;
        public float EstimatedCurrent { get { return estimatedcurrent; } set { SetProperty(ref estimatedcurrent, value); } }

        //系统电压.
        private float userxtdy = 0.0f;
        public float UserXTDY { get { return userxtdy; } set { SetProperty(ref userxtdy, value); } }

        //建议投个数
        private string usermcjygs = string.Empty;
        public string UserMCJYGS { get { return usermcjygs; } set { SetProperty(ref usermcjygs, value); } }


        //第一层状态..
        public bool CheckBoxLevel_1_1 { get; set; }
        public bool CheckBoxLevel_1_2 { get; set; }
        public bool CheckBoxLevel_1_3 { get; set; }
        public bool CheckBoxLevel_1_4 { get; set; }
        public bool CheckBoxLevel_1_5 { get; set; }
        public bool CheckBoxLevel_1_6 { get; set; }

        //第2层状态..
        public bool CheckBoxLevel_2_1 { get; set; }
        public bool CheckBoxLevel_2_2 { get; set; }
        public bool CheckBoxLevel_2_3 { get; set; }
        public bool CheckBoxLevel_2_4 { get; set; }
        public bool CheckBoxLevel_2_5 { get; set; }
        public bool CheckBoxLevel_2_6 { get; set; }

        //第3层状态..
        public bool CheckBoxLevel_3_1 { get; set; }
        public bool CheckBoxLevel_3_2 { get; set; }
        public bool CheckBoxLevel_3_3 { get; set; }
        public bool CheckBoxLevel_3_4 { get; set; }
        public bool CheckBoxLevel_3_5 { get; set; }
        public bool CheckBoxLevel_3_6 { get; set; }


        //第4层状态..
        public bool CheckBoxLevel_4_1 { get; set; }
        public bool CheckBoxLevel_4_2 { get; set; }
        public bool CheckBoxLevel_4_3 { get; set; }
        public bool CheckBoxLevel_4_4 { get; set; }
        public bool CheckBoxLevel_4_5 { get; set; }
        public bool CheckBoxLevel_4_6 { get; set; }


        //第5层状态..
        public bool CheckBoxLevel_5_1 { get; set; }
        public bool CheckBoxLevel_5_2 { get; set; }
        public bool CheckBoxLevel_5_3 { get; set; }
        public bool CheckBoxLevel_5_4 { get; set; }
        public bool CheckBoxLevel_5_5 { get; set; }
        public bool CheckBoxLevel_5_6 { get; set; }

        //第6层状态..
        public bool CheckBoxLevel_6_1 { get; set; }
        public bool CheckBoxLevel_6_2 { get; set; }
        public bool CheckBoxLevel_6_3 { get; set; }
        public bool CheckBoxLevel_6_4 { get; set; }
        public bool CheckBoxLevel_6_5 { get; set; }
        public bool CheckBoxLevel_6_6 { get; set; }


        //开始计算仿真的数据.
        public ICommand BtnCommandStartCount { get; set; }

        //开始执行动作
        public ICommand BtnCommandStartExe { get; set; }

        //取消执行动作
        public ICommand BtnCommandBtnCanExe { get; set; }

        //自动执行的指令 ...
        public List<string> AutoCommandList = new List<string>();

        //每层实际投入数量..
        private float mcsjtrsl = 0.0f;
        public float McsjTrsl { get { return mcsjtrsl; } set { SetProperty(ref mcsjtrsl, value); } }

        //反推实验电流
        private float ftsydl = 0.0f;
        public float Ftsydl { get { return ftsydl; } set { SetProperty(ref ftsydl, value); } }

        //偏差值
        private float pcz = 0.0f;
        public float Pcz { get { return pcz; } set { SetProperty(ref pcz, value); } }


        //显示执行流程..
        private string exerecord = string.Empty;
        public string Exerecord { get { return exerecord; } set { SetProperty(ref exerecord, value); } }


        public Step2WindowModel()
        {
            BtnCommandStartCount = new RelayCommand<object>(DoBtnCommandStartCount);
            BtnCommandStartExe = new RelayCommand<object>(DoBtnCommandStartExe);
            BtnCommandBtnCanExe = new RelayCommand<object>(DoBtnCommandBtnCanExe);
            WeakReferenceMessenger.Default.Register<string>(this, (r, user) =>
            {
                if (user == "结束执行动作全部指令完成")
                {
                    my_task = false;
                }
                Exerecord = user;
            });
        }

        public void OnClose()
        {
            WeakReferenceMessenger.Default.Unregister<string>(this);
        }

        public void SetTextShow()
        {
            if (UserCLS == "1" && UserBLS == 6)
            {
                LevelShowText = "所有层总投补电容数";
            }

        }

        private bool my_task = false;

        //开始执行
        private async void DoBtnCommandStartExe(object param)
        {
            if (my_task == true)
            {
                MessageBox.Show("正在执行动作...");
                return;
            }

            //开始执行自动化动作..
            //这里主线程，会不会等待呢？不会等待。。如果是
            //var t = Task.Run(async () => 注意，上面没有await.. 主线程看到这个代码，是返回。
            //这里有个问题，就是加了 = await 居然报错..注意这里加了= 为什么加个等于就报错呢？因为这里加了= 就有问题了，因为主线程返回了，那么这个=就应该=
            //所以必须使用其他方法。。比如 这样。。 await Task.Run(async () => -- 不能这样直接写..
            //这里 需要1对1来对应了...


            //开始执行自动化动作..

            //这里主线程，会不会等待呢？不会等待。。如果是
            //var t = Task.Run(async () => 注意，上面没有await.. 主线程看到这个代码，是返回。
            //这里有个问题，就是加了 = await 居然报错..注意这里加了= 为什么加个等于就报错呢？因为这里加了= 就有问题了，因为主线程返回了，那么这个=就应该=
            //所以必须使用其他方法。。比如 这样。。 await Task.Run(async () => -- 不能这样直接写..
            myItem[] AllGateF =
            {
                new myItem { Name = "CheckBoxLevel_1_1", State = CheckBoxLevel_1_1 ,Command = new BKCommand { Command = 103.6f, Data = CheckBoxLevel_1_1, CommandDescribe = "1G5和闸" }},
                new myItem { Name = "CheckBoxLevel_1_2", State = CheckBoxLevel_1_2 ,Command = new BKCommand { Command = 103.7f, Data = CheckBoxLevel_1_2, CommandDescribe = "1G5和闸" }},
                new myItem { Name = "CheckBoxLevel_1_3", State = CheckBoxLevel_1_3 ,Command = new BKCommand { Command = 104.0f, Data = CheckBoxLevel_1_3, CommandDescribe = "1G5和闸" }},
                new myItem { Name = "CheckBoxLevel_1_4", State = CheckBoxLevel_1_4 ,Command = new BKCommand { Command = 104.1f, Data = CheckBoxLevel_1_4, CommandDescribe = "1G5和闸" }},
                new myItem { Name = "CheckBoxLevel_1_5", State = CheckBoxLevel_1_5 ,Command = new BKCommand { Command = 104.2f, Data = CheckBoxLevel_1_5, CommandDescribe = "1G5和闸" } },
                new myItem { Name = "CheckBoxLevel_1_6", State = CheckBoxLevel_1_6 ,Command = new BKCommand { Command = 104.3f, Data = CheckBoxLevel_1_6, CommandDescribe = "1G5和闸" } },

                new myItem { Name = "CheckBoxLevel_2_1", State = CheckBoxLevel_2_1 ,Command = new BKCommand { Command = 104.4f, Data = CheckBoxLevel_2_1, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_2_2", State = CheckBoxLevel_2_2 ,Command = new BKCommand { Command = 104.5f, Data = CheckBoxLevel_2_2, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_2_3", State = CheckBoxLevel_2_3 ,Command = new BKCommand { Command = 104.6f, Data = CheckBoxLevel_2_3, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_2_4", State = CheckBoxLevel_2_4 ,Command = new BKCommand { Command = 104.7f, Data = CheckBoxLevel_2_4, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_2_5", State = CheckBoxLevel_2_5 ,Command = new BKCommand { Command = 105.0f, Data = CheckBoxLevel_2_5, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_2_6", State = CheckBoxLevel_2_6 ,Command = new BKCommand { Command = 105.1f, Data = CheckBoxLevel_2_6, CommandDescribe = "" }},

                new myItem { Name = "CheckBoxLevel_3_1", State = CheckBoxLevel_3_1 ,Command = new BKCommand { Command = 105.2f, Data = CheckBoxLevel_3_1, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_3_2", State = CheckBoxLevel_3_2 ,Command = new BKCommand { Command = 105.3f, Data = CheckBoxLevel_3_2, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_3_3", State = CheckBoxLevel_3_3 ,Command = new BKCommand { Command = 105.4f, Data = CheckBoxLevel_3_3, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_3_4", State = CheckBoxLevel_3_4 ,Command = new BKCommand { Command = 105.5f, Data = CheckBoxLevel_3_4, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_3_5", State = CheckBoxLevel_3_5 ,Command = new BKCommand { Command = 105.6f, Data = CheckBoxLevel_3_5, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_3_6", State = CheckBoxLevel_3_6 ,Command = new BKCommand { Command = 105.7f, Data = CheckBoxLevel_3_6, CommandDescribe = "" }},

                new myItem { Name = "CheckBoxLevel_4_1", State = CheckBoxLevel_4_1 ,Command = new BKCommand { Command = 106.0f, Data = CheckBoxLevel_4_1, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_4_2", State = CheckBoxLevel_4_2 ,Command = new BKCommand { Command = 106.1f, Data = CheckBoxLevel_4_2, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_4_3", State = CheckBoxLevel_4_3 ,Command = new BKCommand { Command = 106.2f, Data = CheckBoxLevel_4_3, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_4_4", State = CheckBoxLevel_4_4 ,Command = new BKCommand { Command = 106.3f, Data = CheckBoxLevel_4_4, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_4_5", State = CheckBoxLevel_4_5 ,Command = new BKCommand { Command = 106.4f, Data = CheckBoxLevel_4_5, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_4_6", State = CheckBoxLevel_4_6 ,Command = new BKCommand { Command = 106.5f, Data = CheckBoxLevel_4_6, CommandDescribe = "" }},

                new myItem { Name = "CheckBoxLevel_5_1", State = CheckBoxLevel_5_1 ,Command = new BKCommand { Command = 106.6f, Data = CheckBoxLevel_5_1, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_5_2", State = CheckBoxLevel_5_2 ,Command = new BKCommand { Command = 106.7f, Data = CheckBoxLevel_5_2, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_5_3", State = CheckBoxLevel_5_3 ,Command = new BKCommand { Command = 107.0f, Data = CheckBoxLevel_5_3, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_5_4", State = CheckBoxLevel_5_4 ,Command = new BKCommand { Command = 107.1f, Data = CheckBoxLevel_5_4, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_5_5", State = CheckBoxLevel_5_5 ,Command = new BKCommand { Command = 107.2f, Data = CheckBoxLevel_5_5, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_5_6", State = CheckBoxLevel_5_6 ,Command = new BKCommand { Command = 107.3f, Data = CheckBoxLevel_5_6, CommandDescribe = "" }},

                new myItem { Name = "CheckBoxLevel_6_1", State = CheckBoxLevel_6_1 ,Command = new BKCommand { Command = 107.4f, Data = CheckBoxLevel_6_1, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_6_2", State = CheckBoxLevel_6_2 ,Command = new BKCommand { Command = 107.5f, Data = CheckBoxLevel_6_2, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_6_3", State = CheckBoxLevel_6_3 ,Command = new BKCommand { Command = 107.6f, Data = CheckBoxLevel_6_3, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_6_4", State = CheckBoxLevel_6_4 ,Command = new BKCommand { Command = 107.7f, Data = CheckBoxLevel_6_4, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_6_5", State = CheckBoxLevel_6_5 ,Command = new BKCommand { Command = 108.0f, Data = CheckBoxLevel_6_5, CommandDescribe = "" }},
                new myItem { Name = "CheckBoxLevel_6_6", State = CheckBoxLevel_6_6 ,Command = new BKCommand { Command = 108.1f, Data = CheckBoxLevel_6_6, CommandDescribe = "" }},
            };             
                    //bool state = false;
                    //1.先
             foreach (var item in AllGateF)
             {
                 if (InspectExc(item.Name) == item.State)
                 {
                    continue;
                 }

                 queue.Enqueue(item.Command);

             }
            queue.Enqueue(new BKCommand { Command = 99999, Data = true, CommandDescribe = "全部指令完成" });


            //这里就是 m_task.wait().. 主线程在等待 子线程结束...(本质上，关闭窗口也应该这样的.)
            my_task = false;
      
        }


        public bool InspectExc(string Command)
        {
            switch (Command)
            {
                case "CheckBoxLevel_1_1": return MyTools.myButtonQ1_1.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_1_2": return MyTools.myButtonQ1_2.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_1_3": return MyTools.myButtonQ1_3.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_1_4": return MyTools.myButtonQ1_4.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_1_5": return MyTools.myButtonQ1_5.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_1_6": return MyTools.myButtonQ1_6.myButtonModel.Data; //"总输出AC分闸"

                case "CheckBoxLevel_2_1": return MyTools.myButtonQ2_1.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_2_2": return MyTools.myButtonQ2_2.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_2_3": return MyTools.myButtonQ2_3.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_2_4": return MyTools.myButtonQ2_4.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_2_5": return MyTools.myButtonQ2_5.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_2_6": return MyTools.myButtonQ2_6.myButtonModel.Data; //"总输出AC分闸"

                case "CheckBoxLevel_3_1": return MyTools.myButtonQ3_1.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_3_2": return MyTools.myButtonQ3_2.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_3_3": return MyTools.myButtonQ3_3.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_3_4": return MyTools.myButtonQ3_4.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_3_5": return MyTools.myButtonQ3_5.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_3_6": return MyTools.myButtonQ3_6.myButtonModel.Data; //"总输出AC分闸"

                case "CheckBoxLevel_4_1": return MyTools.myButtonQ4_1.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_4_2": return MyTools.myButtonQ4_2.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_4_3": return MyTools.myButtonQ4_3.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_4_4": return MyTools.myButtonQ4_4.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_4_5": return MyTools.myButtonQ4_5.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_4_6": return MyTools.myButtonQ4_6.myButtonModel.Data; //"总输出AC分闸"

                case "CheckBoxLevel_5_1": return MyTools.myButtonQ5_1.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_5_2": return MyTools.myButtonQ5_2.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_5_3": return MyTools.myButtonQ5_3.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_5_4": return MyTools.myButtonQ5_4.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_5_5": return MyTools.myButtonQ5_5.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_5_6": return MyTools.myButtonQ5_6.myButtonModel.Data; //"总输出AC分闸"

                case "CheckBoxLevel_6_1": return MyTools.myButtonQ6_1.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_6_2": return MyTools.myButtonQ6_2.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_6_3": return MyTools.myButtonQ6_3.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_6_4": return MyTools.myButtonQ6_4.myButtonModel.Data; //"总输出AC分闸"
                case "CheckBoxLevel_6_5": return MyTools.myButtonQ6_5.myButtonModel.Data; //"总输出AC和闸"
                case "CheckBoxLevel_6_6": return MyTools.myButtonQ6_6.myButtonModel.Data; //"总输出AC分闸"
                
            }
            throw new Exception("没有一个选中!");


        }


        //取消执行
        private void DoBtnCommandBtnCanExe(object param)
        {
            queue.Clear();
            my_task = false;
        }


        //开始计算
        private void DoBtnCommandStartCount(object param)
        {
            if (UserBLS == 0)
            {
                MessageBox.Show("无法计算,并联数为0");
                return;
            }

            if (EstimatedVoltage == 0)
            {
                MessageBox.Show("无法计算,用户设置电压为0");
                return;
            }

            if (UserXTDY == 0)
            {
                MessageBox.Show("无法计算,系统电压为0");
                return;
            }

            if (EstimatedCurrent ==  0)
            {
                MessageBox.Show("无法计算,用户设置电流为0");
                return;
            }


            float count = 0.0f;

            //这里有个特殊的东西，就是 1串6并..
            if (UserCLS == "1" && UserBLS == 6)
            {
                float count1 = 0.0f;
                float count2 = 0.0f;
                float count3 = 0.0f;
                float count4 = 0.0f;
                float count5 = 0.0f;
                float count6 = 0.0f;
                //这里我只计算，第1层...
                if (CheckBoxLevel_1_1 == true) count1 += 0.25f;
                if (CheckBoxLevel_1_2 == true) count1 += 0.5f;
                if (CheckBoxLevel_1_3 == true) count1 += 1f;
                if (CheckBoxLevel_1_4 == true) count1 += 2f;
                if (CheckBoxLevel_1_5 == true) count1 += 4f;
                if (CheckBoxLevel_1_6 == true) count1 += 8f;

                //这里我只计算，第2层...
                if (CheckBoxLevel_2_1 == true) count2 += 0.25f;
                if (CheckBoxLevel_2_2 == true) count2 += 0.5f;
                if (CheckBoxLevel_2_3 == true) count2 += 1f;
                if (CheckBoxLevel_2_4 == true) count2 += 2f;
                if (CheckBoxLevel_2_5 == true) count2 += 4f;
                if (CheckBoxLevel_2_6 == true) count2 += 8f;

                //这里我只计算，第3层...
                if (CheckBoxLevel_3_1 == true) count3 += 0.25f;
                if (CheckBoxLevel_3_2 == true) count3 += 0.5f;
                if (CheckBoxLevel_3_3 == true) count3 += 1f;
                if (CheckBoxLevel_3_4 == true) count3 += 2f;
                if (CheckBoxLevel_3_5 == true) count3 += 4f;
                if (CheckBoxLevel_3_6 == true) count3 += 8f;


                //这里我只计算，第4层...
                if (CheckBoxLevel_4_1 == true) count4 += 0.25f;
                if (CheckBoxLevel_4_2 == true) count4 += 0.5f;
                if (CheckBoxLevel_4_3 == true) count4 += 1f;
                if (CheckBoxLevel_4_4 == true) count4 += 2f;
                if (CheckBoxLevel_4_5 == true) count4 += 4f;
                if (CheckBoxLevel_4_6 == true) count4 += 8f;

                //这里我只计算，第5层...
                if (CheckBoxLevel_5_1 == true) count5 += 0.25f;
                if (CheckBoxLevel_5_2 == true) count5 += 0.5f;
                if (CheckBoxLevel_5_3 == true) count5 += 1f;
                if (CheckBoxLevel_5_4 == true) count5 += 2f;
                if (CheckBoxLevel_5_5 == true) count5 += 4f;
                if (CheckBoxLevel_5_6 == true) count5 += 8f;

                //这里我只计算，第6层...
                if (CheckBoxLevel_6_1 == true) count6 += 0.25f;
                if (CheckBoxLevel_6_2 == true) count6 += 0.5f;
                if (CheckBoxLevel_6_3 == true) count6 += 1f;
                if (CheckBoxLevel_6_4 == true) count6 += 2f;
                if (CheckBoxLevel_6_5 == true) count6 += 4f;
                if (CheckBoxLevel_6_6 == true) count6 += 8f;

                //这里是实际投..
                McsjTrsl = count1+ count2+ count3+ count4+ count5+ count6;
                // 反推试验电流 = 实际投 * 单个电容 * 并联数 (预估电压 / 系统电压)
                string data = ConfigurationManager.AppSettings["BASEDATA"];
                float tp;
                float.TryParse(data, out tp);
                Ftsydl = McsjTrsl * tp * UserBLS * (EstimatedVoltage / UserXTDY);
                //计算偏差值
                Pcz = EstimatedCurrent - Ftsydl;
                //保留两位小数
                Pcz = (float)Math.Round(Pcz, 2);
                //在取一个绝对值.
                Pcz = Math.Abs(Pcz);

                return;

            }

            //这里我只计算，第一层...
            if (CheckBoxLevel_1_1 == true) 
            {
                //如果选了第一层，这里会去联动.. 其他层就要和第一层同步...
                if (CheckBoxLevel_2_1 == false || CheckBoxLevel_3_1 == false || CheckBoxLevel_4_1 == false || CheckBoxLevel_5_1 == false || CheckBoxLevel_6_1 == false)
                {
                    MessageBox.Show("选择错误,这样选择,无法计算电流!除1串6并外,其他开关,必须和第一层相同.");
                    return;
                }
                count += 0.25f;
            }

            if (CheckBoxLevel_1_2 == true)
            {
                if (CheckBoxLevel_2_2 == false || CheckBoxLevel_3_2 == false || CheckBoxLevel_4_2 == false || CheckBoxLevel_5_2 == false || CheckBoxLevel_6_2 == false)
                {
                    MessageBox.Show("选择错误,这样选择,无法计算电流!除1串6并外,其他开关,必须和第一层相同.");
                    return;
                }
                count += 0.5f;
            }
            if (CheckBoxLevel_1_3 == true)
            {
                if (CheckBoxLevel_2_3 == false || CheckBoxLevel_3_3 == false || CheckBoxLevel_4_3 == false || CheckBoxLevel_5_3 == false || CheckBoxLevel_6_3 == false)
                {
                    MessageBox.Show("选择错误,这样选择,无法计算电流!除1串6并外,其他开关,必须和第一层相同.");
                    return;
                }
                count += 1f;
            }
            if (CheckBoxLevel_1_4 == true)
            {
                if (CheckBoxLevel_2_4 == false || CheckBoxLevel_3_4 == false || CheckBoxLevel_4_4 == false || CheckBoxLevel_5_4 == false || CheckBoxLevel_6_4 == false)
                {
                    MessageBox.Show("选择错误,这样选择,无法计算电流!除1串6并外,其他开关,必须和第一层相同.");
                    return;
                }

                count += 2f;
            }
            if (CheckBoxLevel_1_5 == true)
            {
                if (CheckBoxLevel_2_5 == false || CheckBoxLevel_3_5 == false || CheckBoxLevel_4_5 == false || CheckBoxLevel_5_5 == false || CheckBoxLevel_6_5 == false)
                {
                    MessageBox.Show("选择错误,这样选择,无法计算电流!除1串6并外,其他开关,必须和第一层相同.");
                    return;
                }

                count += 4f;
            }
            if (CheckBoxLevel_1_6 == true)
            {
                if (CheckBoxLevel_2_6 == false || CheckBoxLevel_3_6 == false || CheckBoxLevel_4_6 == false || CheckBoxLevel_5_6 == false || CheckBoxLevel_6_6 == false)
                {
                    MessageBox.Show("选择错误,这样选择,无法计算电流!除1串6并外,其他开关,必须和第一层相同.");
                    return;
                }

                count += 8f;
            }

            //这里是实际投..
            McsjTrsl = count;
            // 反推试验电流 = 实际投 * 单个电容 * 并联数 (预估电压 / 系统电压)
            string datas = ConfigurationManager.AppSettings["BASEDATA"];
            float temp;
            float.TryParse(datas, out temp);
            Ftsydl = count * temp * UserBLS * (EstimatedVoltage / UserXTDY);
            //计算偏差值
            Pcz = EstimatedCurrent - Ftsydl;
            //保留两位小数
            Pcz = (float)Math.Round(Pcz, 2);
            //在取一个绝对值.
            Pcz = Math.Abs(Pcz);

        }

    }

    public class myItem
    {
        public string Name { get; set; }
        public bool State { get; set; }
        public BKCommand Command { get; set; }
    }
}
