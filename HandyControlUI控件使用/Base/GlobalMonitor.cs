using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using HandyControlUI控件使用.Models;
using Opc.Ua;
using Opc.Ua.Client;

namespace HandyControlUI控件使用.Base
{
    public class centerMessage
    {
        public int MessageType; //消息类型..(这里可以去定义类型，比如 1 - 是定时器..)
        public DataValue Value; //数据值..(一般都是2个字节 。。)
        public DataValueCollection ValueCollection;
    }
        
    public class GlobalMonitor
    {
        
        public GlobalMonitor()
        {
            
        }

        //为什么是一个ObservableCollection 不是LIST呢？
        //因为，界面要直接去绑定到这里，这就是为什么..
        public static ObservableCollection<StudioModel> StudioList { get; set; } = new ObservableCollection<StudioModel>();

        static bool isRunning = true;
        static Task mainTask = null;
        static Session session = null;

        public static void Start()
        {
            OpcUA_Client();

            /*
            mainTask = Task.Run(async () =>
            {               
                //1001 #1 Master device info 8937-45845735 2 
                DataServer dataServer = new DataServer();
                //麻痹的，SQL必须是6.0.0.要不然，就几把报错.... 还不能是高版本..
                var list = dataServer.GetStudio();
                if (list != null)
                    foreach (var item in list)
                    {
                        StudioList.Add(item);
                    }

                while (isRunning)
                {
                    
                    await Task.Delay(1000);
                    /*
                    foreach (var item in DeviceList)
                    {
                        if (item.CommType == 2)// S7通信
                        {
                            // List<string> addrList = item.MonitorValueList.Select(v => v.Address).ToList();

                            
                            //Zhaoxi.Communication.Siemens.S7Net s7Net = new Communication.Siemens.S7Net(item.S7.IP, item.S7.Port, (byte)item.S7.Rock, (byte)item.S7.Slot);

                            List<string> addrList = item.MonitorValueList.Select(v => v.Address).ToList();
                            //var result = s7Net.Read<ushort>(addrList);
                            if (result.IsSuccessed)
                            {
                                for (int i = 0; i < item.MonitorValueList.Count; i++)
                                {
                                    item.MonitorValueList[i].Value = result.Datas[i];
                                }
                            }

                            s7Net.Close();
                            
                        }
                    }
                    
                }
            });
            */
        }

        public static void Stop()
        {
            isRunning = false;
            mainTask.ConfigureAwait(true);
        }

