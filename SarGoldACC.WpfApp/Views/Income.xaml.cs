using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Income;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Income : Window
{
    private readonly IncomeViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    public Income(IncomeViewModel viewModel, IAuthorizationService authorizationService)
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
    private void IncomeWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    private async void ClickSaveIncome(object sender, RoutedEventArgs e)
    {
        Save();
    }
    private void IncomeDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        IncomeDataGrid.EditActionShow = _viewModel.CanAccessIncomeEdit;
        
        IncomeDataGrid.EditAction = async obj =>
        {
            if (obj is IncomeDto cost)
            {
                await _viewModel.EditAsync(cost.Id);
            }
        };
        
        IncomeDataGrid.ColumnConfigKey = $"IncomeGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        IncomeDataGrid.SetColumns(
            new DataGridTextColumn() { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "عنوان", Binding = new Binding("Label"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "توضیحات", Binding = new Binding("Description"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
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
        _viewModel.RiyalBed = 0;
        _viewModel.RiyalBes = 0;
        _viewModel.Description = "";
        _viewModel.Clear();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveIncome();
        ClearForm();
    }
}