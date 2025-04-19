using System.Windows.Input;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Services.Auth;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _username = "";
    private string _password = "";
    private string _errorMessage = "";
    private readonly Action<ViewModelBase> _navigateTo;
    private readonly AuthenticationService _authService;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(nameof(Password)); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(Action<ViewModelBase> navigateTo, AuthenticationService authService)
    {
        _navigateTo = navigateTo;
        _authService = authService;
        LoginCommand = new AsyncRelayCommand(LoginAsync);
    }

    private async Task LoginAsync()
    {
        var user = await _authService.AuthenticateUserAsync(Username, Password);
        if (user != null)
        {
            await App.AuthorizationService.LoadUserPermissionsAsync(user.Id);
            _navigateTo(new HomeViewModel());
        }
        else
        {
            ErrorMessage = "نام کاربری یا رمز عبور اشتباه است.";
        }
    }
}