using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Cash;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class CashViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICashService _cashService;
    private readonly ICurrencyService _currencyService;
    private long? _editingCashId = null;
    
    private string _name;
    private string _label;
    private string _description;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private long _branchId;
    private long _currencyId;
    private ObservableCollection<CashDto> _allCash = new();
    public ObservableCollection<CurrencyDto> Currencies { get; }
    public bool CanAccessCashView => _authorizationService.HasPermission("Cash.View");
    public bool CanAccessCashCreate => _authorizationService.HasPermission("Cash.Create");
    public bool CanAccessCashEdit => _authorizationService.HasPermission("Cash.Edit");
    public bool CanAccessCashDelete => _authorizationService.HasPermission("Cash.Delete");
    
    public bool CanAccessCashCreateOrEdit => _authorizationService.HasPermission("Cash.Create") ||
                                             _authorizationService.HasPermission("Cash.Edit");
    public CashViewModel(
        IAuthorizationService authorizationService, 
        ICashService cashService,
        ICurrencyService currencyService)
    {
        _authorizationService = authorizationService;
        _cashService = cashService;
        _currencyService = currencyService;
        Currencies = new ObservableCollection<CurrencyDto>();
        
        Task.Run(async () =>
        {
            await LoadCashAsync();
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
    public long CurrencyId
    {
        get => _currencyId;
        set => SetProperty(ref _currencyId, value);
    }
    public ObservableCollection<CashDto> AllCash
    {
        get => _allCash;
        set => SetProperty(ref _allCash, value);
    }
    private async Task LoadCashAsync()
    {
        AllCash.Clear();
        // گرفتن لیست کاربران از سرویس
        var cashs = await _cashService.GetAllAsync();
        
        foreach (var b in cashs)
        {
            AllCash.Add(b);
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
    public async Task SaveCash()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingCashId.HasValue)
        {
            CashUpdateDto dto;
            dto = new CashUpdateDto
            {
                Id = _editingCashId.Value,
                Name = Name,
                Label = Label,
                Description = Description,
            };
            
            result = await _cashService.UpdateAsync(dto);
            _editingCashId = null;
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
            var cashDto = new CashCreateDto
            {
                Name = Name,
                Label = Label,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
                CurrencyId = CurrencyId
            };
            result = await _cashService.AddAsync(cashDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("صندوق با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadCashAsync();
    }
    
    public async Task EditAsync(long cashId)
    {
        _editingCashId = cashId;
        var cashDto = await _cashService.GetByIdAsync(cashId);
        Name = cashDto.Name;
        Label = cashDto.Label;
        Description = cashDto.Description;
    }
    
    public async Task DeleteAsync(long cashId)
    {
        await _cashService.DeleteAsync(cashId);
    }
}