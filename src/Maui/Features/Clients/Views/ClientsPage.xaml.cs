using Maui.Features.Clients.ViewModels;

namespace Maui.Features.Clients.Views;

[QueryProperty(nameof(LoadClients), nameof(LoadClients))]
public sealed partial class ClientsPage
{
    public bool LoadClients { get; set; }
    
    public ClientsPage(ClientViewModel clientViewModel)
    {
        InitializeComponent();
        BindingContext = clientViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ClientViewModel vm)
        {
            if (vm.Clients.Count == 0 || LoadClients)
            {
                vm.LoadClientsCommand.ExecuteAsync(null);
                LoadClients = false;
            }
        }
    }
}