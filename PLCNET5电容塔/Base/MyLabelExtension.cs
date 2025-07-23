using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLCNET5电容塔.Base
{
    public static class StackPanelExtensions
    {
            // 添加一个附加属性，例如：IsBold
            public static float GetIsBold(DependencyObject obj)
            {
                return (float)obj.GetValue(IsBoldProperty);
            }

            public static void SetIsBold(DependencyObject obj, float value)
            {
                obj.SetValue(IsBoldProperty, value);
            }

            // 使用DependencyProperty作为底层存储
            public static readonly DependencyProperty IsBoldProperty =
                DependencyProperty.RegisterAttached("IsBold", typeof(float), typeof(StackPanelExtensions), new PropertyMetadata(false, OnIsBoldChanged));

            // 当附加属性值改变时的回调方法
            private static void OnIsBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                
            }
        }
}
