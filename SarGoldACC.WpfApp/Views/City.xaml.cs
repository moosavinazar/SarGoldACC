using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    private readonly CityViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public City(CityViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
        NameBox.AddHandler(CommandManager.PreviewExecutedEvent,
            new ExecutedRoutedEventHandler(CityNameBox_PreviewExecuted), true);
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
        await _viewModel.SaveCity();
        ClearForm();
    }
    private void CityNameBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Paste)
        {
            if (Clipboard.ContainsText())
            {
                string pasteText = Clipboard.GetText();
                if (!Regex.IsMatch(pasteText, @"^.+$"))
                {
                    e.Handled = true;
                }
            }
        }
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        _viewModel.CityName = "";
        NameBox.Focus();
        _viewModel.Clear();
    }
    private async void ClickSaveCity(object sender, RoutedEventArgs e)
    {
        Save();
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