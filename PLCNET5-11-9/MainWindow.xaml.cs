using CommunityToolkit.Mvvm.Messaging;
using PLCNET5_11_9.ViewModel;
using S7.Net.Types;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;

namespace PLCNET5_11_9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// WrapPanel -- 和 Srap不一样，他是从头开始的，S是中间开始的 。。
    /// UniformGrid 默认就是2ROW 2COL。。。
    /// 比如写成 Colums = 2 Rows = 3 -- 就变成 3行2列了..
    /// UniforGrid Colums= 3 -- 这样3个按钮的间隙都是一样的  。。
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();

        static int privot = 0;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;

            //WeakReferenceMessenger.Default.Register<string, string>(this, "ScrollEnd", ScrollEnd);

            WeakReferenceMessenger.Default.Register<string>(this, (r, user) =>
            {
                if (viewModel.alldian_boolinput[400] == true)
                {
                    myButtonGW1A.myButtonModel.Data = true;//工位1合/分
                    myButtonGW1C.myButtonModel.Data = true;//工位1合/分
                }
                if (viewModel.alldian_boolinput[406] == true)
                {
                    myButtonGW1A.myButtonModel.Data = false;//工位1合/分
                    myButtonGW1C.myButtonModel.Data = false;//工位1合/分
                }

                if (viewModel.alldian_boolinput[401] == true)
                {
                    myButtonGW1B.myButtonModel.Data = true;//工位1B合/分
                }

                if (viewModel.alldian_boolinput[407] == true)
                {
                    myButtonGW1B.myButtonModel.Data = false;//工位1B/分
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                if (viewModel.alldian_boolinput[402] == true)
                {
                    myButtonGW2A.myButtonModel.Data = true;//工位2合
                    myButtonGW2C.myButtonModel.Data = true;//工位2合
                }
                if (viewModel.alldian_boolinput[410] == true)
                {
                    myButtonGW2A.myButtonModel.Data = false;//工位2分
                    myButtonGW2C.myButtonModel.Data = false;//工位2分
                }

                if (viewModel.alldian_boolinput[403] == true)
                {
                    myButtonGW2B.myButtonModel.Data = true;//工位2B合
                }

                if (viewModel.alldian_boolinput[411] == true)
                {
                    myButtonGW2B.myButtonModel.Data = false;//工位2B分
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///
                if (viewModel.alldian_boolinput[404] == true)
                {
                    myButtonGW3A.myButtonModel.Data = true;//工位3合
                    myButtonGW3C.myButtonModel.Data = true;//工位3合
                }
                if (viewModel.alldian_boolinput[412] == true)
                {
                    myButtonGW3A.myButtonModel.Data = false;//工位3分
                    myButtonGW3C.myButtonModel.Data = false;//工位3分
                }

                if (viewModel.alldian_boolinput[405] == true)
                {
                    myButtonGW3B.myButtonModel.Data = true;//工位3B合
                }

                if (viewModel.alldian_boolinput[413] == true)
                {
                    myButtonGW3B.myButtonModel.Data = false;//工位3B分
                }

                //////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////
                ///


                if (viewModel.alldian_boolinput[204] == true) //2000三相合闸完成 -- 图G5
                    myButtonG7.myButtonModel.Data = true;

                if (viewModel.alldian_boolinput[205] == true) //2000 单相合闸 -- 图G5
                    myButtonG7.myButtonModel.Data = false;

                


                if (viewModel.alldian_boolinput[206] == true) //中频机组(输出) 合闸   图G8
                    myButtonG8.myButtonModel.Data = true;
                else
                    myButtonG8.myButtonModel.Data = false;

                



                if (viewModel.alldian_boolinput[207] == true) //中频机组（力磁） 分闸     图D7
                    myButtonD7.myButtonModel.Data = true;
                else
                    myButtonD7.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[210] == true) //7500三项合闸 -- 图G5
                    myButtonG5.myButtonModel.Data = true;

                if (viewModel.alldian_boolinput[211] == true) //7500单项合闸 -- 图G5
                    myButtonG5.myButtonModel.Data = false;

                



                if (viewModel.alldian_boolinput[212] == true) // 7.5MVA   工频机组(输出) 合闸 G6图
                    myButtonG6.myButtonModel.Data = true;
                else
                    myButtonG6.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[213] == true) // 7.5MVA   工频机组（力磁） 分闸 D4图
                    myButtonD4.myButtonModel.Data = true;
                else
                    myButtonD4.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[214] == true) //调压器输入   图3-2
                    myButton3_2.myButtonModel.Data = true;
                else
                    myButton3_2.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[215] == true) //调压器(输出)  合闸  7-1 2图
                    myButton7_1_2.myButtonModel.Data = true;
                else
                    myButton7_1_2.myButtonModel.Data = false;


                //////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////
                if (viewModel.alldian_boolinput[223] == true) //7500KVA中频机组电动机信号
                    myButtonG2.myButtonModel.Data = true;
                else
                    myButtonG2.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[203] == true) //2000KVA中频机组电动机信号
                    myButtonG3.myButtonModel.Data = true;
                else
                    myButtonG3.myButtonModel.Data = false;


                ////////////////////////////////////////////////////////////
                if (viewModel.alldian_boolinput[216] == true) //零位信号报警
                    viewModel.mainModel.S2000_1tlw = true;
                else
                    viewModel.mainModel.S2000_1tlw = false;


                ////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////
                if (viewModel.alldian_boolinput[370] == true) //2000KVA中频机组电动机故障报警
                    viewModel.mainModel.S2000_1z = true;           
                else
                    viewModel.mainModel.S2000_1z = false;


                if (viewModel.alldian_boolinput[380] == true) //7500KVA工频机组电动机故障报警"
                    viewModel.mainModel.S7500_1g = true;
                else
                    viewModel.mainModel.S7500_1g = false;


                if (viewModel.alldian_boolinput[390] == true) //支撑变故障报警"
                    viewModel.mainModel.ZCB = true;
                else
                    viewModel.mainModel.ZCB = false;


                if (viewModel.alldian_boolinput[365] == true) //7500KVA工频机组电动机故障报警"
                    viewModel.mainModel.SZBSR = true;
                else
                    viewModel.mainModel.SZBSR = false;



                if (viewModel.TYQError == true) //调压器故障报警"
                    viewModel.mainModel.S2000_1t = true;
                else
                    viewModel.mainModel.S2000_1t = false;




                 



                if (viewModel.alldian_boolinput[300] == true) //中变输入
                    myButton7_4.myButtonModel.Data = true;
                else
                    myButton7_4.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[301] == true) //调压器 7-1 图3 调压器合闸
                    myButton7_1.myButtonModel.Data = true;
                else
                    myButton7_1.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[303] == true) //中频机 图7-3
                    myButton7_3.myButtonModel.Data = true;
                else
                    myButton7_3.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[304] == true) //工频机 图7-2
                    myButton7_2.myButtonModel.Data = true;
                else
                    myButton7_2.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[311] == true) //电抗调节
                    myButton7_5.myButtonModel.Data = true;
                else
                    myButton7_5.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[360] == true) //电容补偿AC//
                {
                    myButton7_6_A.myButtonModel.Data = true;
                    myButton7_6_C.myButtonModel.Data = true;
                }
                else
                {
                    myButton7_6_A.myButtonModel.Data = false;
                    myButton7_6_C.myButtonModel.Data = false;
                }

                if (viewModel.alldian_boolinput[361] == true) //电容补偿B
                    myButton7_6_B.myButtonModel.Data = true;
                else
                    myButton7_6_B.myButtonModel.Data = false;

                if (viewModel.alldian_boolinput[362] == true)
                    myButton3_1.myButtonModel.Data = true;
                else
                    myButton3_1.myButtonModel.Data = false;


                //500就是没有使用的...
                if (viewModel.alldian_boolinput[501] == true) //调压器（单和三 2个单选按钮..）  7-1  图1
                    myButtonG7_1_2.myButtonModel.Data = false;
                else
                    myButtonG7_1_2.myButtonModel.Data = true;



                if (viewModel.alldian_boolinput[1] == true)
                {
                 
                    viewModel.DQCZFS = "PC";
                }
                else
                {
                    viewModel.DQCZFS = "控制台"; //当前操作方式..
                   
                }
                //调压器...

            });

            //WeakReferenceMessenger.Default.Register<string, string>(this, "SWaitchXZ", SWaitchXZ);

            //这里必须是1，因为0 是LANGUAGE 2是我自己定义的..
            viewModel.resources = Application.Current.Resources.MergedDictionaries[1];
            viewModel.InitializeAsync();
        }

        ~MainWindow()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        public void ChangeLanguage(string languageCode)
        {
            //这里和APP里面去对应就OK了... 我懂了,[1] 表示APP合并的XAML，使用数组就可以对应起来了...
            ResourceDictionary newLanguageDict = new ResourceDictionary();
            string resourcePath = $"Resources.{languageCode}.xaml";
            try
            {
                newLanguageDict.Source = new Uri(resourcePath, UriKind.Relative);
                Application.Current.Resources.MergedDictionaries[1] = newLanguageDict;
                //这里要更新一下...
                viewModel.resources = Application.Current.Resources.MergedDictionaries[1];
                Application.Current.Resources["Language"] = languageCode;
                

            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        //这里不要使用等待了..(主线程退出有问题..)

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

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool currentYuyan = true;
        private void Button_Click_Swaich(object sender, RoutedEventArgs e)
        {
            currentYuyan = !currentYuyan;
            if (currentYuyan)
            {
                ChangeLanguage("ch");
                BtnSwaichButton.Content = "中";

            }
            else
            {
                ChangeLanguage("en");
                BtnSwaichButton.Content = "英";
            }
        }


        private void SWaitchXZ(object recipient, string message)
        {
            
            if (message == "ShowAllPlcDian")
            {
            
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            viewModel.MainClose();
        }

        private void MyKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.SystemKey ==  Key.F10)
            {
                // 你的逻辑代码
                Window1 processWindow = new Window1();
                processWindow.ShowDialog();
            }
        }

        /*
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // 按钮被按下时的操作
            viewModel.DoCommand20T_5_UP();
        }

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // 按钮被释放时的操作
            viewModel.DoCommand20T_5_UPSTOP();
        }

        private void Button_PreviewMouseDownTYQDOWN(object sender, MouseButtonEventArgs e)
        {
            viewModel.DoCommand20T_6_UP();
        }

        private void Button_PreviewMouseUpTYQDOWNSTOP(object sender, MouseButtonEventArgs e)
        {
            viewModel.DoCommand20T_6_UPSTOP();
        }
        */
      
    }
}
