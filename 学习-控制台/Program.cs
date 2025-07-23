using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Packets;

namespace 学习_控制台
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MqttClient();

            Console.ReadLine();
        }

        static async void MqttClient()
        {
            MQTTnet.Client.IMqttClient mqttclient = new MQTTnet.MqttFactory().CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("127.0.0.1", 1883)
                .WithClientId("AAA")
                .WithCredentials("admin", "123456")
                .Build();
            CancellationToken token = new CancellationToken();
            var result = await mqttclient.ConnectAsync(options, token);
            if (result.ResultCode == MQTTnet.Client.Connecting.MqttClientConnectResultCode.Success)
            {
                //连接OK
                Debug.WriteLine("连接OK");

                //订阅
                var sub_options = new MqttClientSubscribeOptions();

                MqttTopicFilter mqttTopicFilter = new MqttTopicFilter();
                mqttTopicFilter.Topic = "AAAA";

                sub_options.TopicFilters.Add(mqttTopicFilter);
                await mqttclient.SubscribeAsync(sub_options);


                mqttclient.UseApplicationMessageReceivedHandler(ReciveMessage);
            }

            static void ReciveMessage(MqttApplicationMessageReceivedEventArgs ags)
            {

            }
            

        }
       
    }
}
