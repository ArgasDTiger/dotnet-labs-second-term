using Maui.Features.Clients.ViewModels;

namespace Maui.Features.Clients.Views;

public partial class ClientsPage
{
    public ClientsPage(ClientViewModel clientViewModel)
    {
        InitializeComponent();
        BindingContext = clientViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ClientViewModel { Clients.Count: 0 } vm)
        {
            vm.LoadClientsCommand.ExecuteAsync(null);
        }
    }
}