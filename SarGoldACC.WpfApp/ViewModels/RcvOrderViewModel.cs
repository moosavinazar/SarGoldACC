using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class RcvOrderViewModel : ViewModelBase
{
    private readonly ICounterpartyService _counterpartyService;
    private readonly IAuthorizationService _authorizationService;
    public long SideOneCounterPartyId { get; set; }
    public double WeightBes { get; set; }
    public long RiyalBes { get; set; }
    public string Description { get; set; }
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
    public RcvOrderViewModel(ICounterpartyService counterpartyService, IAuthorizationService authorizationService)
    {
        _counterpartyService = counterpartyService;
        _authorizationService = authorizationService;
        Counterparties = new ObservableCollection<CounterpartyDto>();
    }
    public async Task ReloadAllAsync()
    {
        await LoadCounterpartiesAsync();
    }
    private async Task LoadCounterpartiesAsync()
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