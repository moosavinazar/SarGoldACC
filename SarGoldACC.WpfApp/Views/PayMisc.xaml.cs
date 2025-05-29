using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class PayMisc : Window
{
    private readonly MiscViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    
    public PayMisc(MiscViewModel viewModel, IServiceProvider serviceProvide)
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
            WeightBed = (DataContext as MiscViewModel).Weight,
            Weight750 = (DataContext as MiscViewModel).Weight750,
            Ayar = (DataContext as MiscViewModel).Ayar,
            Certain = (DataContext as MiscViewModel).Certain,
            BoxId = (DataContext as MiscViewModel).BoxId,
            Description = (DataContext as MiscViewModel).Description,
            Type = DocumentItemType.MISC
            // مقداردهی بقیه فیلدهای لازم
        };
        this.DialogResult = true;
        this.Close();
    }
}