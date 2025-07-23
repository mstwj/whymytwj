using PLCNET5电容塔升级.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLCNET5电容塔升级
{
    /// <summary>
    /// Step3Window.xaml 的交互逻辑
    /// </summary>
    public partial class Step3Window : Window
    {



        public Step3WindowModel model = new Step3WindowModel();
        public Step3Window()
        {
            InitializeComponent();
            this.DataContext = model;
        }

        //这里很多都报错了..
        //GetWindowLong(handle, GWL_STYLE);
        //protected override void OnSourceInitialized(EventArgs e)
        //{
            //base.OnSourceInitialized(e);
            //IntPtr handle = new WindowInteropHelper(this).Handle;
            //int style = GetWindowLong(handle, GWL_STYLE);
            //SetWindowLong(handle, GWL_STYLE, style & ~WS_MINIMIZEBOX & ~WS_MAXIMIZEBOX);
            //MessageBox(IntPtr.Zero, "引用验证成功", "USER32.dll 测试", 0x40); // 0x40 表示信息图标
        //}


    }
}
