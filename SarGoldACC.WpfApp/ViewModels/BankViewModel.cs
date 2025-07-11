using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Bank;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class BankViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IBankService _bankService;
    private readonly ICurrencyService _currencyService;
    private long? _editingBankId = null;

    private string _name;
    private string _branch;
    private string _iban;
    private string _cardNumber;
    private string _accountNumber;
    private string _description;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private long _branchId;
    private long _currencyId;
    private ObservableCollection<BankDto> _allBanks = new();
    public ObservableCollection<CurrencyDto> Currencies { get; }
    public bool CanAccessBankView => _authorizationService.HasPermission("Bank.View");
    public bool CanAccessBankCreate => _authorizationService.HasPermission("Bank.Create");
    public bool CanAccessBankEdit => _authorizationService.HasPermission("Bank.Edit");
    public bool CanAccessBankDelete => _authorizationService.HasPermission("Bank.Delete");
    public bool CanAccessBankCreateOrEdit => _authorizationService.HasPermission("Bank.Create") ||
                                                 _authorizationService.HasPermission("Bank.Edit");
    public bool CanAccessCurrencyButton => _authorizationService.HasPermission("Currency.View") ||
                                           _authorizationService.HasPermission("Currency.Create") ||
                                           _authorizationService.HasPermission("Currency.Edit") ||
                                           _authorizationService.HasPermission("Currency.Delete");
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
    public BankViewModel(
        IAuthorizationService authorizationService, 
        IBankService bankService,
        ICurrencyService currencyService)
    {
        _authorizationService = authorizationService;
        _bankService = bankService;
        _currencyService = currencyService;
        Currencies = new ObservableCollection<CurrencyDto>();
        CurrencyId = 1;
        Task.Run(async () =>
        {
            await LoadBankAsync();
            await LoadCurrenciesAsync();
        }).GetAwaiter().GetResult();
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
    public string Branch
    {
        get => _branch;
        set
        {
            if (_branch != value)
            {
                _branch = value;
                OnPropertyChanged(nameof(Branch));
                ValidateAll();
            }
        }
    }

    public string Iban
    {
        get => _iban;
        set
        {
            if (_iban != value)
            {
                _iban = value;
                OnPropertyChanged(nameof(Iban));
                ValidateAll();
            }
        }
    }
    
    public string CardNumber
    {
        get => _cardNumber;
        set => SetProperty(ref _cardNumber, value);
    }

    public string AccountNumber
    {
        get => _accountNumber;
        set
        {
            if (_accountNumber != value)
            {
                _accountNumber = value;
                OnPropertyChanged(nameof(AccountNumber));
                ValidateAll();
            }
        }
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
    
    public ObservableCollection<BankDto> AllBanks
    {
        get => _allBanks;
        set => SetProperty(ref _allBanks, value);
    }
    
    private async Task LoadBankAsync()
    {
        AllBanks.Clear();
        // گرفتن لیست کاربران از سرویس
        var banks = await _bankService.GetAllAsync();
        
        foreach (var b in banks)
        {
            AllBanks.Add(b);
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
    
    public async Task SaveBank()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingBankId.HasValue)
        {
            BankUpdateDto dto;
            dto = new BankUpdateDto
            {
                Id = _editingBankId.Value,
                Name = Name,
                Branch = Branch,
                Iban = Iban,
                CardNumber = CardNumber,
                AccountNumber = AccountNumber,
                Description = Description,
            };
            
            result = await _bankService.UpdateAsync(dto);
            _editingBankId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("بانک با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var bankDto = new BankCreateDto
            {
                Name = Name,
                Branch = Branch,
                Iban = Iban,
                CardNumber = CardNumber,
                AccountNumber = AccountNumber,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
                CurrencyId = CurrencyId
            };
            result = await _bankService.AddAsync(bankDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("بانک با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadBankAsync();
    }
    
    public async Task EditAsync(long bankId)
    {
        _editingBankId = bankId;
        var bankDto = await _bankService.GetByIdAsync(bankId);
        Name = bankDto.Name;
        Branch = bankDto.Branch;
        Iban = bankDto.Iban;
        CardNumber = bankDto.CardNumber;
        AccountNumber = bankDto.AccountNumber;
        Description = bankDto.Description;
    }
    
    public async Task DeleteAsync(long bankId)
    {
        await _bankService.DeleteAsync(bankId);
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
            if (columnName == nameof(Branch))
            {
                if (string.IsNullOrWhiteSpace(Branch) || !Regex.IsMatch(Branch, @"^.+$"))
                    return true;
            }
            if (columnName == nameof(AccountNumber))
            {
                if (string.IsNullOrWhiteSpace(AccountNumber) || !Regex.IsMatch(AccountNumber, @"^(\d+)$"))
                    return true;
            }
            if (columnName == nameof(Iban))
            {
                if (string.IsNullOrWhiteSpace(Iban) || !Regex.IsMatch(Iban, @"^(\d{24})$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Name),
        nameof(Branch),
        nameof(AccountNumber),
        nameof(Iban)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    
}