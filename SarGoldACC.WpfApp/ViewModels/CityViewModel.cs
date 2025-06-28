using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;
using SarGoldACC.WpfApp.Views;

namespace SarGoldACC.WpfApp.ViewModels;

public class CityViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICityService _cityService;
    
    private long? _editingCityId = null;
    private string _cityName;
    public string CityName
    {
        get => _cityName;
        set
        {
            if (_cityName != value)
            {
                _cityName = value;
                OnPropertyChanged(nameof(CityName));
                ValidateAll();
            }
        }
    }
    
    private ObservableCollection<CityDto> _allCities = new();
    public ObservableCollection<CityDto> AllCities
    {
        get => _allCities;
        set => SetProperty(ref _allCities, value);
    }
    
    public bool CanAccessCityView => _authorizationService.HasPermission("City.View");
    public bool CanAccessCityCreate => _authorizationService.HasPermission("City.Create");
    public bool CanAccessCityEdit => _authorizationService.HasPermission("City.Edit");
    public bool CanAccessCityDelete => _authorizationService.HasPermission("City.Delete");
    public bool CanAccessCityCreateOrEdit => _authorizationService.HasPermission("City.Create") ||
                                              _authorizationService.HasPermission("City.Edit");
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

    // IDataErrorInfo
    public string Error => null;
    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(CityName))
            {
                if (string.IsNullOrWhiteSpace(CityName))
                    return "نام شهر الزامی است.";

                if (!Regex.IsMatch(CityName, @"^.+$"))
                    return "نام شهر الزامی است";
            }
            return null;
        }
    }
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => !string.IsNullOrWhiteSpace(this[p]));
        CanSave = !hasError;
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(CityName),
    };
    public void Clear()
    {
        _editingCityId = null;
    }
    public CityViewModel(IAuthorizationService authorizationService, 
        ICityService branchService)
    {
        _authorizationService = authorizationService;
        _cityService = branchService;
        AllCities = new ObservableCollection<CityDto>();
        
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadCitiesAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task SaveCity()
    {
        var result = new ResultDto
        {
            Success = false
        };
        if (_editingCityId.HasValue)
        {
            var dto = new CityDto
            {
                Id = _editingCityId.Value,
                Name = CityName
            };
            result = await _cityService.UpdateAsync(dto);
            _editingCityId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("شهر با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var branchCreate = new CityCreateDto
            {
                Name = CityName
            };
            result = await _cityService.AddAsync(branchCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("شهر با موفقیت ذخیره شد.");
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
        await _cityService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long branchId)
    {
        _editingCityId = branchId;
        var city = await _cityService.GetByIdAsync(branchId);
        CityName = city.Name;
    }
    
    private async Task LoadCitiesAsync()
    {
        AllCities.Clear();
        var groups = await _cityService.GetAllAsync();
        foreach (var g in groups)
        {
            AllCities.Add(g);
        }
    }

}