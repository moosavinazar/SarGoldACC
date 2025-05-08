using System.ComponentModel;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;

    private readonly IAuthorizationService _authorizationService;

    public object CurrentViewModel => _navigationStore.CurrentViewModel;
    
    public bool IsRibbonVisible => !(CurrentViewModel is LoginViewModel);

    public bool CanAccessGroupButton => _authorizationService.HasPermission("Group.View") ||
                                        _authorizationService.HasPermission("Group.Create") ||
                                        _authorizationService.HasPermission("Group.Edit") ||
                                        _authorizationService.HasPermission("Group.Delete");
    public bool CanAccessBranchButton => _authorizationService.HasPermission("Branch.View") ||
                                        _authorizationService.HasPermission("Branch.Create") ||
                                        _authorizationService.HasPermission("Branch.Edit") ||
                                        _authorizationService.HasPermission("Branch.Delete");
    
    public bool CanAccessUserButton => _authorizationService.HasPermission("User.View") ||
                                         _authorizationService.HasPermission("User.Create") ||
                                         _authorizationService.HasPermission("User.Edit") ||
                                         _authorizationService.HasPermission("User.Delete");

    public MainViewModel(NavigationStore navigationStore, 
        IAuthenticationService authenticationService, 
        IAuthorizationService authorizationService)
    {
        _navigationStore = navigationStore;
        _authorizationService = authorizationService;
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
            OnPropertyChanged(nameof(CanAccessGroupButton));
            OnPropertyChanged(nameof(CanAccessBranchButton));
            OnPropertyChanged(nameof(CanAccessUserButton));
        }
    }
}