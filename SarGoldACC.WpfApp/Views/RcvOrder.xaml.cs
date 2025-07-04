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

public partial class RcvOrder : Window
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    private readonly RcvOrderViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public DocumentItemDto ResultItem { get; private set; }
    public long SideOneCounterpartyId { get; set; }
    public RcvOrder(RcvOrderViewModel viewModel, IServiceProvider serviceProvider)
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
        CounterpartySelectorControl.ServiceProvider = _serviceProvider;
    }
    private void RcvOrderWindow_KeyDown(object sender, KeyEventArgs e)
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
    private void ClickSaveRcvOrder(object sender, RoutedEventArgs e)
    {
        ResultItem = new DocumentItemDto
        {
            CounterpartySideTwoId = (int)(DataContext as RcvOrderViewModel).CounterpartyId,
            WeightBes = (DataContext as RcvOrderViewModel).WeightBes,
            RiyalBes = (DataContext as RcvOrderViewModel).RiyalBes,
            Description = (DataContext as RcvOrderViewModel).Description,
            Type = DocumentItemType.ORDER
            // مقداردهی بقیه فیلدهای لازم
        };

        this.DialogResult = true;
        this.Close();
    }
    private void WeightBesBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(WeightBes);
        WeightBes.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void WeightBesBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (WeightBes.Text == "")
        {
            _viewModel.WeightBes = 0;
            WeightBes.Text = "0";
        }
    }
    private void Weight_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(\d+)?(\.\d{0,3})?$");
    }
    private void RiyalBesBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(RiyalBes);
        RiyalBes.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void RiyalBesBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (RiyalBes.Text == "")
        {
            _viewModel.RiyalBes = 0;
            RiyalBes.Text = "0";
        }
    }
    private void Riyal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(0|\d)$");
    }
}