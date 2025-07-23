using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts.Defaults;
using LiveCharts;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using 维通利智耐压台.Base;
using Microsoft.Win32;
using System.Windows;

namespace 维通利智耐压台.Model
{
    public class ExportModel : ObservableObject
    {        
        private string operate = string.Empty;
        public string Operate { get => operate; set { SetProperty(ref operate, value); } }

        private string airpressure = string.Empty;
        public string Airpressure { get => airpressure; set { SetProperty(ref airpressure, value); } }

        private string temperature = string.Empty;
        public string Temperature { get => temperature; set { SetProperty(ref temperature, value); } }


        public string ProductName { get; set; } = string.Empty;
        //产品编号
        public string ProductNumber { get; set; } = string.Empty;
        //产品型号
        public string ProductType { get; set; } = string.Empty;
        //图号
        public string ProductTuhao { get; set; } = string.Empty;
        //标准电压
        private string productStardVotil = string.Empty;
        public string ProductStardVotil { get => productStardVotil; set { SetProperty(ref productStardVotil, value); } }

        private string productStardPartial = string.Empty;
        //标准局放
        public string ProductStardPartial { get => productStardPartial; set { SetProperty(ref productStardPartial, value); } }

        //开始时间
        public string RecordDateTimer { get; set; } = string.Empty;
        //电压
        public string Votil { get; set; } = string.Empty;
        //局放
        public string Partial { get; set; } = string.Empty;
        //施加部位
        public string ProductParts { get; set; } = string.Empty;
        public string ProductQualified { get; set; } = string.Empty;

        public string LevelV1 { get; set; } = string.Empty;
        public string LevelV2 { get; set; } = string.Empty;
        public string LevelV3 { get; set; } = string.Empty;

        public string LevelTime1 { get; set; } = string.Empty;
        public string LevelTime2 { get; set; } = string.Empty;
        public string LevelTime3 { get; set; } = string.Empty;

        public string LevelPc1 { get; set; } = string.Empty;
        public string LevelPc2 { get; set; } = string.Empty;
        public string LevelPc3 { get; set; } = string.Empty;


        //就是第一次SET有用第2次都没用了..
        private string levelisgood1 = string.Empty;
        private string levelisgood2 = string.Empty;
        private string levelisgood3 = string.Empty;
        public string LevelIsGood1 { get => levelisgood1; set { SetProperty(ref levelisgood1, value); } }
        public string Leve2IsGood2 { get => levelisgood2; set { SetProperty(ref levelisgood2, value); } }
        public string Leve3IsGood3 { get => levelisgood3; set { SetProperty(ref levelisgood3, value); } }

        public string Savepng { get; set; } = string.Empty;

        //第一次初始化的时候会SET.. 以后在赋值，就不会刷新了..
        //这里必须这样..
        private string selectfinesave  = string.Empty;
        public string SelectFineSave { get => selectfinesave; set { SetProperty(ref selectfinesave, value); } }


        //public List<string> QueryTimePoints { get; set; } = new List<string>();

        public ChartValues<ObservableValue> QueryServerLineData { get; set; } = new ChartValues<ObservableValue>();
        public ChartValues<ObservableValue> QueryServerLineData2 { get; set; } = new ChartValues<ObservableValue>();

        private WaitWindow _waitWindow;

        public ICommand BtnCommandSaveWord { get; set; }


        public ICommand BtnCommandSavePath { get; set; }

        public ExportModel()
        {
            BtnCommandSaveWord = new RelayCommand<object>(DoBtnCommandSaveWord);
            BtnCommandSavePath = new RelayCommand<object>(DoBtnCommandSavePath);
        }

        private void DoBtnCommandSavePath(object param)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Doc files (*.doc)|*.doc"; // 设置文件过滤器
            saveFileDialog.Title = "请选择保存的文件名"; // 对话框标题
            saveFileDialog.ShowDialog(); // 显示对话框

            if (saveFileDialog.FileName != "") // 用户点击了“保存”按钮
            {
                // 这里可以执行保存文件的操作
                SelectFineSave = saveFileDialog.FileName;               
            }
        }

        private void DoBtnCommandSaveWord(object param)
        {
            // 显示等待窗口
            _waitWindow = new WaitWindow();

            //可以等一下管家线程..
            Task ReTs = Task.Run(async () =>
            {
                try
                {
                    MyConvert.ExportToWordUsingInteropWord(
                        SelectFineSave,
                        Operate, RecordDateTimer, ProductNumber,
                        Temperature, ProductParts, ProductTuhao,
                        Airpressure, ProductName, ProductType,
                        ProductStardVotil, ProductStardPartial,
                        LevelV1, LevelTime1, LevelPc1, LevelIsGood1,
                        LevelV2, LevelTime2, LevelPc2, Leve2IsGood2,
                        LevelV3, LevelTime3, LevelPc3, Leve3IsGood3,
                        Savepng
                        );                
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);                    
                }
                finally
                {
                    // 工作完成后，使用Dispatcher.Invoke来通知主线程更新UI元素
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // 更新UI元素，例如设置一个标签的文本                        
                        OnSaveIsOver();
                    });
                }
            });
            //这里有个问题，就是主线程在这里就堵塞了..
            _waitWindow.ShowDialog();
        }

        void OnSaveIsOver()
        {
            MessageBox.Show("任务完成!");
            _waitWindow.Close();
        }
    }
}
