using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Branch : Window
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    private readonly BranchViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    
    public Branch(BranchViewModel viewModel, IAuthorizationService authorizationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authorizationService = authorizationService;
        DataContext = _viewModel;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        
    }

    private void BranchWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private void BranchNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SaveButton.Focus();
            e.Handled = true;
        }
    }

    private async void ClickSaveBranch(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveBranch();
        BranchNameBox.Text = "";
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private void ClearForm()
    {
        BranchNameBox.Text = "";
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
        
        // فوکوس را از فرم بگیر و بازگردان
        WindowFocusHelper.SimulateFocusLossAndRestore(this);

        BranchNameBox.Focus();
        _viewModel.Clear();
    }
    private void BranchNameBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(BranchNameBox);
        BranchNameBox.SelectAll();
        // تنظیم زبان فارسی
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
    }
    
    private void BranchNameBox_Loaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(Validation.HasErrorProperty, typeof(TextBox))
            .AddValueChanged(BranchNameBox, (s, args) =>
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
    
    private void BranchNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^.+$");
    }
    private void BranchDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        BranchDataGrid.DeleteActionShow = _viewModel.CanAccessBranchDelete;
        BranchDataGrid.EditActionShow = _viewModel.CanAccessBranchEdit;
        
        BranchDataGrid.DeleteAction = async obj =>
        {
            if (obj is BranchDto branch)
            {
                await _viewModel.DeleteAsync(branch.Id);
            }
        };
        
        BranchDataGrid.EditAction = async obj =>
        {
            if (obj is BranchDto branch)
            {
                await _viewModel.EditAsync(branch.Id);
            }
        };
        
        BranchDataGrid.ColumnConfigKey = $"BranchGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        BranchDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "شعبه", Binding = new Binding("Name"), Width = new DataGridLength(5, DataGridLengthUnitType.Star) }
        );
    }
}