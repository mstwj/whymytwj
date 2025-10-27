using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using 无功功率补偿.View;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace 无功功率补偿.ViewModel
{
    public class MainWindowViewModel : ObservableValidator
    {
        private float adianya = 220;
        public float Adianya { get { return adianya; } set { SetProperty(ref adianya, value); } }

        private float bdianya = 220;
        public float Bdianya { get { return bdianya; } set { SetProperty(ref bdianya, value); } }

        private float cdianya = 220;
        public float Cdianya { get { return cdianya; } set { SetProperty(ref cdianya, value); } }



        private float adianliu = 245;
        public float Adianliu { get { return adianliu; } set { SetProperty(ref adianliu, value); } }

        private float bdianliu = 245;
        public float Bdianliu { get { return bdianliu; } set { SetProperty(ref bdianliu, value); } }

        private float cdianliu = 245;
        public float Cdianliu { get { return cdianliu; } set { SetProperty(ref cdianliu, value); } }

        private float oldfactor = 0.50f;
        public float Oldfactor { get { return oldfactor; } set { SetProperty(ref oldfactor, value); } }


        /// <summary>
        /// ////////////////////////////显示////////////////////////////////////////////
        /// </summary>
        /// 
        private float showAdianya;
        public float ShowAdianya { get { return showAdianya; } set { SetProperty(ref showAdianya, value); } }

        private float showBdianya;
        public float ShowBdianya { get { return showBdianya; } set { SetProperty(ref showBdianya, value); } }

        private float showCdianya;
        public float ShowCdianya { get { return showCdianya; } set { SetProperty(ref showCdianya, value); } }




        //电流
        private float showAdianliu;
        public float ShowAdianliu { get { return showAdianliu; } set { SetProperty(ref showAdianliu, value); } }

        private float showBdianliu;
        public float ShowBdianliu { get { return showBdianliu; } set { SetProperty(ref showBdianliu, value); } }

        private float showCdianliu;
        public float ShowCdianliu { get { return showCdianliu; } set { SetProperty(ref showCdianliu, value); } }

        //有功功率
        private float showAyggl;
        public float ShowAyggl { get { return showAyggl; } set { SetProperty(ref showAyggl, value); } }

        private float showByggl;
        public float ShowByggl { get { return showByggl; } set { SetProperty(ref showByggl, value); } }

        private float showCyggl;
        public float ShowCyggl { get { return showCyggl; } set { SetProperty(ref showCyggl, value); } }


        //无功功率
        private float showAwggl;
        public float ShowAwggl { get { return showAwggl; } set { SetProperty(ref showAwggl, value); } }

        private float showBwggl;
        public float ShowBwggl { get { return showBwggl; } set { SetProperty(ref showBwggl, value); } }

        private float showCwggl;
        public float ShowCwggl { get { return showCwggl; } set { SetProperty(ref showCwggl, value); } }

        //实在功率
        private float showASzgl;
        public float ShowASzgl { get { return showASzgl; } set { SetProperty(ref showASzgl, value); } }

        private float showBSzgl;
        public float ShowBSzgl { get { return showBSzgl; } set { SetProperty(ref showBSzgl, value); } }

        private float showCSzgl;
        public float ShowCSzgl { get { return showCSzgl; } set { SetProperty(ref showCSzgl, value); } }


        //功率因数
        private float showAglys;
        public float ShowAglys { get { return showAglys; } set { SetProperty(ref showAglys, value); } }

        private float showBglys;
        public float ShowBglys { get { return showBglys; } set { SetProperty(ref showBglys, value); } }

        private float showCglys;
        public float ShowCglys { get { return showCglys; } set { SetProperty(ref showCglys, value); } }


        /// <summary>
        /// //////////////////////////////
        /// </summary>
        /// 
        private float showAdianyajb;
        public float ShowAdianyajb { get { return showAdianyajb; } set { SetProperty(ref showAdianyajb, value); } }


        private float showBdianyajb;
        public float ShowBdianyajb { get { return showBdianyajb; } set { SetProperty(ref showBdianyajb, value); } }


        private float showCdianyajb;
        public float ShowCdianyajb { get { return showCdianyajb; } set { SetProperty(ref showCdianyajb, value); } }




        private float showAdianliujb;
        public float ShowAdianliujb { get { return showAdianliujb; } set { SetProperty(ref showAdianliujb, value); } }


        private float showBdianliujb;
        public float ShowBdianliujb { get { return showBdianliujb; } set { SetProperty(ref showBdianliujb, value); } }


        private float showCdianliujb;
        public float ShowCdianliujb { get { return showCdianliujb; } set { SetProperty(ref showCdianliujb, value); } }


        //ABC 三相无功功率
        private float showABCwggl;
        public float ShowABCwggl { get { return showABCwggl; } set { SetProperty(ref showABCwggl, value); } }

        //ABC 三相 实在功率 
        private float sShowABCszgl;
        public float ShowABCszgl { get { return sShowABCszgl; } set { SetProperty(ref sShowABCszgl, value); } }


        //碳排放 1.2.3 火
        private float showtpf1, showtpf2, showtpf3;
        public float ShowTPF1 { get { return showtpf1; } set { SetProperty(ref showtpf1, value); } }
        public float ShowTPF2 { get { return showtpf2; } set { SetProperty(ref showtpf2, value); } }
        public float ShowTPF3 { get { return showtpf3; } set { SetProperty(ref showtpf3, value); } }

        //碳排放 4.5.6 风
        private float showtpf4, showtpf5, showtpf6;
        public float ShowTPF4 { get { return showtpf4; } set { SetProperty(ref showtpf4, value); } }
        public float ShowTPF5 { get { return showtpf5; } set { SetProperty(ref showtpf5, value); } }
        public float ShowTPF6 { get { return showtpf6; } set { SetProperty(ref showtpf6, value); } }

        //碳排放 7.8.9 光伏
        private float showtpf7, showtpf8, showtpf9;
        public float ShowTPF7 { get { return showtpf7; } set { SetProperty(ref showtpf7, value); } }
        public float ShowTPF8 { get { return showtpf8; } set { SetProperty(ref showtpf8, value); } }
        public float ShowTPF9 { get { return showtpf9; } set { SetProperty(ref showtpf9, value); } }

        //碳排放 10.11.12 水利
        private float showtpf10, showtpf11, showtpf12;
        public float ShowTPF10 { get { return showtpf10; } set { SetProperty(ref showtpf10, value); } }
        public float ShowTPF11 { get { return showtpf11; } set { SetProperty(ref showtpf11, value); } }
        public float ShowTPF12 { get { return showtpf12; } set { SetProperty(ref showtpf12, value); } }

        private float showdrwd = 21.0f;
        public float ShowDRWD { get { return showdrwd; } set { SetProperty(ref showdrwd, value); } }


        private float showtrdn;
        public float ShowTrdn { get { return showtrdn; } set { SetProperty(ref showtrdn, value); } }

        //按钮计算
        public ICommand BtnCommandStart { get; set; }

        //第2个按钮计算补偿
        public ICommand BtnCommandStart2 { get; set; }
        public ICommand BtnCommandStart3 { get; set; }
        public ICommand BtnCommandPut { get; set; }
        public ICommand BtnCommandStartStop { get; set; }

        public ICommand BtnCommandStart5 { get; set; }

        public ICommand BtnCommandStartStop5 { get; set; }

        public MainWindowViewModel()
        {
            BtnCommandStart = new RelayCommand<object>(DoBtnCommandStart);
            BtnCommandStart2 = new RelayCommand<object>(DoBtnCommandStart2);
            BtnCommandStart3 = new RelayCommand<object>(DoBtnCommandStart3);
            BtnCommandPut = new RelayCommand<object>(DoBtnCommandPut);
            BtnCommandStartStop = new RelayCommand<object>(DoBtnCommandStartStop);
            BtnCommandStart5 = new RelayCommand<object>(DoBtnCommandStart5);
            BtnCommandStartStop5 = new RelayCommand<object>(DoBtnCommandStartStop5);
        }


        private void DoBtnCommandStartStop5(object param)
        {


        }

        private void DoBtnCommandStart5(object param)
        {
            //前一个表..(写2个表.. 前一个表是0.5 后一个表是0.99 -- 有中间值..)
            //前一个表..
            //电压 -- 变，小变..
            //电流 -- 变，中变..




        }

        private void DoBtnCommandStartStop(object param)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }

        private void DoBtnCommandPut(object param)
        {            
        }

        private void DoBtnCommandStart(object param)
        {
        }

        private void DoBtnCommandStart3(object param)
        {
            Test1 test1 = new Test1();
            test1.ShowDialog(); 
        }



        private CancellationTokenSource? cancellationTokenSource { get; set; }



        private void WriteTongWenjun(Capacitancecompensation capacitancecompensation,float Newfactor)
        {            
            //电压 ABC
            ShowAdianya = capacitancecompensation.UserAAvoltage;
            ShowBdianya = capacitancecompensation.UserBAvoltage;
            ShowCdianya = capacitancecompensation.UserCAvoltage;

            //电流            
            var result = capacitancecompensation.GetAelectric(Newfactor);
            ShowAdianliu = result.Item1;
            ShowBdianliu = result.Item2;
            ShowCdianliu = result.Item3;

            //有功
            result = capacitancecompensation.GetActivePower();
            ShowAyggl = result.Item1;
            ShowByggl = result.Item2;
            ShowCyggl = result.Item3;

            //无功
            result = capacitancecompensation.GetReactivePower(Newfactor);
            ShowAwggl = result.Item1;
            ShowBwggl = result.Item2;
            ShowCwggl = result.Item3;

            int testTrdn = 10;
            int i = 0;
            while (testTrdn < ShowAwggl + ShowBwggl + ShowCwggl)
            {
                testTrdn += 10;
                i++;
            }
            ShowTrdn = i;


            //实在功率
            result = capacitancecompensation.GetApparentpower(Newfactor);
            ShowASzgl = result.Item1;
            ShowBSzgl = result.Item2;
            ShowCSzgl = result.Item3;

            //功率因数
            ShowAglys = Newfactor;
            ShowBglys = Newfactor;
            ShowCglys = Newfactor;

            //ABC 波形畸变
            result = capacitancecompensation.HarmonicVoltageRate(Newfactor);
            ShowAdianyajb = result.Item1;
            ShowBdianyajb = result.Item2;
            ShowCdianyajb = result.Item3;

            //ABC 波形畸变
            result = capacitancecompensation.HarmonicVoltageRate(Newfactor);
            ShowAdianliujb = result.Item1;
            ShowAdianliujb = result.Item2;
            ShowAdianliujb = result.Item3;


            //(火 -- 国家定死的)0.94
            result = capacitancecompensation.Carbon123(Newfactor);
            ShowTPF1 = result.Item1;
            ShowTPF2 = result.Item2;
            ShowTPF3 = result.Item3;

            //(风 -- 国家定死的)0.03
            result = capacitancecompensation.Carbon456(Newfactor);
            ShowTPF4 = result.Item1;
            ShowTPF5 = result.Item2;
            ShowTPF6 = result.Item3;


            //(光伏 -- 国家定死的)0.05
            result = capacitancecompensation.Carbon789(Newfactor);
            ShowTPF7 = result.Item1;
            ShowTPF8 = result.Item2;
            ShowTPF9 = result.Item3;

            //(水利 -- 国家定死的)0.03
            result = capacitancecompensation.Carbon101112(Newfactor);
            ShowTPF10 = result.Item1;
            ShowTPF11 = result.Item2;
            ShowTPF12 = result.Item3;


            ShowDRWD = capacitancecompensation.DRWD;

        }


        private void DoBtnCommandStart2(object param)
        {
            Capacitancecompensation capacitancecompensation = new Capacitancecompensation();

            capacitancecompensation.UserAAelectric = Adianliu;
            capacitancecompensation.UserBAelectric = Bdianliu;
            capacitancecompensation.UserCAelectric = Cdianliu;

            capacitancecompensation.UserAAvoltage = Adianya;
            capacitancecompensation.UserBAvoltage = Bdianya;
            capacitancecompensation.UserCAvoltage = Cdianya;

            capacitancecompensation.oldpowerfactor = Oldfactor;

            float Newfactor = Oldfactor;

            
                    
            Task.Run(async () =>
            {
                cancellationTokenSource = new ();
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    //电流 ABC
                    //这里是2个表，1个表是没开设备的，1个表是开了设备的..

                    if (Newfactor < 0.99)
                    {
                        Newfactor += 0.01f;
                        //只要后面2位...
                        Newfactor = (float)Math.Round(Newfactor, 2); 
                    }



                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        capacitancecompensation.JIsuanInitializing();

                        Adianya = capacitancecompensation.UserAAvoltage;
                        Bdianya = capacitancecompensation.UserBAvoltage;
                        Cdianya = capacitancecompensation.UserCAvoltage;

                        Adianliu = capacitancecompensation.UserAAelectric;
                        Bdianliu = capacitancecompensation.UserBAelectric;
                        Cdianliu = capacitancecompensation.UserCAelectric;

                        //表1
                        WriteTongWenjun(capacitancecompensation,0.5f);

                        //表2
                        WriteTongWenjun(capacitancecompensation,Newfactor);

                    });


                    try
                    {                        
                        await Task.Delay(1000, cancellationTokenSource.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        //点击停止 -- 会发送一个异常...
                    }
                }
            });    
        }        
    }
}

