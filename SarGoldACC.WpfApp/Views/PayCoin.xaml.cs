using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayCoin : Window
{
    private readonly CoinViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    
    public PayCoin(CoinViewModel viewModel, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        CoinCategoryComboBox.ServiceProvider = _serviceProvider;
        BoxComboBox.ServiceProvider = _serviceProvider;
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
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
    
    private async void ClickAddCoinCategory(object sender, RoutedEventArgs e)
    {
        var coinCategoryWindow = _serviceProvider.GetRequiredService<CoinCategory>();
        coinCategoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        coinCategoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private async void ClickAddBox(object sender, RoutedEventArgs e)
    {
        var boxWindow = _serviceProvider.GetRequiredService<Box>();
        boxWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        boxWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }

    private void ClickSave(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            WeightBed = (DataContext as CoinViewModel).Weight,
            Weight750 = (DataContext as CoinViewModel).Weight750,
            Ayar = (DataContext as CoinViewModel).Ayar,
            Name = (DataContext as CoinViewModel).Name,
            OjratR = (DataContext as CoinViewModel).OjratR,
            OjratP = (DataContext as CoinViewModel).OjratP,
            CoinCategoryId = (DataContext as CoinViewModel).CoinCategoryId,
            BoxId = (DataContext as CoinViewModel).BoxId,
            Description = (DataContext as CoinViewModel).Description,
            Type = DocumentItemType.COIN
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
    private void CoinCategorySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.CoinCategoryId == 0)
        {
            _viewModel.CoinCategoryId = 1;
        }
    }
    private void BoxSelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.BoxId == 0)
        {
            _viewModel.BoxId = 1;
        }
    }
}