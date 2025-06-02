using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using PersianDateControlsPlus.PersianDate;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class CustomerViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICustomerService _customerService;
    private readonly ICityService _cityService;
    private long? _editingCustomerId = null;
    
    private string _name;
    private string _idCode;
    private string _phone;
    private string _cellPhone;
    private string _address;
    private string _photo;
    private BitmapImage _photoPreview;
    public BitmapImage PhotoPreview
    {
        get => _photoPreview;
        set
        {
            _photoPreview = value;
            OnPropertyChanged(nameof(PhotoPreview));
        }
    }
    private string _moaref;
    private DateTime? _birthDate;
    private PersianDate? _persianBirthDate;
    private string _email;
    private string _storeName;
    private double _weightLimit;
    private long _riyalLimit;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private string _description;
    private long _branchId;
    private long _cityId;
    private ObservableCollection<CustomerDto> _allCustomers = new();
    public ObservableCollection<CityDto> Cities { get; }
    
    
    public bool CanAccessCustomerView => _authorizationService.HasPermission("Customer.View");
    public bool CanAccessCustomerCreate => _authorizationService.HasPermission("Customer.Create");
    public bool CanAccessCustomerEdit => _authorizationService.HasPermission("Customer.Edit");
    public bool CanAccessCustomerDelete => _authorizationService.HasPermission("Customer.Delete");
    
    public bool CanAccessCustomerCreateOrEdit => _authorizationService.HasPermission("Customer.Create") ||
                                             _authorizationService.HasPermission("Customer.Edit");

    public CustomerViewModel(
        IAuthorizationService authorizationService, 
        ICustomerService customerService,
        ICityService cityService)
    {
        _authorizationService = authorizationService;
        _customerService = customerService;
        _cityService = cityService;
        Cities = new ObservableCollection<CityDto>();
        
        Task.Run(async () =>
        {
            await LoadCitiesAsync();
            await LoadCustomerAsync();
        }).GetAwaiter().GetResult();
    }

    public string Name
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
    
    public DateTime? BirthDate
    {
        get => _birthDate;
        set
        {
            if (SetProperty(ref _birthDate, value))
            {
                if (value.HasValue)
                {
                    PersianDate = new PersianDate(value.Value);
                }
                else
                {
                    PersianDate = null;
                }
            }
        }
    }


    public PersianDate? PersianDate
    {
        get => _persianBirthDate;
        set
        {
            if (SetProperty(ref _persianBirthDate, value))
            {
                // اگر مقدار PersianDate دارد، تبدیل به DateTime کن
                if (value.HasValue)
                {
                    BirthDate = value.Value.ToDateTime();
                }
                else
                {
                    BirthDate = null;
                }
            }
        }
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
    
    public long CityId
    {
        get => _cityId;
        set => SetProperty(ref _cityId, value);
    }
    
    public ObservableCollection<CustomerDto> AllCustomers
    {
        get => _allCustomers;
        set => SetProperty(ref _allCustomers, value);
    }
    
    public async Task SaveCustomer()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingCustomerId.HasValue)
        {
            CustomerUpdateDto dto;
            dto = new CustomerUpdateDto()
            {
                Id = _editingCustomerId.Value,
                Name = Name,
                IdCode = IdCode,
                CellPhone = CellPhone,
                Phone = Phone,
                Address = Address,
                Photo = Photo,
                Moaref = Moaref,
                BirthDate = BirthDate,
                Email = Email,
                StoreName = StoreName,
                WeightLimit = WeightLimit,
                RiyalLimit = RiyalLimit,
                Description = Description,
                CityId = CityId
            };
            
            result = await _customerService.UpdateAsync(dto);
            _editingCustomerId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("مشتری با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var customerDto = new CustomerCreateDto
            {
                Name = Name,
                IdCode = IdCode,
                Phone = Phone,
                CellPhone = CellPhone,
                Address = Address,
                Photo = Photo,
                Moaref = Moaref,
                BirthDate = BirthDate,
                Email = Email,
                StoreName = StoreName,
                WeightLimit = WeightLimit,
                RiyalLimit = RiyalLimit,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
                CityId = CityId
            };
            result = await _customerService.AddAsync(customerDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("کاربر با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadCustomerAsync();
    }
    
    public async Task EditAsync(long customerId)
    {
        _editingCustomerId = customerId;
        var customerDto = await _customerService.GetByIdAsync(customerId);
        Name = customerDto.Name;
        IdCode = customerDto.IdCode;
        Phone = customerDto.Phone;
        CellPhone = customerDto.CellPhone;
        Address = customerDto.Address;
        Photo = customerDto.Photo;
        Moaref = customerDto.Moaref;
        BirthDate = (DateTime)customerDto.BirthDate;
        Email = customerDto.Email;
        StoreName = customerDto.StoreName;
        WeightLimit = customerDto.WeightLimit;
        RiyalLimit = customerDto.RiyalLimit;
        Description = customerDto.Description;
    }
    
    public async Task DeleteAsync(long userId)
    {
        await _customerService.DeleteAsync(userId);
    }
    
    private async Task LoadCustomerAsync()
    {
        AllCustomers.Clear();
        // گرفتن لیست کاربران از سرویس
        var customers = await _customerService.GetAllAsync();
        
        foreach (var c in customers)
        {
            AllCustomers.Add(c);
        }
    }
    private async Task LoadCitiesAsync()
    {
        Cities.Clear();
        var cities = await _cityService.GetAllAsync();
        foreach (var c in cities)
        {
            Cities.Add(c);
        }
    }
}