        #region OPCUA
        static async void OpcUA_Client()
        {
            //有点像 单利模式..
            //1.对于匿名，只要一个IP地址...
            //ApplicationConfiguration configuration
            //ConfiguredEndpoint endpoint 服务器地址.
            //, bool updateBeforeConnect, string sessionName, uint sessionTimeout, IUserIdentity identity, IList<string> preferredLocales, 
            mainTask = Task.Run(async () =>
            {
                try
                {
                    //证书就算了..
                    //匿名只要一个地址..(没有外网，这样就OK了..只要不暴露就可以了..)
                    session = await AnonymousConnection("opc.tcp://127.0.0.1:49320");

                    //可以用户名和密码登入..
                    //session = await UserNameConnection("opc.tcp://127.0.0.1:49320", "tong", "tong");

                    //浏览节点...
                    //Browser browser = new Browser(session);
                    //要读标签，必须这样写..
                    //这里是怎么来的，直接看的..
                    //var collection = browser.Browse("ns=2;s=通道 1.设备 1.标记 1");
                    //读-同步
                    //SyncRead(session);

                    //读-异步
                    //AsSyncRead(session);

                    //订阅 -- 变化就读，不变就不读..
                    Subscription(session);

                    while (isRunning)
                    {
                        await Task.Delay(1000);
                        //读-同步
                        SyncRead(session);
                    }

                    //写 -- 同步
                    //SyncWrite(session);

                    //写 -- 异步..
                    //AsyncWrite(session);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            
            Console.WriteLine("Is, Good!");
        }
        #endregion


        static void SyncRead(Session session)
        {
            try
            {
                ReadValueIdCollection readValueIds = new ReadValueIdCollection();
                readValueIds.Add(new ReadValueId
                {
                    NodeId = "ns=2;s=通道 2.电流表.aaaa",
                    AttributeId = Attributes.Value
                });
                
                readValueIds.Add(new ReadValueId
                {
                    NodeId = "ns=2;s=通道 2.电流表.bbbb",
                    AttributeId = Attributes.Value
                });
                /*
                readValueIds.Add(new ReadValueId
                {
                    NodeId = "ns = 2; s = 通道 1.设备 1.标记 1",
                    AttributeId = Attributes.Value
                });
                */
                session.Read(new RequestHeader(),
                    0,//有数据缓存，是服务器缓存的数据，0是当前.
                    TimestampsToReturn.Both,//返回2方的时间.
                    readValueIds,
                    out DataValueCollection results,
                    out DiagnosticInfoCollection diagnostics
                    );

                centerMessage centerMessage = new centerMessage();
                centerMessage.MessageType = 2;    
                centerMessage.ValueCollection = results;
                WeakReferenceMessenger.Default.Send(centerMessage);

            }
            catch (Exception ex)
            {             
                throw;
            }
        }

        static void Subscription(Session session)
        {
            var sub = session.DefaultSubscription;

            MonitoredItem mi = new MonitoredItem();
            mi.StartNodeId = "ns=2;s=通道 1.设备 1.标记 1";
            mi.Notification += Mi_Notification;

            //MonitoredItem mi2 = new MonitoredItem();         
            //mi2.StartNodeId = "ns=2;s=通道 2.电流表.aaaa";
            //mi2.Notification += Mi_Notification;

            sub.AddItem(mi);
            //sub.AddItem(mi2);
            session.AddSubscription(sub);

            /*
            sub.PublishStatusChanged += (se, ev) =>
            {
                //如果订阅失败了..
                if (sub.PublishingStopped)
                {
                    //订阅已断开..
                }
            };
            */

            sub.Create(); //通知服务器..
        }

        private static void Mi_Notification(
            MonitoredItem monitoredItem,
            MonitoredItemNotificationEventArgs e)
        {
            var item = e.NotificationValue as MonitoredItemNotification;
   
            if (monitoredItem.StartNodeId.ToString().Equals("ns=2;s=通道 1.设备 1.标记 1", StringComparison.Ordinal))
            {
                //定时器来的消息。。
                centerMessage centerMessage = new centerMessage();
                centerMessage.MessageType = 1;
                centerMessage.Value = item.Value;
                WeakReferenceMessenger.Default.Send(centerMessage);
            }

            //if (monitoredItem.StartNodeId.ToString().Equals("ns=2;s=通道 2.电流表.aaaa", StringComparison.Ordinal))
            //{
           //     centerMessage centerMessage = new centerMessage();
           //     centerMessage.MessageType = 2;
           //     centerMessage.Value = item.Value;
           //     WeakReferenceMessenger.Default.Send(centerMessage);
           // }

        }

        static async void AsSyncRead(Session session)
        {
            try
            {
                ReadValueIdCollection readValueIds = new ReadValueIdCollection();
                readValueIds.Add(new ReadValueId
                {
                    NodeId = "ns=2;s=通道 1.设备 1.标记 1",
                    AttributeId = Attributes.Value
                });

                CancellationToken ct = new CancellationToken();
                ReadResponse readResponse = await session.ReadAsync(new RequestHeader(),
                    0,//有数据缓存，是服务器缓存的数据，0是当前.
                    TimestampsToReturn.Both,//返回2方的时间.
                    readValueIds,
                    ct);

                foreach (var item in readResponse.Results)
                {
                    if (item.WrappedValue.TypeInfo.ValueRank == -1)
                        Console.WriteLine(item);
                    if (item.WrappedValue.TypeInfo.ValueRank == 1)
                        foreach (var v in (short[])item.WrappedValue.Value)
                        {
                            Console.WriteLine(v);
                        }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        static Task<Session> AnonymousConnection(string endpoint)
        {

            return Session.Create(
           new Opc.Ua.ApplicationConfiguration()
           {
               ClientConfiguration = new ClientConfiguration()
           },
           new ConfiguredEndpoint(null, new EndpointDescription(endpoint)), //连接的IP..
           true,//不知道..
           "zhaoxi-opc",//SESION NAME,连接上服务器后，我是什么名字呢？这个顺便定义..
           5000, //长时间不通信，就踢..
           new UserIdentity(),//必须这样..
           new List<string>() { } //不知道 。。
           );

        }

        static Task<Session> UserNameConnection(string endpoint, string username, string password)
        {
            //异常: SHA1 signed cretificaties
            ApplicationConfiguration configuration = new ApplicationConfiguration();
            configuration.ClientConfiguration = new ClientConfiguration();

            CertificateValidator validator = new CertificateValidator();
            validator.CertificateValidation += (se, ev) =>
            {
                if (ev.Error.StatusCode.Code == StatusCodes.BadCertificateUntrusted)
                    ev.Accept = true;
            };
            //validator.Update(new SecurityConfiguration
            //{
            //    RejectSHA1SignedCertificates = false,
            //});
            configuration.CertificateValidator = validator;

            return Session.Create(
           configuration,
           new ConfiguredEndpoint(null, new EndpointDescription(endpoint)), //连接的IP..
           true,//不知道..
           "zhaoxi-opc",//SESION NAME,连接上服务器后，我是什么名字呢？这个顺便定义..
           5000, //长时间不通信，就踢..
           new UserIdentity(username, password),//必须这样..
           new List<string>() { } //不知道 。。
           );

        }


        static void SyncWrite(Session session)
        {
            WriteValueCollection values = new WriteValueCollection();
            WriteValue wv = new WriteValue();
            wv.NodeId = "ns=2;s=通道 1.设备 1.标记 2";
            wv.AttributeId = Attributes.Value; //我要写到VALUE这个属性里面去.
            wv.Value = new DataValue()
            {
                //数据类型必须严格一直..
                Value = (ushort)888
            }; //这里的888是我自己瞎写的..这里必须转short
            values.Add(wv);

            session.Write(new RequestHeader(), values,
                out StatusCodeCollection statuses,
                out DiagnosticInfoCollection dic);

            foreach (var item in statuses)
            {
                //这里输出是FLASE 为什么。
                //不是写的有问题，是一个隐蔽问题，我们要SHORT，你写个888 是一个INT
                Console.WriteLine(StatusCode.IsGood(item));
            }
        }

        static void AsyncWrite(Session session)
        {
            WriteValueCollection values = new WriteValueCollection();
            WriteValue wv = new WriteValue();
            wv.NodeId = "ns=2;s=通道 1.设备 1.标记 2";
            wv.AttributeId = Attributes.Value; //我要写到VALUE这个属性里面去.
            wv.Value = new DataValue()
            {
                //数据类型必须严格一直..
                Value = (ushort)222
            }; //这里的888是我自己瞎写的..这里必须转short
            values.Add(wv);

            CancellationToken ct = new CancellationToken();
            WriteResponse response = session.WriteAsync(new RequestHeader(), values, ct).Result;


            foreach (var item in response.Results)
            {
                //这里输出是FLASE 为什么。
                //不是写的有问题，是一个隐蔽问题，我们要SHORT，你写个888 是一个INT
                Console.WriteLine(StatusCode.IsGood(item));
            }
        }
    }
}
