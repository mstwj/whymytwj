using PLCNET5电容塔升级.ViewModel;
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
using System.Windows.Shapes;

namespace PLCNET5电容塔升级
{
    /// <summary>
    /// Step2Window.xaml 的交互逻辑
    /// </summary>
    public partial class Step2Window : Window
    {
        public Step2WindowModel model = new Step2WindowModel();
        public Step2Window()
        {
            InitializeComponent();
            this.DataContext = model;
        }

        //点击了某个开关..
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox obj = sender as CheckBox;
            //如果是第一组就要联动了..
            string text = obj.Name;
            string[] fruits = text.Split('_');
            if (fruits[1] == "1")
            {
                //这里肯定是第一组...
                if (fruits[2] == "1")
                {
                    //这里就必须第一组联动了..
                    if (obj.IsChecked == true)
                    {
                        CheckBoxLevel_2_1.IsChecked = true;
                        CheckBoxLevel_3_1.IsChecked = true;
                        CheckBoxLevel_4_1.IsChecked = true;
                        CheckBoxLevel_5_1.IsChecked = true;
                        CheckBoxLevel_6_1.IsChecked = true;
                    }
                    if (obj.IsChecked == false)
                    {
                        CheckBoxLevel_2_1.IsChecked = false;
                        CheckBoxLevel_3_1.IsChecked = false;
                        CheckBoxLevel_4_1.IsChecked = false;
                        CheckBoxLevel_5_1.IsChecked = false;
                        CheckBoxLevel_6_1.IsChecked = false;
                    }
                }

                //这里肯定是第2组...
                if (fruits[2] == "2")
                {
                    //这里就必须第一组联动了..
                    if (obj.IsChecked == true)
                    {
                        CheckBoxLevel_2_2.IsChecked = true;
                        CheckBoxLevel_3_2.IsChecked = true;
                        CheckBoxLevel_4_2.IsChecked = true;
                        CheckBoxLevel_5_2.IsChecked = true;
                        CheckBoxLevel_6_2.IsChecked = true;
                    }
                    if (obj.IsChecked == false)
                    {
                        CheckBoxLevel_2_2.IsChecked = false;
                        CheckBoxLevel_3_2.IsChecked = false;
                        CheckBoxLevel_4_2.IsChecked = false;
                        CheckBoxLevel_5_2.IsChecked = false;
                        CheckBoxLevel_6_2.IsChecked = false;
                    }
                }

                //这里肯定是第3组...
                if (fruits[2] == "3")
                {
                    //这里就必须第3组联动了..
                    if (obj.IsChecked == true)
                    {
                        CheckBoxLevel_2_3.IsChecked = true;
                        CheckBoxLevel_3_3.IsChecked = true;
                        CheckBoxLevel_4_3.IsChecked = true;
                        CheckBoxLevel_5_3.IsChecked = true;
                        CheckBoxLevel_6_3.IsChecked = true;
                    }
                    if (obj.IsChecked == false)
                    {
                        CheckBoxLevel_2_3.IsChecked = false;
                        CheckBoxLevel_3_3.IsChecked = false;
                        CheckBoxLevel_4_3.IsChecked = false;
                        CheckBoxLevel_5_3.IsChecked = false;
                        CheckBoxLevel_6_3.IsChecked = false;
                    }
                }

                //这里肯定是第2组...
                if (fruits[2] == "4")
                {
                    //这里就必须第4组联动了..
                    if (obj.IsChecked == true)
                    {
                        CheckBoxLevel_2_4.IsChecked = true;
                        CheckBoxLevel_3_4.IsChecked = true;
                        CheckBoxLevel_4_4.IsChecked = true;
                        CheckBoxLevel_5_4.IsChecked = true;
                        CheckBoxLevel_6_4.IsChecked = true;
                    }
                    if (obj.IsChecked == false)
                    {
                        CheckBoxLevel_2_4.IsChecked = false;
                        CheckBoxLevel_3_4.IsChecked = false;
                        CheckBoxLevel_4_4.IsChecked = false;
                        CheckBoxLevel_5_4.IsChecked = false;
                        CheckBoxLevel_6_4.IsChecked = false;

                    }
                }

                //这里肯定是第2组...
                if (fruits[2] == "5")
                {
                    //这里就必须第5组联动了..
                    if (obj.IsChecked == true)
                    {

                        CheckBoxLevel_2_5.IsChecked = true;
                        CheckBoxLevel_3_5.IsChecked = true;
                        CheckBoxLevel_4_5.IsChecked = true;
                        CheckBoxLevel_5_5.IsChecked = true;
                        CheckBoxLevel_6_5.IsChecked = true;
                    }
                    if (obj.IsChecked == false)
                    {

                        CheckBoxLevel_2_5.IsChecked = false;
                        CheckBoxLevel_3_5.IsChecked = false;
                        CheckBoxLevel_4_5.IsChecked = false;
                        CheckBoxLevel_5_5.IsChecked = false;
                        CheckBoxLevel_6_5.IsChecked = false;
                    }
                }

                //这里肯定是第2组...
                if (fruits[2] == "6")
                {
                    //这里就必须第6组联动了..
                    if (obj.IsChecked == true)
                    {
                        CheckBoxLevel_2_6.IsChecked = true;
                        CheckBoxLevel_3_6.IsChecked = true;
                        CheckBoxLevel_4_6.IsChecked = true;
                        CheckBoxLevel_5_6.IsChecked = true;
                        CheckBoxLevel_6_6.IsChecked = true;
                    }
                    if (obj.IsChecked == false)
                    {
                        CheckBoxLevel_2_6.IsChecked = false;
                        CheckBoxLevel_3_6.IsChecked = false;
                        CheckBoxLevel_4_6.IsChecked = false;
                        CheckBoxLevel_5_6.IsChecked = false;
                        CheckBoxLevel_6_6.IsChecked = false;
                    }
                }

            }
        }

        private void myClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.OnClose();
        }
    }
    }
