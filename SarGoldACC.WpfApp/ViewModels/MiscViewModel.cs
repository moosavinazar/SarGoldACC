using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class MiscViewModel
{
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
    public long BoxId { get; set; }
    public ObservableCollection<BoxDto> Boxes { get; }
    public bool Certain { get; set; }
    public int Ayar { get; set; } = 750;
    public double Weight { get; set; }
    public double Weight750 { get; set; }
    public string Description { get; set; }
    public bool CanAccessBoxButton => _authorizationService.HasPermission("Box.View") ||
                                      _authorizationService.HasPermission("Box.Create") ||
                                      _authorizationService.HasPermission("Box.Edit") ||
                                      _authorizationService.HasPermission("Box.Delete");
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
    public async Task ReloadAllAsync()
    {
        await LoadBoxesAsync();
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