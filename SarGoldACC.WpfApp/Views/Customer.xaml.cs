using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Customer : Window
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    
    private readonly CustomerViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    const int SW_MINIMIZE = 6;
    const int SW_RESTORE = 9;

    
    public Customer(CustomerViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
        _serviceProvider = serviceProvider;
        // هندل کردن Paste به صورت Preview در سطح Command
        CellPhone.AddHandler(CommandManager.PreviewExecutedEvent,
            new ExecutedRoutedEventHandler(CellPhone_PreviewExecuted), true);
        NameBox.AddHandler(CommandManager.PreviewExecutedEvent,
            new ExecutedRoutedEventHandler(NameBox_PreviewExecuted), true);
        Phone.AddHandler(CommandManager.PreviewExecutedEvent,
            new ExecutedRoutedEventHandler(Phone_PreviewExecuted), true);
        IdCode.AddHandler(CommandManager.PreviewExecutedEvent,
            new ExecutedRoutedEventHandler(IdCode_PreviewExecuted), true);
    }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        NameBox.Focus();
    }
    private void CustomerWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
        else if (e.Key == Key.Enter && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && SaveButton.IsEnabled)
        {
            Save();
        }
        else if (e.Key == Key.F5)
        {
            ClearForm();
        }
    }
    private void NameBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(NameBox);
        NameBox.SelectAll();
        // تنظیم زبان فارسی
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
    }
    private void CellPhone_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(CellPhone);
        CellPhone.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
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
    private void StoreNameBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(StoreName);
        StoreName.SelectAll();
        // تنظیم زبان فارسی
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
    }
    private void WeightLimitBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(WeightLimit);
        WeightLimit.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void Weight_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(\d+)?(\.\d{0,3})?$");
    }
    private void WeightLimitBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (WeightLimit.Text == "")
        {
            _viewModel.WeightLimit = 0;
            WeightLimit.Text = "0";
        }
    }
    private void RiyalLimitBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(RiyalLimit);
        RiyalLimit.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void Riyal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(0|\d)$");
    }
    private void RiyalLimitBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (RiyalLimit.Text == "")
        {
            _viewModel.RiyalLimit = 0;
            RiyalLimit.Text = "0";
        }
    }
    private void MoarefBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(Moaref);
        Moaref.SelectAll();
        // تنظیم زبان فارسی
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
    }
    private void EmailBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(Email);
        Email.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void ClickSaveCustomer(object sender, RoutedEventArgs e)
    {
        Save();
    }
    private void ClickClearForm(object sender, RoutedEventArgs e)
    {
        ClearForm();
    }
    private async void Save()
    {
        await _viewModel.SaveCustomer();
        ClearForm();
    }
    private void ClearForm()
    {
        NameBox.Text = "";
        CellPhone.Text = "";
        _viewModel.CityId = 0;
        Phone.Text = "";
        WeightBed.Text = "0";
        WeightBes.Text = "0";
        RiyalBed.Text = "0";
        RiyalBes.Text = "0";
        StoreName.Text = "";
        WeightLimit.Text = "0";
        RiyalLimit.Text = "0";
        IdCode.Text = "";
        Moaref.Text = "";
        Email.Text = "";
        Address.Text = "";
        Description.Text = "";
        _viewModel.BirthDate = null;
        _viewModel.PhotoPreview = null;
        _viewModel.PhotoFileName = "";
        _viewModel.PhotoBytes = null;
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
        
        // فوکوس را از فرم بگیر و بازگردان
        WindowFocusHelper.SimulateFocusLossAndRestore(this);

        CityComboBox.Focus();
        NameBox.Focus();
        _viewModel.Clear();
    }

    private void ChoosePhotoButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;

            // بارگذاری تصویر برای پیش‌نمایش
            var bitmap = new BitmapImage(new Uri(selectedFilePath));
            CustomerViewModel vm = (CustomerViewModel)this.DataContext;
            vm.PhotoPreview = bitmap;

            // خواندن فایل به صورت byte[]
            byte[] fileBytes = File.ReadAllBytes(selectedFilePath);

            // ذخیره در ViewModel
            vm.PhotoBytes = fileBytes;
            vm.PhotoFileName = Path.GetFileName(selectedFilePath);
        }
    }

    private void CityComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null)
        {
            comboBox.IsDropDownOpen = true;
        }
    }
    private void CityComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Insert)
        {
            OpenAddCityWindow();
        }
    }
    private async void ClickAddCity(object sender, RoutedEventArgs e)
    {
        OpenAddCityWindow();
    }
    
    private async void OpenAddCityWindow()
    {
        var cityWindow = _serviceProvider.GetRequiredService<City>();
        cityWindow.Owner = this; // اختیاریه: مشخص می‌کنه پنجره اصلی کیه
        cityWindow.ShowDialog(); // برای مودال بودن، یا از Show() برای غیرمودال
    }

    private void CustomerDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        // CustomerDataGrid.DeleteActionShow = _viewModel.CanAccessCustomerDelete;
        CustomerDataGrid.EditActionShow = _viewModel.CanAccessCustomerEdit;
        
        /*CustomerDataGrid.DeleteAction = async obj =>
        {
            if (obj is CustomerDto customer)
            {
                await _viewModel.DeleteAsync(customer.Id);
            }
        };*/
        
        CustomerDataGrid.EditAction = async obj =>
        {
            if (obj is CustomerDto customer)
            {
                CityComboBox.Text = "";
                await _viewModel.EditAsync(customer.Id);
            }
        };
        
        CustomerDataGrid.ColumnConfigKey = $"CustomerGrid_{_authorizationService.GetCurrentUserIdAsString()}"; // یا هر شناسه خاصی که می‌خواهید
        CustomerDataGrid.SetColumns(
            new DataGridTextColumn { Header = "شناسه", Binding = new Binding("Id"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام", Binding = new Binding("Name"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "موبایل", Binding = new Binding("CellPhone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "تلفن", Binding = new Binding("Phone"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "تاریخ تولد", Binding = new Binding("BirthDate"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "نام فروشگاه", Binding = new Binding("StoreName"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "محدودیت وزن", Binding = new Binding("WeightLimit"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "محدودیت ریال", Binding = new Binding("RiyalLimit"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "کد ملی", Binding = new Binding("IdCode"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "معرف", Binding = new Binding("Moaref"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "ایمیل", Binding = new Binding("Email"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "آدرس", Binding = new Binding("Address"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) },
            new DataGridTextColumn { Header = "توضیحات", Binding = new Binding("Description"), Width = new DataGridLength(3, DataGridLengthUnitType.Star) }
        );
    }
    private void CellPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
    }
    private void CellPhone_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Paste)
        {
            if (Clipboard.ContainsText())
            {
                string pasteText = Clipboard.GetText();
                if (!Regex.IsMatch(pasteText, @"^09\d{9}$"))
                {
                    e.Handled = true;
                }
            }
        }
    }
    private void CellPhone_Loaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(Validation.HasErrorProperty, typeof(TextBox))
            .AddValueChanged(CellPhone, (s, args) =>
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
    private void NameBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Paste)
        {
            if (Clipboard.ContainsText())
            {
                string pasteText = Clipboard.GetText();
                if (!Regex.IsMatch(pasteText, @"^.+$"))
                {
                    e.Handled = true;
                }
            }
        }
    }
    private void NameBox_Loaded(object sender, RoutedEventArgs e)
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
    private void CityComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
        // تمرکز روی ComboBox
        // Keyboard.Focus(CityComboBox);

        // گرفتن TextBox داخلی
        var textBox = GetComboBoxTextBox(CityComboBox);
        if (textBox != null)
        {
            // تنظیم زبان فارسی
            LoadKeyboardLayout("00000429", 1);
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");
            textBox.SelectAll();
        }
    }
    private TextBox GetComboBoxTextBox(ComboBox comboBox)
    {
        return comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
    }
    private void CityComboBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.CityId == 0 || _viewModel.SearchText == "")
        {
            _viewModel.CityId = 1;
            _viewModel.SearchText = "نامعلوم";
        }
    }
    private void IdCode_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(IdCode);
        IdCode.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void IdCode_Loaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(Validation.HasErrorProperty, typeof(TextBox))
            .AddValueChanged(IdCode, (s, args) =>
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
    private void IdCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
    }
    private void IdCode_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Paste)
        {
            if (Clipboard.ContainsText())
            {
                string pasteText = Clipboard.GetText();
                if (!Regex.IsMatch(pasteText, @"^(|\d{10})$"))
                {
                    e.Handled = true;
                }
            }
        }
    }
    private void Phone_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(Phone);
        Phone.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void Phone_Loaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(Validation.HasErrorProperty, typeof(TextBox))
            .AddValueChanged(CellPhone, (s, args) =>
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
    private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^(|\d{1,11})$");
    }
    private void Phone_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command == ApplicationCommands.Paste)
        {
            if (Clipboard.ContainsText())
            {
                string pasteText = Clipboard.GetText();
                if (!Regex.IsMatch(pasteText, @"^(|\d{1,11})$"))
                {
                    e.Handled = true;
                }
            }
        }
    }
}