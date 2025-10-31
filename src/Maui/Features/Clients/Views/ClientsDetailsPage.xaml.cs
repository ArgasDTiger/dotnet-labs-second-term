using Maui.Features.Clients.ViewModels;

namespace Maui.Features.Clients.Views;

public partial class ClientsDetailsPage
{
    public ClientsDetailsPage(ClientDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}