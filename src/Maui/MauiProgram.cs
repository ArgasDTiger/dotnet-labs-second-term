using Maui.Views;
using Microsoft.Extensions.Logging;
using Maui.Constants;
using Maui.Extensions;
using Maui.Services;
using Maui.ViewModels;

namespace Maui;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<WelcomePage>();
        builder.Services.AddSingleton<ClientsPage>();
        builder.Services.AddSingleton<MoviesPage>();

        builder.Services.AddSingleton<MovieViewModel>();
        builder.Services.AddSingleton<ClientViewModel>();

        builder.Services.AddApiHttpClient<IMoviesService, MoviesService>();
        builder.Services.AddApiHttpClient<IClientsService, ClientsService>();

        return builder.Build();
    }
}