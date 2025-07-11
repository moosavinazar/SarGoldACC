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
    public long CoinCategoryId { get; set; }
    public ObservableCollection<CoinCategoryDto> CoinCategories { get; }
    public long BoxId { get; set; }
    public ObservableCollection<BoxDto> Boxes { get; }
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
    public int Ayar { get; set; }
    public double Weight { get; set; }
    public double? Weight750 { get; set; }
    public long? OjratR { get; set; }
    public double? OjratP { get; set; }
    
    public string Description { get; set; }
    
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