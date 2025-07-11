using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class MadeSubCategoryViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IMadeSubCategoryService _madeSubCategoriesService;
    private readonly IMadeCategoryService _madeCategoryService;
    
    private long? _editingMadeSubCategoryId = null;
    private string _name;
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
    
    private ObservableCollection<MadeSubCategoryDto> _allMadeSubCategories = new();
    public ObservableCollection<MadeSubCategoryDto> AllMadeSubCategories
    {
        get => _allMadeSubCategories;
        set => SetProperty(ref _allMadeSubCategories, value);
    }
    
    public long MadeCategoryId { get; set; }
    public ObservableCollection<MadeCategoryDto> MadeCategories { get; }
    
    public bool CanAccessMadeSubCategoryView => _authorizationService.HasPermission("MadeSubCategory.View");
    public bool CanAccessMadeSubCategoryCreate => _authorizationService.HasPermission("MadeSubCategory.Create");
    public bool CanAccessMadeSubCategoryEdit => _authorizationService.HasPermission("MadeSubCategory.Edit");
    public bool CanAccessMadeSubCategoryDelete => _authorizationService.HasPermission("MadeSubCategory.Delete");
    public bool CanAccessMadeSubCategoryCreateOrEdit => _authorizationService.HasPermission("MadeSubCategory.Create") ||
                                              _authorizationService.HasPermission("MadeSubCategory.Edit");
    
    public bool CanAccessMadeCategoryButton => _authorizationService.HasPermission("MadeCategory.View") ||
                                      _authorizationService.HasPermission("MadeCategory.Create") ||
                                      _authorizationService.HasPermission("MadeCategory.Edit") ||
                                      _authorizationService.HasPermission("MadeCategory.Delete");
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
            if (columnName == nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name) || !Regex.IsMatch(Name, @"^.+$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Name),
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingMadeSubCategoryId = null;
    }

    public MadeSubCategoryViewModel(IAuthorizationService authorizationService, 
        IMadeSubCategoryService madeSubCategoriesService, IMadeCategoryService madeCategoryService)
    {
        _authorizationService = authorizationService;
        _madeSubCategoriesService = madeSubCategoriesService;
        _madeCategoryService = madeCategoryService;
        AllMadeSubCategories = new ObservableCollection<MadeSubCategoryDto>();
        MadeCategories = new ObservableCollection<MadeCategoryDto>();
        MadeCategoryId = 1;
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadMadeSubCategoriesAsync();
            await LoadMadeCategoriesAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task SaveMadeSubCategory()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingMadeSubCategoryId.HasValue)
        {
            var dto = new MadeSubCategoryDto
            {
                Id = _editingMadeSubCategoryId.Value,
                Name = Name
            };
            result = await _madeSubCategoriesService.UpdateAsync(dto);
            _editingMadeSubCategoryId = null;
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
            var madeSubCategoriesCreate = new MadeSubCategoryCreateDto
            {
                Name = Name,
                MadeCategoryId = MadeCategoryId
            };
            result = await _madeSubCategoriesService.AddAsync(madeSubCategoriesCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("دسته بندی با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadMadeSubCategoriesAsync();
    }
    
    public async Task DeleteAsync(long groupId)
    {
        await _madeSubCategoriesService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long madeSubCategoriesId)
    {
        _editingMadeSubCategoryId = madeSubCategoriesId;
        var madeSubCategories = await _madeSubCategoriesService.GetByIdAsync(madeSubCategoriesId);
        Name = madeSubCategories.Name;
    }
    
    private async Task LoadMadeSubCategoriesAsync()
    {
        AllMadeSubCategories.Clear();
        var groups = await _madeSubCategoriesService.GetAllAsync();
        foreach (var g in groups)
        {
            AllMadeSubCategories.Add(g);
        }
    }
    
    private async Task LoadMadeCategoriesAsync()
    {
        MadeCategories.Clear();
        var madeCategories = await _madeCategoryService.GetAllAsync();
        foreach (var m in madeCategories)
        {
            MadeCategories.Add(m);
        }
    }

}