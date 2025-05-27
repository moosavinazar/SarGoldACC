using System.Collections.ObjectModel;
using System.ComponentModel;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Stores;

namespace SarGoldACC.WpfApp.ViewModels;

public class RcvMeltedViewModel : ViewModelBase
{
    private readonly ILaboratoryService _laboratoryService;
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
    public long LaboratoryId { get; set; }
    public ObservableCollection<LaboratoryDto> Laboratories { get; }
    public long BoxId { get; set; }
    public ObservableCollection<BoxDto> Boxes { get; }
    public string Ang { get; set; }
    public bool Certain { get; set; }
    public int Ayar { get; set; }
    public double Weight { get; set; }
    public string Description { get; set; }
    
    public bool CanAccessLaboratoryButton => _authorizationService.HasPermission("Laboratory.View") ||
                                             _authorizationService.HasPermission("Laboratory.Create") ||
                                             _authorizationService.HasPermission("Laboratory.Edit") ||
                                             _authorizationService.HasPermission("Laboratory.Delete");
    public bool CanAccessBoxButton => _authorizationService.HasPermission("Box.View") ||
                                      _authorizationService.HasPermission("Box.Create") ||
                                      _authorizationService.HasPermission("Box.Edit") ||
                                      _authorizationService.HasPermission("Box.Delete");

    public RcvMeltedViewModel(ICounterpartyService counterpartyService, ILaboratoryService laboratoryService, 
        IBoxService boxService, IAuthorizationService authorizationService)
    {
        _laboratoryService = laboratoryService;
        _boxService = boxService;
        _authorizationService = authorizationService;
        Laboratories = new ObservableCollection<LaboratoryDto>();
        Boxes = new ObservableCollection<BoxDto>();
        
        Task.Run(async () =>
        {
            await LoadLaboratoriesAsync();
            await LoadBoxesAsync();
        }).GetAwaiter().GetResult();
    }
    public async Task ReloadAllAsync()
    {
        await LoadLaboratoriesAsync();
        await LoadBoxesAsync();
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
        var boxes = await _boxService.GetAllAsync();
        foreach (var b in boxes)
        {
            Boxes.Add(b);
        }
    }
}