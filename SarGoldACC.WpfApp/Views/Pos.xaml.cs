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
    
    public Pos(PosViewModel viewModel, IAuthorizationService authorizationService)
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
    private void PosWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private async void ClickSavePos(object sender, RoutedEventArgs e)
    {
        await _viewModel.SavePos();
        NameBox.Text = "";
        CodeBox.Text = "";
        RiyalBes.Text = "";
        RiyalBed.Text = "";
        Description.Text = "";
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
}