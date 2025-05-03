using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class GroupViewModel : ViewModelBase
{
    private readonly IPermissionService _permissionService;
    private readonly IGroupService _groupService;
    private readonly GridSettingsFileService _settingsService;
    private const string GridName = "GroupGrid";
    private string _groupName;
    private string _groupLabel;

    public string GroupName
    {
        get => _groupName;
        set => SetProperty(ref _groupName, value);
    }

    public string GroupLabel
    {
        get => _groupLabel;
        set => SetProperty(ref _groupLabel, value);
    }
    
    public bool _showIdColumn = true;
    public bool _showNameColumn = true;
    public bool _showLabelColumn = true;
    public Action<bool, bool, bool>? SetColumnsVisibilityAction { get; set; }
    
    private void UpdateColumnVisibilities()
    {
        SetColumnsVisibilityAction?.Invoke(ShowIdColumn, ShowNameColumn, ShowLabelColumn);
    }
    
    public bool ShowIdColumn
    {
        get => _showIdColumn;
        set
        {
            if (SetProperty(ref _showIdColumn, value))
            {
                UpdateColumnVisibilities();
                // فراخوانی ناهمزمان بدون مسدود کردن UI
                Task.Run(async () => await SaveGridSettings());
            }
        }
    }

    public bool ShowNameColumn
    {
        get => _showNameColumn;
        set
        {
            if (SetProperty(ref _showNameColumn, value))
            {
                UpdateColumnVisibilities();
                Task.Run(async () => await SaveGridSettings());
            }
        }
    }

    public bool ShowLabelColumn
    {
        get => _showLabelColumn;
        set
        {
            if (SetProperty(ref _showLabelColumn, value))
            {
                UpdateColumnVisibilities();
                Task.Run(async () => await SaveGridSettings());
            }
        }
    }
    
    public ObservableCollection<PermissionDto> AllPermissions { get; }
    public ObservableCollection<PermissionDto> SelectedPermissions { get; }
    public ObservableCollection<GroupDto> AllGroups { get; }

    public ICommand SaveCommand { get; }

    public GroupViewModel(IPermissionService permissionService, IGroupService groupService, IAuthorizationService authorizationService)
    {
        _permissionService = permissionService;
        _groupService = groupService;
        _settingsService = new GridSettingsFileService(authorizationService.GetCurrentUserIdAsString());
        AllPermissions = new ObservableCollection<PermissionDto>();
        SelectedPermissions = new ObservableCollection<PermissionDto>();
        AllGroups = new ObservableCollection<GroupDto>();

        SaveCommand = new AsyncRelayCommand(SaveGroup);

        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadGridSettings();
            await LoadPermissionsAsync();
            await LoadGroupsAsync();
        }).GetAwaiter().GetResult();
        // _ = LoadPermissionsAsync().ContinueWith(t =>
        // {
        //     if (t.Exception != null)
        //         Debug.WriteLine("خطا در LoadPermissionsAsync: " + t.Exception.InnerException?.Message);
        // });
    }
    
    public async Task LoadGridSettings()
    {
        var all = await _settingsService.LoadSettingsAsync();
        var setting = all.FirstOrDefault(s => s.GridName == GridName);
        if (setting == null) return;

        setting.ColumnVisibility.TryGetValue("Id", out var idVisible);
        setting.ColumnVisibility.TryGetValue("Name", out var nameVisible);
        setting.ColumnVisibility.TryGetValue("Label", out var labelVisible);

        // فقط در صورت تفاوت مقدار، خاصیت را تنظیم کنید
        if (_showIdColumn != idVisible)
            ShowIdColumn = idVisible;
        if (_showNameColumn != nameVisible)
            ShowNameColumn = nameVisible;
        if (_showLabelColumn != labelVisible)
            ShowLabelColumn = labelVisible;
        Console.WriteLine(ShowIdColumn);
        Console.WriteLine(ShowNameColumn);
        Console.WriteLine(ShowLabelColumn);
    }
    
    public async Task SaveGridSettings()
    {
        var all = await _settingsService.LoadSettingsAsync();
        var setting = all.FirstOrDefault(s => s.GridName == GridName);
        if (setting == null)
        {
            setting = new GridColumnSetting { GridName = GridName };
            all.Add(setting);
        }

        setting.ColumnVisibility["Id"] = ShowIdColumn;
        setting.ColumnVisibility["Name"] = ShowNameColumn;
        setting.ColumnVisibility["Label"] = ShowLabelColumn;

        await _settingsService.SaveSettingsAsync(all);
    }

    
    private async Task LoadPermissionsAsync()
    {
        var permissions = await _permissionService.GetAllAsync();
        foreach (var p in permissions)
        {
            AllPermissions.Add(p);
        }
    }
    
    private async Task LoadGroupsAsync()
    {
        var groups = await _groupService.GetAllAsync();
        foreach (var g in groups)
        {
            AllGroups.Add(g);
        }
    }

    private async Task SaveGroup()
    {
        var group = new Group
        {
            Name = GroupName,
            Label = GroupLabel,
            GroupPermissions = SelectedPermissions
                .Select(p => new GroupPermission { PermissionId = p.Id })
                .ToList()
        };

        // ذخیره در دیتابیس...
    }
}