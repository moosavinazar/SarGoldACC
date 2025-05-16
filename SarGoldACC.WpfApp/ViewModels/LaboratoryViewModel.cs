using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class LaboratoryViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ILaboratoryService _laboratoryService;
    private readonly ICityService _cityService;
    private long? _editingLaboratoryId = null;
    
    private string _name;
    private string _photo;
    private string _phone;
    private string _cellPhone;
    private string _ivrPhone;
    private double _weightBed;
    private double _weightBes;
    private long _riyalBed;
    private long _riyalBes;
    private string _description;
    private long _branchId;
    private long _cityId;
    private ObservableCollection<LaboratoryDto> _allLaboratories = new();
    public ObservableCollection<CityDto> Cities { get; }
    public bool CanAccessLaboratoryView => _authorizationService.HasPermission("Laboratory.View");
    public bool CanAccessLaboratoryCreate => _authorizationService.HasPermission("Laboratory.Create");
    public bool CanAccessLaboratoryEdit => _authorizationService.HasPermission("Laboratory.Edit");
    public bool CanAccessLaboratoryDelete => _authorizationService.HasPermission("Laboratory.Delete");
    
    public bool CanAccessLaboratoryCreateOrEdit => _authorizationService.HasPermission("Laboratory.Create") ||
                                                 _authorizationService.HasPermission("Laboratory.Edit");
    public LaboratoryViewModel(
        IAuthorizationService authorizationService, 
        ILaboratoryService laboratoryService,
        ICityService cityService)
    {
        _authorizationService = authorizationService;
        _laboratoryService = laboratoryService;
        _cityService = cityService;
        Cities = new ObservableCollection<CityDto>();
        
        Task.Run(async () =>
        {
            await LoadCitiesAsync();
            await LoadLaboratoryAsync();
        }).GetAwaiter().GetResult();
    }
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
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
    public string IVRPhone
    {
        get => _ivrPhone;
        set => SetProperty(ref _ivrPhone, value);
    }
    public string Photo
    {
        get => _photo;
        set => SetProperty(ref _photo, value);
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
    public ObservableCollection<LaboratoryDto> AllLaboratories
    {
        get => _allLaboratories;
        set => SetProperty(ref _allLaboratories, value);
    }
    private async Task LoadLaboratoryAsync()
    {
        AllLaboratories.Clear();
        // گرفتن لیست کاربران از سرویس
        var laboratorys = await _laboratoryService.GetAllAsync();
        
        foreach (var c in laboratorys)
        {
            AllLaboratories.Add(c);
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
    public async Task SaveLaboratory()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingLaboratoryId.HasValue)
        {
            LaboratoryUpdateDto dto;
            dto = new LaboratoryUpdateDto()
            {
                Id = _editingLaboratoryId.Value,
                Name = Name,
                CellPhone = CellPhone,
                IVRPhone = IVRPhone,
                Phone = Phone,
                Photo = Photo,
                Description = Description,
                CityId = CityId
            };
            
            result = await _laboratoryService.UpdateAsync(dto);
            _editingLaboratoryId = null;
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
            var laboratoryDto = new LaboratoryCreateDto
            {
                Name = Name,
                Phone = Phone,
                CellPhone = CellPhone,
                IVRPhone = IVRPhone,
                Photo = Photo,
                Description = Description,
                WeightBed = WeightBed,
                WeightBes = WeightBes,
                RiyalBed = RiyalBed,
                RiyalBes = RiyalBes,
                CityId = CityId
            };
            result = await _laboratoryService.AddAsync(laboratoryDto);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("کاربر با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadLaboratoryAsync();
    }
    
    public async Task EditAsync(long laboratoryId)
    {
        _editingLaboratoryId = laboratoryId;
        var laboratoryDto = await _laboratoryService.GetByIdAsync(laboratoryId);
        Name = laboratoryDto.Name;
        Phone = laboratoryDto.Phone;
        CellPhone = laboratoryDto.CellPhone;
        IVRPhone = laboratoryDto.IVRPhone;
        Photo = laboratoryDto.Photo;
        Description = laboratoryDto.Description;
    }
    
    public async Task DeleteAsync(long userId)
    {
        await _laboratoryService.DeleteAsync(userId);
    }
}