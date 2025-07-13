using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Bank;
using SarGoldACC.Core.DTOs.Pos;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class PosViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IPosService _posService;
    private readonly IBankService _bankService;
    private long? _editingPosId = null;

    private string _name;
    private string _code;
    private string _description;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private long _bankId;
    private ObservableCollection<PosDto> _allPoses = new();
    private ObservableCollection<BankDto> _banks;
    public ObservableCollection<BankDto> Banks
    {
        get => _banks;
        set => SetProperty(ref _banks, value);
    }
    
    public bool CanAccessPosView => _authorizationService.HasPermission("Pos.View");
    public bool CanAccessPosCreate => _authorizationService.HasPermission("Pos.Create");
    public bool CanAccessPosEdit => _authorizationService.HasPermission("Pos.Edit");
    public bool CanAccessPosDelete => _authorizationService.HasPermission("Pos.Delete");
    
    public bool CanAccessPosCreateOrEdit => _authorizationService.HasPermission("Pos.Create") ||
                                                 _authorizationService.HasPermission("Pos.Edit");
    
    public bool CanAccessBankButton => _authorizationService.HasPermission("Bank.Create") ||
                                       _authorizationService.HasPermission("Bank.Edit");
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
    public PosViewModel(
        IAuthorizationService authorizationService, 
        IPosService posService,
        IBankService bankService)
    {
        _authorizationService = authorizationService;
        _posService = posService;
        _bankService = bankService;
        Banks = new ObservableCollection<BankDto>();
        
        Task.Run(async () =>
        {
            await LoadBanksAsync();
            await LoadPosAsync();
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
    public string Code
    {
        get => _code;
        set
        {
            if (_code != value)
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
                ValidateAll();
            }
        }
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
    
    public long BankId
    {
        get => _bankId;
        set => SetProperty(ref _bankId, value);
    }
    
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    
    public ObservableCollection<PosDto> AllPoses
    {
        get => _allPoses;
        set => SetProperty(ref _allPoses, value);
    }
    
    public async Task SavePos()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingPosId.HasValue)
        {
            PosUpdateDto dto;
            dto = new PosUpdateDto
            {
                Id = _editingPosId.Value,
                Name = Name,
                Code = Code,
                Description = Description,
            };
            
            result = await _posService.UpdateAsync(dto);
            _editingPosId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("پوز بانکی با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var customerDto = new PosCreateDto
            {
                Name = Name,
                Code = Code,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
                BankId = BankId
            };
            result = await _posService.AddAsync(customerDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("پوز بانکی با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadPosAsync();
    }
    
    public async Task EditAsync(long customerId)
    {
        _editingPosId = customerId;
        var customerDto = await _posService.GetByIdAsync(customerId);
        Name = customerDto.Name;
        Code = customerDto.Code;
        Description = customerDto.Description;
    }
    
    public async Task DeleteAsync(long userId)
    {
        await _posService.DeleteAsync(userId);
    }
    
    private async Task LoadPosAsync()
    {
        AllPoses.Clear();
        // گرفتن لیست کاربران از سرویس
        var customers = await _posService.GetAllAsync();
        
        foreach (var c in customers)
        {
            AllPoses.Add(c);
        }
    }
    private async Task LoadBanksAsync()
    {
        Banks.Clear();
        var banks = await _bankService.GetAllAsync();
        foreach (var b in banks)
        {
            Banks.Add(b);
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
            if (columnName == nameof(Code))
            {
                if (string.IsNullOrWhiteSpace(Code) || !Regex.IsMatch(Code, @"^.+$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Name),
        nameof(Code)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingPosId = null;
    }
}