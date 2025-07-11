using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
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
    private readonly IAuthorizationService _authorizationService;
    private string _groupName;
    private string _groupLabel;
    private long? _editingGroupId = null;
    
    public bool CanAccessGroupView => _authorizationService.HasPermission("Group.View");
    public bool CanAccessGroupCreate => _authorizationService.HasPermission("Group.Create");
    public bool CanAccessGroupEdit => _authorizationService.HasPermission("Group.Edit");
    public bool CanAccessGroupDelete => _authorizationService.HasPermission("Group.Delete");
    
    public bool CanAccessGroupCreateOrEdit => _authorizationService.HasPermission("Group.Create") ||
                                            _authorizationService.HasPermission("Group.Edit");
    private bool _canSave;
    public bool CanSave
    {
        get => _canSave;
        set
        {
            if (_canSave != value)
            {
                _canSave = value;
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public string GroupName
    {
        get => _groupName;
        set
        {
            if (_groupName != value)
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
                ValidateAll();
            }
        }
    }
    public string GroupLabel
    {
        get => _groupLabel;
        set
        {
            if (_groupLabel != value)
            {
                _groupLabel = value;
                OnPropertyChanged(nameof(GroupLabel));
                ValidateAll();
            }
        }
    }
    public ObservableCollection<PermissionDto> AllPermissions { get; }
    private ObservableCollection<PermissionDto> _selectedPermissions;
    public ObservableCollection<PermissionDto> SelectedPermissions
    {
        get => _selectedPermissions;
        set
        {
            if (_selectedPermissions != null)
                _selectedPermissions.CollectionChanged -= SelectedPermissions_CollectionChanged;

            _selectedPermissions = value;

            if (_selectedPermissions != null)
                _selectedPermissions.CollectionChanged += SelectedPermissions_CollectionChanged;

            OnPropertyChanged(nameof(SelectedPermissions));
            ValidateAll();
        }
    }

    private void SelectedPermissions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ValidateAll();
    }
    
    private ObservableCollection<GroupDto> _allGroups = new();
    public ObservableCollection<GroupDto> AllGroups
    {
        get => _allGroups;
        set => SetProperty(ref _allGroups, value);
    }


    public ICommand SaveCommand { get; }

    public GroupViewModel(IPermissionService permissionService, IGroupService groupService, IAuthorizationService authorizationService)
    {
        _permissionService = permissionService;
        _groupService = groupService;
        _authorizationService = authorizationService;
        AllPermissions = new ObservableCollection<PermissionDto>();
        SelectedPermissions = new ObservableCollection<PermissionDto>();
        AllGroups = new ObservableCollection<GroupDto>();
        SelectedPermissions.CollectionChanged += (s, e) => ValidateAll();
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
    public bool this[string columnName]
    {
        get
        {
            if (columnName == nameof(GroupName))
            {
                if (string.IsNullOrWhiteSpace(GroupName) || !Regex.IsMatch(GroupName, @"^.+$"))
                    return true;
            }
            if (columnName == nameof(GroupLabel))
            {
                if (string.IsNullOrWhiteSpace(GroupLabel) || !Regex.IsMatch(GroupLabel, @"^.+$"))
                    return true;
            }
            if (columnName == nameof(SelectedPermissions))
            {
                if (SelectedPermissions == null || SelectedPermissions.Count == 0)
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(GroupName),
        nameof(GroupLabel),
        nameof(SelectedPermissions)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingGroupId = null;
    }
}