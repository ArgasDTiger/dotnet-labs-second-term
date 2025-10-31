using Maui.Features.Movies.ViewModels;

namespace Maui.Features.Movies.Views;

[QueryProperty(nameof(LoadMovies), "LoadMovies")]
public sealed partial class MoviesPage
{
    // This property will be set by Shell navigation
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
            // Check if we're appearing for the first time OR
            // if we were navigated back to with the 'LoadMovies' flag
            if (vm.Movies.Count == 0 || LoadMovies)
            {
                vm.LoadMoviesCommand.ExecuteAsync(null);
                
                // Reset the flag
                LoadMovies = false;
            }
        }
    }
}