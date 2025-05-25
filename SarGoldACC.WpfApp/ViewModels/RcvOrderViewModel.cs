using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class RcvOrderViewModel : ViewModelBase
{
    private readonly ICounterpartyService _counterpartyService;
    
    public long CounterpartyId { get; set; }
    public double WeightBes { get; set; }
    public long RiyalBes { get; set; }
    public ObservableCollection<CounterpartyDto> Counterparties { get; }
    
    public RcvOrderViewModel(ICounterpartyService counterpartyService)
    {
        _counterpartyService = counterpartyService;
        Counterparties = new ObservableCollection<CounterpartyDto>();
        
        Task.Run(async () =>
        {
            await LoadCustomersAsync();
        }).GetAwaiter().GetResult();
    }
    
    private async Task LoadCustomersAsync()
    {
        Counterparties.Clear();
        var counterparties = await _counterpartyService.GetAllAsync();
        foreach (var c in counterparties)
        {
            Counterparties.Add(c);
        }
    }
}