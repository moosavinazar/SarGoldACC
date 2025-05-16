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
    
    public Bank(BankViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }
    private void BankWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private async void ClickSaveBank(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveBank();
        NameBox.Text = "";
        BranchBox.Text = "";
        AccountNumberBox.Text = "";
        CardNumberBox.Text = "";
        IbanBox.Text = "";
        RiyalBes.Text = "";
        RiyalBed.Text = "";
        Description.Text = "";
    }
    
    private void BankDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        BankDataGrid.EditActionShow = _viewModel.CanAccessBankEdit;
        
        BankDataGrid.EditAction = async obj =>
        {
            if (obj is BankDto customer)
            {
                await _viewModel.EditAsync(customer.Id);
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
}