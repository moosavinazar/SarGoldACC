using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IDocumentService
{
    Task<ResultDto> AddCounterpartyOpeningEntry(OrderDto openingEntry);
    Task<ResultDto> AddOrderEntry(DocumentItemListDto documentItemList, DocumentType type);
}