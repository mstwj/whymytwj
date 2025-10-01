using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControlUI控件使用.Base;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using Opc.Ua;

namespace HandyControlUI控件使用.ViewModels
{
    public class UserCamer1 : ObservableObject, IDisposable
    {

        private int mytest;
        public int MYTEST
         {
            get { return mytest; }

            set { SetProperty(ref mytest, value); }
        }


        private UInt16 _aaaa;

        private UInt16 _bbbb;

        public UInt16 AAAA
        {
            get { return _aaaa; }

            set { SetProperty(ref _aaaa, value); }
        }      

        public UInt16 BBBB
        {
            get { return _bbbb; }

            set { SetProperty(ref _bbbb, value); }
        }

        public ICommand BtnCommandSendHandJSOK { get; set; }

        public ICommand BtnCommandSendHandJSCansel { get; set; }


        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public UserCamer1()
        {

            BtnCommandSendHandJSOK = new RelayCommand<object>(DoBtnCommandSendHandJSOK);

            BtnCommandSendHandJSCansel = new RelayCommand<object>(DoBtnCommandSendHandJSCansel);




            WeakReferenceMessenger.Default.Register<centerMessage>(this, (r, user) =>
            {
                if (user.MessageType == 2)
                {                   
                    DataValue item = user.ValueCollection[0]; 
                    if(item.StatusCode == StatusCodes.Good)
                    {                            
                        AAAA = (UInt16)item.Value;
                    }
                    if (item.StatusCode == StatusCodes.Bad)
                    {
                        AAAA = 0;
                    }
                    DataValue item2 = user.ValueCollection[1];
                    if (item2.StatusCode == StatusCodes.Good)
                    {
                        BBBB = (UInt16)item2.Value;
                    }

                }
            });
        }

        private void DoBtnCommandSendHandJSCansel(object sender)
        {
            tokenSource.Cancel();
            return;
        }

        private void DoBtnCommandSendHandJSOK(object sender)
        {
            //千万不要在这里做 任务。。任务必须在 全局做，要不然就会有 脏数据...
            //await -- 注意UI线程不会卡。。 会继续执行，如果不是UI线程，会卡。。。
            //这里如果是绑定了，直接就红了...(点击确定后，使用的是过去的数值..)
            //如果是UI线程，如果在一个函数里面，这个函数就马上结束了，控制器马上返回给UI线程...不会等待..
            //问题是，只能使用1次..
            /*
            CancellationToken cancellationToken;
            cancellationToken = tokenSource.Token;
            //Task.是个任务..
            Task task1 = Task.Run(new Action(() => {

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        MYTEST = 0;
                        break;
                    }
                    MYTEST++;
                    Thread.Sleep(2000);
                }

            }), cancellationToken);//第2个是参数，线程参数传递...
            
            Task<int> task2 = Task.Run(new Func<int>(() => {

                return 100;
            }));//第2个是参数，线程参数传递...
            
            task2.Wait();//主线程等待。。 会UI卡...

            MYTEST = task2.Result;//通过Result得到数据..
            

            Task<int> task3 = Task.Run(new Func<int>(() => {
                return 200;
            }));//第2个是参数，线程参数传递...

            Task.Run(() =>
            {
                //也是等待线程结束.. 3000是什么意思，就是等3秒..
                //如果3秒，还没有执行结束，我就不等了..
                task3.Wait(3000);
                //这里要特别注意，得到result的值，也会让线程卡住...
                MYTEST = task3.Result;

            });            
            

            Task task4 = Task.Run(() => { MYTEST = 10; Task.Delay(1000); });

            Task task5 = task4.ContinueWith((item) =>
            {
                MYTEST = 20;
                Task.Delay(1000);
            },TaskContinuationOptions.None);
            //TaskContinuationOptions.None 任务成功执行了。。我才执行.
            //TaskContinuationOptions.Faile 任务执行错误了。。我才执行.
            */
            Task task1 = Task.Run(() => 
            {
                Debug.WriteLine("线程1开始");
                Thread.Sleep(3000);
                Debug.WriteLine("线程1结束"); 
            });
            Task task2 = Task.Run(() => 
            {
                Debug.WriteLine("线程2开始");
                Thread.Sleep(8000);
                Debug.WriteLine("线程2结束"); 
            });

            Task.Run(() =>
            {
                Debug.WriteLine("线程3开始");
                Task.WaitAll(task1, task2);
                Debug.WriteLine("线程3结束");
            });

            //Task.WaitAll(task1, task2); //等待所有..
            //Task.WaitAny(task1, task2); //任何一个..

            //Debug.WriteLine("主线程结束");

            return;
        }



        public void Dispose()
        {
            //这里走不过来... 不知道为什么.. 使用CREATEACTION就是走不过啊里..
        }
    }
}
