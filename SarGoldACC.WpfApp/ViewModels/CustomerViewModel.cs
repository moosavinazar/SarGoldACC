using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class CustomerViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    
    private string _name;
    private string _idCode;
    private string _phone;
    private string _cellPhone;
    private string _address;
    private string _photo;
    private string _moaref;
    private DateTime _birthDate;
    private string _email;
    private string _storeName;
    private double _weightLimit;
    private long _riyalLimit;
    private string _description;
    private long _branchId;
    private ObservableCollection<CustomerDto> _allCustomers = new();
    
    
    public bool CanAccessCustomerView => _authorizationService.HasPermission("Customer.View");
    public bool CanAccessCustomerCreate => _authorizationService.HasPermission("Customer.Create");
    public bool CanAccessCustomerEdit => _authorizationService.HasPermission("Customer.Edit");
    public bool CanAccessCustomerDelete => _authorizationService.HasPermission("Customer.Delete");
    
    public bool CanAccessCustomerCreateOrEdit => _authorizationService.HasPermission("Customer.Create") ||
                                             _authorizationService.HasPermission("Customer.Edit");

    public CustomerViewModel(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public string UserName
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    
    public string IdCode
    {
        get => _idCode;
        set => SetProperty(ref _idCode, value);
    }
    
    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }
    
    public string CellPhone
    {
        get => _cellPhone;
        set => SetProperty(ref _cellPhone, value);
    }
    
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
    
    public string Photo
    {
        get => _photo;
        set => SetProperty(ref _photo, value);
    }
    
    public string Moaref
    {
        get => _moaref;
        set => SetProperty(ref _moaref, value);
    }
    
    public DateTime BirthDate
    {
        get => _birthDate;
        set => SetProperty(ref _birthDate, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string StoreName
    {
        get => _storeName;
        set => SetProperty(ref _storeName, value);
    }

    public double WeightLimit
    {
        get => _weightLimit;
        set => SetProperty(ref _weightLimit, value);
    }

    public long RiyalLimit
    {
        get => _riyalLimit;
        set => SetProperty(ref _riyalLimit, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public long BranchId
    {
        get => _branchId;
        set => SetProperty(ref _branchId, value);
    }
    
    public ObservableCollection<CustomerDto> AllCustomers
    {
        get => _allCustomers;
        set => SetProperty(ref _allCustomers, value);
    }
}