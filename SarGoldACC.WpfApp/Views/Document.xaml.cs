using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Repositories;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Control;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Document : Window
{
    private readonly DocumentViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    
    public Document(DocumentViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
        _serviceProvider = serviceProvider;
    }
    
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // گرفتن رزولوشن صفحه اصلی
        var screenWidth = SystemParameters.PrimaryScreenWidth;
        var screenHeight = SystemParameters.PrimaryScreenHeight;

        // محاسبه عرض و ارتفاع مورد نظر (مثلاً 80% عرض و 80% ارتفاع)
        this.Width = screenWidth * 0.8;
        this.Height = screenHeight * 0.8;

        // مرکز کردن پنجره (اگر لازم بود دستی)
        this.Left = (screenWidth - this.Width) / 2;
        this.Top = (screenHeight - this.Height) / 2;
        
        // تمرکز روی بخش متنی ComboBox
        await Task.Delay(100); // صبر کوتاه برای اطمینان از آماده بودن UI
        CounterpartySelectorControl.ServiceProvider = _serviceProvider;
    }
    private void DocumentWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }

        if (e.Key is Key.OemPlus or Key.Add)
        {
            RcvOredrButton.Visibility = Visibility.Visible;
            PayOredrButton.Visibility = Visibility.Hidden;
            RcvMeltedButton.Visibility = Visibility.Visible;
            PayMeltedButton.Visibility = Visibility.Hidden;
            RcvMiscButton.Visibility = Visibility.Visible;
            PayMiscButton.Visibility = Visibility.Hidden;
            RcvMadeButton.Visibility = Visibility.Visible;
            PayMadeButton.Visibility = Visibility.Hidden;
            RcvCoinButton.Visibility = Visibility.Visible;
            PayCoinButton.Visibility = Visibility.Hidden;
        }
        
        if (e.Key is Key.OemMinus or Key.Subtract)
        {
            RcvOredrButton.Visibility = Visibility.Hidden;
            PayOredrButton.Visibility = Visibility.Visible;
            RcvMeltedButton.Visibility = Visibility.Hidden;
            PayMeltedButton.Visibility = Visibility.Visible;
            RcvMiscButton.Visibility = Visibility.Hidden;
            PayMiscButton.Visibility = Visibility.Visible;
            RcvMadeButton.Visibility = Visibility.Hidden;
            PayMadeButton.Visibility = Visibility.Visible;
            RcvCoinButton.Visibility = Visibility.Hidden;
            PayCoinButton.Visibility = Visibility.Visible;
        }
    }

    private async void ClickAddCustomer(object sender, RoutedEventArgs e)
    {
        OpenAddCustomerWindow();
    }

    private async void ClickReport(object sender, RoutedEventArgs e)
    {
    }
    
    private async void ClickTenDeal(object sender, RoutedEventArgs e)
    {
    }
    
    private void ClickRcvOrder(object sender, RoutedEventArgs e)
    {
        var rcvOrderWindow = _serviceProvider.GetRequiredService<RcvOrder>();
        rcvOrderWindow.Owner = this;
        rcvOrderWindow.SideOneCounterpartyId = _viewModel.CounterpartyId;
        bool? result = rcvOrderWindow.ShowDialog();
        if (result == true && rcvOrderWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvOrderWindow.ResultItem);
        }
    }
    
    private void ClickPayOrder(object sender, RoutedEventArgs e)
    {
        var payOrderWindow = _serviceProvider.GetRequiredService<PayOrder>();
        payOrderWindow.Owner = this;
        payOrderWindow.SideOneCounterpartyId = _viewModel.CounterpartyId;
        bool? result = payOrderWindow.ShowDialog();
        if (result == true && payOrderWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payOrderWindow.ResultItem);
        }
    }
    
    private void ClickRcvMelted(object sender, RoutedEventArgs e)
    {
        var rcvMeltedWindow = _serviceProvider.GetRequiredService<RcvMelted>();
        rcvMeltedWindow.Owner = this;

        bool? result = rcvMeltedWindow.ShowDialog();
        if (result == true && rcvMeltedWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvMeltedWindow.ResultItem);
        }
    }
    
    private void ClickPayMelted(object sender, RoutedEventArgs e)
    {
        var payMeltedWindow = _serviceProvider.GetRequiredService<PayMelted>();
        payMeltedWindow.Owner = this;

        bool? result = payMeltedWindow.ShowDialog();
        if (result == true && payMeltedWindow.ResultItems != null && payMeltedWindow.ResultItems.Any())
        {
            foreach (var item in payMeltedWindow.ResultItems)
            {
                _viewModel.DocumentItems.Add(item);
            }
        }
    }

    private void ClickRcvMisc(object sender, RoutedEventArgs e)
    {
        var rcvMiscWindow = _serviceProvider.GetRequiredService<RcvMisc>();
        rcvMiscWindow.Owner = this;

        bool? result = rcvMiscWindow.ShowDialog();
        if (result == true && rcvMiscWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvMiscWindow.ResultItem);
        }
    }

    private void ClickPayMisc(object sender, RoutedEventArgs e)
    {
        var payMiscWindow = _serviceProvider.GetRequiredService<PayMisc>();
        payMiscWindow.Owner = this;

        bool? result = payMiscWindow.ShowDialog();
        if (result == true && payMiscWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payMiscWindow.ResultItem);
        }
    }
    
    private void ClickRcvMade(object sender, RoutedEventArgs e)
    {
        var rcvMiscWindow = _serviceProvider.GetRequiredService<RcvMade>();
        rcvMiscWindow.Owner = this;

        bool? result = rcvMiscWindow.ShowDialog();
        if (result == true && rcvMiscWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvMiscWindow.ResultItem);
        }
    }
    
    private void ClickPayMade(object sender, RoutedEventArgs e)
    {
        var payMadeWindow = _serviceProvider.GetRequiredService<PayMade>();
        payMadeWindow.Owner = this;

        bool? result = payMadeWindow.ShowDialog();
        if (result == true && payMadeWindow.ResultItems != null && payMadeWindow.ResultItems.Any())
        {
            foreach (var item in payMadeWindow.ResultItems)
            {
                _viewModel.DocumentItems.Add(item);
            }
        }
    }
    
    private void ClickRcvCoin(object sender, RoutedEventArgs e)
    {
        var rcvCoinWindow = _serviceProvider.GetRequiredService<RcvCoin>();
        rcvCoinWindow.Owner = this;

        bool? result = rcvCoinWindow.ShowDialog();
        if (result == true && rcvCoinWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(rcvCoinWindow.ResultItem);
        }
    }
    
    private void ClickPayCoin(object sender, RoutedEventArgs e)
    {
        var payCoinWindow = _serviceProvider.GetRequiredService<PayCoin>();
        payCoinWindow.Owner = this;

        bool? result = payCoinWindow.ShowDialog();
        if (result == true && payCoinWindow.ResultItem != null)
        {
            _viewModel.DocumentItems.Add(payCoinWindow.ResultItem);
        }
    }

    private async void ClickSaveDocument(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveDocument(DocumentType.FINAL);
    }
    
    private async void ClickSaveTemporaryDocument(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveDocument(DocumentType.TEMPORARY);
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
        // جلوگیری از وارد شدن از طریق کلیدهای فیزیکی + و -
        if (e.Key == Key.OemPlus || e.Key == Key.Add || e.Key == Key.OemMinus || e.Key == Key.Subtract)
        {
            e.Handled = true;
            DocumentWindow_KeyDown(sender, e);
        }
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

}