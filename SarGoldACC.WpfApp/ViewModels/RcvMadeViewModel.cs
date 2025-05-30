using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class RcvMadeViewModel : ViewModelBase
{
    private readonly IMadeSubCategoryService _madeSubCategoryService;
    private readonly IBoxService _boxService;
    private readonly IAuthorizationService _authorizationService;
    public long MadeSubCategoryId { get; set; }
    public ObservableCollection<MadeSubCategoryDto> MadeSubCategories { get; }
    public long BoxId { get; set; }
    public ObservableCollection<BoxDto> Boxes { get; }
    public string Name { get; set; }
    public int Ayar { get; set; }
    public double Weight { get; set; }
    public double? Weight750 { get; set; }
    public string Barcode { get; set; }
    public string Photo { get; set; }
    public long? OjratR { get; set; }
    public double? OjratP { get; set; }
    public string Description { get; set; }
    
    public bool CanAccessMadeSubCategoryButton => _authorizationService.HasPermission("MadeSubCategory.View") ||
                                             _authorizationService.HasPermission("MadeSubCategory.Create") ||
                                             _authorizationService.HasPermission("MadeSubCategory.Edit") ||
                                             _authorizationService.HasPermission("MadeSubCategory.Delete");
    public bool CanAccessBoxButton => _authorizationService.HasPermission("Box.View") ||
                                      _authorizationService.HasPermission("Box.Create") ||
                                      _authorizationService.HasPermission("Box.Edit") ||
                                      _authorizationService.HasPermission("Box.Delete");

    public RcvMadeViewModel(IMadeSubCategoryService madeSubCategoryService, 
        IBoxService boxService, IAuthorizationService authorizationService)
    {
        _madeSubCategoryService = madeSubCategoryService;
        _boxService = boxService;
        _authorizationService = authorizationService;
        MadeSubCategories = new ObservableCollection<MadeSubCategoryDto>();
        Boxes = new ObservableCollection<BoxDto>();
        
        Task.Run(async () =>
        {
            await LoadMadeSubCategoriesAsync();
            await LoadBoxesAsync();
        }).GetAwaiter().GetResult();
    }
    public async Task ReloadAllAsync()
    {
        await LoadMadeSubCategoriesAsync();
        await LoadBoxesAsync();
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
        var boxes = await _boxService.GetAllAsync();
        foreach (var b in boxes)
        {
            Boxes.Add(b);
        }
    }
}