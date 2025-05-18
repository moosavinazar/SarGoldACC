using System.Collections;

namespace SarGoldACC.Core.DTOs.Document;

public class DocumentItemListDto
{
    public DocumentItemListDto(ICollection<DocumentItemDto> documentItems)
    {
        DocumentItems = documentItems;
    }

    public long CounterpartySideOneId { get; set; }
    public ICollection<DocumentItemDto> DocumentItems { get; set; }
}