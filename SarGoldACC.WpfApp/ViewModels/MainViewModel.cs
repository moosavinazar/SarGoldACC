using System.ComponentModel;
using SarGoldACC.Core.Services.Auth;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;

    private object _currentViewModel;
    public object CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsRibbonVisible));
        }
    }
    
    public bool IsRibbonVisible => !(CurrentViewModel is LoginViewModel);

    public MainViewModel()
    {
        _navigationStore = new NavigationStore();
        var authService = new AuthenticationService(App.DbContextFactory);
        _navigationStore.CurrentViewModel = new LoginViewModel(NavigateTo, authService);

        _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
    }
    
    private void NavigateTo(ViewModelBase viewModel)
    {
        _navigationStore.CurrentViewModel = viewModel;
    }

    private void OnCurrentViewModelChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
            OnPropertyChanged(nameof(CurrentViewModel));
    }
}