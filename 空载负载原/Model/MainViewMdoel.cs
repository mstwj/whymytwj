using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Modbus.Device;
using S7.Net;
using S7.Net.Types;
using 空载_负载.Base;
using 空载_负载.Base.Table;
using 空载_负载.View;

namespace 空载_负载.Model
{
    public class MainViewMdoel : ObservableValidator
    {
        public Sampleinformation m_sampleinformation { get; set; } = new Sampleinformation();
       
        private object _contentView;
        public object ContentView { get => _contentView; set { SetProperty(ref _contentView, value); } }

        public void DoBtnCommandStart(string param)
        {

            if (ContentView != null && ContentView.GetType().Name == param) return;

            if (ContentView != null)
            {
                //不为空.. 切换其他...
                if (this.ContentView.GetType().Name == "NoloadPage")
                {
                    if (((NoloadPage)this.ContentView).model.Close() == false)
                    {
                        MessageBox.Show("无法切换,请先停止空载设备");
                    }
                }
            }

            Type type = Assembly.GetExecutingAssembly().GetType("空载_负载.View." + param)!;
            this.ContentView = Activator.CreateInstance(type)!;

            //这里，我不知道是什么试验，不对，我是知道的..
            if (param == "NoloadPage")
            {
                //空载试验..
                ((NoloadPage)this.ContentView).model.Initiate(m_sampleinformation);
            }

            if (param == "LoadPage")
            {
                //负载试验..
            }

            if (param == "Induction")
            {
                //感应试验..
            }

        }

    }

    public class MessageInit
    {        
        public string Message { get; set; }
        public AnalysisData Data { get; set; } = null;
    }
}
