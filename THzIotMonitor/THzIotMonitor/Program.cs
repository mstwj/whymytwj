using Microsoft.EntityFrameworkCore;
using System;
using THzIotPlatform.Data;


var builder = WebApplication.CreateBuilder(args);

// 注册 MySQL 数据库上下文（关键步骤）
builder.Services.AddDbContext<THzIotPlatform.Data.ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 43)) // 替换为你的 MySQL 版本
    ));



// 注册 MySQL 数据库上下文（关键步骤）
builder.Services.AddDbContext<THzIotPlatform.Data.AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 43)) // 替换为你的 MySQL 版本
    ));





// 添加控制器
builder.Services.AddControllersWithViews();

// 关键：注册数据服务（确保这行代码存在）
builder.Services.AddDataServices();

// 添加MVC服务
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 设置 JSON 序列化编码为 UTF-8
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

var app = builder.Build();




app.UseCors("AllowAll");

// 关键：开发环境启用详细错误页（生产环境需关闭）
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 👉 加上这行，显示详细错误
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// 配置HTTP请求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



// 配置默认路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// 确保数据库已创建
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // 确保数据库已创建
        // 可以在这里添加初始数据
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();
