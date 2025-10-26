using Maui.Features.Movies.ViewModels;

namespace Maui.Features.Movies.Views;

public sealed partial class MoviesPage
{
    public MoviesPage(MovieViewModel movieViewModel)
    {
        InitializeComponent();
        BindingContext = movieViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MovieViewModel { Movies.Count: 0 } vm)
        {
            vm.LoadMoviesCommand.ExecuteAsync(null);
        }
    }
}