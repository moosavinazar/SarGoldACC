using System.Windows.Input;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _username;
    private string _errorMessage;
    private readonly Action<ViewModelBase> _navigateTo;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(Action<ViewModelBase> navigateTo)
    {
        _navigateTo = navigateTo;
        LoginCommand = new RelayCommand(Login);
    }

    private void Login()
    {
        // برای تست فقط یوزر admin و رمز 1234 رو قبول می‌کنیم
        if (Username == "admin")
        {
            _navigateTo(new HomeViewModel()); // رفتن به Home
        }
        else
        {
            ErrorMessage = "نام کاربری یا رمز عبور اشتباه است.";
        }
    }
}