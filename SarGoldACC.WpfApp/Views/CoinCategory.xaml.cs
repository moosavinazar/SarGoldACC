﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.CoinCategory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class CoinCategory : Window
{
    private readonly CoinCategoryViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public CoinCategory(CoinCategoryViewModel viewModel, 
        IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
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

    private async void ClickSaveCoinCategory(object sender, RoutedEventArgs e)
    {
        Save();
    }

    private void CoinCategoryDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        CoinCategoryDataGrid.DeleteActionShow = _viewModel.CanAccessCoinCategoryDelete;
        CoinCategoryDataGrid.EditActionShow = _viewModel.CanAccessCoinCategoryEdit;
        
        CoinCategoryDataGrid.DeleteAction = async obj =>
        {
            if (obj is CoinCategoryDto coinCategory)
            {
                await _viewModel.DeleteAsync(coinCategory.Id);
            }
        };
        
        CoinCategoryDataGrid.EditAction = async obj =>
        {
            if (obj is CoinCategoryDto coinCategory)
            {
                await _viewModel.EditAsync(coinCategory.Id);
            }
        };
        
        CoinCategoryDataGrid.ColumnConfigKey = $"CoinCategoryGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        CoinCategoryDataGrid.SetColumns(
            new DataGridTextColumn() { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "دسته بندی", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "عیار", Binding = new Binding("Ayar"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "وزن", Binding = new Binding("Weight"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "۷۵۰", Binding = new Binding("Weight750"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        NameBox.Focus();
        ClearForm();
    }

    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.Ayar = 750;
        _viewModel.Weight = 0;
        _viewModel.Weight750 = 0;
        _viewModel.Clear();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveCoinCategory();
        ClearForm();
    }
}