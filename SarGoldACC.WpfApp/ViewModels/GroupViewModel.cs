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
    public ObservableCollection<PermissionDto> AllPermissions { get; }
    public ObservableCollection<PermissionDto> SelectedPermissions { get; }
    private ObservableCollection<GroupDto> _allGroups = new();
    public ObservableCollection<GroupDto> AllGroups
    {
        get => _allGroups;
        set => SetProperty(ref _allGroups, value);
    }


    public ICommand SaveCommand { get; }

    public GroupViewModel(IPermissionService permissionService, IGroupService groupService)
    {
        _permissionService = permissionService;
        _groupService = groupService;
        AllPermissions = new ObservableCollection<PermissionDto>();
        SelectedPermissions = new ObservableCollection<PermissionDto>();
        AllGroups = new ObservableCollection<GroupDto>();

        SaveCommand = new AsyncRelayCommand(SaveGroup);

        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadPermissionsAsync();
            await LoadGroupsAsync();
        }).GetAwaiter().GetResult();
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