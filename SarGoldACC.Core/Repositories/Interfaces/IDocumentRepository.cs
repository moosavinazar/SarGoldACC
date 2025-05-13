using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IDocumentRepository
{
    Task<Document> GetByIdAsync(long id);
    Task<List<Document>> GetAllAsync();
    Task<Document> AddAsync(Document document);
    Document AddWithoutSave(Document document);
    Task UpdateAsync(Document document);
    Task DeleteAsync(Document document);
}