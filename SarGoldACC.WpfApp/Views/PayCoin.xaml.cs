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
        await ReloadListsAsync();
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
        await _viewModel.ReloadAllPayAsync();
    }
    private void ClickSave(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            WeightBed = (DataContext as CoinViewModel).Weight,
            Weight = (DataContext as CoinViewModel).Weight,
            Weight750 = (DataContext as CoinViewModel).Weight750,
            Ayar = (DataContext as CoinViewModel).Ayar,
            Count = (DataContext as CoinViewModel).Count,
            Name = (DataContext as CoinViewModel).Name,
            OjratR = (DataContext as CoinViewModel).OjratR,
            OjratP = (DataContext as CoinViewModel).OjratP,
            CoinCategoryId = (DataContext as CoinViewModel).PayCoinCategoryId,
            BoxId = (DataContext as CoinViewModel).BoxId,
            Description = (DataContext as CoinViewModel).Description,
            Type = DocumentItemType.COIN,
            TypeTitle = "پرداخت سکه"
            // مقداردهی بقیه فیلدهای لازم
        };
        this.DialogResult = true;
        this.Close();
    }
    private void CoinCategorySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.PayCoinCategoryId == 0)
        {
            _viewModel.PayCoinCategoryId = 1;
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