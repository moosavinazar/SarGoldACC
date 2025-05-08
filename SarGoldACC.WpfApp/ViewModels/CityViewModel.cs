using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class CityViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICityService _branchService;
    
    private long? _editingCityId = null;
    private string _branchName;
    public string CityName
    {
        get => _branchName;
        set => SetProperty(ref _branchName, value);
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

    public CityViewModel(IAuthorizationService authorizationService, 
        ICityService branchService)
    {
        _authorizationService = authorizationService;
        _branchService = branchService;
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
            result = await _branchService.UpdateAsync(dto);
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
            result = await _branchService.AddAsync(branchCreate);
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
        await _branchService.DeleteAsync(groupId);
    }

    public async Task EditAsync(long branchId)
    {
        _editingCityId = branchId;
        var city = await _branchService.GetByIdAsync(branchId);
        CityName = city.Name;
    }
    
    private async Task LoadCitiesAsync()
    {
        AllCities.Clear();
        var groups = await _branchService.GetAllAsync();
        foreach (var g in groups)
        {
            AllCities.Add(g);
        }
    }

}