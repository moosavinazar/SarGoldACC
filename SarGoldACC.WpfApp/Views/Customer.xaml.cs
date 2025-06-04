using System.IO;
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
    private void CellPhoneBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Phone.Focus();
            e.Handled = true;
        }
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
}