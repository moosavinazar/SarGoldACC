using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class BranchViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IBranchService _branchService;
    
    private long? _editingBranchId = null;
    private string _branchName;
    public string BranchName
    {
        get => _branchName;
        set => SetProperty(ref _branchName, value);
    }
    
    private ObservableCollection<BranchDto> _allBranches = new();
    public ObservableCollection<BranchDto> AllBranches
    {
        get => _allBranches;
        set => SetProperty(ref _allBranches, value);
    }
    
    public bool CanAccessBranchView => _authorizationService.HasPermission("Branch.View");
    public bool CanAccessBranchCreate => _authorizationService.HasPermission("Branch.Create");
    public bool CanAccessBranchEdit => _authorizationService.HasPermission("Branch.Edit");
    public bool CanAccessBranchDelete => _authorizationService.HasPermission("Branch.Delete");
    public bool CanAccessBranchCreateOrEdit => _authorizationService.HasPermission("Branch.Create") ||
                                              _authorizationService.HasPermission("Branch.Edit");
    // IDataErrorInfo
    public string Error => null;
    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(BranchName))
            {
                if (string.IsNullOrWhiteSpace(BranchName))
                    return "نام شعبه الزامی است.";

                if (!Regex.IsMatch(BranchName, @"^.+$"))
                    return "نام شعبه الزامی است";
            }
            return null;
        }
    }
    public BranchViewModel(IAuthorizationService authorizationService, 
        IBranchService branchService)
    {
        _authorizationService = authorizationService;
        _branchService = branchService;
        AllBranches = new ObservableCollection<BranchDto>();
        
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadBranchesAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task SaveBranch()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingBranchId.HasValue)
        {
            var dto = new BranchDto
            {
                Id = _editingBranchId.Value,
                Name = BranchName
            };
            result = await _branchService.UpdateAsync(dto);
            _editingBranchId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("شعبه با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var branchCreate = new BranchCreateDto
            {
                Name = BranchName
            };
            result = await _branchService.AddAsync(branchCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("شعبه با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadBranchesAsync();
    }
    
    public async Task DeleteAsync(long groupId)
    {
        await _branchService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long branchId)
    {
        _editingBranchId = branchId;
        var branch = await _branchService.GetByIdAsync(branchId);
        BranchName = branch.Name;
    }
    
    private async Task LoadBranchesAsync()
    {
        AllBranches.Clear();
        var groups = await _branchService.GetAllAsync();
        foreach (var g in groups)
        {
            AllBranches.Add(g);
        }
    }
    public void Clear()
    {
        _editingBranchId = null;
    }

}