using Microsoft.Extensions.Configuration;
using SarGoldACC.Core.Data;
using System.Windows;

namespace SarGoldACC.WpfApp
{
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }
        public static IDbContextFactory DbContextFactory { get; private set; }

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