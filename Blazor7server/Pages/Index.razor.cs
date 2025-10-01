using Microsoft.AspNetCore.Components;

namespace Blazor7server.Pages
{
    public partial class Index
    {
        //加了标签，就不是属性了，是参数了...--对外的.. 创建类的地方，会赋值..
        [Parameter]
        public string Name { get; set; }

        private string Value { get; set; } = "123123";

        public override Task SetParametersAsync(ParameterView parameters)
        {
            //最开始这里..
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            //如果这里有个TASK，会2次刷新.. 同步一次，异步一次..
            //这里不要调用JS，为什么，因为页面UI还没有被渲染..
            //第2步：这里...
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            //第一次是系统同步刷新，fistRender就是true
            //如果是滴2次刷新，就是FALSE了..
            //这个是 线缆完成了JS回调来的...
            //这里才表示 页面已经渲染完成了..
            base.OnAfterRender(firstRender);

            StateHasChanged();
        }

        private void OnClick()
        {
            Value = "111111";
        }

    }
}