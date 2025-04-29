using System.IO;
using Microsoft.Extensions.Configuration;
using SarGoldACC.Core.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Repositories;
using SarGoldACC.Core.Services;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.Core.Services.Mapper;
using SarGoldACC.WpfApp.Stores;
using SarGoldACC.WpfApp.ViewModels;

namespace SarGoldACC.WpfApp
{
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }
        
        public static IServiceProvider ServiceProvider { get; private set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            
            base.OnStartup(e);
            
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var services = new ServiceCollection();
            
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthenticationRepository, AuthenticateRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            
            services.AddSingleton<NavigationStore>();
            // services.AddSingleton<MainViewModel>();
            
            services.AddSingleton<MainViewModel>(provider =>
            {
                var navigationStore = provider.GetRequiredService<NavigationStore>();
                var authService = provider.GetRequiredService<IAuthenticationService>();
                var authorizationService = provider.GetRequiredService<IAuthorizationService>();
                return new MainViewModel(navigationStore, authService, authorizationService);
            });

            ServiceProvider = services.BuildServiceProvider();
            
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
            
        }
    }
}