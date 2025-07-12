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
    
    public long CoinCategoryId
    {
        get => _coinCategoryId;
        set
        {
            if (_coinCategoryId != value)
            {
                _coinCategoryId = value;
                OnPropertyChanged(nameof(CoinCategoryId));
                Name = "پرداخت سکه - " + _coinCategoryService.GetByIdAsync(CoinCategoryId);
                ValidateAll();
            }
        }
    }
    public ObservableCollection<CoinCategoryDto> CoinCategories { get; }
    public long BoxId
    {
        get => _boxId;
        set => SetProperty(ref _boxId, value);
    }
    public ObservableCollection<BoxDto> Boxes { get; }
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
        set => SetProperty(ref _count, value);
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
        BoxId = 1;
        CoinCategoryId = 1;
        Task.Run(async () =>
        {
            await LoadCoinCategoriesAsync();
            await LoadBoxesAsync();
        }).GetAwaiter().GetResult();
    }
    public async Task ReloadAllAsync()
    {
        await LoadCoinCategoriesAsync();
        await LoadBoxesAsync();
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