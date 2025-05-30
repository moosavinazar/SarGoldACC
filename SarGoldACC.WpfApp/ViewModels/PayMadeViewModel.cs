using System.Collections.ObjectModel;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs.Made;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class PayMadeViewModel : ViewModelBase
{
    private readonly IMadeRepository _madeRepository;
    private readonly AppDbContext _dbContext;
    public string Description { get; set; }
    public ObservableCollection<MadePayDataGridDto> MadeItems { get; set; } = new();

    public PayMadeViewModel(IMadeRepository madeRepository, AppDbContext dbContext)
    {
        _madeRepository = madeRepository;
        _dbContext = dbContext;
        Task.Run(async () =>
        {
            await LoadMadeAsync();
        }).GetAwaiter().GetResult();
    }
    private async Task LoadMadeAsync()
    {
        MadeItems.Clear();
        var Mades = await _madeRepository.GetAllWithDetailAsync();
        foreach (var m in Mades)
        {
            MadeItems.Add(new MadePayDataGridDto
            {
                Id = m.Id,
                Name = m.Name,
                Ayar = m.Ayar,
                Weight = m.Weight,
                Weight750 = m.Weight750,
                Barcode = m.Barcode,
                Photo = m.Photo,
                OjratP = m.OjratP,
                OjratR = m.OjratR,
                Description = m.Description,
                MadeSubCategoryId = m.MadeSubCategoryId,
                MadeSubCategoryName = m.MadeSubCategory.Name,
                MadeCategoryId = m.MadeSubCategory.MadeCategoryId,
                MadeCategoryName = m.MadeSubCategory.MadeCategory.Name,
                BoxId = m.Box.Id,
                BoxName = m.Box.Name,
            });
        }
    }
}