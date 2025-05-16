using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class City : Window
{
    private readonly CityViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public City(CityViewModel viewModel, IAuthorizationService authorizationService)
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

    private void CityWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private void CityNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SaveButton.Focus();
            e.Handled = true;
        }
    }

    private async void ClickSaveCity(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveCity();
        CityNameBox.Text = "";
    }

    private void CityDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        CityDataGrid.DeleteActionShow = _viewModel.CanAccessCityDelete;
        CityDataGrid.EditActionShow = _viewModel.CanAccessCityEdit;
        
        CityDataGrid.DeleteAction = async obj =>
        {
            if (obj is CityDto city)
            {
                await _viewModel.DeleteAsync(city.Id);
            }
        };
        
        CityDataGrid.EditAction = async obj =>
        {
            if (obj is CityDto city)
            {
                await _viewModel.EditAsync(city.Id);
            }
        };
        
        CityDataGrid.ColumnConfigKey = $"CityGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        CityDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شهر", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
}