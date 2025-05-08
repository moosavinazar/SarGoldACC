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
using SarGoldACC.WpfApp.Views;

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
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IGroupService, GroupService>();
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticateRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IBranchService, BranchService>();
            
            services.AddScoped<ICityRepository, CityRepository>();
            
            
            services.AddSingleton<NavigationStore>();
            // services.AddSingleton<MainViewModel>();
            
            services.AddSingleton<MainViewModel>(provider =>
            {
                var navigationStore = provider.GetRequiredService<NavigationStore>();
                var authService = provider.GetRequiredService<IAuthenticationService>();
                var authorizationService = provider.GetRequiredService<IAuthorizationService>();
                return new MainViewModel(navigationStore, authService, authorizationService);
            });
            
            services.AddTransient<GroupViewModel>();
            services.AddTransient<Group>();
            services.AddTransient<BranchViewModel>();
            services.AddTransient<Branch>();
            services.AddTransient<UserViewModel>();
            services.AddTransient<User>();

            ServiceProvider = services.BuildServiceProvider();
            
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainViewModel, ServiceProvider);
            mainWindow.Show();
            
        }
    }
}