using Microsoft.AspNetCore.Components;

namespace Blazor7server.Compnments
{
    public partial class DummyInput
    {
        //Value这个数据出父组件过来...
        [Parameter]
        public string? Value { get; set; }

        
        [Parameter]
        public EventCallback<string?> ValueChanged { get; set; }

        //ValueString 是 我自己绑定的数据... Edit改变了，我就改变了...
        private string? ValueString
        {
            get { return Value; }
            set
            {
                if (Value != value)
                {
                    Value = value;
                    if (ValueChanged.HasDelegate)
                    {
                        //通知页面发生改变...
                        ValueChanged.InvokeAsync(Value);
                    }
                }                
            }
        }
    }
}