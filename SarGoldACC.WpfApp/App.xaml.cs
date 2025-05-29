using System.Globalization;
using System.IO;
using Microsoft.Extensions.Configuration;
using SarGoldACC.Core.Data;
using System.Windows;
using System.Windows.Markup;
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
            services.AddScoped<ICityService, CityService>();
            
            services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
            services.AddScoped<ICounterpartyService, CounterpartyService>();
            
            services.AddScoped<IGeneralAccountRepository, GeneralAccountRepository>();
            services.AddScoped<IGeneralAccountService, GeneralAccountService>();
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IBankService, BankService>();
            
            services.AddScoped<IPosRepository, PosRepository>();
            services.AddScoped<IPosService, PosService>();
            
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentService, DocumentService>();
            
            services.AddScoped<ICashRepository, CashRepository>();
            services.AddScoped<ICashService, CashService>();
            
            services.AddScoped<ILaboratoryRepository, LaboratoryRepository>();
            services.AddScoped<ILaboratoryService, LaboratoryService>();
            
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IIncomeService, IncomeService>();
            
            services.AddScoped<ICostRepository, CostRepository>();
            services.AddScoped<ICostService, CostService>();
            
            services.AddScoped<IBoxRepository, BoxRepository>();
            services.AddScoped<IBoxService, BoxService>();
            
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceRowRepository, InvoiceRowRepository>();
            services.AddScoped<IOrderAmountRepository, OrderAmountRepository>();
            services.AddScoped<IMeltedRepository, MeltedRepository>();
            services.AddScoped<ISubMeltedRepository, SubMeltedRepository>();
            services.AddScoped<IMiscRepository, MiscRepository>();
            
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
            services.AddTransient<CityViewModel>();
            services.AddTransient<City>();
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<Customer>();
            services.AddTransient<BankViewModel>();
            services.AddTransient<Bank>();
            services.AddTransient<PosViewModel>();
            services.AddTransient<Pos>();
            services.AddTransient<CurrencyViewModel>();
            services.AddTransient<Currency>();
            services.AddTransient<CashViewModel>();
            services.AddTransient<Cash>();
            services.AddTransient<LaboratoryViewModel>();
            services.AddTransient<Laboratory>();
            services.AddTransient<IncomeViewModel>();
            services.AddTransient<Income>();
            services.AddTransient<CostViewModel>();
            services.AddTransient<Cost>();
            services.AddTransient<DocumentViewModel>();
            services.AddTransient<Document>();
            services.AddTransient<PayOrderViewModel>();
            services.AddTransient<PayOrder>();
            services.AddTransient<RcvOrderViewModel>();
            services.AddTransient<RcvOrder>();
            services.AddTransient<BoxViewModel>();
            services.AddTransient<Box>();
            services.AddTransient<RcvMeltedViewModel>();
            services.AddTransient<RcvMelted>();
            services.AddTransient<PayMeltedViewModel>();
            services.AddTransient<PayMelted>();
            services.AddTransient<MiscViewModel>();
            services.AddTransient<RcvMisc>();
            services.AddTransient<PayMisc>();

            ServiceProvider = services.BuildServiceProvider();
            
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainViewModel, ServiceProvider);
            mainWindow.Show();
            
        }
    }
}