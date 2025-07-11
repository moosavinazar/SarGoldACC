using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Box : Window
{
    private readonly BoxViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    public Box(BoxViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
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
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        NameBox.Focus();
        BranchSelectorControl.ServiceProvider = _serviceProvider;
    }

    private async void ClickSave(object sender, RoutedEventArgs e)
    {
        Save();
    }
    private void DataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        DataGrid.DeleteActionShow = _viewModel.CanAccessBoxDelete;
        DataGrid.EditActionShow = _viewModel.CanAccessBoxEdit;
        
        DataGrid.DeleteAction = async obj =>
        {
            if (obj is BoxDto box)
            {
                await _viewModel.DeleteAsync(box.Id);
            }
        };
        
        DataGrid.EditAction = async obj =>
        {
            if (obj is BoxDto box)
            {
                await _viewModel.EditAsync(box.Id);
            }
        };
        
        DataGrid.ColumnConfigKey = $"BoxGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        DataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "وزن", Binding = new Binding("Weight"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نوع", Binding = new Binding("Type"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شعبه", Binding = new Binding("BranchName"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void BranchSelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.BranchId == 0)
        {
            _viewModel.BranchId = 1;
        }
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.BranchId = 1;
        _viewModel.Weight = 0;
        _viewModel.Type = 0;
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveAsync();
        ClearForm();
    }
}