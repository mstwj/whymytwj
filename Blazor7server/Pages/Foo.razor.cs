using System;

namespace Blazor7server.Pages
{
    public partial class Foo
    {
        private List<string> Items { get; } = new List<string>();

        private string BeginTime { get; } = DateTime.Now.ToString();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Items.Add($"Foo:OnInitialized{DateTime.Now}");
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Items.Add($"Foo:OnParametersSet{DateTime.Now}");
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            //这里的确是进来了 -- 可是为什么Items.Add 就看不到呢？非要去手动刷新一次
            //执行StateHasChanged()呢？
            base.OnAfterRender(firstRender);

            Items.Add($"Foo:OnAfterRender{DateTime.Now}");

           // await Task.Delay(500);

         //   if(firstRender)
         //   {
          //      StateHasChanged();
          //  }
        }
    }
}