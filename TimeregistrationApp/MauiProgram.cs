using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TimeregistrationApp.Views;
using TimeregistrationApp.Repositories;
using TimeregistrationApp.Services;
using TimeregistrationApp.ViewModels;

namespace TimeregistrationApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


        builder.Services.AddSingleton<TimeRegistrationSQLiteRepository>();
        builder.Services.AddSingleton<TimeService>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();

        builder.Services.AddSingleton<TijdRegistratieListView>();
        builder.Services.AddSingleton<TijdRegistratieListViewModel>();

        builder.Services.AddSingleton<TijdRegistratieDetailView>();
        builder.Services.AddSingleton<TijdRegistratieDetailViewModel>();

        builder.Services.AddSingleton<MaandTotalenListView>();
        builder.Services.AddSingleton<MaandTotalenListViewModel>();

        return builder.Build();
    }
}
