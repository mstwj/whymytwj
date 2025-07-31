using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace 上位机推送
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private double _value;
        // 需要C#代码进行值更新，并且通知到页面
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        CancellationTokenSource cts = new CancellationTokenSource();
        Task Task = null;

        //MQTT必须要 用户名和密码，不是什么设备都可以连上来的... 1个mqtt服务器就有一个用户名和密码..
        IManagedMqttClient client = null;
        bool state = false;

        public MainWindow()
        {
            InitializeComponent();

            // 设置数据源
            this.DataContext = this;

            // 开始执行PLC点位获取动作
            // 连接到PLC
            //S7.Net.Plc plc = new S7.Net.Plc(S7.Net.CpuType.S7200Smart, "192.168.2.1", 102, 0, 1);
            //plc.Open();

            Task = Task.Run(async () =>
            {
                while (!cts.IsCancellationRequested)
                {
                    await Task.Delay(2000);
                    // 200SmartPLC里面   VW0  -》 666
                    //Value = double.Parse(plc.Read("DB1.DBW0").ToString());

                    //这里我随机给个数据...

                    Random random = new Random();
                    double randomNumber = random.NextDouble();
                    //Console.WriteLine(randomNumber); // 输出一个0.0到1.0之间的随机数

                    Value = randomNumber;


                    // 如果MQTT客户端连接成功，那么开始分发数据
                    if (state)
                    {
                        // 当拿到最新的点位数据后，由MQTT客户端对象提交上去
                        // 主题  
                        // 负载   打包一起发   Json   
                        DataEntity dataEntity = new DataEntity();
                        dataEntity.DeviceId = 1;

                        Random random2 = new Random();
                        double randomNumber2 = random.NextDouble();
                        dataEntity.DeviceValue1 = randomNumber2;
                        randomNumber2 = random.NextDouble();
                        dataEntity.DeviceValue2 = randomNumber2;
                        randomNumber2 = random.NextDouble();
                        dataEntity.DeviceValue3 = randomNumber2;
                        randomNumber2 = random.NextDouble();
                        dataEntity.DeviceValue4 = randomNumber2;
                        randomNumber2 = random.NextDouble();
                        dataEntity.DeviceValue5 = randomNumber2;
                        randomNumber2 = random.NextDouble();
                        dataEntity.DeviceValue6 = randomNumber2;

                        //dataEntity.Points.Add(new PointEntity { PropId = 3, Value = Value });

                        //提交上去..PublishAsync--就是发布的意思..
                        //连接OK了后，是不是要实时的发送呢？是的，那么就发AAA，谁定义了AAA就得到了数据
                        //1AAA 就是 主题 2 负载...
                        client.PublishAsync("AAA", System.Text.Json.JsonSerializer.Serialize(dataEntity));
                    }
                }
            }, cts.Token);

            MqttClient();

        }

        //结束线程 -- 主线程在等待所有线程..
        protected override void OnClosed(EventArgs e)
        {
            cts.Cancel();
            Task.WaitAll(new Task[] { Task });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void MqttClient()
        {
            // 服务器相关配置
            // 192.168.151.137  1883   admin   123456
            // Client ID
            // 实例化一个客户端对象 
            client = new MqttFactory().CreateManagedMqttClient();
            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e =>
            {
                state = true;
            });

            //服务器通过ID来管理 客户端的。。。
            IMqttClientOptions clientOptions = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("192.168.200.232", 1883)
                .WithCredentials("admin", "123456")
                .Build();

            IManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(clientOptions)
                .Build();
            client.StartAsync(options);// 连接到某个服务器
        }


    }

    class DataEntity
    {
        public int DeviceId { get; set; }
        public double DeviceValue1 { get; set; }
        public double DeviceValue2 { get; set; }
        public double DeviceValue3 { get; set; }
        public double DeviceValue4 { get; set; }
        public double DeviceValue5 { get; set; }
        public double DeviceValue6 { get; set; }

    }
   
}