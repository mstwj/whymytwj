using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Modbus.Device;
using NLog;
using 功率分析仪NP3000.Table;
using 功率分析仪NP3000.View1;

namespace 功率分析仪NP3000.Model
{
    public class PageDeveclKongzaiModelcs
    {
        private SerialPort serialPort = new SerialPort();
        private ModbusSerialMaster master = null;

        private Task rotobTaskT = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Queue<int> queue = new Queue<int>();        

        //设备显示区...
        public UserMainControlModel m_userMainControlModel;
        //控制记录..
        public UserListControl m_userListControl;
        //下面区域
        public UserPanelKongzaiModel m_userPanelKongzaiModel;
        //用户信息
        UserProInfoControlModel m_userProInfoControlModel;

        public PageDeveclKongzaiModelcs(UserMainControlModel userMainControlModel,
            UserListControl userListControl, 
            UserPanelKongzaiModel userPanelKongzai,
            UserProInfoControlModel userProInfoControlModel)
        {
            m_userMainControlModel = userMainControlModel;
            m_userListControl = userListControl;
            m_userPanelKongzaiModel = userPanelKongzai;
            m_userProInfoControlModel = userProInfoControlModel;       
            
        }

        private async Task<bool> ExePlcCommand(int command)
        {
            bool Success = false;
            string error = string.Empty;

            m_userListControl.AddRecord("请等待,回应!"+command.ToString());
            var timeout = Task.Delay(8000);
            var t = Task.Run(async () =>
            {                
                try
                {
                    switch (command)
                    {
                        case 1: break;
                        case 2: break;
                        case 3: break;
                    }
                    Success = true;                    
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    m_userListControl.AddRecord("新线程-任务异常:" + error,true);  
                    Success = false;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                m_userListControl.AddRecord("超时,请检查通信是否正常!", true);
                return false;
            }

            if (Success)
            {                
                return true;
            }
            else
            {
                m_userListControl.AddRecord("机器人线程-错误Success==false", true);                 
                return false;
            }
        }


        public async void InitializeAsync(table_proinfo table_Proinfo)
        {
            if (table_Proinfo.Id == 0)
            {
                MessageBox.Show("无法初始化,没有选择样品!"); 
                return;
            }
            //1.先设置产品..
            m_userProInfoControlModel.YanPingBianhao = table_Proinfo.YanPingBianhao;
            m_userProInfoControlModel.GuigeXinhao = table_Proinfo.GuigeXinhao;
            m_userProInfoControlModel.Tuhao = table_Proinfo.Tuhao;
            m_userProInfoControlModel.XiangShu = table_Proinfo.XiangShu;
            m_userProInfoControlModel.Laozhu = table_Proinfo.Laozhu;
            m_userProInfoControlModel.GaoYaLaozhu = table_Proinfo.GaoYaLaozhu;
            m_userProInfoControlModel.DiYaLaozhu = table_Proinfo.DiYaLaozhu;
            m_userProInfoControlModel.DanWei = table_Proinfo.DanWei;
            m_userProInfoControlModel.CaiZhi = table_Proinfo.CaiZhi;
            m_userProInfoControlModel.EDingLongLiang = table_Proinfo.EDingLongLiang;
            m_userProInfoControlModel.TiaoYaBili = table_Proinfo.TiaoYaBili;
            m_userProInfoControlModel.GaoYaEDingDianYa = table_Proinfo.GaoYaEDingDianYa;
            m_userProInfoControlModel.DiYaEDingDianYa = table_Proinfo.DiYaEDingDianYa;
            m_userProInfoControlModel.BiaoHao = table_Proinfo.BiaoHao;
            m_userProInfoControlModel.ZhuHao = table_Proinfo.ZhuHao;

            //先打开端口...
            try
            {
                m_userListControl.AddRecord("设置串口");

                serialPort.BaudRate = 9600; // 设置波特率
                serialPort.Parity = Parity.None; // 设置奇偶校验
                serialPort.DataBits = 8; // 设置数据位数
                serialPort.StopBits = StopBits.One; // 设置停止位
                serialPort.Handshake = Handshake.None; // 设置握手协议(这里如果是 FULUKE就是硬件..)

                master = ModbusSerialMaster.CreateRtu(serialPort);//这里传入的就是我们创建的串口对象
                master.Transport.ReadTimeout = 1000;// 设置超时时间默认为500毫秒
                serialPort.PortName = ConfigurationManager.AppSettings["DEVICE_COM"];
                //serialPort.Open(); //打开串口
                m_userListControl.AddRecord("设置串口完成!");

            }
            catch (Exception ex)
            {
                m_userListControl.AddRecord("设置串口失败!" + ex.Message);
                return;
            }

            rotobTaskT = Task.Run(async () =>
            {
                System.DateTime? precurrentTime = System.DateTime.UtcNow;
                TimeSpan? timeSpan = null;

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        //这里是需要TRY的。。 我执行代码的时候，遇到了一个问题，就是主线程没有加线程分发.. 导致.. 报错..
                        Thread.Sleep(10);
                        System.DateTime currentTime = System.DateTime.UtcNow;

                        TimeSpan ts = (TimeSpan)(currentTime - precurrentTime);
                        //得到毫秒数...
                        double milliseconds = ts.TotalMilliseconds;

                        //定时时间到了..
                        if (milliseconds > 1000)
                        {
                            /*
                            //设置为后台执行..
                            var myresult1 = await ExePlcCommand(2);
                            //必须要判断，要不然 子线程他不等..
                            if (!myresult1)
                            {
                                m_userListControl.AddRecord("定时器指令读取PLC执行失败-后台线程退出!", true);
                                return;
                            }
                            */
                            precurrentTime = currentTime;
                            continue;
                        }
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();                        
                        var myresult2 = await ExePlcCommand(Command);
                        if (myresult2)
                        {
                            m_userListControl.AddRecord("指令执行成功!", false);
                        }
                        else
                        {
                            m_userListControl.AddRecord("指令执行失败!-后台线程退出", true);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        m_userListControl.AddRecord($"{ex.Message}", true);                        
                        return;
                    }
                }
            }, cts.Token);

            ///m_userMainControlModel.CurrentNumber1_1 = 100.4f;
        }

        
    }
}
