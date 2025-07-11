using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
    private string _madeCategoryName;
    public string MadeCategoryName
    {
        get => _madeCategoryName;
        set
        {
            if (_madeCategoryName != value)
            {
                _madeCategoryName = value;
                OnPropertyChanged(nameof(MadeCategoryName));
                ValidateAll();
            }
        }
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

    public bool this[string columnName]
    {
        get
        {
            if (columnName == nameof(MadeCategoryName))
            {
                if (string.IsNullOrWhiteSpace(MadeCategoryName) || !Regex.IsMatch(MadeCategoryName, @"^.+$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(MadeCategoryName),
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingMadeCategoryId = null;
    }
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