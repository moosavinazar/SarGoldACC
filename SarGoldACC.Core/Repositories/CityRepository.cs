using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<City> GetByIdAsync(long id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task AddAsync(City branch)
    {
        _context.Cities.Add(branch);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(City branch)
    {
        _context.Cities.Update(branch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(City branch)
    {
        _context.Cities.Remove(branch);
        await _context.SaveChangesAsync();
    }
}