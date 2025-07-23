using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class BoxRepository : IBoxRepository
{
    private readonly AppDbContext _context;

    public BoxRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Box> GetByIdAsync(long id)
    {
        return await _context.Boxes.FindAsync(id);
    }

    public async Task<List<Box>> GetAllAsync()
    {
        return await _context.Boxes.ToListAsync();
    }
    public async Task<List<Box>> GetAllByTypeAsync(BoxType type)
    {
        return await _context.Boxes
            .Where(b => b.Type == type)
            .ToListAsync();
    }
    
    public async Task AddAsync(Box box)
    {
        _context.Boxes.Add(box);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Box box)
    {
        _context.Boxes.Update(box);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Box box)
    {
        _context.Boxes.Remove(box);
        await _context.SaveChangesAsync();
    }
}