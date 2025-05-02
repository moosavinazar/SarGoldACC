using System.ComponentModel;
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
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabMain;
                break;
            case Key.D2:
            case Key.NumPad2:
                if (MainRibbon.SelectedTabItem == TabBaseInfo)
                {
                    OpenGroupWindow();
                    break;
                }
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabProducts;
                break;
            case Key.D3:
            case Key.NumPad3:
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabBaseInfo;
                break;
            case Key.D4:
            case Key.NumPad4:
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabReport;
                break;
            case Key.D5:
            case Key.NumPad5:
                MainRibbon.IsMinimized = false;
                MainRibbon.SelectedTabItem = TabSetting;
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
    
}