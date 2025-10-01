using Microsoft.AspNetCore.Components;

namespace Blazor7server.Compnments
{
    public partial class Dummy
    {
        [Parameter]
        public string? Icon{ get; set; }

        
        [Parameter]
        public int? Value { get; set; }

        [Parameter] 
        public string? Name { get; set; }



        private string ClassMy = "btn btn-success"; 
        //Task就是异步的
        //也可以是同步的 VOID
        private  Task OnClick()
        {
            System.Console.WriteLine("button click");
            if (ClassMy == "btn btn-success")
                ClassMy = "btn btn-primary";
            else
                ClassMy = "btn btn-success";
            return Task.CompletedTask;
        }

        private void OnClick2()
        {
            System.Console.WriteLine("button click 2");
            
        }
    }
}