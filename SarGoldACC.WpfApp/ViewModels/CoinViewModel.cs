using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.CoinCategory;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class CoinViewModel : ViewModelBase
{
    private readonly ICoinCategoryService _coinCategoryService;
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
    
    private long _coinCategoryId;
    private long _boxId;
    private string _name;
    private int _count;
    private int _ayar;
    private double _weight;
    private double _weight750;
    private long _ojratR;
    private int _ojratP;
    private string _description;
    
    public long PayCoinCategoryId
    {
        get => _coinCategoryId;
        set
        {
            if (_coinCategoryId != value)
            {
                _coinCategoryId = value;
                OnPropertyChanged(nameof(PayCoinCategoryId));
                _ = UpdatePayNameAsync(); // بدون await چون setter نمی‌تونه async باشه
                ValidateAll();
            }
        }
    }
    public long RcvCoinCategoryId
    {
        get => _coinCategoryId;
        set
        {
            if (_coinCategoryId != value)
            {
                _coinCategoryId = value;
                OnPropertyChanged(nameof(RcvCoinCategoryId));
                _ = UpdateRcvNameAsync(); // بدون await چون setter نمی‌تونه async باشه
                ValidateAll();
            }
        }
    }
    private async Task UpdateValueAsync()
    {
        var category = await _coinCategoryService.GetByIdAsync(PayCoinCategoryId);
        Ayar = category?.Ayar ?? 0;
        Weight = category?.Weight * Count ?? 0;
        Weight = Math.Round(Weight, 3);
        Weight750 = category?.Weight750 * Count ?? 0;
        Weight750 = Math.Round(Weight750, 3);
    }
    private async Task UpdatePayNameAsync()
    {
        var category = await _coinCategoryService.GetByIdAsync(PayCoinCategoryId);
        Name = "پرداخت سکه - " + category?.Name;
        await UpdateValueAsync();
    }
    private async Task UpdateRcvNameAsync()
    {
        var category = await _coinCategoryService.GetByIdAsync(RcvCoinCategoryId);
        Name = "دریافت سکه - " + category?.Name;
        await UpdateValueAsync();
    }
    private ObservableCollection<CoinCategoryDto> _coinCategories;
    public ObservableCollection<CoinCategoryDto> CoinCategories
    {
        get => _coinCategories;
        set => SetProperty(ref _coinCategories, value);
    }
    public long BoxId
    {
        get => _boxId;
        set => SetProperty(ref _boxId, value);
    }

    private ObservableCollection<BoxDto> _boxes;
    public ObservableCollection<BoxDto> Boxes
    {
        get => _boxes;
        set => SetProperty(ref _boxes, value);
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
    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                _count = value;
                OnPropertyChanged(nameof(Count));
                _ = UpdateValueAsync();
                ValidateAll();
            }
        }
    }
    public int Ayar
    {
        get => _ayar;
        set => SetProperty(ref _ayar, value);
    }
    public double Weight
    {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }
    public double Weight750
    {
        get => _weight750;
        set => SetProperty(ref _weight750, value);
    }
    public long OjratR
    {
        get => _ojratR;
        set => SetProperty(ref _ojratR, value);
    }
    public int OjratP
    {
        get => _ojratP;
        set => SetProperty(ref _ojratP, value);
    }
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    
    public bool CanAccessCoinCategoryButton => _authorizationService.HasPermission("CoinCategory.View") ||
                                             _authorizationService.HasPermission("CoinCategory.Create") ||
                                             _authorizationService.HasPermission("CoinCategory.Edit") ||
                                             _authorizationService.HasPermission("CoinCategory.Delete");
    public bool CanAccessBoxButton => _authorizationService.HasPermission("Box.View") ||
                                      _authorizationService.HasPermission("Box.Create") ||
                                      _authorizationService.HasPermission("Box.Edit") ||
                                      _authorizationService.HasPermission("Box.Delete");
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

    public CoinViewModel(ICoinCategoryService coinCategoryService, 
        IBoxService boxService, IAuthorizationService authorizationService)
    {
        _coinCategoryService = coinCategoryService;
        _boxService = boxService;
        _authorizationService = authorizationService;
        CoinCategories = new ObservableCollection<CoinCategoryDto>();
        Boxes = new ObservableCollection<BoxDto>();
    }
    public async Task InitializeAsync()
    {
        await LoadCoinCategoriesAsync();
        await LoadBoxesAsync();
    }
    public async Task ReloadAllRcvAsync()
    {
        await LoadCoinCategoriesAsync();
        await LoadBoxesAsync();
        BoxId = 1;
        Count = 1;
        RcvCoinCategoryId = 1;
    }
    public async Task ReloadAllPayAsync()
    {
        await LoadCoinCategoriesAsync();
        await LoadBoxesAsync();
        BoxId = 1;
        Count = 1;
        PayCoinCategoryId = 1;
    }
    
    private async Task LoadCoinCategoriesAsync()
    {
        CoinCategories.Clear();
        var coinCategories = await _coinCategoryService.GetAllAsync();
        foreach (var m in coinCategories)
        {
            CoinCategories.Add(m);
        }
    }
    
    private async Task LoadBoxesAsync()
    {
        Boxes.Clear();
        var boxes = await _boxService.GetAllAsync();
        foreach (var b in boxes)
        {
            Boxes.Add(b);
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
        nameof(Name)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
}