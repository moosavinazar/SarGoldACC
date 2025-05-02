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
    public string GroupName { get; set; }
    public string GroupLabel { get; set; }
    
    public bool ShowIdColumn { get; set; } = true;
    public bool ShowNameColumn { get; set; } = true;
    public bool ShowLabelColumn { get; set; } = true;
    
    private readonly GridSettingsFileService _settingsService = new();
    private const string GridName = "GroupGrid";

    public ObservableCollection<PermissionDto> AllPermissions { get; }
    public ObservableCollection<PermissionDto> SelectedPermissions { get; }
    public ObservableCollection<GroupDto> AllGroups { get; }

    public ICommand SaveCommand { get; }

    public GroupViewModel(IPermissionService permissionService, IGroupService groupService)
    {
        _permissionService = permissionService;
        _groupService = groupService;
        AllPermissions = new ObservableCollection<PermissionDto>();
        SelectedPermissions = new ObservableCollection<PermissionDto>();
        AllGroups = new ObservableCollection<GroupDto>();

        SaveCommand = new AsyncRelayCommand(SaveGroup);

        _ = LoadPermissionsAsync(); // شروع async بدون انتظار در کانستراکتور
        _ = LoadGroupsAsync();
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

        ShowIdColumn = idVisible;
        ShowNameColumn = nameVisible;
        ShowLabelColumn = labelVisible;
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