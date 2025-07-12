using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Pos;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Pos : Window
{
    private readonly PosViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    public Pos(PosViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
        _serviceProvider = serviceProvider;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        BankComboBox.ServiceProvider = _serviceProvider;
    }
    private void PosWindow_KeyDown(object sender, KeyEventArgs e)
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
    
    private async void ClickSavePos(object sender, RoutedEventArgs e)
    {
        Save();
    }
    
    private void PosDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        PosDataGrid.EditActionShow = _viewModel.CanAccessPosEdit;
        
        PosDataGrid.EditAction = async obj =>
        {
            if (obj is PosDto pos)
            {
                await _viewModel.EditAsync(pos.Id);
            }
        };
        
        PosDataGrid.ColumnConfigKey = $"PosGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        PosDataGrid.SetColumns(
            new DataGridTextColumn() { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "کد", Binding = new Binding("Code"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "توضیحات", Binding = new Binding("Description"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
    private void BankSelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.BankId == 0)
        {
            _viewModel.BankId = 1;
        }
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private async void Save()
    {
        if (!_viewModel.CanSave) return;
        await _viewModel.SavePos();
        ClearForm();
    }
    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.BankId = 1;
        _viewModel.Code = "";
        _viewModel.RiyalBed = 0;
        _viewModel.RiyalBes = 0;
        _viewModel.Description = "";
        _viewModel.Clear();
    }
}