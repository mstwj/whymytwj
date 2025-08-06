using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 串口测试.Base;

namespace 串口测试.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyBase
    {
        private UIElement _mainContent;

        

        public UIElement MainContent
        {
            get { return _mainContent; }
            set
            {
                Set<UIElement>(ref _mainContent, value);
            }
        }

        public CommandBase TabChangedCommand { get; set; }

        public MainWindowViewModel()
        {
            TabChangedCommand = new CommandBase(OnTabChanged);


            //默认情况下就打开这个..
            OnTabChanged("串口测试.Views.SystemMonitor");
        }

        private void OnTabChanged(object obj)
        {
            if (obj == null) return;
            // 完整方式
            //string[] strValues = o.ToString().Split('|');
            //Assembly assembly = Assembly.LoadFrom(strValues[0]);
            //Type type = assembly.GetType(strValues[1]);
            //this.MainContent = (UIElement)Activator.CreateInstance(type);

            // 简化方式，必须在同一个程序集下
            //等于说每次都是一个新的页面...(原来的页面，就消失了...)
            Type type = Type.GetType(obj.ToString());
            this.MainContent = (UIElement)Activator.CreateInstance(type);
        }
    }
}
