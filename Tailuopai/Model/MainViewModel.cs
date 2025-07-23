using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Tailuopai.Base;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

namespace Tailuopai.Model
{
    //如果是安卓,http会异常：必须在安卓里面去设置.
    //android:usesCleartextTraffic="true" -- 注意..
    public class MainViewModel : ObservableObject
    {        
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Queue<int> queue { get; set; } = new Queue<int>();

        public ObservableCollection<LightItemModel> Items { get; set; } = new ObservableCollection<LightItemModel>();

        public List<string> PItems { get; set; } = new List<string>();
        

        private string imageshebei1;
        public string ImageShebei1 { get => imageshebei1 ; set { SetProperty(ref imageshebei1, value); } }

        private string imageshebei2;
        public string ImageShebei2 { get => imageshebei2; set { SetProperty(ref imageshebei2, value); } }

        private string imageshebei3;
        public string ImageShebei3 { get => imageshebei3; set { SetProperty(ref imageshebei3, value); } }


        private int imageScaleY1 = 1;
        public int ImageScaleY1 { get => imageScaleY1; set { SetProperty(ref imageScaleY1, value); } }

        private int imageScaleY2 = 1;
        public int ImageScaleY2 { get => imageScaleY2; set { SetProperty(ref imageScaleY2, value); } }

        private int imageScaleY3 = 1;
        public int ImageScaleY3 { get => imageScaleY3; set { SetProperty(ref imageScaleY3, value); } }


        private bool mainShowB = true;
        public bool MainShowB { get => mainShowB; set { SetProperty(ref mainShowB, value); } }

        private bool mainShowW = false;
        public bool MainShowW { get => mainShowW; set { SetProperty(ref mainShowW, value); } }


        public LightItemModelThress[] ItemsThess { get; set; }

        public string PSelect { get; set; } = string.Empty;

        private int ItemsThessNumber = 0;
        public ICommand EditButtonCommand { get; set; }        
        public ICommand BtnCommandStart { get; set; }
        public ICommand ReadmeButtonCommand { get; set; }
        public ICommand BtnSelectCommandWish { get; set; }

