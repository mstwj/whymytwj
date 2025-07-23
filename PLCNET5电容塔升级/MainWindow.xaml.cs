using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using PLCNET5电容塔.ViewModel;
using PLCNET5电容塔升级;
using PLCNET5电容塔升级.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PLCNET5电容塔
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        //为什么上了VPN我好像就无法推送了呢?
        MainViewModel mainViewModel = null;
        private DispatcherTimer timer;

        bool[] alldian = new bool[200];

        private List<TextBlock> allLabel = new List<TextBlock>();        

        public MainWindow()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<string, string>(this, "EnableAll", EnableAll);
            WeakReferenceMessenger.Default.Register<string, string>(this, "ScrollEnd", ScrollEnd);
            WeakReferenceMessenger.Default.Register<string, string>(this, "SWaitchXZ", SWaitchXZ);

            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
            mainViewModel.SetAllDian(alldian);


            MyTools.myButtonGAC = myButtonGA;
            MyTools.myButtonGB = myButtonGB;
            MyTools.myButton1G1 = myButton1G1;
            MyTools.myButton1G2 = myButton1G2;
            MyTools.myButton1G3 = myButton1G3;
            MyTools.myButton1G4 = myButton1G4;
            MyTools.myButton1G5 = myButton1G5;
            MyTools.myButton1G6 = myButton1G6;
            MyTools.myButtonK1 = myButton1K1;
            MyTools.myButtonK2 = myButton1K2;
            MyTools.myButtonG1 = myButtonG1;
            MyTools.myButtonG2 = myButtonG2;
            MyTools.myButtonG3 = myButtonG3;
            MyTools.myButtonG4 = myButtonG4;
            MyTools.myButtonG5 = myButtonG5;

            MyTools.myButtonQ1_1 = myButtonQ1_1;
            MyTools.myButtonQ1_2 = myButtonQ1_2;
            MyTools.myButtonQ1_3 = myButtonQ1_3;
            MyTools.myButtonQ1_4 = myButtonQ1_4;
            MyTools.myButtonQ1_5 = myButtonQ1_5;
            MyTools.myButtonQ1_6 = myButtonQ1_6;

            MyTools.myButtonQ2_1 = myButtonQ2_1;
            MyTools.myButtonQ2_2 = myButtonQ2_2;
            MyTools.myButtonQ2_3 = myButtonQ2_3;
            MyTools.myButtonQ2_4 = myButtonQ2_4;
            MyTools.myButtonQ2_5 = myButtonQ2_5;
            MyTools.myButtonQ2_6 = myButtonQ2_6;

            MyTools.myButtonQ3_1 = myButtonQ3_1;
            MyTools.myButtonQ3_2 = myButtonQ3_2;
            MyTools.myButtonQ3_3 = myButtonQ3_3;
            MyTools.myButtonQ3_4 = myButtonQ3_4;
            MyTools.myButtonQ3_5 = myButtonQ3_5;
            MyTools.myButtonQ3_6 = myButtonQ3_6;

            MyTools.myButtonQ4_1 = myButtonQ4_1;
            MyTools.myButtonQ4_2 = myButtonQ4_2;
            MyTools.myButtonQ4_3 = myButtonQ4_3;
            MyTools.myButtonQ4_4 = myButtonQ4_4;
            MyTools.myButtonQ4_5 = myButtonQ4_5;
            MyTools.myButtonQ4_6 = myButtonQ4_6;


            MyTools.myButtonQ5_1 = myButtonQ5_1;
            MyTools.myButtonQ5_2 = myButtonQ5_2;
            MyTools.myButtonQ5_3 = myButtonQ5_3;
            MyTools.myButtonQ5_4 = myButtonQ5_4;
            MyTools.myButtonQ5_5 = myButtonQ5_5;
            MyTools.myButtonQ5_6 = myButtonQ5_6;


            MyTools.myButtonQ6_1 = myButtonQ6_1;
            MyTools.myButtonQ6_2 = myButtonQ6_2;
            MyTools.myButtonQ6_3 = myButtonQ6_3;
            MyTools.myButtonQ6_4 = myButtonQ6_4;
            MyTools.myButtonQ6_5 = myButtonQ6_5;
            MyTools.myButtonQ6_6 = myButtonQ6_6;

            myuserc11.SetTextData("1");
            myuserc12.SetTextData("2");
            myuserc13.SetTextData("3");
            myuserc14.SetTextData("4");
            myuserc15.SetTextData("5");
            myuserc16.SetTextData("6");                    

        }

        ~MainWindow()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        //这里调用就是跳转到最前面...
        private void ScrollEnd(object recipient, string message)
        {            
            Application.Current.Dispatcher.Invoke(() =>
            {
                //获取ListView的最后一项
                if (myListBox.Items.Count > 3)
                {
                    var lastItem = myListBox.Items[myListBox.Items.Count - 1];
                    // 滚动到最后一项
                    myListBox.ScrollIntoView(lastItem);
                }
            });
        }

        private void SWaitchXZ(object recipient, string message)
        {
            if (message == "1")
            {
                mytabcontrol.SelectedIndex = 0;                
            }
            if (message == "2")
            {
                mytabcontrol.SelectedIndex = 1;
                
            }
            if (message == "3")
            {
                mytabcontrol.SelectedIndex = 2;                
            }

            if (message == "ShowAllPlcDian")
            {
                //下面圆形气信号...



                myuserc11.m_userMode.Q1_1 = alldian[0]; //AQ1 - 1 合信号
                myuserc11.m_userMode.Q1_2 = alldian[1]; //AQ1 - 2 合信号
                myuserc11.m_userMode.Q1_3 = alldian[2]; //AQ1 - 3 合信号
                myuserc11.m_userMode.Q1_4 = alldian[3]; //AQ1 - 4 合信号
                myuserc11.m_userMode.Q1_5 = alldian[4]; //AQ1 - 5 合信号
                myuserc11.m_userMode.Q1_6 = alldian[5]; //AQ1 - 6 合信号

                myuserc12.m_userMode.Q1_1 = alldian[6]; //AQ2 - 1 合信号
                myuserc12.m_userMode.Q1_2 = alldian[7];//AQ2 - 2 合信号
                myuserc12.m_userMode.Q1_3 = alldian[8];//AQ2 - 3 合信号
                myuserc12.m_userMode.Q1_4 = alldian[9];//AQ2 - 4 合信号
                myuserc12.m_userMode.Q1_5 = alldian[10];//AQ2 - 5 合信号
                myuserc12.m_userMode.Q1_6 = alldian[11];//AQ2 - 6 合信号

                myuserc13.m_userMode.Q1_1 = alldian[12];//AQ3 - 1 合信号
                myuserc13.m_userMode.Q1_2 = alldian[13];//AQ3 - 2 合信号
                myuserc13.m_userMode.Q1_3 = alldian[14];//AQ3 - 3 合信号
                myuserc13.m_userMode.Q1_4 = alldian[15];//AQ3 - 4 合信号
                myuserc13.m_userMode.Q1_5 = alldian[16];//AQ3 - 5 合信号
                myuserc13.m_userMode.Q1_6 = alldian[17];//AQ3 - 6 合信号

                myuserc14.m_userMode.Q1_1 = alldian[18]; //AQ4 - 1 合信号
                myuserc14.m_userMode.Q1_2 = alldian[19];//AQ4 - 2 合信号
                myuserc14.m_userMode.Q1_3 = alldian[20];//AQ4 - 3 合信号
                myuserc14.m_userMode.Q1_4 = alldian[21];//AQ4 - 4 合信号
                myuserc14.m_userMode.Q1_5 = alldian[22];//AQ4 - 5 合信号
                myuserc14.m_userMode.Q1_6 = alldian[23];//AQ4 - 6 合信号

                myuserc15.m_userMode.Q1_1 = alldian[24];//AQ5 - 1 合信号
                myuserc15.m_userMode.Q1_2 = alldian[25];//AQ5 - 2 合信号
                myuserc15.m_userMode.Q1_3 = alldian[26];//AQ5 - 3 合信号
                myuserc15.m_userMode.Q1_4 = alldian[27];//AQ5 - 4 合信号
                myuserc15.m_userMode.Q1_5 = alldian[28];//AQ5 - 5 合信号
                myuserc15.m_userMode.Q1_6 = alldian[29];//AQ5 - 6 合信号
                _ = alldian[30]; //空
                _ = alldian[31]; //空

                myuserc16.m_userMode.Q1_1 = alldian[32];//AQ6 - 1 合信号
                myuserc16.m_userMode.Q1_2 = alldian[33];//AQ6 - 2 合信号
                myuserc16.m_userMode.Q1_3 = alldian[34];//AQ6 - 3 合信号
                myuserc16.m_userMode.Q1_4 = alldian[35];//AQ6 - 4 合信号
                myuserc16.m_userMode.Q1_5 = alldian[36];//AQ6 - 5 合信号
                myuserc16.m_userMode.Q1_6 = alldian[37];//AQ6 - 6 合信号

                _ = alldian[38]; //空
                _ = alldian[39]; //空

                myuserc11.m_userMode.Q2_1 = alldian[40];//BQ1 - 1 合信号
                myuserc11.m_userMode.Q2_2 = alldian[41];//BQ1 - 2 合信号
                myuserc11.m_userMode.Q2_3 = alldian[42];//BQ1 - 3 合信号
                myuserc11.m_userMode.Q2_4 = alldian[43];//BQ1 - 4 合信号
                myuserc11.m_userMode.Q2_5 = alldian[44];//BQ1 - 5 合信号
                myuserc11.m_userMode.Q2_6 = alldian[45];//BQ1 - 6 合信号

                myuserc12.m_userMode.Q2_1 = alldian[46];//BQ2 - 1 合信号
                myuserc12.m_userMode.Q2_2 = alldian[47];//BQ2 - 2 合信号
                myuserc12.m_userMode.Q2_3 = alldian[48];//BQ2 - 3 合信号
                myuserc12.m_userMode.Q2_4 = alldian[49];//BQ2 - 4 合信号
                myuserc12.m_userMode.Q2_5 = alldian[50];//BQ2 - 5 合信号
                myuserc12.m_userMode.Q2_6 = alldian[51];//BQ2 - 6 合信号

                myuserc13.m_userMode.Q2_1 = alldian[52];//BQ3 - 1 合信号
                myuserc13.m_userMode.Q2_2 = alldian[53];//BQ3 - 2 合信号
                myuserc13.m_userMode.Q2_3 = alldian[54];//BQ3 - 3 合信号
                myuserc13.m_userMode.Q2_4 = alldian[55];//BQ3 - 4 合信号
                myuserc13.m_userMode.Q2_5 = alldian[56];//BQ3 - 5 合信号
                myuserc13.m_userMode.Q2_6 = alldian[57];//BQ3 - 6 合信号

                myuserc14.m_userMode.Q2_1 = alldian[58];//BQ4 - 1 合信号
                myuserc14.m_userMode.Q2_2 = alldian[59];//BQ4 - 2 合信号
                myuserc14.m_userMode.Q2_3 = alldian[60];//BQ4 - 3 合信号
                myuserc14.m_userMode.Q2_4 = alldian[61];//BQ4 - 4 合信号

                _ = alldian[62]; //空
                _ = alldian[63]; //空

                myuserc14.m_userMode.Q2_5 = alldian[64];//BQ4 - 5 合信号
                myuserc14.m_userMode.Q2_6 = alldian[65];//BQ4 - 6 合信号



                myuserc15.m_userMode.Q2_1 = alldian[66];//BQ5 - 1 合信号
                myuserc15.m_userMode.Q2_2 = alldian[67];//BQ5 - 2 合信号
                myuserc15.m_userMode.Q2_3 = alldian[68];//BQ5 - 3 合信号
                myuserc15.m_userMode.Q2_4 = alldian[69];//BQ5 - 4 合信号
                myuserc15.m_userMode.Q2_5 = alldian[70];//BQ5 - 5 合信号
                myuserc15.m_userMode.Q2_6 = alldian[71];//BQ5 - 6 合信号

                myuserc16.m_userMode.Q2_1 = alldian[72];//BQ6 - 1 合信号
                myuserc16.m_userMode.Q2_2 = alldian[73];//BQ6 - 2 合信号
                myuserc16.m_userMode.Q2_3 = alldian[74];//BQ6 - 3 合信号
                myuserc16.m_userMode.Q2_4 = alldian[75];//BQ6 - 4 合信号
                myuserc16.m_userMode.Q2_5 = alldian[76];//BQ6 - 5 合信号
                myuserc16.m_userMode.Q2_6 = alldian[77];//BQ6 - 6 合信号

                _ = alldian[78]; //空
                _ = alldian[79]; //空

                myuserc11.m_userMode.Q3_1 = alldian[80];//CQ1 - 1 合信号
                myuserc11.m_userMode.Q3_2 = alldian[81];//CQ1 - 2 合信号
                myuserc11.m_userMode.Q3_3 = alldian[82];//CQ1 - 3 合信号
                myuserc11.m_userMode.Q3_4 = alldian[83];//CQ1 - 4 合信号                
                myuserc11.m_userMode.Q3_5 = alldian[84];//CQ1 - 5 合信号
                myuserc11.m_userMode.Q3_6 = alldian[85];//CQ1 - 6 合信号

                _ = alldian[86]; //空
                _ = alldian[87]; //空
              // mainViewModel.mainModel.IsZKG = alldian[87];    //总开会
               // mainViewModel.mainModel.IsZKG = alldian[86];    //总开会



                myuserc12.m_userMode.Q3_1 = alldian[88];//CQ2 - 1 合信号
                myuserc12.m_userMode.Q3_2 = alldian[89];//CQ2 - 2 合信号
                myuserc12.m_userMode.Q3_3 = alldian[90];//CQ2 - 3 合信号
                myuserc12.m_userMode.Q3_4 = alldian[91];//CQ2 - 4 合信号
                myuserc12.m_userMode.Q3_5 = alldian[92];//CQ2 - 5 合信号
                myuserc12.m_userMode.Q3_6 = alldian[93];//CQ2 - 6 合信号

                myuserc13.m_userMode.Q3_1 = alldian[94];//CQ3 - 1 合信号
                myuserc13.m_userMode.Q3_2 = alldian[95];//CQ3 - 2 合信号
                myuserc13.m_userMode.Q3_3 = alldian[96];//CQ3 - 3 合信号
                myuserc13.m_userMode.Q3_4 = alldian[97];//CQ3 - 4 合信号
                myuserc13.m_userMode.Q3_5 = alldian[98];//CQ3 - 5 合信号
                myuserc13.m_userMode.Q3_6 = alldian[99];//CQ3 - 6 合信号



                myuserc14.m_userMode.Q3_1 = alldian[100];//CQ4 - 1 合信号
                myuserc14.m_userMode.Q3_2 = alldian[101];//CQ4 - 2 合信号
                myuserc14.m_userMode.Q3_3 = alldian[102];//CQ4 - 3 合信号
                myuserc14.m_userMode.Q3_4 = alldian[103];//CQ4 - 4 合信号
                myuserc14.m_userMode.Q3_5 = alldian[104];//CQ4 - 5 合信号
                myuserc14.m_userMode.Q3_6 = alldian[105];//CQ4 - 6 合信号

                myuserc15.m_userMode.Q3_1 = alldian[106];//CQ5 - 1 合信号
                myuserc15.m_userMode.Q3_2 = alldian[107];//CQ5 - 2 合信号
                myuserc15.m_userMode.Q3_3 = alldian[108];//CQ5 - 3 合信号
                myuserc15.m_userMode.Q3_4 = alldian[109];//CQ5 - 4 合信号
                myuserc15.m_userMode.Q3_5 = alldian[110];//CQ5 - 5 合信号
                myuserc15.m_userMode.Q3_6 = alldian[111];//CQ5 - 6 合信号

                _ = alldian[112]; //空

                myuserc16.m_userMode.Q3_1 = alldian[113];//CQ6 - 1 合信号
                myuserc16.m_userMode.Q3_2 = alldian[114];//CQ6 - 2 合信号
                myuserc16.m_userMode.Q3_3 = alldian[115];//CQ6 - 3 合信号
                myuserc16.m_userMode.Q3_4 = alldian[116];//CQ6 - 4 合信号
                myuserc16.m_userMode.Q3_5 = alldian[117];//CQ6 - 5 合信号
                myuserc16.m_userMode.Q3_6 = alldian[118];//CQ6 - 6 合信号


                if (myuserc11.m_userMode.Q1_1 && myuserc11.m_userMode.Q2_1 && myuserc11.m_userMode.Q3_1) myButtonQ1_1.myButtonModel.Data = true; else myButtonQ1_1.myButtonModel.Data = false;
                if (myuserc11.m_userMode.Q1_2 && myuserc11.m_userMode.Q2_2 && myuserc11.m_userMode.Q3_2) myButtonQ1_2.myButtonModel.Data = true; else myButtonQ1_2.myButtonModel.Data = false;
                if (myuserc11.m_userMode.Q1_3 && myuserc11.m_userMode.Q2_3 && myuserc11.m_userMode.Q3_3) myButtonQ1_3.myButtonModel.Data = true; else myButtonQ1_3.myButtonModel.Data = false;
                if (myuserc11.m_userMode.Q1_4 && myuserc11.m_userMode.Q2_4 && myuserc11.m_userMode.Q3_4) myButtonQ1_4.myButtonModel.Data = true; else myButtonQ1_4.myButtonModel.Data = false;
                if (myuserc11.m_userMode.Q1_5 && myuserc11.m_userMode.Q2_5 && myuserc11.m_userMode.Q3_5) myButtonQ1_5.myButtonModel.Data = true; else myButtonQ1_5.myButtonModel.Data = false;
                if (myuserc11.m_userMode.Q1_6 && myuserc11.m_userMode.Q2_6 && myuserc11.m_userMode.Q3_6) myButtonQ1_6.myButtonModel.Data = true; else myButtonQ1_6.myButtonModel.Data = false;


                if (myuserc12.m_userMode.Q1_1 && myuserc12.m_userMode.Q2_1 && myuserc12.m_userMode.Q3_1) myButtonQ2_1.myButtonModel.Data = true; else myButtonQ2_1.myButtonModel.Data = false;
                if (myuserc12.m_userMode.Q1_2 && myuserc12.m_userMode.Q2_2 && myuserc12.m_userMode.Q3_2) myButtonQ2_2.myButtonModel.Data = true; else myButtonQ2_2.myButtonModel.Data = false;
                if (myuserc12.m_userMode.Q1_3 && myuserc12.m_userMode.Q2_3 && myuserc12.m_userMode.Q3_3) myButtonQ2_3.myButtonModel.Data = true; else myButtonQ2_3.myButtonModel.Data = false;
                if (myuserc12.m_userMode.Q1_4 && myuserc12.m_userMode.Q2_4 && myuserc12.m_userMode.Q3_4) myButtonQ2_4.myButtonModel.Data = true; else myButtonQ2_4.myButtonModel.Data = false;
                if (myuserc12.m_userMode.Q1_5 && myuserc12.m_userMode.Q2_5 && myuserc12.m_userMode.Q3_5) myButtonQ2_5.myButtonModel.Data = true; else myButtonQ2_5.myButtonModel.Data = false;
                if (myuserc12.m_userMode.Q1_6 && myuserc12.m_userMode.Q2_6 && myuserc12.m_userMode.Q3_6) myButtonQ2_6.myButtonModel.Data = true; else myButtonQ2_6.myButtonModel.Data = false;

                if (myuserc13.m_userMode.Q1_1 && myuserc13.m_userMode.Q2_1 && myuserc13.m_userMode.Q3_1) myButtonQ3_1.myButtonModel.Data = true; else myButtonQ3_1.myButtonModel.Data = false;
                if (myuserc13.m_userMode.Q1_2 && myuserc13.m_userMode.Q2_2 && myuserc13.m_userMode.Q3_2) myButtonQ3_2.myButtonModel.Data = true; else myButtonQ3_2.myButtonModel.Data = false;
                if (myuserc13.m_userMode.Q1_3 && myuserc13.m_userMode.Q2_3 && myuserc13.m_userMode.Q3_3) myButtonQ3_3.myButtonModel.Data = true; else myButtonQ3_3.myButtonModel.Data = false;
                if (myuserc13.m_userMode.Q1_4 && myuserc13.m_userMode.Q2_4 && myuserc13.m_userMode.Q3_4) myButtonQ3_4.myButtonModel.Data = true; else myButtonQ3_4.myButtonModel.Data = false;
                if (myuserc13.m_userMode.Q1_5 && myuserc13.m_userMode.Q2_5 && myuserc13.m_userMode.Q3_5) myButtonQ3_5.myButtonModel.Data = true; else myButtonQ3_5.myButtonModel.Data = false;
                if (myuserc13.m_userMode.Q1_6 && myuserc13.m_userMode.Q2_6 && myuserc13.m_userMode.Q3_6) myButtonQ3_6.myButtonModel.Data = true; else myButtonQ3_6.myButtonModel.Data = false;

                if (myuserc14.m_userMode.Q1_1 && myuserc14.m_userMode.Q2_1 && myuserc14.m_userMode.Q3_1) myButtonQ4_1.myButtonModel.Data = true; else myButtonQ4_1.myButtonModel.Data = false;
                if (myuserc14.m_userMode.Q1_2 && myuserc14.m_userMode.Q2_2 && myuserc14.m_userMode.Q3_2) myButtonQ4_2.myButtonModel.Data = true; else myButtonQ4_2.myButtonModel.Data = false;
                if (myuserc14.m_userMode.Q1_3 && myuserc14.m_userMode.Q2_3 && myuserc14.m_userMode.Q3_3) myButtonQ4_3.myButtonModel.Data = true; else myButtonQ4_3.myButtonModel.Data = false;
                if (myuserc14.m_userMode.Q1_4 && myuserc14.m_userMode.Q2_4 && myuserc14.m_userMode.Q3_4) myButtonQ4_4.myButtonModel.Data = true; else myButtonQ4_4.myButtonModel.Data = false;
                if (myuserc14.m_userMode.Q1_5 && myuserc14.m_userMode.Q2_5 && myuserc14.m_userMode.Q3_5) myButtonQ4_5.myButtonModel.Data = true; else myButtonQ4_5.myButtonModel.Data = false;
                if (myuserc14.m_userMode.Q1_6 && myuserc14.m_userMode.Q2_6 && myuserc14.m_userMode.Q3_6) myButtonQ4_6.myButtonModel.Data = true; else myButtonQ4_6.myButtonModel.Data = false;

                if (myuserc15.m_userMode.Q1_1 && myuserc15.m_userMode.Q2_1 && myuserc15.m_userMode.Q3_1) myButtonQ5_1.myButtonModel.Data = true; else myButtonQ5_1.myButtonModel.Data = false;
                if (myuserc15.m_userMode.Q1_2 && myuserc15.m_userMode.Q2_2 && myuserc15.m_userMode.Q3_2) myButtonQ5_2.myButtonModel.Data = true; else myButtonQ5_2.myButtonModel.Data = false;
                if (myuserc15.m_userMode.Q1_3 && myuserc15.m_userMode.Q2_3 && myuserc15.m_userMode.Q3_3) myButtonQ5_3.myButtonModel.Data = true; else myButtonQ5_3.myButtonModel.Data = false;
                if (myuserc15.m_userMode.Q1_4 && myuserc15.m_userMode.Q2_4 && myuserc15.m_userMode.Q3_4) myButtonQ5_4.myButtonModel.Data = true; else myButtonQ5_4.myButtonModel.Data = false;
                if (myuserc15.m_userMode.Q1_5 && myuserc15.m_userMode.Q2_5 && myuserc15.m_userMode.Q3_5) myButtonQ5_5.myButtonModel.Data = true; else myButtonQ5_5.myButtonModel.Data = false;
                if (myuserc15.m_userMode.Q1_6 && myuserc15.m_userMode.Q2_6 && myuserc15.m_userMode.Q3_6) myButtonQ5_6.myButtonModel.Data = true; else myButtonQ5_6.myButtonModel.Data = false;

                if (myuserc16.m_userMode.Q1_1 && myuserc16.m_userMode.Q2_1 && myuserc16.m_userMode.Q3_1) myButtonQ6_1.myButtonModel.Data = true; else myButtonQ6_1.myButtonModel.Data = false;
                if (myuserc16.m_userMode.Q1_2 && myuserc16.m_userMode.Q2_2 && myuserc16.m_userMode.Q3_2) myButtonQ6_2.myButtonModel.Data = true; else myButtonQ6_2.myButtonModel.Data = false;
                if (myuserc16.m_userMode.Q1_3 && myuserc16.m_userMode.Q2_3 && myuserc16.m_userMode.Q3_3) myButtonQ6_3.myButtonModel.Data = true; else myButtonQ6_3.myButtonModel.Data = false;
                if (myuserc16.m_userMode.Q1_4 && myuserc16.m_userMode.Q2_4 && myuserc16.m_userMode.Q3_4) myButtonQ6_4.myButtonModel.Data = true; else myButtonQ6_4.myButtonModel.Data = false;
                if (myuserc16.m_userMode.Q1_5 && myuserc16.m_userMode.Q2_5 && myuserc16.m_userMode.Q3_5) myButtonQ6_5.myButtonModel.Data = true; else myButtonQ6_5.myButtonModel.Data = false;
                if (myuserc16.m_userMode.Q1_6 && myuserc16.m_userMode.Q2_6 && myuserc16.m_userMode.Q3_6) myButtonQ6_6.myButtonModel.Data = true; else myButtonQ6_6.myButtonModel.Data = false;

                //从120 开始 -- 就是主信号... -- 39PLC信号.
                if (alldian[120] == true) myButtonGA.myButtonModel.Data = true;   //总A合
                if (alldian[121] == true) myButtonGA.myButtonModel.Data = false; //总A分

                if (alldian[122] == true) myButtonGB.myButtonModel.Data = true; //总B合
                if (alldian[123] == true) myButtonGB.myButtonModel.Data = false; //总B分

                 myButtonGC.myButtonModel.Data = myButtonGA.myButtonModel.Data; //总B合
                //if (alldian[125] == true) myButtonGC.myButtonModel.Data = false; //总B分

                _ = alldian[126]; //空
                _ = alldian[127];//空

                if (alldian[128] == true) myButtonG1.myButtonModel.Data = true; //总B合 Ig1 = true;   //G1合
                if (alldian[129] == true) myButtonG1.myButtonModel.Data = false; //总B合 = false; //G1分

                if (alldian[130] == true) myButtonG2.myButtonModel.Data = true; //总B合 = true; //G2合
                if (alldian[131] == true) myButtonG2.myButtonModel.Data = false; //总B合 = false; //G2分

                if (alldian[132] == true) myButtonG3.myButtonModel.Data = true; //总B合 = true; //G3合
                if (alldian[133] == true) myButtonG3.myButtonModel.Data = false; //总B合 = false; //G3分

                _ = alldian[134]; //空
                _ = alldian[135];//空


                if (alldian[136] == true) myButtonG4.myButtonModel.Data = true; //总B合4 = true; //G4合
                if (alldian[137] == true) myButtonG4.myButtonModel.Data = false; //总B合 = false; //G4分

                if (alldian[138] == true) myButtonG5.myButtonModel.Data = true; //G5合
                if (alldian[139] == true) myButtonG5.myButtonModel.Data = false; //G5分

                if (alldian[140] == true) myButton1G1.myButtonModel.Data = true; //A1G1合
                if (alldian[141] == true) myButton1G1.myButtonModel.Data = false; //A1G1分

                if (alldian[142] == true) myButton1G2.myButtonModel.Data = true; //A1G2合
                if (alldian[143] == true) myButton1G2.myButtonModel.Data = false; //A1G2分

                if (alldian[142] == true && alldian[143] == true)
                {
                    //mainViewModel.AddRecord("严重错误,A1G2又有和闸信号，又有分闸信号，无法判断当前状态!", true);
                }

                if (alldian[144] == true) myButton1G3.myButtonModel.Data = true; //A1G3合
                if (alldian[145] == true) myButton1G3.myButtonModel.Data = false; //A1G3分

                if (alldian[146] == true) myButton1G4.myButtonModel.Data = true; //A1G4合
                if (alldian[147] == true) myButton1G4.myButtonModel.Data = false; //A1G4分

                if (alldian[148] == true) myButton1G5.myButtonModel.Data = true; //A1G5合
                if (alldian[149] == true) myButton1G5.myButtonModel.Data = false; //A1G5分

                if (alldian[150] == true) myButton1G6.myButtonModel.Data = true; //A1G6合
                if (alldian[151] == true) myButton1G6.myButtonModel.Data = false; //A1G6分


                if (alldian[152] == true) myButton2G1.myButtonModel.Data = true; //B1G1合
                if (alldian[153] == true) myButton2G1.myButtonModel.Data = false; //B1G1分

                if (alldian[154] == true) myButton2G2.myButtonModel.Data = true; //B1G2合
                if (alldian[155] == true) myButton2G2.myButtonModel.Data = false; //B1G2分

                if (alldian[156] == true) myButton2G3.myButtonModel.Data = true; //B1G3合
                if (alldian[157] == true) myButton2G3.myButtonModel.Data = false; //B1G3分

                if (alldian[158] == true) myButton2G4.myButtonModel.Data = true; //B1G4合
                if (alldian[159] == true) myButton2G4.myButtonModel.Data = false; //B1G4分

                if (alldian[160] == true) myButton2G5.myButtonModel.Data = true; //B1G5合
                if (alldian[161] == true) myButton2G5.myButtonModel.Data = false; //B1G5分

                if (alldian[162] == true) myButton2G6.myButtonModel.Data = true; //B1G6合
                if (alldian[163] == true) myButton2G6.myButtonModel.Data = false; //B1G6分


                if (alldian[164] == true) myButton3G1.myButtonModel.Data = true; //C1G1合
                if (alldian[165] == true) myButton3G1.myButtonModel.Data = false; //C1G1分

                if (alldian[166] == true) myButton3G2.myButtonModel.Data = true; //C1G2合
                if (alldian[167] == true) myButton3G2.myButtonModel.Data = false; //C1G2分

                if (alldian[168] == true) myButton3G3.myButtonModel.Data = true; //C1G3合
                if (alldian[169] == true) myButton3G3.myButtonModel.Data = false; //C1G3分

                if (alldian[170] == true) myButton3G4.myButtonModel.Data = true; //C1G4合
                if (alldian[171] == true) myButton3G4.myButtonModel.Data = false; //C1G4分

                if (alldian[172] == true) myButton3G5.myButtonModel.Data = true; //C1G5合
                if (alldian[173] == true) myButton3G5.myButtonModel.Data = false; //C1G5分

                if (alldian[174] == true) myButton3G6.myButtonModel.Data = true; //C1G6合
                if (alldian[175] == true) myButton3G6.myButtonModel.Data = false; //C1G6分



                if (alldian[176] == true) myButton1K1.myButtonModel.Data = true; //K1合
                if (alldian[177] == true) myButton1K1.myButtonModel.Data = false; //K1分

                if (alldian[178] == true) myButton1K2.myButtonModel.Data = true; //K2合
                if (alldian[179] == true) myButton1K2.myButtonModel.Data = false; //K2分

                if (alldian[180] == true) myButton2K1.myButtonModel.Data = true; //K3合
                if (alldian[181] == true) myButton2K1.myButtonModel.Data = false; //K3分

                _ = alldian[182]; //空
                _ = alldian[183]; //空            

                if (alldian[184] == true) myButton2K2.myButtonModel.Data = true; //K4合
                if (alldian[185] == true) myButton2K2.myButtonModel.Data = false; //K4分

                if (alldian[186] == true) myButton3K1.myButtonModel.Data = true; //K5合
                if (alldian[187] == true) myButton3K1.myButtonModel.Data = false; //K5分

                if (alldian[188] == true) myButton3K2.myButtonModel.Data = true; //K6合
                if (alldian[189] == true) myButton3K2.myButtonModel.Data = false; //K6分

                 mainViewModel.mainModel.IsZKG = alldian[191];    //总开会




            }

        }

        private void EnableAll(object recipient, string message)
        {


            if (message == "Dn")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //这里报错了--异常.，说什么另一个线程又这个对象..（搞不懂了.）
                    progressBar.Visibility = Visibility.Visible;
                });                
            }
            if (message == "En")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //这里报错了--异常.，说什么另一个线程又这个对象..（搞不懂了.）
                    progressBar.Visibility = Visibility.Hidden;
                });                
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainViewModel.MainClose();
        }

        //关闭..
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MyKeyUp(object sender, KeyEventArgs e)
        {            
            if (e.SystemKey == Key.F10)
            {
                // 你的逻辑代码
                SetOneWindow waitWindow = new SetOneWindow();
                waitWindow.ShowDialog();
            }
        }
    }

}
