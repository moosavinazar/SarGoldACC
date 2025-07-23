using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayOrder : Window
{
    private readonly PayOrderViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    public long SideOneCounterpartyId { get; set; }
    public PayOrder(PayOrderViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _serviceProvider = serviceProvider;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        _viewModel.SideOneCounterPartyId = SideOneCounterpartyId;
        await _viewModel.ReloadAllAsync();
        CounterpartyComboBox.ServiceProvider = _serviceProvider;
    }
    private void PayOrderWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    private bool _isFirstActivation = true;
    private async void Window_Activated(object sender, EventArgs e)
    {
        if (_isFirstActivation)
        {
            _isFirstActivation = false;
            return; // بار اول هنگام Load انجام شده است
        }

        await ReloadListsAsync();
    }
    private async Task ReloadListsAsync()
    {
        await _viewModel.ReloadAllAsync();
    }
    private void ClickSavePayOrder(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            CounterpartySideTwoId = (int)(DataContext as PayOrderViewModel).CounterpartyId,
            WeightBed = (DataContext as PayOrderViewModel).WeightBed,
            Weight = (DataContext as PayOrderViewModel).WeightBed,
            RiyalBed = (DataContext as PayOrderViewModel).RiyalBed,
            Description = (DataContext as PayOrderViewModel).Description,
            Type = DocumentItemType.ORDER,
            TypeTitle = "حواله پرداخت"
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
}