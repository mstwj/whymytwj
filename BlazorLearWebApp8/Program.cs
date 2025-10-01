using BlazorLearWebApp8.Components;
using BlazorLearWebApp8.Entity;
using BootstrapBlazor.Components;
using FreeSql;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

//1.现在.NET8 - 选择SERVER模式，为什么，因为使用人不多。一般在网络内.
//2.现在选择GLOD 为什么，这个是全局渲染模式，因为JS回调，这里很方便很多都是系统给你渲染..
//3.现在按照. BootstrapBlazor 8.06 版本，必须安装 BootstrapBlazor FONAT。8.06 这个是图标库..
//4. @using BootstrapBlazor.Components 写一下，看我是加哪里的..
//5.一定要删除原来的 BOOTSTRAS因为，冲突了.. 把原来的替换我的，看我怎么搞的..
//6.JS脚本，看我怎么放的，不要问为什么..
//7. builder.Services.AddBootstrapBlazor(); -- 看我..
//8. BootstrapBlazorRoot 包一下，看我。。。
//9 app.css 很多东西删除掉..
//10. 可以进行消息分发...()服务器给所有的客户都发消息，我就没看--以后如果需要了，我在看..

//10数据库这一块，我还是使用我自己的，Freesql是什么，怎么使用，我现在还不直达..
//11.项目模板..  开始的时候，我们费好多力气，又这又那个的..

//FreeSql是一个功能强大且灵活的.NET对象关系映射（ORM）组件，
//https://github.com/dotnetcore/FreeSql/tree/master --- 例子..
//看我怎么安装数据库的，现在数据库就安装OK了...

var builder = WebApplication.CreateBuilder(args);

//制定是.Sqlite 为什么，因为我只安装了SQLLITE..
//开发是TRUE，生产是FALSE; -- 真的创建了一个document.db..
//这里太牛逼了，这里直接就出数据库了。也出了表，4个表，都是空表..
//为什么我是数据保存失败呢？
IFreeSql fsql = new FreeSql.FreeSqlBuilder()
  .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=document.db")
  .UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
  .Build(); //be sure to define as singleton mode

BaseEntity.Initialization(fsql, null);

//添加MVC的路由方式..--第一，添加Connections这个必须这样写..
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBootstrapBlazor();

//必须添加啊
builder.Services.AddScoped(typeof(IDataService<>), typeof(FreesqlDataService<>));
//添加COOIKE支持..
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
{
    //现在的情况是主页，进不去了，开始就跳转到LOGIN了，你必须设置用户名和密码了。
    //如果，COOK是已经登入的状态才能进去了..
    //如果COOIKE有是数据，开始就跳转到LOGIN页面，然后自动登入..
    config.LoginPath = "/Login";

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

//app.UseAuthentication();
//app.UseAuthorization();


//MVC添加默认MVC路由..
app.MapDefaultControllerRoute();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
