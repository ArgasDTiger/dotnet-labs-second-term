using Maui.Features.Movies.ViewModels;

namespace Maui.Features.Movies.Views;

public partial class MovieUpdatePage
{
    public MovieUpdatePage(MovieUpdateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
