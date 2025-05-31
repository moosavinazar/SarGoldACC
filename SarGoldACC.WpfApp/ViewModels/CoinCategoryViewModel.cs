using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CoinCategory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class CoinCategoryViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICoinCategoryService _coinCategoriesService;
    
    private long? _editingCoinCategoryId = null;
    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    private double _weight;
    public double Weight
    {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }
    private double _weight750;
    public double Weight750
    {
        get => _weight750;
        set => SetProperty(ref _weight750, value);
    }
    private int _ayar;
    public int Ayar
    {
        get => _ayar;
        set => SetProperty(ref _ayar, value);
    }
    private ObservableCollection<CoinCategoryDto> _allCoinCategories = new();
    public ObservableCollection<CoinCategoryDto> CoinCategories
    {
        get => _allCoinCategories;
        set => SetProperty(ref _allCoinCategories, value);
    }
    
    public long MadeCategoryId { get; set; }
    
    public bool CanAccessCoinCategoryView => _authorizationService.HasPermission("CoinCategory.View");
    public bool CanAccessCoinCategoryCreate => _authorizationService.HasPermission("CoinCategory.Create");
    public bool CanAccessCoinCategoryEdit => _authorizationService.HasPermission("CoinCategory.Edit");
    public bool CanAccessCoinCategoryDelete => _authorizationService.HasPermission("CoinCategory.Delete");
    public bool CanAccessCoinCategoryCreateOrEdit => _authorizationService.HasPermission("CoinCategory.Create") ||
                                              _authorizationService.HasPermission("CoinCategory.Edit");

    public CoinCategoryViewModel(IAuthorizationService authorizationService, 
        ICoinCategoryService coinCategoriesService, IMadeCategoryService madeCategoryService)
    {
        _authorizationService = authorizationService;
        _coinCategoriesService = coinCategoriesService;
        CoinCategories = new ObservableCollection<CoinCategoryDto>();
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadCoinCategoriesAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task SaveCoinCategory()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingCoinCategoryId.HasValue)
        {
            var dto = new CoinCategoryDto
            {
                Id = _editingCoinCategoryId.Value,
                Name = Name
            };
            result = await _coinCategoriesService.UpdateAsync(dto);
            _editingCoinCategoryId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("دسته بندی سکه با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var coinCategoriesCreate = new CoinCategoryCreateDto
            {
                Name = Name,
                Weight = Weight,
                Weight750 = Weight750,
                Ayar = Ayar
            };
            result = await _coinCategoriesService.AddAsync(coinCategoriesCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("دسته بندی سکه با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadCoinCategoriesAsync();
    }
    
    public async Task DeleteAsync(long groupId)
    {
        await _coinCategoriesService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long coinCategoriesId)
    {
        _editingCoinCategoryId = coinCategoriesId;
        var coinCategories = await _coinCategoriesService.GetByIdAsync(coinCategoriesId);
        Name = coinCategories.Name;
    }
    
    private async Task LoadCoinCategoriesAsync()
    {
        CoinCategories.Clear();
        var coinCategories = await _coinCategoriesService.GetAllAsync();
        foreach (var c in coinCategories)
        {
            CoinCategories.Add(c);
        }
    }

}