using Maui.Features.Movies.ViewModels;

namespace Maui.Features.Movies.Views;

[QueryProperty(nameof(LoadMovies), nameof(LoadMovies))]
public sealed partial class MoviesPage
{
    public bool LoadMovies { get; set; }
    
    public MoviesPage(MovieViewModel movieViewModel)
    {
        InitializeComponent();
        BindingContext = movieViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MovieViewModel vm)
        {
            if (vm.Movies.Count == 0 || LoadMovies)
            {
                vm.LoadMoviesCommand.ExecuteAsync(null);
                LoadMovies = false;
            }
        }
    }
}