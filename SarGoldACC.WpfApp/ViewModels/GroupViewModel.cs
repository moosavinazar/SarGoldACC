using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class GroupViewModel : ViewModelBase
{
    private readonly IPermissionService _permissionService;
    public string GroupName { get; set; }
    public string GroupLabel { get; set; }

    public ObservableCollection<PermissionDto> AllPermissions { get; }
    public ObservableCollection<PermissionDto> SelectedPermissions { get; }

    public ICommand SaveCommand { get; }

    public GroupViewModel(IPermissionService permissionService)
    {
        _permissionService = permissionService;
        AllPermissions = new ObservableCollection<PermissionDto>();
        SelectedPermissions = new ObservableCollection<PermissionDto>();

        SaveCommand = new AsyncRelayCommand(SaveGroup);

        _ = LoadPermissionsAsync(); // شروع async بدون انتظار در کانستراکتور
        // _ = LoadPermissionsAsync().ContinueWith(t =>
        // {
        //     if (t.Exception != null)
        //         Debug.WriteLine("خطا در LoadPermissionsAsync: " + t.Exception.InnerException?.Message);
        // });
    }
    
    private async Task LoadPermissionsAsync()
    {
        var permissions = await _permissionService.GetAllAsync();
        foreach (var p in permissions)
        {
            AllPermissions.Add(p);
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