        //看不到的问题，有空格..
        private string[] MYPAIARRAY = new string[78] 
        {
            "愚者","魔术师","女祭司","女皇","皇帝","教皇","恋人",
            "战车","力量","隐士","命运之轮","正义","倒吊人","死神",
            "节制","恶魔","塔","星星","月亮","太阳","审判","世界",
            "圣杯1","圣杯2","圣杯3","圣杯4","圣杯5","圣杯6","圣杯7","圣杯8","圣杯9","圣杯10","圣杯11","圣杯12","圣杯13","圣杯14",
            "宝剑1","宝剑2","宝剑3","宝剑4","宝剑5","宝剑6","宝剑7","宝剑8","宝剑9","宝剑10","宝剑11","宝剑12","宝剑13","宝剑14",
            "权杖1","权杖2","权杖3","权杖4","权杖5","权杖6","权杖7","权杖8","权杖9","权杖10","权杖11","权杖12","权杖13","权杖14",
            "星币1","星币2","星币3","星币4","星币5","星币6","星币7","星币8","星币9","星币10","星币11","星币12","星币13","星币14"        
        };
        public MainViewModel()
        {
            //Items = new List<LightItemModel>();
            ItemsThess = new LightItemModelThress[3];
            ItemsThess[0] = new LightItemModelThress();
            ItemsThess[1] = new LightItemModelThress();
            ItemsThess[2] = new LightItemModelThress();

            PItems.Add("对自己爱情做预测");
            PItems.Add("对自己事业做预测");
            PItems.Add("对自己工作做预测");                        
            PItems.Add("对自己和女朋友的未来爱情做预测");
            PItems.Add("对自己和男朋友的未来爱情做预测");

            Random randomb = new Random();
            for (int i = 0;i<78;i++)
            {
                bool randomBool = randomb.Next(2) == 1;
                LightItemModel m_data = new LightItemModel();
                m_data.ItemType = i;                
                m_data.IsOpen = randomBool;
                m_data.Value1 = "tailuopai.png";
                m_data.EditButtonCommand = new RelayCommand<object>(DoImageButtonCommand);
                m_data.mySelf = m_data;
                m_data.Describe = MYPAIARRAY[i];
                m_data.Header = "密";
                Items.Add(m_data);                
            }

            /*
            Random random = new Random();
            var shuffledNumbers = Items.OrderBy(ItemType => random.Next()).ToList();
            //shuffledNumbers -- 这是一个新的数组..
            //int k = 0;
            foreach (var number in shuffledNumbers)
            {
                Console.WriteLine(number.ItemType.ToString());
                //Items[k].ItemType = (int)number;
                //Items[k].Header = Items[k].ItemType.ToString();
                //k++;
            }
            Items = shuffledNumbers;
            */

            RestartXipai();


            EditButtonCommand = new RelayCommand<object>(DoEditButtonCommand);
            BtnCommandStart = new RelayCommand<object>(DoBtnCommandStart);
            ReadmeButtonCommand = new RelayCommand<object>(DoReadmeButtonCommand);
            BtnSelectCommandWish = new RelayCommand<object>(DoBtnSelectCommandWish);

            Task ReTs = Task.Run(async () =>
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

                        if (milliseconds > 1000)
                        {
                            //定时时间到了..(1秒1次)                            
                            precurrentTime = currentTime;
                            continue;
                        }
                        if (queue.Count <= 0) continue;
                        int Command = queue.Dequeue();
                        //手机和PC有很大区别，就是线程.. PC的线程可以直接去控制界面..
                        //手机就不可以..
                        ShowWaitDialog(true);
                        //必须等1秒..动画..
                        Thread.Sleep(1000);

                        string result = string.Empty;
                        string title = string.Empty;
                        
                        var myresult = await ExePlcCommand(Command);
                        if (myresult) result = "执行成功"; 
                        else result = "执行失败"+ execerror;                        
                        
                        ShowWaitDialog(false);
                        //必须等1秒..动画..
                        Thread.Sleep(200);
                        if (Command == 1) title = "洗牌";
                        if (Command == 2) title = "占卜";
                        ShowResultDialog(title,result);
                        
                    }
                    catch (Exception ex)
                    {
                        //AddRecord($"管理线程异常:{ex.Message}", true);
                        return;
                    }
                }
            }, cts.Token);

        }

        //这里必须返回 -- 因为要await..
        async Task<bool> StartDivination()
        {
            try
            {
                HttpResponseMessage response;
                using var httpClient = new HttpClient();

                string describe1 = ItemsThess[0].Describe;
                string describe2 = ItemsThess[1].Describe;
                string describe3 = ItemsThess[2].Describe;
                string describe4 = PSelect;
                if (ItemsThess[0].Straight == true) describe1 += "正位"; else describe1 += "逆位";
                if (ItemsThess[0].Straight == true) describe2 += "正位"; else describe2 += "逆位";
                if (ItemsThess[0].Straight == true) describe3 += "正位"; else describe3 += "逆位";

                string myquery = "第1张牌(" + describe1 + "),第2张牌(" + describe2 + "),第3张牌(" + describe3 + ")结合上述三张塔罗牌,"+ describe4;                
                string uriString = "http://twjgod.w1.luyouxia.net/api/GetAnswer/id=" + myquery;                
                var uri = new Uri(uriString);
                response = await httpClient.GetAsync(uri);//异常，网站变了.
                //这里为什么不等待呢？就直接返回了呢?
                //加了await 工作线程等待了..
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(); //得到返回字符串.--
                jsontoken = JsonSerializer.Deserialize<MyData>(json); //字符串转类. -- 异常. 数据结构便了.
                
                return true;
            }
            catch(Exception ex)
            {
                execerror = ex.Message;
                Console.WriteLine(ex.Message);
                return false;         
            }
        }

        void RestartXipai()
        {
            Random random = new Random();
            var shuffledNumbers = Items.OrderBy(ItemType => random.Next()).ToList();            
            
            Items.Clear();
            
            foreach (var number in shuffledNumbers)
            {
                Console.WriteLine(number.ItemType.ToString() + number.Describe);
                number.IsChecked = false;
                Items.Add(number);
            }

            ItemsThessNumber = 0;

            ItemsThess[0] = new LightItemModelThress();
            ItemsThess[1] = new LightItemModelThress();
            ItemsThess[2] = new LightItemModelThress();

            ImageShebei1 = string.Empty;
            ImageShebei2 = string.Empty;
            ImageShebei3 = string.Empty;

            return;
        }

        void ShowResultDialog(string title,string result)
        {
            Application.Current.MainPage.Dispatcher.DispatchAsync(() =>
            {
                // 你的UI更新代码
                Application.Current.MainPage.DisplayAlert(title, result, "确定");

                if (title == "占卜" && result == "执行成功")
                {
                    var waitingView = new WaitPage();
                    waitingView.model.ImageShebei1 = ImageShebei1;
                    waitingView.model.ImageShebei2 = ImageShebei2;
                    waitingView.model.ImageShebei3 = ImageShebei3;
                    waitingView.model.ImageScaleY1 = ImageScaleY1;
                    waitingView.model.ImageScaleY2 = ImageScaleY2;
                    waitingView.model.ImageScaleY3 = ImageScaleY3;
                    waitingView.model.Describe1 = ItemsThess[0].Describe;
                    waitingView.model.Describe2 = ItemsThess[1].Describe;
                    waitingView.model.Describe3 = ItemsThess[2].Describe;

                    if (ItemsThess[0].Straight == true) waitingView.model.Describe1 += "正位";
                    else waitingView.model.Describe1 += "逆位";

                    if (ItemsThess[1].Straight == true) waitingView.model.Describe2 += "正位";
                    else waitingView.model.Describe2 += "逆位";

                    if (ItemsThess[2].Straight == true) waitingView.model.Describe3 += "正位";
                    else waitingView.model.Describe3 += "逆位";

                    waitingView.model.Anwser = jsontoken.presets2 + jsontoken.data4;

                    Application.Current.MainPage.Navigation.PushModalAsync(waitingView);

                }
            });

        }

        void ShowWaitDialog(bool isshow)
        {
            Application.Current.MainPage.Dispatcher.DispatchAsync(() =>
            {
                // 你的UI更新代码
                // 你的UI更新代码
                if (isshow == true)
                {
                    MainShowB = false;
                    MainShowW = true;
                }
                else
                {
                    MainShowB = true;
                    MainShowW = false;

                    //Application.Current.MainPage.Navigation.PopModalAsync(); // 如果是使用PushModalAsync显示的话
                }
            });

        }

        private string execerror = string.Empty;
        private MyData jsontoken = null;
        private async Task<bool> ExePlcCommand(int command)
        {
            bool Success = false;
            
            //AddRecord("管理线程-请等待,设备回应!", false);
            var timeout = Task.Delay(8000);
            var t = Task.Run(async () =>
            {
                try
                {
                    switch (command)
                    {
                        case 1: RestartXipai(); break;
                        case 2:
                            {
                                //这里我对await 有了另一种理解，也就是说，一个函数如果不加await -- 这个函数里面，不管有没有wait
                                //好像都不会等待，如果比如现在这个情况..
                                //ExePlcCommand为子线程直接调用 -- StartDivination开始我没有await 子线程执行这个函数的时候，直接返回，不管里面怎么去调用，
                                //可是我加上了await, 就等待了。。 很神奇.. 就去等待了..
                                bool result = await StartDivination();
                                if (result == false)
                                {                                    
                                    Success = false;
                                    return;
                                }
                            }
                            break;
                        case 3: break; 
                        case 4: break;
                    }
                    Success = true;
                }
                catch (Exception ex)
                {
                    //总异常:
                    execerror = "工作线程异常:"+ex.Message;
                    //AddRecord("工作线程-执行异常:" + error + "当前指令:" + command.ToString(), true);
                    Success = false;
                    return;
                }
            });
            //这里有个问题，就是主线程现在在这里等着...
            var completedTask = await Task.WhenAny(timeout, t);
            if (completedTask == timeout)
            {
                execerror = "超时";
                Success = false;
                //AddRecord("管理线程-超时,请检查设备是否正常!" + command.ToString(), true);
                return false;
            }

            if (Success)
            {
                execerror = "执行成功";
                return true;
            }
            else
            {                
                return false;
            }
        }

        private async void DoBtnSelectCommandWish(object button)
        {
            //string action = await DisplayActionSheet("Title", "Cancel", null, "Option 1", "Option 2");            
        }

        private void DoReadmeButtonCommand(object button)
        {
            string temp1 = "首先，塔罗牌绝对不是百分百准确的。别听那些吹得天花乱坠的塔罗师忽悠你，说什么“绝对准确”、“包你发财”之类的。命运这东西，" +
               "从来都是掌握在自己手里的。塔罗牌只是给你一个参考，帮你理清思路，而不是直接告诉你未来的走向。" +
               "如果您有什么想法请联系作者:mstwj@163.com或者QQ:393445755";
            if (jsontoken!=null) temp1 = jsontoken.presets1;
            
            Application.Current.MainPage.DisplayAlert("塔罗牌:", temp1, "确定");
        }
        private void DoBtnCommandStart(object button)
        {
            if (ItemsThessNumber != 3)
            {
                Application.Current.MainPage.DisplayAlert("错误", "必须选三张牌", "确定");
                return;
            }

            if (PSelect != null)
            {
                ImageShebei1 = ItemsThess[0].ImageAddress;
                //要反一下.
                if (ItemsThess[0].Straight == false) ImageScaleY1 = -1; else ImageScaleY1 = 1;

                ImageShebei2 = ItemsThess[1].ImageAddress;
                if (ItemsThess[1].Straight == false) ImageScaleY2 = -1; else ImageScaleY2 = 1;

                ImageShebei3 = ItemsThess[2].ImageAddress;
                if (ItemsThess[2].Straight == false) ImageScaleY3 = -1; else ImageScaleY3 = 1;


                //发送给服务器，开始算...
                if (queue.Count > 0) return;
                queue.Enqueue(2);

            } else
            {
                Application.Current.MainPage.DisplayAlert("错误", "必须选一个占卜目的", "确定");
            }
        }

        private void DoEditButtonCommand(object button)
        {
            //重新洗牌..
            if (queue.Count > 0) return;
            queue.Enqueue(1);

        }

        private void DoImageButtonCommand(object button)
        {
            //每个按钮
            //检查还有没有空曹..
            if (ItemsThessNumber == 3)
            {
                Application.Current.MainPage.DisplayAlert("错误", "已选三张牌，无法在选择", "确定");
                return;
            }


            LightItemModel p = button as LightItemModel;
            if (p.IsChecked == true)
            {
                Application.Current.MainPage.DisplayAlert("错误", "本张牌,已被选过", "确定");
                return;
            }

            ItemsThess[ItemsThessNumber].ImageAddress = "my"+p.ItemType.ToString()+".jpeg";
            ItemsThess[ItemsThessNumber].Describe = p.Describe;
            ItemsThess[ItemsThessNumber].ImageBack = p.Value1;
            ItemsThess[ItemsThessNumber].Straight = p.IsOpen;            

            if (ItemsThessNumber == 0) ImageShebei1 = p.Value1;
            if (ItemsThessNumber == 1) ImageShebei2 = p.Value1;
            if (ItemsThessNumber == 2) ImageShebei3 = p.Value1;

            ItemsThessNumber++;

            //设置为选中...
            p.IsChecked = true;
        }

    }
}
