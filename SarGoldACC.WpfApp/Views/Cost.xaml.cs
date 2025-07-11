using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Cost;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Cost : Window
{
    private readonly CostViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    public Cost(CostViewModel viewModel, IAuthorizationService authorizationService)
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
    private void CostWindow_KeyDown(object sender, KeyEventArgs e)
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
    private async void ClickSaveCost(object sender, RoutedEventArgs e)
    {
        Save();
    }
    private void CostDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        CostDataGrid.EditActionShow = _viewModel.CanAccessCostEdit;
        
        CostDataGrid.EditAction = async obj =>
        {
            if (obj is CostDto cost)
            {
                await _viewModel.EditAsync(cost.Id);
            }
        };
        
        CostDataGrid.ColumnConfigKey = $"CostGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        CostDataGrid.SetColumns(
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
        await _viewModel.SaveCost();
        ClearForm();
    }
}