using System.Collections.ObjectModel;
using System.Windows.Input;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class GroupViewModel : ViewModelBase
{
    public string GroupName { get; set; }

    public ObservableCollection<Permission> AllPermissions { get; }
    public ObservableCollection<Permission> SelectedPermissions { get; }

    public ICommand SaveCommand { get; }

    public GroupViewModel()
    {
        AllPermissions = new ObservableCollection<Permission>(/* لیست permissions از دیتابیس */);
        SelectedPermissions = new ObservableCollection<Permission>();

        SaveCommand = new AsyncRelayCommand(SaveGroup);
    }

    private async Task SaveGroup()
    {
        var group = new Group
        {
            Name = GroupName,
            GroupPermissions = SelectedPermissions
                .Select(p => new GroupPermission { PermissionId = p.Id })
                .ToList()
        };

        // ذخیره در دیتابیس...
    }
}