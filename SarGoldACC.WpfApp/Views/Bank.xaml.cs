using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Bank;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Bank : Window
{
    private readonly BankViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    public Bank(BankViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
        _serviceProvider = serviceProvider;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        Name.Focus();
        CurrencyComboBox.ServiceProvider = _serviceProvider;
    }
    private void BankWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
        else if (e.Key == Key.Enter && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && SaveButton.IsEnabled)
        {
            Save();
        }
        else if (e.Key == Key.F5)
        {
            ClearForm();
        }
    }
    
    private async void ClickSaveBank(object sender, RoutedEventArgs e)
    {
        Save();
    }
    
    private void BankDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        BankDataGrid.EditActionShow = _viewModel.CanAccessBankEdit;
        
        BankDataGrid.EditAction = async obj =>
        {
            if (obj is BankDto bank)
            {
                await _viewModel.EditAsync(bank.Id);
            }
        };
        
        BankDataGrid.ColumnConfigKey = $"BankGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        BankDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شعبه", Binding = new Binding("Branch"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شماره حساب", Binding = new Binding("AccountNumber"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شماره کارت", Binding = new Binding("CardNumber"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شماره شبا", Binding = new Binding("Iban"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "توضیحات", Binding = new Binding("Description"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void CurrencySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.CurrencyId == 0)
        {
            _viewModel.CurrencyId = 1;
        }
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.Branch = "";
        _viewModel.CurrencyId = 1;
        _viewModel.AccountNumber = "";
        _viewModel.CardNumber = "";
        _viewModel.Iban = "";
        _viewModel.RiyalBed = 0;
        _viewModel.RiyalBes = 0;
        _viewModel.Description = "";
        _viewModel.Clear();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveBank();
        ClearForm();
    }
}