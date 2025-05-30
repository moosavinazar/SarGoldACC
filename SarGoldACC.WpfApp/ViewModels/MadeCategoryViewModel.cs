using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class MadeCategoryViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IMadeCategoryService _madeSubCategoryService;
    
    private long? _editingMadeCategoryId = null;
    private string _madeSubCategoryName;
    public string MadeCategoryName
    {
        get => _madeSubCategoryName;
        set => SetProperty(ref _madeSubCategoryName, value);
    }
    
    private ObservableCollection<MadeCategoryDto> _allMadeCategories = new();
    public ObservableCollection<MadeCategoryDto> AllMadeCategories
    {
        get => _allMadeCategories;
        set => SetProperty(ref _allMadeCategories, value);
    }
    
    public bool CanAccessMadeCategoryView => _authorizationService.HasPermission("MadeCategory.View");
    public bool CanAccessMadeCategoryCreate => _authorizationService.HasPermission("MadeCategory.Create");
    public bool CanAccessMadeCategoryEdit => _authorizationService.HasPermission("MadeCategory.Edit");
    public bool CanAccessMadeCategoryDelete => _authorizationService.HasPermission("MadeCategory.Delete");
    public bool CanAccessMadeCategoryCreateOrEdit => _authorizationService.HasPermission("MadeCategory.Create") ||
                                              _authorizationService.HasPermission("MadeCategory.Edit");

    public MadeCategoryViewModel(IAuthorizationService authorizationService, 
        IMadeCategoryService madeSubCategoryService)
    {
        _authorizationService = authorizationService;
        _madeSubCategoryService = madeSubCategoryService;
        AllMadeCategories = new ObservableCollection<MadeCategoryDto>();
        
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadMadeCategoriesAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task SaveMadeCategory()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingMadeCategoryId.HasValue)
        {
            var dto = new MadeCategoryDto
            {
                Id = _editingMadeCategoryId.Value,
                Name = MadeCategoryName
            };
            result = await _madeSubCategoryService.UpdateAsync(dto);
            _editingMadeCategoryId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("دسته بندی با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var madeSubCategoryCreate = new MadeCategoryCreateDto
            {
                Name = MadeCategoryName
            };
            result = await _madeSubCategoryService.AddAsync(madeSubCategoryCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("دسته بندی با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadMadeCategoriesAsync();
    }
    
    public async Task DeleteAsync(long groupId)
    {
        await _madeSubCategoryService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long madeSubCategoryId)
    {
        _editingMadeCategoryId = madeSubCategoryId;
        var madeCategory = await _madeSubCategoryService.GetByIdAsync(madeSubCategoryId);
        MadeCategoryName = madeCategory.Name;
    }
    
    private async Task LoadMadeCategoriesAsync()
    {
        AllMadeCategories.Clear();
        var madeSubCategories = await _madeSubCategoryService.GetAllAsync();
        foreach (var m in madeSubCategories)
        {
            AllMadeCategories.Add(m);
        }
    }

}