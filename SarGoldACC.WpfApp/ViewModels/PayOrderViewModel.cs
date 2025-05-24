using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class PayOrderViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICustomerService _customerService;
    
    public long CustomerId { get; set; }
    public double WeightBed { get; set; }
    public long RiyalBed { get; set; }

    
    public ObservableCollection<CustomerDto> Customers { get; }


    public PayOrderViewModel(IAuthorizationService authorizationService, ICustomerService customerService)
    {
        _authorizationService = authorizationService;
        _customerService = customerService;
        Customers = new ObservableCollection<CustomerDto>();
        
        Task.Run(async () =>
        {
            await LoadCustomersAsync();
        }).GetAwaiter().GetResult();
    }
    
    private async Task LoadCustomersAsync()
    {
        Customers.Clear();
        var customers = await _customerService.GetAllAsync();
        foreach (var c in customers)
        {
            Customers.Add(c);
        }
    }
}