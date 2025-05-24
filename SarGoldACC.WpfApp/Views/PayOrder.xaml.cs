using System.Windows;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayOrder : Window
{
    private readonly PayOrderViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public DocumentItemDto ResultItem { get; private set; }

    
    public PayOrder(PayOrderViewModel viewModel, IAuthorizationService authorizationService)
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
    private void PayOrderWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private async void ClickAddCustomer(object sender, RoutedEventArgs e)
    {
    }

    private void ClickSavePayOrder(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            CounterpartySideTwoId = (int)(DataContext as PayOrderViewModel).CustomerId,
            WeightBed = (DataContext as PayOrderViewModel).WeightBed,
            RiyalBed = (DataContext as PayOrderViewModel).RiyalBed,
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }

}