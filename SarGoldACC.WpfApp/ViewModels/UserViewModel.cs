using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows.Input;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.Core.Utils;
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
    private long? _editingUserId = null;

    public string UserName
    {
        get => _userName;
        set
        {
            if (_userName != value)
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
                ValidateAll();
            }
        }
    }
    
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public long SelectedBranchId
    {
        get => _branchId;
        set => SetProperty(ref _branchId, value);
    }
    
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }
    
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                ValidateAll();
            }
        }
    }
    
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                ValidateAll();
            }
        }
    }
    
    public long BranchId
    {
        get => _branchId;
        set => SetProperty(ref _branchId, value);
    }
    
    public ObservableCollection<GroupDto> AllGroups { get; }
    private ObservableCollection<GroupDto> _selectedGroups;
    public ObservableCollection<GroupDto> SelectedGroups
    {
        get => _selectedGroups;
        set
        {
            if (_selectedGroups != null)
                _selectedGroups.CollectionChanged -= SelectedGroups_CollectionChanged;

            _selectedGroups = value;

            if (_selectedGroups != null)
                _selectedGroups.CollectionChanged += SelectedGroups_CollectionChanged;

            OnPropertyChanged(nameof(_selectedGroups));
            ValidateAll();
        }
    }

    private void SelectedGroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ValidateAll();
    }

    private ObservableCollection<BranchDto> _branches;
    public ObservableCollection<BranchDto> Branches
    {
        get => _branches;
        set => SetProperty(ref _branches, value);
    }
    
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
    public bool CanAccessBranchButton => _authorizationService.HasPermission("Branch.Create") ||
                                         _authorizationService.HasPermission("Branch.Edit");
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

        SelectedBranchId = _authorizationService.GetCurrentUser().BranchId;

    }
    
    public async Task LoadGroupsAsync()
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
    
        // گرفتن لیست کاربران از سرویس
        var users = await _userService.GetAllAsync();
    
        // گرفتن لیست شعب برای نگاشت
        var branches = await _branchService.GetAllAsync();
    
        // نگاشت BranchId به BranchName با Dictionary برای دسترسی سریع‌تر
        var branchDict = branches.ToDictionary(b => b.Id, b => b.Name);

        foreach (var u in users)
        {
            u.BranchName = branchDict.TryGetValue(u.BranchId, out var name) ? name : "نامشخص";
            AllUsers.Add(u);
        }
    }


    public async Task SaveUser()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingUserId.HasValue)
        {
            UserUpdateDto dto;
            if (Password != null)
            {
                dto = new UserUpdateDto
                {
                    Id = _editingUserId.Value,
                    Username = UserName,
                    Password = PasswordHasher.HashPassword(Password),
                    Name = Name,
                    PhoneNumber = PhoneNumber,
                    BranchId = SelectedBranchId,
                    UserGroups = SelectedGroups
                        .Select(p => new UserGroup
                        {
                            GroupId = p.Id,
                            UserId = _editingUserId.Value // یا 0 اگر جدید باشه
                        })
                        .ToList()
                };
            }
            else
            {
                dto = new UserUpdateDto
                {
                    Id = _editingUserId.Value,
                    Username = UserName,
                    Name = Name,
                    PhoneNumber = PhoneNumber,
                    BranchId = SelectedBranchId,
                    UserGroups = SelectedGroups
                        .Select(p => new UserGroup
                        {
                            GroupId = p.Id,
                            UserId = _editingUserId.Value // یا 0 اگر جدید باشه
                        })
                        .ToList()
                };
            }
            
            result = await _userService.UpdateAsync(dto);
            _editingUserId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("کاربر با موفقیت ویرایش شد.");
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
                MessageBoxHelper.ShowSuccess("کاربر با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadUsersAsync();
    }
    
    public async Task EditAsync(long userId)
    {
        _editingUserId = userId;
        var userDto = await _userService.GetByIdAsync(userId);
        UserName = userDto.Username;
        Name = userDto.Name;
        PhoneNumber = userDto.PhoneNumber;
        SelectedBranchId = userDto.BranchId;
        var groupsIds = userDto.UserGroups?.Select(gp => gp.GroupId).ToList() ?? new List<long>();
        
        SelectedGroups.Clear();
        AllGroups.Clear();

        var allGroups = await _groupService.GetAllAsync();
    
        foreach (var group in allGroups)
        {
            if (groupsIds.Contains(group.Id))
                SelectedGroups.Add(group);
            else
                AllGroups.Add(group);
        }
    }
    
    public async Task DeleteAsync(long userId)
    {
        await _userService.DeleteAsync(userId);
    }
    public bool this[string columnName]
    {
        get
        {
            if (columnName == nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name) || !Regex.IsMatch(Name, @"^.+$"))
                    return true;
            }
            if (columnName == nameof(UserName))
            {
                if (string.IsNullOrWhiteSpace(UserName) || !Regex.IsMatch(UserName, @"^.+$"))
                    return true;
            }
            if (columnName == nameof(PhoneNumber))
            {
                if (string.IsNullOrWhiteSpace(PhoneNumber) || !Regex.IsMatch(PhoneNumber, @"^09\d{9}$"))
                    return true;
            }
            if (columnName == nameof(SelectedGroups))
            {
                if (SelectedGroups == null || SelectedGroups.Count == 0)
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Name),
        nameof(UserName),
        nameof(PhoneNumber),
        nameof(SelectedGroups)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingUserId = null;
    }
}