using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Currency : Window
{
    private readonly CurrencyViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public Currency(CurrencyViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }
    
    private void CurrencyWindow_KeyDown(object sender, KeyEventArgs e)
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
    
    private async void ClickSaveCurrency(object sender, RoutedEventArgs e)
    {
        Save();
    }
    
    private void CurrencyDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        CurrencyDataGrid.DeleteActionShow = _viewModel.CanAccessCurrencyDelete;
        CurrencyDataGrid.EditActionShow = _viewModel.CanAccessCurrencyEdit;
        
        CurrencyDataGrid.DeleteAction = async obj =>
        {
            if (obj is CurrencyDto currency)
            {
                await _viewModel.DeleteAsync(currency.Id);
            }
        };
        
        CurrencyDataGrid.EditAction = async obj =>
        {
            if (obj is CurrencyDto currency)
            {
                await _viewModel.EditAsync(currency.Id);
            }
        };
        
        CurrencyDataGrid.ColumnConfigKey = $"CurrencyGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        CurrencyDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "عنوان", Binding = new Binding("Label"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.Label = "";
        _viewModel.Clear();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveCurrency();
        ClearForm();
    }
}