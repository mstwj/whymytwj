using Blazorise;
using Blazorise.Bootstrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using webapiabing;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Charts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 注册 Blazorise 服务
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddBootstrap5Components()
    .AddChartJs();  // 注册图表服务


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
