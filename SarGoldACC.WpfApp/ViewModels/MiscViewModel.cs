using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class MiscViewModel : ViewModelBase
{
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
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
    public MiscViewModel(IBoxService boxService, IAuthorizationService authorizationService)
    {
        _boxService = boxService;
        _authorizationService = authorizationService;
        Boxes = new ObservableCollection<BoxDto>();
        
        Task.Run(async () =>
        {
            await LoadBoxesAsync();
        }).GetAwaiter().GetResult();
    }
    public async Task InitializeAsync()
    {
        await LoadBoxesAsync();
    }
    public async Task ReloadAllAsync()
    {
        await LoadBoxesAsync();
        BoxId = 1;
    }
   
    private async Task LoadBoxesAsync()
    {
        Boxes.Clear();
        var boxes = await _boxService.GetAllByTypeAsync(BoxType.Misc);
        foreach (var b in boxes)
        {
            Boxes.Add(b);
        }
    }
}