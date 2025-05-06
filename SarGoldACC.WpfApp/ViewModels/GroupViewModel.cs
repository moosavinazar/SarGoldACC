using System.Collections.ObjectModel;
using System.Windows.Input;
using SarGoldACC.Core.DTOs;
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
    private string _groupName;
    private string _groupLabel;
    private long? _editingGroupId = null;

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

        SaveCommand = new AsyncRelayCommand(async () => await SaveGroup());

        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadPermissionsAsync();
            await LoadGroupsAsync();
        }).GetAwaiter().GetResult();
    }
    
    public async Task LoadPermissionsAsync()
    {
        var permissions = await _permissionService.GetAllAsync();
        foreach (var p in permissions)
        {
            AllPermissions.Add(p);
        }
    }
    
    private async Task LoadGroupsAsync()
    {
        AllGroups.Clear();
        var groups = await _groupService.GetAllAsync();
        foreach (var g in groups)
        {
            AllGroups.Add(g);
        }
    }

    public async Task SaveGroup()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingGroupId.HasValue)
        {
            var dto = new GroupDto
            {
                Id = _editingGroupId.Value,
                Name = GroupName,
                Label = GroupLabel,
                GroupPermissions = SelectedPermissions
                    .Select(p => new GroupPermission
                    {
                        PermissionId = p.Id,
                        GroupId = _editingGroupId.Value // یا 0 اگر جدید باشه
                    })
                    .ToList()
            };
            result = await _groupService.UpdateAsync(dto);
            _editingGroupId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("گروه با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var groupDto = new GroupCreateDto
            {
                Name = GroupName,
                Label = GroupLabel,
                GroupPermissions = SelectedPermissions
                    .Select(p => p.Id)
                    .ToList()
            };
        
            result = await _groupService.AddAsync(groupDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("گروه با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadGroupsAsync();
    }

    public async Task EditAsync(long groupId)
    {
        _editingGroupId = groupId;
        var groupDto = await _groupService.GetByIdAsync(groupId);
        GroupName = groupDto.Name;
        GroupLabel = groupDto.Label;
        var permissionIds = groupDto.GroupPermissions?.Select(gp => gp.PermissionId).ToList() ?? new List<long>();
        Console.WriteLine(permissionIds.Count);

        SelectedPermissions.Clear();
        AllPermissions.Clear();

        var allPermissions = await _permissionService.GetAllAsync();
    
        foreach (var permission in allPermissions)
        {
            if (permissionIds.Contains(permission.Id))
                SelectedPermissions.Add(permission);
            else
                AllPermissions.Add(permission);
        }
    }
    
    public async Task DeleteAsync(long groupId)
    {
        await _groupService.DeleteAsync(groupId);
    }
}