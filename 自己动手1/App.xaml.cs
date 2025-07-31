using System.Configuration;
using System.Data;
using System.Windows;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;

namespace 自己动手1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //定义一个事件..
        public static event EventHandler<string> MqttMessageReceived;

        public App()
        {
            StartMqttClient();
        }


        IManagedMqttClient client = null;
        private void StartMqttClient()
        {
            // 服务器相关配置
            // 192.168.151.137  1883   admin   123456
            // Client ID
            // 实例化一个客户端对象 
            client = new MqttFactory().CreateManagedMqttClient();
            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e =>
            {
                //state = true;
                //告诉服务器，我就是要监听AAA...
                MqttTopicFilter filter = new MqttTopicFilter();
                filter.Topic = "AAA";
                client.SubscribeAsync(filter);
            });
            //因为人家推送的是AAA的信息，我要去订阅，才可以的..
            client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
            {
                //接收到AAA的消息，AAA是什么，是我订阅的...
                //数据来了，上抛，就怎么的简单..
                MqttMessageReceived?.Invoke(this, e.ApplicationMessage.ConvertPayloadToString());
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

}
