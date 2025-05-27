using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class RcvMelted : Window
{
    private readonly RcvMeltedViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    
    public RcvMelted(RcvMeltedViewModel viewModel, IServiceProvider serviceProvide)
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
    
    private async void ClickAddLaboratory(object sender, RoutedEventArgs e)
    {
        var laboratoryWindow = _serviceProvider.GetRequiredService<Laboratory>();
        laboratoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        laboratoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
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
            WeightBes = (DataContext as RcvMeltedViewModel).Weight,
            Ayar = (DataContext as RcvMeltedViewModel).Ayar,
            Certain = (DataContext as RcvMeltedViewModel).Certain,
            Ang = (DataContext as RcvMeltedViewModel).Ang,
            LaboratoryId = (DataContext as RcvMeltedViewModel).LaboratoryId,
            BoxId = (DataContext as RcvMeltedViewModel).BoxId,
            Description = (DataContext as RcvMeltedViewModel).Description,
            Type = DocumentItemType.MELTED
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
}