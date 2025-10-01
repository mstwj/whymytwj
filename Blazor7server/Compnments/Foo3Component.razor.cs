using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Blazor7server.Compnments
{
    public partial class Foo3Component 
    {
        [CascadingParameter]
        [NotNull]
        public List<string>? Foos { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Foos.Add("2");
        }
    }
}