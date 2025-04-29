using System.ComponentModel;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;

    public object CurrentViewModel => _navigationStore.CurrentViewModel;
    
    public bool IsRibbonVisible => !(CurrentViewModel is LoginViewModel);

    public MainViewModel(NavigationStore navigationStore, 
        IAuthenticationService authenticationService, 
        IAuthorizationService authorizationService)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModel = new LoginViewModel(NavigateTo, authenticationService, authorizationService);
        _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
    }
    
    public void NavigateTo(ViewModelBase viewModel)
    {
        _navigationStore.CurrentViewModel = viewModel;
    }

    public void OnCurrentViewModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(IsRibbonVisible));
        }
    }
}