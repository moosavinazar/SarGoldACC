using System.IO;
using Microsoft.Extensions.Configuration;
using SarGoldACC.Core.Data;
using System.Windows;
using SarGoldACC.Core.Services.Auth;

namespace SarGoldACC.WpfApp
{
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }
        public static IDbContextFactory DbContextFactory { get; private set; }
        
        public static IAuthorizationService AuthorizationService { get; private set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var connectionString = config.GetConnectionString("DefaultConnection");
            
            var contextFactory = new DbContextFactory(connectionString); // اگر داریش
            var dbContext = contextFactory.CreateDbContext();

            AuthorizationService = new AuthorizationService(dbContext);

            base.OnStartup(e);
        }

        public App()
        {
            // خواندن تنظیمات
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            // گرفتن ConnectionString
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            // ایجاد DbContextFactory
            DbContextFactory = new DbContextFactory(connectionString);
        }
    }
}