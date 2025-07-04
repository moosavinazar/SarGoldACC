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

public partial class PayMisc : Window
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
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
        Ayar.Focus();
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
        OpenAddBoxWindow();
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
    private void BoxComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null)
        {
            comboBox.IsDropDownOpen = true;
        }
    }
    private void BoxComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Insert)
        {
            OpenAddBoxWindow();
        }
    }
    private async void OpenAddBoxWindow()
    {
        var boxWindow = _serviceProvider.GetRequiredService<Box>();
        boxWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        boxWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    private void AyarBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(Ayar);
        Ayar.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void AyarBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (Ayar.Text == "")
        {
            _viewModel.Ayar = 750;
            Ayar.Text = "750";
        }
    }
    private void Ayar_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(0|\d)$");
    }
    private void WeightBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(WeightBox);
        WeightBox.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void WeightBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (string.IsNullOrWhiteSpace(WeightBox.Text))
        {
            _viewModel.Weight = 0;
            WeightBox.Text = "0";
        }
        else
        {
            if (double.TryParse(WeightBox.Text, out var weight))
            {
                _viewModel.Weight = weight;
            }
        }

        if (_viewModel.Ayar == 0) _viewModel.Ayar = 750; // جلوگیری از تقسیم بر صفر

        _viewModel.Weight750 = (_viewModel.Weight * _viewModel.Ayar) / 750;
        Weight750.Text = _viewModel.Weight750?.ToString("0.###");
    }
    private void Weight_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(\d+)?(\.\d{0,3})?$");
    }
}