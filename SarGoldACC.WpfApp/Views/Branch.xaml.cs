using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Branch : Window
{
    private readonly BranchViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public Branch(BranchViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
    }
    private void BranchWindow_KeyDown(object sender, KeyEventArgs e)
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
    private async void ClickSaveBranch(object sender, RoutedEventArgs e)
    {
        Save();
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private void ClearForm()
    {
        _viewModel.BranchName = "";
        BranchNameBox.Focus();
        _viewModel.Clear();
    }
    private void BranchDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        BranchDataGrid.DeleteActionShow = _viewModel.CanAccessBranchDelete;
        BranchDataGrid.EditActionShow = _viewModel.CanAccessBranchEdit;
        
        BranchDataGrid.DeleteAction = async obj =>
        {
            if (obj is BranchDto branch)
            {
                await _viewModel.DeleteAsync(branch.Id);
            }
        };
        
        BranchDataGrid.EditAction = async obj =>
        {
            if (obj is BranchDto branch)
            {
                await _viewModel.EditAsync(branch.Id);
            }
        };
        
        BranchDataGrid.ColumnConfigKey = $"BranchGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        BranchDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شعبه", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveBranch();
        ClearForm();
    }
}