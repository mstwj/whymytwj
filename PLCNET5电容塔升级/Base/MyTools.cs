using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using PLCNET5电容塔;

namespace PLCNET5电容塔升级.Base
{
    public static class MyTools
    {
        //这里是给对话框STEP1缓存使用的..
        public static myButtonTemplate myButtonGAC { get; set; }
        public static myButtonTemplate myButtonGB { get; set; }
        public static myButtonTemplate myButton1G1 { get; set; }
        public static myButtonTemplate myButton1G2 { get; set; }
        public static myButtonTemplate myButton1G3 { get; set; }
        public static myButtonTemplate myButton1G4 { get; set; }
        public static myButtonTemplate myButton1G5 { get; set; }
        public static myButtonTemplate myButton1G6 { get; set; }
        public static myButtonTemplate myButtonK1 { get; set; }
        public static myButtonTemplate myButtonK2 { get; set; }
        public static myButtonTemplate myButtonG1 { get; set; }
        public static myButtonTemplate myButtonG2 { get; set; }
        public static myButtonTemplate myButtonG3 { get; set; }
        public static myButtonTemplate myButtonG4 { get; set; }
        public static myButtonTemplate myButtonG5 { get; set; }

        public static myButtonTemplate myButtonQ1_1 { get; set; }
        public static myButtonTemplate myButtonQ1_2 { get; set; }
        public static myButtonTemplate myButtonQ1_3 { get; set; }
        public static myButtonTemplate myButtonQ1_4 { get; set; }
        public static myButtonTemplate myButtonQ1_5 { get; set; }
        public static myButtonTemplate myButtonQ1_6 { get; set; }

        public static myButtonTemplate myButtonQ2_1 { get; set; }
        public static myButtonTemplate myButtonQ2_2 { get; set; }

        public static myButtonTemplate myButtonQ2_3 { get; set; }

        public static myButtonTemplate myButtonQ2_4 { get; set; }   
        public static myButtonTemplate myButtonQ2_5 { get; set; }
        public static myButtonTemplate myButtonQ2_6 { get; set; }

        public static myButtonTemplate myButtonQ3_1 { get; set; }
        public static myButtonTemplate myButtonQ3_2 { get; set; }
        public static myButtonTemplate myButtonQ3_3 { get; set; }
        public static myButtonTemplate myButtonQ3_4 { get; set; }        
        public static myButtonTemplate myButtonQ3_5 { get; set; }
        public static myButtonTemplate myButtonQ3_6 { get; set; }

        public static myButtonTemplate myButtonQ4_1 { get; set; }
        public static myButtonTemplate myButtonQ4_2 { get; set; }
        public static myButtonTemplate myButtonQ4_3 { get; set; }
        public static myButtonTemplate myButtonQ4_4 { get; set; }
        public static myButtonTemplate myButtonQ4_5 { get; set; }
        public static myButtonTemplate myButtonQ4_6 { get; set; }

        public static myButtonTemplate myButtonQ5_1 { get; set; }
        public static myButtonTemplate myButtonQ5_2 { get; set; }
        public static myButtonTemplate myButtonQ5_3 { get; set; }
        public static myButtonTemplate myButtonQ5_4 { get; set; }
        public static myButtonTemplate myButtonQ5_5 { get; set; }
        public static myButtonTemplate myButtonQ5_6 { get; set; }


        public static myButtonTemplate myButtonQ6_1 { get; set; }
        public static myButtonTemplate myButtonQ6_2 { get; set; }
        public static myButtonTemplate myButtonQ6_3 { get; set; }
        public static myButtonTemplate myButtonQ6_4 { get; set; }
        public static myButtonTemplate myButtonQ6_5 { get; set; }
        public static myButtonTemplate myButtonQ6_6 { get; set; }

    }

    public static class VisualTreeHelpers
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static System.Windows.Controls.Button ScanButtonFromStackPanel(object stackPanel, string buttonName)
        {
            foreach (var child in VisualTreeHelpers.FindVisualChildren<UIElement>((DependencyObject)stackPanel))
            {
                //得到开始测量按钮
                if (child.GetType().Name == "Button")
                {
                    System.Windows.Controls.Button button = child as System.Windows.Controls.Button;
                    if (button.Name == buttonName)
                    {
                        //child.IsEnabled = false;
                        return button;
                    }
                }
            }
            return null;

        }
    }

}
