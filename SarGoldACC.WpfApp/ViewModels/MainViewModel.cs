using System.ComponentModel;
using SarGoldACC.Core.Services;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;

    public object CurrentViewModel => _navigationStore.CurrentViewModel;
    
    public bool IsRibbonVisible => !(CurrentViewModel is LoginViewModel);

    public MainViewModel(AuthenticationService authenticationService, AuthorizationService authorizationService)
    {
        _navigationStore = new NavigationStore
        {
            CurrentViewModel = new LoginViewModel(NavigateTo, authenticationService, authorizationService)
        };
        _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
    }
    
    private void NavigateTo(ViewModelBase viewModel)
    {
        _navigationStore.CurrentViewModel = viewModel;
    }

    private void OnCurrentViewModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(IsRibbonVisible));
        }
    }
}