using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui.Shared.ViewModels;

public abstract partial class ViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isLoading;
    
    [ObservableProperty]
    private string _title = string.Empty;
}