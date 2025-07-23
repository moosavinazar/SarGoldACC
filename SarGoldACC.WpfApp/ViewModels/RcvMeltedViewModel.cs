using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class RcvMeltedViewModel : ViewModelBase
{
    private readonly ILaboratoryService _laboratoryService;
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
    private long _laboratoryId;
    public long LaboratoryId
    {
        get => _laboratoryId;
        set => SetProperty(ref _laboratoryId, value);
    }
    private ObservableCollection<LaboratoryDto> _laboratories;
    public ObservableCollection<LaboratoryDto> Laboratories
    {
        get => _laboratories;
        set => SetProperty(ref _laboratories, value);
    }
    private long _boxId;
    public long BoxId
    {
        get => _boxId;
        set => SetProperty(ref _boxId, value);
    }
    private ObservableCollection<BoxDto> _boxes;
    public ObservableCollection<BoxDto> Boxes
    {
        get => _boxes;
        set => SetProperty(ref _boxes, value);
    }
    private string _ang;
    public string Ang
    {
        get => _ang;
        set
        {
            if (_ang != value)
            {
                _ang = value;
                OnPropertyChanged(nameof(Ang));
                ValidateAll();
            }
        }
    }
    private bool _certain;
    public bool Certain
    {
        get => _certain;
        set => SetProperty(ref _certain, value);
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
    private string _description;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    
    public bool CanAccessLaboratoryButton => _authorizationService.HasPermission("Laboratory.View") ||
                                             _authorizationService.HasPermission("Laboratory.Create") ||
                                             _authorizationService.HasPermission("Laboratory.Edit") ||
                                             _authorizationService.HasPermission("Laboratory.Delete");
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
    public RcvMeltedViewModel(ILaboratoryService laboratoryService, 
        IBoxService boxService, IAuthorizationService authorizationService)
    {
        _laboratoryService = laboratoryService;
        _boxService = boxService;
        _authorizationService = authorizationService;
        Laboratories = new ObservableCollection<LaboratoryDto>();
        Boxes = new ObservableCollection<BoxDto>();
    }
    public async Task InitializeAsync()
    {
        await LoadLaboratoriesAsync();
        await LoadBoxesAsync();
    }
    public async Task ReloadAllAsync()
    {
        await LoadLaboratoriesAsync();
        await LoadBoxesAsync();
        LaboratoryId = 1;
        BoxId = 1;
    }
    
    private async Task LoadLaboratoriesAsync()
    {
        Laboratories.Clear();
        var laboratories = await _laboratoryService.GetAllAsync();
        foreach (var l in laboratories)
        {
            Laboratories.Add(l);
        }
    }
    
    private async Task LoadBoxesAsync()
    {
        Boxes.Clear();
        var boxes = await _boxService.GetAllByTypeAsync(BoxType.Melted);
        foreach (var b in boxes)
        {
            Boxes.Add(b);
        }
    }
    public bool this[string columnName]
    {
        get
        {
            if (columnName == nameof(Ang))
            {
                if (string.IsNullOrWhiteSpace(Ang) || !Regex.IsMatch(Ang, @"^.+$"))
                    return true;
            }
            return false;
        }
    }
    private readonly string[] _validatedProperties = new[]
    {
        nameof(Ang)
    };
    private void ValidateAll()
    {
        bool hasError = _validatedProperties.Any(p => this[p]);
        CanSave = !hasError;
    }
}