using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class MadeSubCategoryRepository : IMadeSubCategoryRepository
{
    private readonly AppDbContext _context;

    public MadeSubCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MadeSubCategory> GetByIdAsync(long id)
    {
        return await _context.MadeSubCategories.FindAsync(id);
    }

    public async Task<List<MadeSubCategory>> GetAllAsync()
    {
        return await _context.MadeSubCategories.ToListAsync();
    }

    public async Task AddAsync(MadeSubCategory madeSubCategory)
    {
        _context.MadeSubCategories.Add(madeSubCategory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MadeSubCategory madeSubCategory)
    {
        _context.MadeSubCategories.Update(madeSubCategory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MadeSubCategory madeSubCategory)
    {
        _context.MadeSubCategories.Remove(madeSubCategory);
        await _context.SaveChangesAsync();
    }
    
}