using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class MadeCategoryRepository : IMadeCategoryRepository
{
    private readonly AppDbContext _context;

    public MadeCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MadeCategory> GetByIdAsync(long id)
    {
        return await _context.MadeCategories.FindAsync(id);
    }

    public async Task<List<MadeCategory>> GetAllAsync()
    {
        return await _context.MadeCategories.ToListAsync();
    }

    public async Task AddAsync(MadeCategory madeCategory)
    {
        _context.MadeCategories.Add(madeCategory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MadeCategory madeCategory)
    {
        _context.MadeCategories.Update(madeCategory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MadeCategory madeCategory)
    {
        _context.MadeCategories.Remove(madeCategory);
        await _context.SaveChangesAsync();
    }
    
}