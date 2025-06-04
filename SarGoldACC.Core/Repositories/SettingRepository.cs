using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class SettingRepository : ISettingRepository
{
    private readonly AppDbContext _context;

    public SettingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Setting> GetByIdAsync(long id)
    {
        return await _context.Settings.FindAsync(id);
    }

    public async Task<List<Setting>> GetAllAsync()
    {
        return await _context.Settings.ToListAsync();
    }

    public async Task AddAsync(Setting setting)
    {
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Setting setting)
    {
        _context.Settings.Update(setting);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Setting setting)
    {
        _context.Settings.Remove(setting);
        await _context.SaveChangesAsync();
    }
}