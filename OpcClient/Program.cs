using Opc.Ua;
using Opc.Ua.Client;

namespace OpcClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //因为这里是异步，所以，主线程直接放回了..
            OpcUA_Client();

            Console.ReadLine();
        }

        #region OPCUA
        static async void OpcUA_Client()
        {
            //有点像 单利模式..
            //1.对于匿名，只要一个IP地址...
            //ApplicationConfiguration configuration
            //ConfiguredEndpoint endpoint 服务器地址.
            //, bool updateBeforeConnect, string sessionName, uint sessionTimeout, IUserIdentity identity, IList<string> preferredLocales, 

            try
            {
                //证书就算了..
                //匿名只要一个地址..(没有外网，这样就OK了..只要不暴露就可以了..)
                //Session session = await AnonymousConnection("opc.tcp://127.0.0.1:49320");

                //可以用户名和密码登入..
                Session session = await UserNameConnection("opc.tcp://127.0.0.1:49320","tong","tong");

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
                //Subscription(session);

                //写 -- 同步
                //SyncWrite(session);

                //写 -- 异步..
                AsyncWrite(session);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
                    NodeId = "ns=2;s=通道 1.设备 1.标记 1",
                    AttributeId = Attributes.Value
                });
                /*
                readValueIds.Add(new ReadValueId
                {
                    NodeId = "ns = 2; s = 通道 1.设备 1.标记 1",
                    AttributeId = Attributes.Value
                });
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
            sub.AddItem(mi);
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
            Console.Write(monitoredItem.StartNodeId + "  ");
            Console.WriteLine(item.Value);
            
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
                    if (item.WrappedValue.TypeInfo.ValueRank ==1)
                        foreach(var v in (short[])item.WrappedValue.Value)
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

        static Task<Session> UserNameConnection(string endpoint,string username,string password)
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
            validator.Update(new SecurityConfiguration
            {
                RejectSHA1SignedCertificates = false,
            });
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
            wv.Value = new DataValue() {
                //数据类型必须严格一直..
                Value = (ushort)888
            }; //这里的888是我自己瞎写的..这里必须转short
            values.Add(wv);

            session.Write(new RequestHeader(), values,
                out StatusCodeCollection statuses,
                out DiagnosticInfoCollection dic);

            foreach(var item in statuses)
            {
                //这里输出是FLASE 为什么。
                //不是写的有问题，是一个隐蔽问题，我们要SHORT，你写个888 是一个INT
                Console.WriteLine(StatusCode.IsGood(item));
            }
        }

        static  void AsyncWrite(Session session)
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
            WriteResponse response =  session.WriteAsync(new RequestHeader(), values,  ct).Result;
                

            foreach (var item in response.Results)
            {
                //这里输出是FLASE 为什么。
                //不是写的有问题，是一个隐蔽问题，我们要SHORT，你写个888 是一个INT
                Console.WriteLine(StatusCode.IsGood(item));
            }
        }


    }
}
