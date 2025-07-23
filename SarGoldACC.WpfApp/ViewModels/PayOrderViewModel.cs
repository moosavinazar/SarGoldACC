using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class PayOrderViewModel : ViewModelBase
{
    private readonly ICounterpartyService _counterpartyService;
    private readonly IAuthorizationService _authorizationService;
    private long _sideOneCounterPartyId;
    public long SideOneCounterPartyId
    {
        get => _sideOneCounterPartyId;
        set => SetProperty(ref _sideOneCounterPartyId, value);
    }
    private double _weightBed;
    public double WeightBed
    {
        get => _weightBed;
        set => SetProperty(ref _weightBed, value);
    }
    private long _riyalBed;
    public long RiyalBed
    {
        get => _riyalBed;
        set => SetProperty(ref _riyalBed, value);
    }
    private string _description;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    private  ObservableCollection<CounterpartyDto> _counterparties;
    public ObservableCollection<CounterpartyDto> Counterparties
    {
        get => _counterparties;
        set
        {
            _counterparties = value;
            OnPropertyChanged();
        }
    }
    private string _userImagePath = "pack://application:,,,/Resources/Icons/UserLarge.png";
    public string UserImagePath
    {
        get => _userImagePath;
        set
        {
            _userImagePath = value;
            OnPropertyChanged(nameof(UserImagePath));
        }
    }
    public bool IsCounterpartySelected => CounterpartyId != 0;
    private long _counterpartyId;
    public long CounterpartyId
    {
        get => _counterpartyId;
        set
        {
            if (_counterpartyId != value)
            {
                _counterpartyId = value;
                OnPropertyChanged();
                // بدون await فراخوانی async
                _ = LoadCounterpartyAsync(_counterpartyId);
            }
        }
    }
    private async Task LoadCounterpartyAsync(long id)
    {
        var counterparty = await _counterpartyService.GetByIdAsync(id);
        //TODO
        UserImagePath = counterparty.Customer?.Photo ?? "pack://application:,,,/Resources/Icons/UserLarge.png";
        OnPropertyChanged(nameof(IsCounterpartySelected));
    }
    public bool CanAccessCustomerButton => _authorizationService.HasPermission("Customer.View") ||
                                           _authorizationService.HasPermission("Customer.Create") ||
                                           _authorizationService.HasPermission("Customer.Edit") ||
                                           _authorizationService.HasPermission("Customer.Delete");
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
    public PayOrderViewModel(ICounterpartyService counterpartyService, IAuthorizationService authorizationService)
    {
        _counterpartyService = counterpartyService;
        _authorizationService = authorizationService;
        Counterparties = new ObservableCollection<CounterpartyDto>();
    }
    public async Task ReloadAllAsync()
    {
        await LoadCounterpatiesAsync();
    }
    private async Task LoadCounterpatiesAsync()
    {
        Counterparties.Clear();
        var counterparties = await _counterpartyService.GetAllAsync();
        foreach (var c in counterparties)
        {
            if (c.Id != SideOneCounterPartyId)
            {
                Counterparties.Add(c);
            }
        }
    }
}