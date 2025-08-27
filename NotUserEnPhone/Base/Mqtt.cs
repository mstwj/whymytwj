using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace NotUserEnPhone.Base
{
    public class Mqtt
    {
        IManagedMqttClient MqttClient;
        public Mqtt()
        {
            MqttClient = new MqttFactory().CreateManagedMqttClient();
            var clientOptions = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("192.168.10.13", 1883)  // IP    服务在本机     程序运行在模拟器（虚拟机）
                .WithCredentials("admin", "123456");
            var option = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(clientOptions.Build());
            MqttClient.StartAsync(option.Build()).GetAwaiter().GetResult();

            // 订阅一个主题 
            MqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("Zhaoxi").Build());
            MqttClient.UseApplicationMessageReceivedHandler(msg =>
            {
                //MqttMessageReceived?.Invoke(msg);
            });
        }

        public void Publish(string topic, byte[] msg)
        {
            MqttApplicationMessage message = new MqttApplicationMessage();
            message.Topic = topic;
            message.Payload = msg;
            message.QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce;

            MqttClient.PublishAsync(message).GetAwaiter().GetResult();
        }
    }
}
