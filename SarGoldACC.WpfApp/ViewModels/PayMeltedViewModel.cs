using System.Collections.ObjectModel;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs.SubMeltedDto;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.WpfApp.ViewModels;

public class PayMeltedViewModel : ViewModelBase
{
    private readonly ISubMeltedRepository _subMeltedRepository;
    private readonly AppDbContext _dbContext;
    public string Description { get; set; }
    public ObservableCollection<SubMeltedPayDataGridDto> SubMeltedItems { get; set; } = new();

    public PayMeltedViewModel(ISubMeltedRepository subMeltedRepository, AppDbContext dbContext)
    {
        _subMeltedRepository = subMeltedRepository;
        _dbContext = dbContext;
        Task.Run(async () =>
        {
            await LoadSubMeltedAsync();
        }).GetAwaiter().GetResult();
    }
    private async Task LoadSubMeltedAsync()
    {
        SubMeltedItems.Clear();
        var subMelteds = await _subMeltedRepository.GetAllWithDetailAsync();
        foreach (var s in subMelteds)
        {
            SubMeltedItems.Add(new SubMeltedPayDataGridDto
            {
                Id = s.Id,
                Ang = s.Melted.Ang,
                Certain = s.Melted.Certain,
                Ayar = s.Melted.Ayar,
                Weight = s.Weight,
                LaboratoryId = s.Melted.Laboratory.Id,
                LaboratoryName = s.Melted.Laboratory.Name,
                BoxId = s.Box.Id,
                BoxName = s.Box.Name,
            });
        }
    }
}