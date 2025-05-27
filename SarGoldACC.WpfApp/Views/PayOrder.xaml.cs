using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayOrder : Window
{
    private readonly PayOrderViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
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
    private async void ClickAddCustomer(object sender, RoutedEventArgs e)
    {
        var customerWindow = _serviceProvider.GetRequiredService<Customer>();
        customerWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        customerWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }

    private void ClickSavePayOrder(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            CounterpartySideTwoId = (int)(DataContext as PayOrderViewModel).CounterpartyId,
            WeightBed = (DataContext as PayOrderViewModel).WeightBed,
            RiyalBed = (DataContext as PayOrderViewModel).RiyalBed,
            Description = (DataContext as PayOrderViewModel).Description,
            Type = DocumentItemType.ORDER
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }

}