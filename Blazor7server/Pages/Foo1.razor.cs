using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor7server.Pages
{
    public partial class Foo1
    {
        //CascadingValue 组件通过 Value 属性提供一个值，子组件可以通过 [CascadingParameter] 特性获取这个值
        private List<string> Foos { get; } = new();

        [Inject]
        [NotNull]
        private IJSRuntime? JSRuntime {  get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private void Test()
        {
            //地址假跳..false,真的是true
            NavigationManager.NavigateTo("/demo",true);
        }
    }
}