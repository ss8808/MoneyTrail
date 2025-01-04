using Microsoft.Extensions.Logging;
using MoneyTrail.Data;
using MoneyTrail.Services;
using MudBlazor.Services;

namespace MoneyTrail
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<InMemory>();
            builder.Services.AddScoped<TransactionService>();


            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
