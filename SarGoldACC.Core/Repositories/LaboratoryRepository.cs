using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class LaboratoryRepository : ILaboratoryRepository
{
    private readonly AppDbContext _context;

    public LaboratoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Laboratory> GetByIdAsync(long id)
    {
        return await _context.Laboratories.FindAsync(id);
    }

    public async Task<List<Laboratory>> GetAllAsync()
    {
        return await _context.Laboratories.ToListAsync();
    }

    public async Task UpdateAsync(Laboratory laboratory)
    {
        _context.Laboratories.Update(laboratory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Laboratory laboratory)
    {
        _context.Laboratories.Remove(laboratory);
        await _context.SaveChangesAsync();
    }
}