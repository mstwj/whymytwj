namespace Blazor7server.Pages
{
    public partial class Foo5
    {
        public List<Person> people = new()
    {
        { new Person() { Data="100" } },
        { new Person() {Data="100" } },
        { new Person() {Data="100" }  }
    };

        private CancellationTokenSource? cancellationTokenSource { get; set; }

        //开始的时候，创建了..每次更新，都要到这里，第一次的时候，fistrReder是true..
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Task.Run(async () =>
                {
                    cancellationTokenSource = new();
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            await Task.Delay(2000, cancellationTokenSource.Token);

                            //每2秒加一个..
                            people.Add(new Person() { Data = "2132" });

                            //异步刷新.
                            await InvokeAsync(StateHasChanged);
                        }
                        catch (TaskCanceledException)
                        {

                        }
                    }
                });
            }
        }

        public class Person
        {
            public string? Data { get; set; }
        }

        

        //我。。。。 。。。。。我。。。。为什么老师的有个参数，我没有，就总提示我，没有实现接口...
        public void Dispose()
        {
            //if (disposing)
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                }
            }
        }

        //void IDisposable.Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}