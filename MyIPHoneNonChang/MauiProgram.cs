using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using MyIPHoneNonChang.ViewModels;
using MyIPHoneNonChang.ViewModels.Dialog;
using MyIPHoneNonChang.Views;
using MyIPHoneNonChang.Views.Dialog;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MyIPHoneNonChang
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLiveCharts()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("iconfont.ttf", "Iconfont");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransientWithShellRoute<FarmView, FramViewModel>("farm");

            builder.Services.AddTransientPopup<SettingsView, SettingsViewModel>();
            builder.Services.AddTransientPopup<DataView, DataViewModel>();

            return builder.Build();
        }
    }
}
