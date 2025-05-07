using System.Collections.ObjectModel;
using System.Windows.Input;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class UserViewModel : ViewModelBase
{
    private readonly IGroupService _groupService;
    private readonly IBranchService _branchService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserService _userService;
    private string _userName;
    private string _password;
    private string _confirmPassword;
    private string _name;
    private string _phoneNumber;
    private long _branchId;
    private long? _editingGroupId = null;
    
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }
    
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }
    
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }
    
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }
    
    public long BranchId
    {
        get => _branchId;
        set => SetProperty(ref _branchId, value);
    }
    
    public ObservableCollection<GroupDto> AllGroups { get; }
    public ObservableCollection<GroupDto> SelectedGroups { get; }
    public ObservableCollection<BranchDto> Branches { get; }
    
    private ObservableCollection<UserDto> _allUsers = new();
    public ObservableCollection<UserDto> AllUsers
    {
        get => _allUsers;
        set => SetProperty(ref _allUsers, value);
    }
    
    public ICommand SaveCommand { get; }
    
    public bool CanAccessUserView => _authorizationService.HasPermission("User.View");
    public bool CanAccessUserCreate => _authorizationService.HasPermission("User.Create");
    public bool CanAccessUserEdit => _authorizationService.HasPermission("User.Edit");
    public bool CanAccessUserDelete => _authorizationService.HasPermission("User.Delete");
    
    public bool CanAccessUserCreateOrEdit => _authorizationService.HasPermission("User.Create") ||
                                              _authorizationService.HasPermission("User.Edit");

    public UserViewModel(IGroupService groupService, IBranchService branchService,
        IAuthorizationService authorizationService, IUserService userService)
    {
        _groupService = groupService;
        _branchService = branchService;
        _authorizationService = authorizationService;
        _userService = userService;
        AllGroups = new ObservableCollection<GroupDto>();
        SelectedGroups = new ObservableCollection<GroupDto>();
        Branches = new ObservableCollection<BranchDto>();
        AllUsers = new ObservableCollection<UserDto>();
        
        SaveCommand = new AsyncRelayCommand(async () => await SaveUser());
        Task.Run(async () =>
        {
            await LoadGroupsAsync();
            await LoadBranchesAsync();
            await LoadUsersAsync();
        }).GetAwaiter().GetResult();
        
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
    
    private async Task LoadBranchesAsync()
    {
        Branches.Clear();
        var branches = await _branchService.GetAllAsync();
        foreach (var b in branches)
        {
            Branches.Add(b);
        }
    }
    
    private async Task LoadUsersAsync()
    {
        AllUsers.Clear();
        var users = await _userService.GetAllAsync();
        foreach (var u in users)
        {
            AllUsers.Add(u);
        }
    }

    public async Task SaveUser()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingGroupId.HasValue)
        {
            var dto = new UserUpdateDto()
            {
                Id = _editingGroupId.Value,
                Name = Name,
                PhoneNumber = PhoneNumber,
                BranchId = BranchId,
                UserGroups = SelectedGroups
                    .Select(p => new UserGroup
                    {
                        GroupId = p.Id,
                        UserId = _editingGroupId.Value // یا 0 اگر جدید باشه
                    })
                    .ToList()
            };
            result = await _userService.UpdateAsync(dto);
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
            var userDto = new UserCreateDto
            {
                Username = _userName,
                Password = Password,
                Name = Name,
                PhoneNumber = PhoneNumber,
                BranchId = BranchId,
                UserGroups = SelectedGroups
                    .Select(g => g.Id)
                    .ToList()
            };
        
            result = await _userService.AddAsync(userDto);
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
}