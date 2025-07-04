using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Box : Window
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    private readonly BoxViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    public Box(BoxViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvide)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
        _serviceProvider = serviceProvide;
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        NameBox.Focus();
        BranchSelectorControl.ServiceProvider = _serviceProvider;
    }

    private async void ClickSave(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveAsync();
        NameBox.Text = "";
        WeightBox.Text = "";
    }
    private void NameBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(NameBox);
        NameBox.SelectAll();
        // تنظیم زبان فارسی
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
    }
    
    private void BranchNameBox_Loaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(Validation.HasErrorProperty, typeof(TextBox))
            .AddValueChanged(NameBox, (s, args) =>
            {
                var tb = s as TextBox;
                if (Validation.GetHasError(tb))
                {
                    ToolTip tt = new ToolTip
                    {
                        Content = Validation.GetErrors(tb)[0].ErrorContent,
                        IsOpen = true,
                        PlacementTarget = tb,
                        StaysOpen = true,
                        Placement = System.Windows.Controls.Primitives.PlacementMode.Right
                    };
                    tb.ToolTip = tt;
                }
                else
                {
                    if (tb.ToolTip is ToolTip ttip)
                        ttip.IsOpen = false;
                }
            });
    }
    
    private void NameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^.+$");
    }
    private void DataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        DataGrid.DeleteActionShow = _viewModel.CanAccessBoxDelete;
        DataGrid.EditActionShow = _viewModel.CanAccessBoxEdit;
        
        DataGrid.DeleteAction = async obj =>
        {
            if (obj is BoxDto box)
            {
                await _viewModel.DeleteAsync(box.Id);
            }
        };
        
        DataGrid.EditAction = async obj =>
        {
            if (obj is BoxDto box)
            {
                await _viewModel.EditAsync(box.Id);
            }
        };
        
        DataGrid.ColumnConfigKey = $"BoxGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        DataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "وزن", Binding = new Binding("Weight"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نوع", Binding = new Binding("Type"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شعبه", Binding = new Binding("BranchName"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
}