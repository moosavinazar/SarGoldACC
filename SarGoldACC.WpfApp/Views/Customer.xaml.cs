using System.Windows;
using System.Windows.Input;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Customer : Window
{
    private readonly CustomerViewModel _viewModel;
    
    public Customer(CustomerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
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
            ReyalLimit.Focus();
            e.Handled = true;
        }
    }
    private void ReyalLimitBox_KeyDown(object sender, KeyEventArgs e)
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
    }

    private void CustomerDataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        
    }
}