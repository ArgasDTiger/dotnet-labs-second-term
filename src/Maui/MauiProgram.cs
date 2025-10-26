using Maui.Features.Clients.Services;
using Maui.Features.Clients.Views;
using Maui.Views;
using Microsoft.Extensions.Logging;
using Maui.Features.Movies.Services;
using Maui.Features.Movies.Views;
using Maui.Features.Clients.ViewModels;
using Maui.Features.Movies.ViewModels;
using Maui.Shared.Extensions;

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