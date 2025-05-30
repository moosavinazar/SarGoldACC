using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class RcvMade : Window
{
    private readonly RcvMadeViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    
    public RcvMade(RcvMadeViewModel viewModel, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
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
    
    private async void ClickAddMadeSubCategory(object sender, RoutedEventArgs e)
    {
        var madeSubCategoryWindow = _serviceProvider.GetRequiredService<MadeSubCategory>();
        madeSubCategoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        madeSubCategoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
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
            WeightBes = (DataContext as RcvMadeViewModel).Weight,
            Weight750 = (DataContext as RcvMadeViewModel).Weight750,
            Ayar = (DataContext as RcvMadeViewModel).Ayar,
            Barcode = (DataContext as RcvMadeViewModel).Barcode,
            Name = (DataContext as RcvMadeViewModel).Name,
            Photo = (DataContext as RcvMadeViewModel).Photo,
            OjratR = (DataContext as RcvMadeViewModel).OjratR,
            OjratP = (DataContext as RcvMadeViewModel).OjratP,
            MadeSubCategoryId = (DataContext as RcvMadeViewModel).MadeSubCategoryId,
            BoxId = (DataContext as RcvMadeViewModel).BoxId,
            Description = (DataContext as RcvMadeViewModel).Description,
            Type = DocumentItemType.MADE
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
}