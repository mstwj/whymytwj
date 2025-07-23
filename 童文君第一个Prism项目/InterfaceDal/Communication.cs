using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using 童文君第一个Prism项目.Interface;

namespace 童文君第一个Prism项目.InterfaceDal
{
    public class Communication : ICommunication
    {
                                          
        public event EventHandler<byte[]> Receiveved;
        public bool Connect(string host, string port)
        {
            Task.Run(async () =>
            {
                IMqttClient mqttclient = new MqttFactory().CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer("127.0.0.1", 1883)
                    .WithClientId("AAA")
                    .WithCredentials("admin", "123456")
                    .Build();
                CancellationToken token = new CancellationToken();
                var result = await mqttclient.ConnectAsync(options, token);
                if (result.ResultCode == MqttClientConnectResultCode.Success)
                {
                    //连接OK
                    Debug.WriteLine("连接OK");

                    //订阅
                    var sub_options = new MqttClientSubscribeOptions();

                    MqttTopicFilter mqttTopicFilter = new MqttTopicFilter();
                    mqttTopicFilter.Topic = "AAAA";

                    sub_options.TopicFilters.Add(mqttTopicFilter);
                    await mqttclient.SubscribeAsync(sub_options);


                    mqttclient.ApplicationMessageReceivedAsync += My_MqttClient;
                }
            });
            return true;
        }

        private Task My_MqttClient(MqttApplicationMessageReceivedEventArgs arg)
        {
            //arg.ApplicationMessage.Payload();
            Receiveved?.Invoke(this,arg.ApplicationMessage.Payload);
            return Task.Run(() =>
            {

            });
        }
    }
}
