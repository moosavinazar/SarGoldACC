using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.WpfApp.Configuration;

public static class Startup
{
    public static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUserService, UserService>();

        var serviceProvider = services.BuildServiceProvider();

        var userService = serviceProvider.GetRequiredService<IUserService>();
        
        return services.BuildServiceProvider();
    }
}