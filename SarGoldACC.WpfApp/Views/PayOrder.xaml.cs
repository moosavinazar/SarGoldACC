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
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
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
    }
    private void CounterpartyComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null)
        {
            comboBox.IsDropDownOpen = true;
        }
    }
    private void CounterpartyComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Insert)
        {
            OpenAddCustomerWindow();
        }
    }
    private async void OpenAddCustomerWindow()
    {
        var customerWindow = _serviceProvider.GetRequiredService<Customer>();
        customerWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        customerWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
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
    private void RiyalBedBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(RiyalBed);
        RiyalBed.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void RiyalBedBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (RiyalBed.Text == "")
        {
            _viewModel.RiyalBed = 0;
            RiyalBed.Text = "0";
        }
    }
    private void Riyal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(0|\d)$");
    }
    private void WeightBedBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(WeightBed);
        WeightBed.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void WeightBedBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (WeightBed.Text == "")
        {
            _viewModel.WeightBed = 0;
            WeightBed.Text = "0";
        }
    }
    private void Weight_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(\d+)?(\.\d{0,3})?$");
    }
}