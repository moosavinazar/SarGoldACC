using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Branch> GetByIdAsync(long id)
    {
        return await _context.Branches.FindAsync(id);
    }

    public async Task<List<Branch>> GetAllAsync()
    {
        return await _context.Branches.ToListAsync();
    }

    public async Task<Branch> AddAsync(Branch branch)
    {
        var result = _context.Branches.Add(branch);
        await _context.SaveChangesAsync();
        return result.Entity;
    }
    
    public Branch AddWithoutSave(Branch branch)
    {
        var result = _context.Branches.Add(branch);
        return result.Entity;
    }

    public async Task UpdateAsync(Branch branch)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Branch branch)
    {
        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync();
    }
}