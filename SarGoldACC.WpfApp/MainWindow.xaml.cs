﻿using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Fluent;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.WpfApp.ViewModels;
using SarGoldACC.WpfApp.Views;

namespace SarGoldACC.WpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : RibbonWindow
{
    private readonly IServiceProvider _serviceProvider;
    public MainWindow(MainViewModel mainViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        
        DataContext = mainViewModel;
        
        Loaded += (s, e) =>
        {
            if (MainRibbon != null)
            {
                MainRibbon.IsMinimized = true;
                MainRibbon.SelectedTabItem = null; // هیچ تبی انتخاب نش
            }
            if (DataContext is MainViewModel vm)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
                
            }
        };
        
     
    }
    
    
    
    private void RibbonWindow_KeyDown(object sender, KeyEventArgs e)
    {
        // بررسی دقیق‌تر کنترل‌هایی که باید از تغییر تب جلوگیری کنن
        var focusedElement = Keyboard.FocusedElement as FrameworkElement;

        if (focusedElement is 
            System.Windows.Controls.TextBox or 
            System.Windows.Controls.PasswordBox or 
            System.Windows.Controls.ComboBox or 
            System.Windows.Controls.Primitives.TextBoxBase or
            System.Windows.Controls.Primitives.Selector)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.D1:
            case Key.NumPad1:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && UserGroup.Visibility == Visibility.Visible)
                {
                    OpenUserWindow();
                    break;
                }
                if (MainRibbon.SelectedTabItem == TabMain && Document.Visibility == Visibility.Visible)
                {
                    OpenDocumentWindow();
                    break;
                }
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabMain;
                break;
            case Key.D2:
            case Key.NumPad2:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && UserGroup.Visibility == Visibility.Visible)
                {
                    OpenGroupWindow();
                    break;
                }
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabProducts;
                break;
            case Key.D3:
            case Key.NumPad3:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && Branch.Visibility == Visibility.Visible)
                {
                    OpenBranchWindow();
                    break;
                }
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabBaseInfo;
                break;
            case Key.D4:
            case Key.NumPad4:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && City.Visibility == Visibility.Visible)
                {
                    OpenCityWindow();
                    break;
                }
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabReport;
                break;
            case Key.D5:
            case Key.NumPad5:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && City.Visibility == Visibility.Visible)
                {
                    OpenCashWindow();
                    break;
                }
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabSetting;
                break;
            case Key.D6:
            case Key.NumPad6:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && City.Visibility == Visibility.Visible)
                {
                    OpenCustomerWindow();
                    break;
                }
                break;
            case Key.D8:
            case Key.NumPad8:
                if (MainRibbon.SelectedTabItem == TabBaseInfo && City.Visibility == Visibility.Visible)
                {
                    OpenBankWindow();
                    break;
                }
                break;
            case Key.Escape:
                MainRibbon.SelectedTabItem = null;
                MainRibbon.IsMinimized = true;
                break;
        }
    }
    
    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.CurrentViewModel))
        {
            if (DataContext is MainViewModel vm)
            {
                if (vm.CurrentViewModel is HomeViewModel)
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
        }
    }
    
    private void OpenGroupWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenGroupWindow();
    }

    private void OpenGroupWindow()
    {
        var groupWindow = _serviceProvider.GetRequiredService<Group>();
        groupWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        groupWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenBranchWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenBranchWindow();
    }
    
    private void OpenBranchWindow()
    {
        var branchWindow = _serviceProvider.GetRequiredService<Branch>();
        branchWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        branchWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenUserWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenUserWindow();
    }
    
    private void OpenUserWindow()
    {
        var userWindow = _serviceProvider.GetRequiredService<User>();
        userWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        userWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenCityWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenCityWindow();
    }
    
    private void OpenCityWindow()
    {
        var cityWindow = _serviceProvider.GetRequiredService<City>();
        cityWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        cityWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenCustomerWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenCustomerWindow();
    }
    
    private void OpenCustomerWindow()
    {
        var customerWindow = _serviceProvider.GetRequiredService<Customer>();
        customerWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        customerWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenBankWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenBankWindow();
    }
    
    private void OpenBankWindow()
    {
        var bankWindow = _serviceProvider.GetRequiredService<Bank>();
        bankWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        bankWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenPosWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenPosWindow();
    }
    
    private void OpenPosWindow()
    {
        var bankWindow = _serviceProvider.GetRequiredService<Pos>();
        bankWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        bankWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenCurrencyWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenCurrencyWindow();
    }
    
    private void OpenCurrencyWindow()
    {
        var currencyWindow = _serviceProvider.GetRequiredService<Currency>();
        currencyWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        currencyWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenCashWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenCashWindow();
    }
    
    private void OpenCashWindow()
    {
        var cashWindow = _serviceProvider.GetRequiredService<Cash>();
        cashWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        cashWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenLaboratoryWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenLaboratoryWindow();
    }
    
    private void OpenLaboratoryWindow()
    {
        var laboratoryWindow = _serviceProvider.GetRequiredService<Laboratory>();
        laboratoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        laboratoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenIncomeWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenIncomeWindow();
    }
    
    private void OpenIncomeWindow()
    {
        var incomeWindow = _serviceProvider.GetRequiredService<Income>();
        incomeWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        incomeWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenCostWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenCostWindow();
    }
    
    private void OpenCostWindow()
    {
        var costWindow = _serviceProvider.GetRequiredService<Cost>();
        costWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        costWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenDocumentWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenDocumentWindow();
    }

    private void OpenDocumentWindow()
    {
        var documentWindow = _serviceProvider.GetRequiredService<Document>();
        documentWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        documentWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenBoxWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenBoxWindow();
    }

    private void OpenBoxWindow()
    {
        var boxWindow = _serviceProvider.GetRequiredService<Box>();
        boxWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        boxWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenMadeSubCategoryWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenMadeSubCategoryWindow();
    }

    private void OpenMadeSubCategoryWindow()
    {
        var madeSubCategoryWindow = _serviceProvider.GetRequiredService<MadeSubCategory>();
        madeSubCategoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        madeSubCategoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenCoinCategoryWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenCoinCategoryWindow();
    }

    private void OpenCoinCategoryWindow()
    {
        var coinCategoryWindow = _serviceProvider.GetRequiredService<CoinCategory>();
        coinCategoryWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        coinCategoryWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
    private void OpenSettingWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OpenSettingWindow();
    }

    private void OpenSettingWindow()
    {
        var setingWindow = _serviceProvider.GetRequiredService<Setting>();
        setingWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        setingWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }
    
}