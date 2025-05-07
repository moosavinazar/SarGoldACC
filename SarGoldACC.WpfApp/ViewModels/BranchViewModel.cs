using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class BranchViewModel : ViewModelBase
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
    
    public bool CanAccessBranchView => _authorizationService.HasPermission("Branch.View");
    public bool CanAccessBranchCreate => _authorizationService.HasPermission("Branch.Create");
    public bool CanAccessBranchEdit => _authorizationService.HasPermission("Branch.Edit");
    public bool CanAccessBranchDelete => _authorizationService.HasPermission("Branch.Delete");
    public bool CanAccessBranchCreateOrEdit => _authorizationService.HasPermission("Branch.Create") ||
                                              _authorizationService.HasPermission("Branch.Edit");

    public BranchViewModel(IAuthorizationService authorizationService, 
        IBranchService branchService)
    {
        _authorizationService = authorizationService;
        _branchService = branchService;
    }

    public async Task SaveBranch()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingBranchId.HasValue)
        {
            
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
    }

}