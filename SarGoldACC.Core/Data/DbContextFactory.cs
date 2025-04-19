using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;

namespace SarGoldACC.Core.Data;

    public interface IDbContextFactory
    {
        AppDbContext CreateDbContext();
    }

    public class DbContextFactory : IDbContextFactory
    {
        private readonly string _connectionString;

        public DbContextFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

            _connectionString = connectionString;
        }

        public AppDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }