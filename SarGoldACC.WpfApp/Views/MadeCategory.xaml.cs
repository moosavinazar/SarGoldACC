﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class MadeCategory : Window
{
    private readonly MadeCategoryViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public MadeCategory(MadeCategoryViewModel viewModel, IAuthorizationService authorizationService)
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
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveMadeCategory();
        ClearForm();
    }
    private async void ClickSaveMadeCategory(object sender, RoutedEventArgs e)
    {
        Save();
    }

    private void MadeCategoryDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        MadeCategoryDataGrid.DeleteActionShow = _viewModel.CanAccessMadeCategoryDelete;
        MadeCategoryDataGrid.EditActionShow = _viewModel.CanAccessMadeCategoryEdit;
        
        MadeCategoryDataGrid.DeleteAction = async obj =>
        {
            if (obj is MadeCategoryDto madeCategory)
            {
                await _viewModel.DeleteAsync(madeCategory.Id);
            }
        };
        
        MadeCategoryDataGrid.EditAction = async obj =>
        {
            if (obj is MadeCategoryDto madeCategory)
            {
                await _viewModel.EditAsync(madeCategory.Id);
            }
        };
        
        MadeCategoryDataGrid.ColumnConfigKey = $"MadeCategoryGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        MadeCategoryDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "دسته بندی", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        _viewModel.MadeCategoryName = "";
        MadeCategoryNameBox.Focus();
        _viewModel.Clear();
    }
}