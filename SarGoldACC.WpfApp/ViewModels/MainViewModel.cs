﻿using System.ComponentModel;
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

    public bool CanAccessDocumentButton => _authorizationService.HasPermission("Document.Order") ||
                                           _authorizationService.HasPermission("Document.Melted") ||
                                           _authorizationService.HasPermission("Document.Misc") ||
                                           _authorizationService.HasPermission("Document.Made") ||
                                           _authorizationService.HasPermission("Document.Coin");
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
    
    public bool CanAccessCityButton => _authorizationService.HasPermission("City.View") ||
                                         _authorizationService.HasPermission("City.Create") ||
                                         _authorizationService.HasPermission("City.Edit") ||
                                         _authorizationService.HasPermission("City.Delete");
    
    public bool CanAccessCustomerButton => _authorizationService.HasPermission("Customer.View") ||
                                       _authorizationService.HasPermission("Customer.Create") ||
                                       _authorizationService.HasPermission("Customer.Edit") ||
                                       _authorizationService.HasPermission("Customer.Delete");
    
    public bool CanAccessBankButton => _authorizationService.HasPermission("Bank.View") ||
                                           _authorizationService.HasPermission("Bank.Create") ||
                                           _authorizationService.HasPermission("Bank.Edit") ||
                                           _authorizationService.HasPermission("Bank.Delete");
    
    public bool CanAccessPosButton => _authorizationService.HasPermission("Pos.View") ||
                                       _authorizationService.HasPermission("Pos.Create") ||
                                       _authorizationService.HasPermission("Pos.Edit") ||
                                       _authorizationService.HasPermission("Pos.Delete");
    
    public bool CanAccessCurrencyButton => _authorizationService.HasPermission("Currency.View") ||
                                           _authorizationService.HasPermission("Currency.Create") ||
                                           _authorizationService.HasPermission("Currency.Edit") ||
                                           _authorizationService.HasPermission("Currency.Delete");
    
    public bool CanAccessCashButton => _authorizationService.HasPermission("Cash.View") ||
                                           _authorizationService.HasPermission("Cash.Create") ||
                                           _authorizationService.HasPermission("Cash.Edit") ||
                                           _authorizationService.HasPermission("Cash.Delete");
    
    public bool CanAccessLaboratoryButton => _authorizationService.HasPermission("Laboratory.View") ||
                                       _authorizationService.HasPermission("Laboratory.Create") ||
                                       _authorizationService.HasPermission("Laboratory.Edit") ||
                                       _authorizationService.HasPermission("Laboratory.Delete");
    
    public bool CanAccessIncomeButton => _authorizationService.HasPermission("Income.View") ||
                                             _authorizationService.HasPermission("Income.Create") ||
                                             _authorizationService.HasPermission("Income.Edit") ||
                                             _authorizationService.HasPermission("Income.Delete");
    
    public bool CanAccessCostButton => _authorizationService.HasPermission("Cost.View") ||
                                         _authorizationService.HasPermission("Cost.Create") ||
                                         _authorizationService.HasPermission("Cost.Edit") ||
                                         _authorizationService.HasPermission("Cost.Delete");
    
    public bool CanAccessBoxButton => _authorizationService.HasPermission("Box.View") ||
                                       _authorizationService.HasPermission("Box.Create") ||
                                       _authorizationService.HasPermission("Box.Edit") ||
                                       _authorizationService.HasPermission("Box.Delete");
    
    public bool CanAccessMadeSubCategoryButton => _authorizationService.HasPermission("MadeSubCategory.View") ||
                                              _authorizationService.HasPermission("MadeSubCategory.Create") ||
                                              _authorizationService.HasPermission("MadeSubCategory.Edit") ||
                                              _authorizationService.HasPermission("MadeSubCategory.Delete");
    
    public bool CanAccessCoinCategoryButton => _authorizationService.HasPermission("CoinCategory.View") ||
                                                  _authorizationService.HasPermission("CoinCategory.Create") ||
                                                  _authorizationService.HasPermission("CoinCategory.Edit") ||
                                                  _authorizationService.HasPermission("CoinCategory.Delete");
    
    public bool CanAccessSettingButton => _authorizationService.HasPermission("Setting.View");

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
            OnPropertyChanged(nameof(CanAccessCityButton));
            OnPropertyChanged(nameof(CanAccessCustomerButton));
            OnPropertyChanged(nameof(CanAccessBankButton));
            OnPropertyChanged(nameof(CanAccessPosButton));
            OnPropertyChanged(nameof(CanAccessCurrencyButton));
            OnPropertyChanged(nameof(CanAccessCashButton));
            OnPropertyChanged(nameof(CanAccessLaboratoryButton));
            OnPropertyChanged(nameof(CanAccessIncomeButton));
            OnPropertyChanged(nameof(CanAccessCostButton));
            OnPropertyChanged(nameof(CanAccessBoxButton));
            OnPropertyChanged(nameof(CanAccessMadeSubCategoryButton));
            OnPropertyChanged(nameof(CanAccessCoinCategoryButton));
            OnPropertyChanged(nameof(CanAccessDocumentButton));
            OnPropertyChanged(nameof(CanAccessSettingButton));
        }
    }
}