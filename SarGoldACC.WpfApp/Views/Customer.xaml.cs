using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Customer : Window
{
    [DllImport("user32.dll")]
    static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
    
    private readonly CustomerViewModel _viewModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServiceProvider _serviceProvider;
    
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
    }
    private void NameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            CellPhone.Focus();
            e.Handled = true;
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
    private void CellPhoneBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Phone.Focus();
            e.Handled = true;
        }
    }
    private void CellPhone_GotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        Keyboard.Focus(CellPhone);
        CellPhone.SelectAll();
        // تنظیم زبان انگلیسی
        LoadKeyboardLayout("00000409", 1); // 00000409 = English (United States)
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
    }
    private void PhoneBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            WeightBed.Focus();
            e.Handled = true;
        }
    }
    private void WeightBedBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            WeightBes.Focus();
            e.Handled = true;
        }
    }
    private void WeightBesBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            RiyalBed.Focus();
            e.Handled = true;
        }
    }
    private void RiyalBedBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            RiyalBes.Focus();
            e.Handled = true;
        }
    }
    private void RiyalBesBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            BirthDate.Focus();
            e.Handled = true;
        }
    }
    private void BirthDateBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            StoreName.Focus();
            e.Handled = true;
        }
    }
    private void StoreNameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            WeightLimit.Focus();
            e.Handled = true;
        }
    }
    private void WeightLimitBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            RiyalLimit.Focus();
            e.Handled = true;
        }
    }
    private void RiyalLimitBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            IdCode.Focus();
            e.Handled = true;
        }
    }
    private void IdCodeBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Moaref.Focus();
            e.Handled = true;
        }
    }
    private void MoarefBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Email.Focus();
            e.Handled = true;
        }
    }
    private void EmailBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Address.Focus();
            e.Handled = true;
        }
    }
    private void AddressBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Description.Focus();
            e.Handled = true;
        }
    }
    private void DescriptionBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Address.Focus();
            e.Handled = true;
        }
    }
    private async void ClickSaveCustomer(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveCustomer();
        NameBox.Text = "";
        CellPhone.Text = "";
        _viewModel.CityId = 0;
        Phone.Text = "";
        WeightBed.Text = "";
        WeightBes.Text = "";
        RiyalBed.Text = "";
        RiyalBes.Text = "";
        BirthDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        StoreName.Text = "";
        WeightLimit.Text = "";
        RiyalLimit.Text = "";
        IdCode.Text = "";
        Moaref.Text = "";
        Email.Text = "";
        Address.Text = "";
        Description.Text = "";
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
            .AddValueChanged(Phone, (s, args) =>
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
        TextBox textBox = sender as TextBox;
        string currentText = textBox.Text;

        // محل قرارگیری کرسر را در نظر بگیریم
        int selectionStart = textBox.SelectionStart;
        int selectionLength = textBox.SelectionLength;

        // متن نهایی بعد از اضافه شدن کاراکتر جدید
        string newText = currentText.Remove(selectionStart, selectionLength).Insert(selectionStart, e.Text);

        // اگر خالی است، اجازه بده
        if (string.IsNullOrEmpty(newText))
        {
            e.Handled = false;
            return;
        }

        // بررسی: فقط عدد و حداکثر 11 رقم
        e.Handled = !Regex.IsMatch(newText, @"^\d{1,11}$");
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
        TextBox textBox = sender as TextBox;
        string currentText = textBox.Text;

        // محل قرارگیری کرسر را در نظر بگیریم
        int selectionStart = textBox.SelectionStart;
        int selectionLength = textBox.SelectionLength;

        // متن نهایی بعد از اضافه شدن کاراکتر جدید
        string newText = currentText.Remove(selectionStart, selectionLength).Insert(selectionStart, e.Text);

        // اگر خالی است، اجازه بده
        if (string.IsNullOrEmpty(newText))
        {
            e.Handled = false;
            return;
        }

        // فقط اعداد مجازند و باید دقیقاً 10 رقم باشند
        if (Regex.IsMatch(newText, @"^\d{0,10}$"))
        {
            e.Handled = false; // فعلاً اجازه بده چون ممکنه کاربر هنوز کامل نکرده
        }
        else
        {
            e.Handled = true; // ورودی نامعتبر
        }
    }
}