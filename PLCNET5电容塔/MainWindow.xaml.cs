using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using PLCNET5电容塔.Base;
using PLCNET5电容塔.ViewModel;
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
        //ProcessWindow processWindow;
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
            //NEW 200个点来表示..
            mainViewModel.SetAllDian(alldian);

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
                myuserc11.m_userMode.Q1_1 = alldian[0];
                myuserc11.m_userMode.Q1_2 = alldian[1];
                myuserc11.m_userMode.Q1_3 = alldian[2];
                myuserc11.m_userMode.Q1_4 = alldian[3];
                myuserc11.m_userMode.Q1_5 = alldian[4];
                myuserc11.m_userMode.Q1_6 = alldian[5];

                myuserc12.m_userMode.Q1_1 = alldian[6];
                myuserc12.m_userMode.Q1_2 = alldian[7];
                myuserc12.m_userMode.Q1_3 = alldian[8];
                myuserc12.m_userMode.Q1_4 = alldian[9];
                myuserc12.m_userMode.Q1_5 = alldian[10];
                myuserc12.m_userMode.Q1_6 = alldian[11];

                myuserc13.m_userMode.Q1_1 = alldian[12];
                myuserc13.m_userMode.Q1_2 = alldian[13];
                myuserc13.m_userMode.Q1_3 = alldian[14];
                myuserc13.m_userMode.Q1_4 = alldian[15];
                myuserc13.m_userMode.Q1_5 = alldian[16];
                myuserc13.m_userMode.Q1_6 = alldian[17];

                myuserc14.m_userMode.Q1_1 = alldian[18];
                myuserc14.m_userMode.Q1_2 = alldian[19];
                myuserc14.m_userMode.Q1_3 = alldian[20];
                myuserc14.m_userMode.Q1_4 = alldian[21];
                myuserc14.m_userMode.Q1_5 = alldian[22];
                myuserc14.m_userMode.Q1_6 = alldian[23];

                myuserc15.m_userMode.Q1_1 = alldian[24];
                myuserc15.m_userMode.Q1_2 = alldian[25];
                myuserc15.m_userMode.Q1_3 = alldian[26];
                myuserc15.m_userMode.Q1_4 = alldian[27];
                myuserc15.m_userMode.Q1_5 = alldian[28];
                myuserc15.m_userMode.Q1_6 = alldian[29];
                _ = alldian[30]; //空
                _ = alldian[31]; //空

                myuserc16.m_userMode.Q1_1 = alldian[32];
                myuserc16.m_userMode.Q1_2 = alldian[33];
                myuserc16.m_userMode.Q1_3 = alldian[34];
                myuserc16.m_userMode.Q1_4 = alldian[35];
                myuserc16.m_userMode.Q1_5 = alldian[36];
                myuserc16.m_userMode.Q1_6 = alldian[37];

                _ = alldian[38]; //空
                _ = alldian[39]; //空

                myuserc11.m_userMode.Q2_1 = alldian[40];
                myuserc11.m_userMode.Q2_2 = alldian[41];
                myuserc11.m_userMode.Q2_3 = alldian[42];
                myuserc11.m_userMode.Q2_4 = alldian[43];
                myuserc11.m_userMode.Q2_5 = alldian[44];
                myuserc11.m_userMode.Q2_6 = alldian[45];

                myuserc12.m_userMode.Q2_1 = alldian[46];
                myuserc12.m_userMode.Q2_2 = alldian[47];
                myuserc12.m_userMode.Q2_3 = alldian[48];
                myuserc12.m_userMode.Q2_4 = alldian[49];
                myuserc12.m_userMode.Q2_5 = alldian[50];
                myuserc12.m_userMode.Q2_6 = alldian[51];

                myuserc13.m_userMode.Q2_1 = alldian[52];
                myuserc13.m_userMode.Q2_2 = alldian[53];
                myuserc13.m_userMode.Q2_3 = alldian[54];
                myuserc13.m_userMode.Q2_4 = alldian[55];
                myuserc13.m_userMode.Q2_5 = alldian[56];
                myuserc13.m_userMode.Q2_6 = alldian[57];

                myuserc14.m_userMode.Q2_1 = alldian[58];
                myuserc14.m_userMode.Q2_2 = alldian[59];
                myuserc14.m_userMode.Q2_3 = alldian[60];
                myuserc14.m_userMode.Q2_4 = alldian[61];

                _ = alldian[62]; //空
                _ = alldian[63]; //空

                myuserc14.m_userMode.Q2_5 = alldian[64];
                myuserc14.m_userMode.Q2_6 = alldian[65];



                myuserc15.m_userMode.Q2_1 = alldian[66];
                myuserc15.m_userMode.Q2_2 = alldian[67];
                myuserc15.m_userMode.Q2_3 = alldian[68];
                myuserc15.m_userMode.Q2_4 = alldian[69];
                myuserc15.m_userMode.Q2_5 = alldian[70];
                myuserc15.m_userMode.Q2_6 = alldian[71];

                myuserc16.m_userMode.Q2_1 = alldian[72];
                myuserc16.m_userMode.Q2_2 = alldian[73];
                myuserc16.m_userMode.Q2_3 = alldian[74];
                myuserc16.m_userMode.Q2_4 = alldian[75];
                myuserc16.m_userMode.Q2_5 = alldian[76];
                myuserc16.m_userMode.Q2_6 = alldian[77];

                _ = alldian[78]; //空
                _ = alldian[79]; //空

                myuserc11.m_userMode.Q3_1 = alldian[80];
                myuserc11.m_userMode.Q3_2 = alldian[81];
                myuserc11.m_userMode.Q3_3 = alldian[82];
                myuserc11.m_userMode.Q3_4 = alldian[83];                
                myuserc11.m_userMode.Q3_5 = alldian[84];
                myuserc11.m_userMode.Q3_6 = alldian[85];

                _ = alldian[86]; //空
                _ = alldian[87]; //空


                myuserc12.m_userMode.Q3_1 = alldian[88];
                myuserc12.m_userMode.Q3_2 = alldian[89];
                myuserc12.m_userMode.Q3_3 = alldian[90];
                myuserc12.m_userMode.Q3_4 = alldian[91];
                myuserc12.m_userMode.Q3_5 = alldian[92];
                myuserc12.m_userMode.Q3_6 = alldian[93];

                myuserc13.m_userMode.Q3_1 = alldian[94];
                myuserc13.m_userMode.Q3_2 = alldian[95];
                myuserc13.m_userMode.Q3_3 = alldian[96];
                myuserc13.m_userMode.Q3_4 = alldian[97];
                myuserc13.m_userMode.Q3_5 = alldian[98];
                myuserc13.m_userMode.Q3_6 = alldian[99];



                myuserc14.m_userMode.Q3_1 = alldian[100];
                myuserc14.m_userMode.Q3_2 = alldian[101];
                myuserc14.m_userMode.Q3_3 = alldian[102];
                myuserc14.m_userMode.Q3_4 = alldian[103];
                myuserc14.m_userMode.Q3_5 = alldian[104];
                myuserc14.m_userMode.Q3_6 = alldian[105];
                
                myuserc15.m_userMode.Q3_1 = alldian[106];
                myuserc15.m_userMode.Q3_2 = alldian[107];
                myuserc15.m_userMode.Q3_3 = alldian[108];
                myuserc15.m_userMode.Q3_4 = alldian[109];
                myuserc15.m_userMode.Q3_5 = alldian[110];
                myuserc15.m_userMode.Q3_6 = alldian[111];

                _ = alldian[112]; //空

                myuserc16.m_userMode.Q3_1 = alldian[113];
                myuserc16.m_userMode.Q3_2 = alldian[114];
                myuserc16.m_userMode.Q3_3 = alldian[115];
                myuserc16.m_userMode.Q3_4 = alldian[116];
                myuserc16.m_userMode.Q3_5 = alldian[117];
                myuserc16.m_userMode.Q3_6 = alldian[118];


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

                if (alldian[124] == true) myButtonGC.myButtonModel.Data = true; //总B合
                if (alldian[125] == true) myButtonGC.myButtonModel.Data = false; //总B分

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

        private void User_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(TextBoxU3))
            {
                MessageBox.Show("请注意,输入有误");
                return;
            }
            

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int select = DIANYACOM.SelectedIndex;
            if (select == 0) mainViewModel.UserData1 = 7.5f;
            if (select == 1) mainViewModel.UserData1 = 13f;
            if (select == 2) mainViewModel.UserData1 = 15f;
            if (select == 3) mainViewModel.UserData1 = 22.5f;
            if (select == 4) mainViewModel.UserData1 = 26f;
            if (select == 5) mainViewModel.UserData1 = 30f;
            if (select == 6) mainViewModel.UserData1 = 37.5f;
            if (select == 7) mainViewModel.UserData1 = 39f;
            if (select == 8) mainViewModel.UserData1 = 45f;
            if (select == 9) mainViewModel.UserData1 = 52f;
            if (select == 10) mainViewModel.UserData1 = 65f;
            if (select == 11) mainViewModel.UserData1 = 78f;
            if (select == 12) mainViewModel.UserData1 = 22.5f;
            if (select == 13) mainViewModel.UserData1 = 45f;
            if (select == 14) mainViewModel.UserData1 = 67.5f;

        }
    }

}
