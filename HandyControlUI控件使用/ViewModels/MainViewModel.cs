using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using HandyControlUI控件使用.Base;
using Newtonsoft.Json.Linq;

namespace HandyControlUI控件使用.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        private UInt16 _meTimer;

        public UInt16 MeTimer
        {
            get { return _meTimer; }

            set { SetProperty(ref _meTimer, value); }
        }

        private UIElement _mainContent;

        public UIElement MainContent
        {
            get { return _mainContent; }

            set { SetProperty(ref _mainContent, value); }
        }

        public MainViewModel()
        {
            WeakReferenceMessenger.Default.Register<centerMessage>(this, (r, user) =>
            {
                if (user.MessageType == 1)
                {
                    //如果是定时器消息..
                    MeTimer = (UInt16)user.Value.Value;
                }
            });
        }
    }
}
