using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Cost;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class CostViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICostService _costService;
    private readonly ICurrencyService _currencyService;
    private long? _editingCostId = null;
    
    private string _name;
    private string _label;
    private string _description;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private long _branchId;
    private ObservableCollection<CostDto> _allCost = new();
    public ObservableCollection<CurrencyDto> Currencies { get; }
    public bool CanAccessCostView => _authorizationService.HasPermission("Cost.View");
    public bool CanAccessCostCreate => _authorizationService.HasPermission("Cost.Create");
    public bool CanAccessCostEdit => _authorizationService.HasPermission("Cost.Edit");
    public bool CanAccessCostDelete => _authorizationService.HasPermission("Cost.Delete");
    
    public bool CanAccessCostCreateOrEdit => _authorizationService.HasPermission("Cost.Create") ||
                                             _authorizationService.HasPermission("Cost.Edit");
    public CostViewModel(
        IAuthorizationService authorizationService, 
        ICostService costService,
        ICurrencyService currencyService)
    {
        _authorizationService = authorizationService;
        _costService = costService;
        _currencyService = currencyService;
        Currencies = new ObservableCollection<CurrencyDto>();
        
        Task.Run(async () =>
        {
            await LoadCostAsync();
            await LoadCurrenciesAsync();
        }).GetAwaiter().GetResult();
    }
    
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    public double WeightBed
    {
        get => _weightBed;
        set => SetProperty(ref _weightBed, value);
    }
    public double WeightBes
    {
        get => _weightBes;
        set => SetProperty(ref _weightBes, value);
    }
    public long RiyalBed
    {
        get => _riyalBed;
        set => SetProperty(ref _riyalBed, value);
    }
    public long RiyalBes
    {
        get => _riyalBes;
        set => SetProperty(ref _riyalBes, value);
    }
    public long BranchId
    {
        get => _branchId;
        set => SetProperty(ref _branchId, value);
    }
    public ObservableCollection<CostDto> AllCost
    {
        get => _allCost;
        set => SetProperty(ref _allCost, value);
    }
    private async Task LoadCostAsync()
    {
        AllCost.Clear();
        // گرفتن لیست کاربران از سرویس
        var costs = await _costService.GetAllAsync();
        
        foreach (var b in costs)
        {
            AllCost.Add(b);
        }
    }
    private async Task LoadCurrenciesAsync()
    {
        Currencies.Clear();
        var currencies = await _currencyService.GetAllAsync();
        foreach (var c in currencies)
        {
            Currencies.Add(c);
        }
    }
    public async Task SaveCost()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingCostId.HasValue)
        {
            CostUpdateDto dto;
            dto = new CostUpdateDto
            {
                Id = _editingCostId.Value,
                Name = Name,
                Label = Label,
                Description = Description,
            };
            
            result = await _costService.UpdateAsync(dto);
            _editingCostId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("صندوق با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var costDto = new CostCreateDto
            {
                Name = Name,
                Label = Label,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
            };
            result = await _costService.AddAsync(costDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("صندوق با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadCostAsync();
    }
    
    public async Task EditAsync(long costId)
    {
        _editingCostId = costId;
        var costDto = await _costService.GetByIdAsync(costId);
        Name = costDto.Name;
        Label = costDto.Label;
        Description = costDto.Description;
    }
    
    public async Task DeleteAsync(long costId)
    {
        await _costService.DeleteAsync(costId);
    }
}