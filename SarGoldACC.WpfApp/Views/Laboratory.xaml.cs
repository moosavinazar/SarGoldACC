﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Laboratory : Window
{
    private readonly LaboratoryViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    public Laboratory(LaboratoryViewModel viewModel, IAuthorizationService authorizationService)
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
    private void LaboratoryWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    private async void ClickSaveLaboratory(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveLaboratory();
        NameBox.Text = "";
        CellPhone.Text = "";
        IVRPhone.Text = "";
        _viewModel.CityId = 0;
        Phone.Text = "";
        WeightBed.Text = "";
        WeightBes.Text = "";
        RiyalBed.Text = "";
        RiyalBes.Text = "";
        Description.Text = "";
    }
    private void LaboratoryDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        LaboratoryDataGrid.EditActionShow = _viewModel.CanAccessLaboratoryEdit;

        LaboratoryDataGrid.EditAction = async obj =>
        {
            if (obj is LaboratoryDto laboratory)
            {
                await _viewModel.EditAsync(laboratory.Id);
            }
        };
        
        LaboratoryDataGrid.ColumnConfigKey = $"LaboratoryGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        LaboratoryDataGrid.SetColumns(
            new DataGridTextColumn() { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "تلفن", Binding = new Binding("Phone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "موبایل", Binding = new Binding("CellPhone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "تلفن گویا", Binding = new Binding("IVRPhone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "توضیحات", Binding = new Binding("Description"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
}