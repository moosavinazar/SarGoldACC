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
    
    public Customer(CustomerViewModel viewModel, IAuthorizationService authorizationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _authorizationService = authorizationService;
        _serviceProvider = serviceProvider;
    }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
        Name.Focus();
        CitySelectorControl.ServiceProvider = _serviceProvider;
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
        if (!_viewModel.CanSave) return;
        await _viewModel.SaveCustomer();
        ClearForm();
    }
    private void ClearForm()
    {
        _viewModel.Name = "";
        _viewModel.CellPhone = "";
        _viewModel.CityId = 1;
        _viewModel.Phone = "";
        _viewModel.WeightBed = 0;
        _viewModel.WeightBes = 0;
        _viewModel.RiyalBed = 0;
        _viewModel.RiyalBes = 0;
        _viewModel.StoreName = "";
        _viewModel.WeightLimit = 0;
        _viewModel.RiyalLimit = 0;
        _viewModel.IdCode = "";
        _viewModel.Moaref = "";
        _viewModel.Email = "";
        _viewModel.Address = "";
        _viewModel.Description = "";
        _viewModel.BirthDate = null;
        _viewModel.PhotoPreview = null;
        _viewModel.PhotoFileName = "";
        _viewModel.PhotoBytes = null;
        LoadKeyboardLayout("00000429", 1); // 00000429 = Persian
        InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("fa-IR");

        // CityComboBox.Focus();
        Name.Focus();
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
                // CityComboBox.Text = "";
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
    private void CitySelectorControl_LostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_viewModel.CityId == 0)
        {
            _viewModel.CityId = 1;
        }
    }
}