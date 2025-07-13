using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
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
    private BitmapImage? _photoPreview;
    public byte[] PhotoBytes { get; set; }
    public string PhotoFileName { get; set; }
    public BitmapImage PhotoPreview
    {
        get => _photoPreview;
        set
        {
            _photoPreview = value;
            OnPropertyChanged(nameof(PhotoPreview));
        }
    }
    private ObservableCollection<LaboratoryDto> _allLaboratories = new();
    private ObservableCollection<CityDto> _cities;
    public ObservableCollection<CityDto> Cities
    {
        get => _cities;
        set => SetProperty(ref _cities, value);
    }
    public bool CanAccessLaboratoryView => _authorizationService.HasPermission("Laboratory.View");
    public bool CanAccessLaboratoryCreate => _authorizationService.HasPermission("Laboratory.Create");
    public bool CanAccessLaboratoryEdit => _authorizationService.HasPermission("Laboratory.Edit");
    public bool CanAccessLaboratoryDelete => _authorizationService.HasPermission("Laboratory.Delete");
    
    public bool CanAccessLaboratoryCreateOrEdit => _authorizationService.HasPermission("Laboratory.Create") ||
                                                 _authorizationService.HasPermission("Laboratory.Edit");
    public bool CanAccessCityButton => _authorizationService.HasPermission("City.View") ||
                                       _authorizationService.HasPermission("City.Create") ||
                                       _authorizationService.HasPermission("City.Edit") ||
                                       _authorizationService.HasPermission("City.Delete");
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
    public LaboratoryViewModel(
        IAuthorizationService authorizationService, 
        ILaboratoryService laboratoryService,
        ICityService cityService)
    {
        _authorizationService = authorizationService;
        _laboratoryService = laboratoryService;
        _cityService = cityService;
        Cities = new ObservableCollection<CityDto>();
        CityId = 1;
        Task.Run(async () =>
        {
            await LoadCitiesAsync();
            await LoadLaboratoryAsync();
        }).GetAwaiter().GetResult();
    }
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                ValidateAll();
            }
        }
    }
    public string Phone
    {
        get => _phone;
        set
        {
            if (_phone != value)
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
                ValidateAll();
            }
        }
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
                CityId = CityId,
                PhotoBytes = PhotoBytes,
                PhotoFileName = PhotoFileName
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
                CityId = CityId,
                PhotoBytes = PhotoBytes,
                PhotoFileName = PhotoFileName
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
        CityId = laboratoryDto.CityId;
        // بارگذاری تصویر
        if (!string.IsNullOrEmpty(Photo) && File.Exists(Photo))
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(Photo, UriKind.Absolute);
            bitmap.EndInit();
            bitmap.Freeze();

            PhotoPreview = bitmap;
        }
        else
        {
            PhotoPreview = null;
        }
    }
    
    public async Task DeleteAsync(long userId)
    {
        await _laboratoryService.DeleteAsync(userId);
    }
    public bool this[string columnName]
    {
        get
        {
            if (columnName == nameof(Phone))
            {
                if (string.IsNullOrWhiteSpace(Phone) || !Regex.IsMatch(Phone, @"^(\d+)$"))
                    return true;
            }
            if (columnName == nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name) || !Regex.IsMatch(Name, @"^.+$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Name),
        nameof(Phone)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    public void Clear()
    {
        _editingLaboratoryId = null;
    }
}