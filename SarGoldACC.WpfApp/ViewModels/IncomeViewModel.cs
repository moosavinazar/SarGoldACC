using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Income;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class IncomeViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IIncomeService _incomeService;
    private readonly ICurrencyService _currencyService;
    private long? _editingIncomeId = null;
    
    private string _name;
    private string _label;
    private string _description;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private long _branchId;
    private ObservableCollection<IncomeDto> _allIncome = new();
    public ObservableCollection<CurrencyDto> Currencies { get; }
    public bool CanAccessIncomeView => _authorizationService.HasPermission("Income.View");
    public bool CanAccessIncomeCreate => _authorizationService.HasPermission("Income.Create");
    public bool CanAccessIncomeEdit => _authorizationService.HasPermission("Income.Edit");
    public bool CanAccessIncomeDelete => _authorizationService.HasPermission("Income.Delete");
    
    public bool CanAccessIncomeCreateOrEdit => _authorizationService.HasPermission("Income.Create") ||
                                             _authorizationService.HasPermission("Income.Edit");
    public IncomeViewModel(
        IAuthorizationService authorizationService, 
        IIncomeService incomeService,
        ICurrencyService currencyService)
    {
        _authorizationService = authorizationService;
        _incomeService = incomeService;
        _currencyService = currencyService;
        Currencies = new ObservableCollection<CurrencyDto>();
        
        Task.Run(async () =>
        {
            await LoadIncomeAsync();
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
    public ObservableCollection<IncomeDto> AllIncome
    {
        get => _allIncome;
        set => SetProperty(ref _allIncome, value);
    }
    private async Task LoadIncomeAsync()
    {
        AllIncome.Clear();
        // گرفتن لیست کاربران از سرویس
        var incomes = await _incomeService.GetAllAsync();
        
        foreach (var b in incomes)
        {
            AllIncome.Add(b);
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
    public async Task SaveIncome()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingIncomeId.HasValue)
        {
            IncomeUpdateDto dto;
            dto = new IncomeUpdateDto
            {
                Id = _editingIncomeId.Value,
                Name = Name,
                Label = Label,
                Description = Description,
            };
            
            result = await _incomeService.UpdateAsync(dto);
            _editingIncomeId = null;
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
            var incomeDto = new IncomeCreateDto
            {
                Name = Name,
                Label = Label,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
            };
            result = await _incomeService.AddAsync(incomeDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("صندوق با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadIncomeAsync();
    }
    
    public async Task EditAsync(long incomeId)
    {
        _editingIncomeId = incomeId;
        var incomeDto = await _incomeService.GetByIdAsync(incomeId);
        Name = incomeDto.Name;
        Label = incomeDto.Label;
        Description = incomeDto.Description;
    }
    
    public async Task DeleteAsync(long incomeId)
    {
        await _incomeService.DeleteAsync(incomeId);
    }
}