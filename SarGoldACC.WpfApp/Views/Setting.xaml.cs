using System.Windows;
using System.Windows.Input;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp.Views;

public partial class Setting : Window
{
    private readonly SettingViewModel _viewModel;
    
    public Setting(SettingViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }
    
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
    
    private async void ClickSave(object sender, RoutedEventArgs e)
    {
        await _viewModel.SaveSetting();
    }
}