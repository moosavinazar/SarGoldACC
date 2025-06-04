using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        UsernameBox.Focus();

    }
    
    private void UsernameBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            PasswordBox.Focus();
            e.Handled = true;
        }
    }

    private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var vm = DataContext as LoginViewModel;
            if (vm?.LoginCommand.CanExecute(null) == true)
            {
                vm.LoginCommand.Execute(null);
            }
            e.Handled = true;
        }
    }
}