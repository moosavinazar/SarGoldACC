using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.DTOs.Document;

public class DocumentCreateDto
{
    public DateTime Date { get; set; }
    public DocumentType Type { get; set; }
    public string Description { get; set; }
}