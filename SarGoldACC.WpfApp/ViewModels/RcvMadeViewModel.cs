using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class RcvMadeViewModel : ViewModelBase
{
    private readonly IMadeSubCategoryService _madeSubCategoryService;
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
    private long _madeSubCategoryId;
    public long MadeSubCategoryId
    {
        get => _madeSubCategoryId;
        set => SetProperty(ref _madeSubCategoryId, value);
    }
    private long _boxId;
    public long BoxId
    {
        get => _boxId;
        set => SetProperty(ref _boxId, value);
    }
    private ObservableCollection<MadeSubCategoryDto> _madeSubCategories;
    public ObservableCollection<MadeSubCategoryDto> MadeSubCategories
    {
        get => _madeSubCategories;
        set
        {
            if (_madeSubCategories != value)
            {
                _madeSubCategories = value;
                OnPropertyChanged(nameof(MadeSubCategories));
                _ = UpdateNameAsync(); // بدون await چون setter نمی‌تونه async باشه
                ValidateAll();
            }
        }
    }
    private ObservableCollection<BoxDto> _boxes;
    public ObservableCollection<BoxDto> Boxes
    {
        get => _boxes;
        set => SetProperty(ref _boxes, value);
    }
    private string _name;
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
    private int _ayar;
    public int Ayar
    {
        get => _ayar;
        set => SetProperty(ref _ayar, value);
    }
    private double _weight;
    public double Weight
    {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }
    private double _weight750;
    public double Weight750
    {
        get => _weight750;
        set => SetProperty(ref _weight750, value);
    }
    private long _ojratR;
    public long OjratR
    {
        get => _ojratR;
        set => SetProperty(ref _ojratR, value);
    }
    private int _ojratP;
    public int OjratP
    {
        get => _ojratP;
        set => SetProperty(ref _ojratP, value);
    }
    private string _description;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _barcode;
    public string Barcode
    {
        get => _barcode;
        set => SetProperty(ref _barcode, value);
    }
    private string _photo;
    public string Photo
    {
        get => _photo;
        set => SetProperty(ref _photo, value);
    }
    private BitmapImage? _photoPreview;
    public BitmapImage PhotoPreview
    {
        get => _photoPreview;
        set
        {
            _photoPreview = value;
            OnPropertyChanged(nameof(PhotoPreview));
        }
    }
    public byte[] PhotoBytes { get; set; }
    public string PhotoFileName { get; set; }
    
    public bool CanAccessMadeSubCategoryButton => _authorizationService.HasPermission("MadeSubCategory.View") ||
                                             _authorizationService.HasPermission("MadeSubCategory.Create") ||
                                             _authorizationService.HasPermission("MadeSubCategory.Edit") ||
                                             _authorizationService.HasPermission("MadeSubCategory.Delete");
    public bool CanAccessBoxButton => _authorizationService.HasPermission("Box.View") ||
                                      _authorizationService.HasPermission("Box.Create") ||
                                      _authorizationService.HasPermission("Box.Edit") ||
                                      _authorizationService.HasPermission("Box.Delete");
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
    public RcvMadeViewModel(IMadeSubCategoryService madeSubCategoryService, 
        IBoxService boxService, IAuthorizationService authorizationService)
    {
        _madeSubCategoryService = madeSubCategoryService;
        _boxService = boxService;
        _authorizationService = authorizationService;
        MadeSubCategories = new ObservableCollection<MadeSubCategoryDto>();
        Boxes = new ObservableCollection<BoxDto>();
    }
    public async Task InitializeAsync()
    {
        await LoadMadeSubCategoriesAsync();
        await LoadBoxesAsync();
    }
    public async Task ReloadAllAsync()
    {
        await LoadMadeSubCategoriesAsync();
        await LoadBoxesAsync();
        MadeSubCategoryId = 1;
        BoxId = 1;
    }
    
    private async Task LoadMadeSubCategoriesAsync()
    {
        MadeSubCategories.Clear();
        var madeSubCategories = await _madeSubCategoryService.GetAllAsync();
        foreach (var m in madeSubCategories)
        {
            MadeSubCategories.Add(m);
        }
    }
    
    private async Task LoadBoxesAsync()
    {
        Boxes.Clear();
        var boxes = await _boxService.GetAllByTypeAsync(BoxType.Made);
        foreach (var b in boxes)
        {
            Boxes.Add(b);
        }
    }
    public bool this[string columnName]
    {
        get
        {
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
        nameof(Name)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
    private async Task UpdateNameAsync()
    {
        var madeSubCategory = await _madeSubCategoryService.GetByIdAsync(MadeSubCategoryId);
        Console.WriteLine("TEST");
        Console.WriteLine(madeSubCategory.Name);
        Name = "دریافت ساخته - " + madeSubCategory?.Name;
    }
}