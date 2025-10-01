using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Blazor7server.Compnments
{
    //他自己不渲染，他自己没有UI
    public class Foo2Component : ComponentBase
    {
        [CascadingParameter]
        [NotNull]
        public List<string>? Foos { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Foos.Add("1");
        }
    }
}
