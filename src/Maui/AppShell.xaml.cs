using Maui.Features.Clients.Views;
using Maui.Features.Movies.Views;

namespace Maui;

public sealed partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ClientsDetailsPage), typeof(ClientsDetailsPage));
        Routing.RegisterRoute(nameof(MovieUpdatePage), typeof(MovieUpdatePage));
        Routing.RegisterRoute(nameof(ClientUpdatePage), typeof(ClientUpdatePage));
    }
}