using Blazor8Server.Components;

namespace Blazor8Server
{
    //appseting 配置环境..
    //Pages 具体页面..
    //._Imageport 全局的一个东西...
    //App.razor 根主键..
    //Routes 路由.
    //wwwroot 公共的一些东西...
    //Layout 就是公共布局.
    //Properties 设置环境..
    //BLAZOR 是什么，是MVC的分支，原来是 一个短连接，BLAZOR是一个长连接，为什么要使用BLAZOR呢？ 
    //如果我不想 局部刷新，怎么办，只能使用 BLAZOR了.. -- 为什么，如果要解决局部刷新的问题，原来是JS AJOX来解决这个问题。
    //现在是使用BLAZOR来解决这个问题了... 或者你使用VUE + 后端，这样的方法，这个就不是我们讨论的范围了..
    //MVC的交互，一定是通过JS,AJAX来解决的。
    //MVVM就不是这样的了... 这就是使用BLAZOR的原因，他的开发效率肯定比MVC高...
    //MVC就是BLAZOR SERVER，就这样去理解，只是 BLAZOR SERVER是长连接，底层可以理解为TCP SINGR连接..--都是在服务端计算，把差异，传送过来，修改DOM -- 
    //如果你退出，就会销毁很多东西，比如 TCP连接呀，资源呀等什么东西...


    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
