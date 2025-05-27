using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class PayOrderViewModel : ViewModelBase
{
    private readonly ICounterpartyService _counterpartyService;
    private readonly IAuthorizationService _authorizationService;
    public long CounterpartyId { get; set; }
    public double WeightBed { get; set; }
    public long RiyalBed { get; set; }
    public string Description { get; set; }
    public ObservableCollection<CounterpartyDto> Counterparties { get; }
    public bool CanAccessCustomerButton => _authorizationService.HasPermission("Customer.View") ||
                                           _authorizationService.HasPermission("Customer.Create") ||
                                           _authorizationService.HasPermission("Customer.Edit") ||
                                           _authorizationService.HasPermission("Customer.Delete");
    public PayOrderViewModel(ICounterpartyService counterpartyService, IAuthorizationService authorizationService)
    {
        _counterpartyService = counterpartyService;
        _authorizationService = authorizationService;
        Counterparties = new ObservableCollection<CounterpartyDto>();
        
        Task.Run(async () =>
        {
            await LoadCounterpatiesAsync();
        }).GetAwaiter().GetResult();
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
            Counterparties.Add(c);
        }
    }
}