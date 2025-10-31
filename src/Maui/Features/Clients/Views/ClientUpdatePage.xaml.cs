using Maui.Features.Clients.ViewModels;

namespace Maui.Features.Clients.Views;

public sealed partial class ClientUpdatePage
{
    public ClientUpdatePage(ClientUpdateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}