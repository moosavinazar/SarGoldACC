using System.Windows.Input;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Setting;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ISettingService _settingService;
    
    private long? _editingSettingId = null;
    private string _customerImageUrl;
    private SettingDto settingDto = new SettingDto();
    public ICommand BrowseFolderCommand { get; }
    public string CustomerImageUrl
    {
        get => _customerImageUrl;
        set => SetProperty(ref _customerImageUrl, value);
    }
    
    public bool CanAccessSettingView => _authorizationService.HasPermission("Setting.View");
    public bool CanChangeCustomerUrl => _authorizationService.HasPermission("Setting.CustomerUrl");
    public bool CanAccessSaveButton => _authorizationService.HasPermission("Setting.CustomerUrl");

    public SettingViewModel(IAuthorizationService authorizationService, 
        ISettingService settingService)
    {
        _authorizationService = authorizationService;
        _settingService = settingService;
        BrowseFolderCommand = new AsyncRelayCommand(BrowseFolder);
        Task.Run(async () =>
        {
            await LoadSettingsAsync();
        }).GetAwaiter().GetResult();
    }
    
    private async Task BrowseFolder()
    {
        await Task.Yield(); // تضمین امضای async
        var dialog = new System.Windows.Forms.FolderBrowserDialog();
        var result = dialog.ShowDialog(); // باید در UI thread اجرا شود

        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            CustomerImageUrl = dialog.SelectedPath;
        }
    }
    
    public async Task SaveSetting()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        var dto = new SettingDto
        {
            Id = 1,
            CustomerImageUrl = CustomerImageUrl
        };
        result = await _settingService.UpdateAsync(dto);
        _editingSettingId = null;
        if (result.Success)
        {
            MessageBoxHelper.ShowSuccess("تنظیمات با موفقیت تغییر کرد.");
        }
        else
        {
            MessageBoxHelper.ShowError(result.Message);
        }
    }
    
    private async Task LoadSettingsAsync()
    {
        settingDto = await _settingService.GetByIdAsync(1);
        CustomerImageUrl = settingDto.CustomerImageUrl;
    }

}