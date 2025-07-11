using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class CurrencyViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrencyService _currencyService;
    
    private long? _editingCurrencyId = null;
    private string _name;
    private string _label;
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
    public string Label
    {
        get => _label;
        set
        {
            if (_label != value)
            {
                _label = value;
                OnPropertyChanged(nameof(Label));
                ValidateAll();
            }
        }
    }
    
    private ObservableCollection<CurrencyDto> _allCurrencies = new();
    public ObservableCollection<CurrencyDto> AllCurrencies
    {
        get => _allCurrencies;
        set => SetProperty(ref _allCurrencies, value);
    }
    
    public bool CanAccessCurrencyView => _authorizationService.HasPermission("Currency.View");
    public bool CanAccessCurrencyCreate => _authorizationService.HasPermission("Currency.Create");
    public bool CanAccessCurrencyEdit => _authorizationService.HasPermission("Currency.Edit");
    public bool CanAccessCurrencyDelete => _authorizationService.HasPermission("Currency.Delete");
    public bool CanAccessCurrencyCreateOrEdit => _authorizationService.HasPermission("Currency.Create") ||
                                              _authorizationService.HasPermission("Currency.Edit");
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
    public CurrencyViewModel(IAuthorizationService authorizationService, 
        ICurrencyService currencyService)
    {
        _authorizationService = authorizationService;
        _currencyService = currencyService;
        AllCurrencies = new ObservableCollection<CurrencyDto>();
        
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadCitiesAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task SaveCurrency()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingCurrencyId.HasValue)
        {
            var dto = new CurrencyDto
            {
                Id = _editingCurrencyId.Value,
                Name = Name,
                Label = Label
            };
            result = await _currencyService.UpdateAsync(dto);
            _editingCurrencyId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("واحد پول با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var branchCreate = new CurrencyCreateDto
            {
                Name = Name,
                Label = Label
            };
            result = await _currencyService.AddAsync(branchCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("واحد پول با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadCitiesAsync();
    }
    
    public async Task DeleteAsync(long groupId)
    {
        await _currencyService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long branchId)
    {
        _editingCurrencyId = branchId;
        var currency = await _currencyService.GetByIdAsync(branchId);
        Name = currency.Name;
        Label = currency.Label;
    }
    
    private async Task LoadCitiesAsync()
    {
        AllCurrencies.Clear();
        var groups = await _currencyService.GetAllAsync();
        foreach (var g in groups)
        {
            AllCurrencies.Add(g);
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
            if (columnName == nameof(Label))
            {
                if (string.IsNullOrWhiteSpace(Label) || !Regex.IsMatch(Label, @"^.+$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Name),
        nameof(Label)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingCurrencyId = null;
    }
}