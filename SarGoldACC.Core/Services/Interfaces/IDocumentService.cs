using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Document;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IDocumentService
{
    Task<ResultDto> AddCounterpartyOpeningEntry(CounterPartyOpeningEntryDto openingEntry);
}