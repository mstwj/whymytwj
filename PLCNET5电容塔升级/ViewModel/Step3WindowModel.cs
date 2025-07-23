using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PLCNET5电容塔升级.ViewModel
{
    public class Step3WindowModel : ObservableObject
    {

        // 显式指定路径与函数签名
        [DllImport(@"C:\Windows\System32\user32.dll",
                  CharSet = CharSet.Auto,
                  EntryPoint = "MessageBox")]
        public static extern int MessageBoxA(IntPtr hWnd, string text, string caption, uint type);


        //默认工位
        public int BtnComBoxListGWIndex { get; set; } = 0;
        public ObservableCollection<string> BtnComBoxListGW { get; set; } = new ObservableCollection<string>();
        public Plc plc39 { get; set; }

        public ICommand BtnCommandStartHZ { get; set; }

        public Step3WindowModel()
        {
            BtnComBoxListGW.Add("第1工位");
            BtnComBoxListGW.Add("第2工位");
            BtnCommandStartHZ = new RelayCommand<object>(DoBtnCommandStartHZ);
        }

        private Task m_task = null;

        //显示执行流程..
        private string exerecord = string.Empty;
        public string Exerecord { get { return exerecord; } set { SetProperty(ref exerecord, value); } }

        //确定...
        private async void DoBtnCommandStartHZ(object param)
        {
            //MessageBoxA(IntPtr.Zero, "引用验证成功", "USER32.dll 测试", 0x40); // 0x40 表示信息图标
            if (m_task != null)
            {
                MessageBox.Show("正在执行动作...");
                return;
            }

            /*
            //这里怎么写呢? 不知道...
            m_task = Task.Run(async () =>
            {
                try
                {                    
                    int tryint = 0;
                    Exerecord = "开始执行..";

                    //"总输出AC和闸"
                    plc39.Write("M100.0", true); 
                    Thread.Sleep(300); 
                    plc39.Write("M100.0", false); 
                    
                    Exerecord = "所有流程结束.";
                }
                catch (Exception ex)
                {
                    Exerecord = "自动化异常:" + ex.Message;
                    return;
                }
            });

            //这里就是 m_task.wait().. 主线程在等待 子线程结束...(本质上，关闭窗口也应该这样的.)
            await m_task;            
            m_task = null;
            */
        }
    }
}
