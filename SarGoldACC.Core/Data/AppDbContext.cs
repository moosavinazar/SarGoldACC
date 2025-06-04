using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data.SeedData;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupPermission> GroupPermissions { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<City> Cities { get; set; }
    
    public DbSet<InvoiceRow> InvoiceRows { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Counterparty> Counterparties { get; set; }
    public DbSet<GeneralAccount> GeneralAccounts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerBank> CustomerBanks { get; set; }
    public DbSet<OrderAmount> OrderAmounts { get; set; }
    public DbSet<Cheque> Cheques { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Pos> Poses { get; set; }
    public DbSet<Cash> Cash { get; set; }
    public DbSet<Laboratory> Laboratories { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Cost> Costs { get; set; }
    public DbSet<Melted> Melteds { get; set; }
    public DbSet<SubMelted> SubMelteds { get; set; }
    public DbSet<Box> Boxes { get; set; }
    public DbSet<Misc> Miscs { get; set; }
    public DbSet<MadeCategory> MadeCategories { get; set; }
    public DbSet<MadeSubCategory> MadeSubCategories { get; set; }
    public DbSet<Made> Mades { get; set; }
    public DbSet<CoinCategory> CoinCategories { get; set; }
    public DbSet<Coin> Coins { get; set; }
    public DbSet<Setting> Settings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GroupPermission>()
            .HasKey(gp => new { gp.GroupId, gp.PermissionId });
        modelBuilder.Entity<GroupPermission>()
            .HasOne(gp => gp.Group)
            .WithMany(g => g.GroupPermissions)
            .HasForeignKey(gp => gp.GroupId);
        modelBuilder.Entity<GroupPermission>()
            .HasOne(gp => gp.Permission)
            .WithMany(p => p.GroupPermissions)
            .HasForeignKey(gp => gp.PermissionId);
        
        modelBuilder.Entity<UserGroup>()
            .HasKey(ug => new { ug.UserId, ug.GroupId });
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.UserId);
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(ug => ug.UserGroups)
            .HasForeignKey(ug => ug.GroupId);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Branch)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Counterparty)
            .WithMany(a => a.Invoices)
            .HasForeignKey(i => i.CounterpartyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Document)
            .WithMany(d => d.Invoices)
            .HasForeignKey(i => i.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InvoiceRow>()
            .HasOne(r => r.Invoice)
            .WithMany(i => i.InvoiceRows)
            .HasForeignKey(r => r.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<InvoiceRow>()
            .HasOne(ir => ir.OrderAmount)
            .WithMany(o => o.InvoiceRows)
            .HasForeignKey(ir => ir.OrderAmountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InvoiceRow>()
            .HasOne(ir => ir.SubMelted)
            .WithMany(s => s.InvoiceRows)
            .HasForeignKey(ir => ir.SubMeltedId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InvoiceRow>()
            .HasOne(ir => ir.Misc)
            .WithMany(m => m.InvoiceRows)
            .HasForeignKey(ir => ir.MiscId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InvoiceRow>()
            .HasOne(ir => ir.Made)
            .WithMany(m => m.InvoiceRows)
            .HasForeignKey(ir => ir.MadeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InvoiceRow>()
            .HasOne(ir => ir.Coin)
            .WithMany(c => c.InvoiceRows)
            .HasForeignKey(ir => ir.CoinId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.GeneralAccount)
            .WithMany(g => g.Counterparties)
            .HasForeignKey(c => c.GeneralAccountId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Branch)
            .WithMany(b => b.Counterparties)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Box>()
            .HasOne(b => b.Branch)
            .WithMany(br => br.Boxes)
            .HasForeignKey(b => b.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Melted>()
            .HasOne(m => m.Laboratory)
            .WithMany(l => l.Melteds)
            .HasForeignKey(m => m.LaboratoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Made>()
            .HasOne(m => m.MadeSubCategory)
            .WithMany(s => s.Mades)
            .HasForeignKey(m => m.MadeSubCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Coin>()
            .HasOne(c => c.CoinCategory)
            .WithMany(cc => cc.Coins)
            .HasForeignKey(c => c.CoinCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<SubMelted>()
            .HasOne(s => s.Melted)
            .WithMany(m => m.SubMelteds)
            .HasForeignKey(s => s.MeltedId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<SubMelted>()
            .HasOne(s => s.Box)
            .WithMany(b => b.SubMelteds)
            .HasForeignKey(s => s.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Misc>()
            .HasOne(m => m.Box)
            .WithMany(b => b.Miscs)
            .HasForeignKey(m => m.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Made>()
            .HasOne(m => m.Box)
            .WithMany(b => b.Mades)
            .HasForeignKey(m => m.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Coin>()
            .HasOne(c => c.Box)
            .WithMany(b => b.Coins)
            .HasForeignKey(c => c.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*modelBuilder.Entity<Customer>()
            .HasOne(c => c.Counterparty)
            .WithOne(cp => cp.Customer)
            .HasForeignKey<Counterparty>(cp => cp.CustomerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);*/
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Customer)
            .WithOne(cp => cp.Counterparty)
            .HasForeignKey<Counterparty>(cp => cp.CustomerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Bank)
            .WithOne(b => b.Counterparty)
            .HasForeignKey<Counterparty>(c => c.BankId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Pos)
            .WithOne(p => p.Counterparty)
            .HasForeignKey<Counterparty>(c => c.PosId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Cash)
            .WithOne(ca => ca.Counterparty)
            .HasForeignKey<Counterparty>(c => c.CashId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Income)
            .WithOne(i => i.Counterparty)
            .HasForeignKey<Counterparty>(c => c.IncomeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Cost)
            .WithOne(co => co.Counterparty)
            .HasForeignKey<Counterparty>(c => c.CostId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Counterparty>()
            .HasOne(c => c.Laboratory)
            .WithOne(l => l.Counterparty)
            .HasForeignKey<Counterparty>(c => c.LaboratoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Cheque>()
            .HasOne(ch => ch.Drawer)
            .WithMany(c => c.Drawers)
            .HasForeignKey(ch => ch.DrawerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Cheque>()
            .HasOne(ch => ch.Payee)
            .WithMany(c => c.Payees)
            .HasForeignKey(ch => ch.PayeeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.City)
            .WithMany(ci => ci.Customers)
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Laboratory>()
            .HasOne(l => l.City)
            .WithMany(c => c.Laboratories)
            .HasForeignKey(l => l.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CustomerBank>()
            .HasOne(cb => cb.Customer)
            .WithMany(c => c.CustomerBanks)
            .HasForeignKey(cb => cb.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Bank>()
            .HasOne(b => b.Currency)
            .WithMany(c => c.Banks)
            .HasForeignKey(b => b.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Cash>()
            .HasOne(c => c.Currency)
            .WithMany(cu => cu.Cash)
            .HasForeignKey(c => c.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Pos>()
            .HasOne(p => p.Bank)
            .WithMany(b => b.Pos)
            .HasForeignKey(b => b.BankId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<MadeSubCategory>()
            .HasOne(sc => sc.MadeCategory)
            .WithMany(c => c.MadeSubCategories)
            .HasForeignKey(sc => sc.MadeCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // خواندن داده‌ها از فایل‌های CSV
        // Branch
        var branches = CsvDataReader.ReadBranches();
        modelBuilder.Entity<Branch>().HasData(
            branches.Select(b => 
                new Branch
                {
                    Id = b.Id, 
                    Name = b.Name
                }).ToArray()
        );
        
        // Group
        var groups = CsvDataReader.ReadGroups();
        modelBuilder.Entity<Group>().HasData(
            groups.Select(g => 
                new Group
                {
                    Id = g.Id, 
                    Name = g.Name,
                    Label = g.Label
                }).ToArray()
        );
        
        // Permission
        var permissions = CsvDataReader.ReadPermissions();
        modelBuilder.Entity<Permission>().HasData(
            permissions.Select(p => 
                new Permission
                {
                    Id = p.Id, 
                    Name = p.Name,
                    Label = p.Label
                }).ToArray()
        );
        
        // GroupPermission
        var groupPermissions = CsvDataReader.ReadGroupPermissions();
        modelBuilder.Entity<GroupPermission>().HasData(
            groupPermissions.Select(gp => 
                new GroupPermission
                {
                    GroupId = gp.GroupId, 
                    PermissionId = gp.PermissionId
                }).ToArray()
        );
        
        // User
        var users = CsvDataReader.ReadUsers();
        modelBuilder.Entity<User>().HasData(
            users.Select(u => 
                new User
                {
                    Id = u.Id, 
                    Username = u.Username,
                    Password = u.Password,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    BranchId = u.BranchId
                }).ToArray()
        );
        
        // UserGroup
        var userGroup = CsvDataReader.ReadUserGroup();
        modelBuilder.Entity<UserGroup>().HasData(
            userGroup.Select(ug => 
                new UserGroup
                {
                    UserId = ug.UserId,
                    GroupId = ug.GroupId
                }).ToArray()
        );
        
        // City
        var cities = CsvDataReader.ReadCity();
        modelBuilder.Entity<City>().HasData(
            cities.Select(c =>
                new City
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToArray()
        );
        
        // GeneralAccount
        var generalAccounts = CsvDataReader.ReadGeneralAccount();
        modelBuilder.Entity<GeneralAccount>().HasData(
            generalAccounts.Select(g => 
                new GeneralAccount
                {
                    Id = g.Id, 
                    Title = g.Title
                }).ToArray()
        );
        
        // CounterParty
        var counterParties = CsvDataReader.ReadCounterparty();
        modelBuilder.Entity<Counterparty>().HasData(
            counterParties.Select(c => 
                new Counterparty
                {
                    Id = c.Id,
                    GeneralAccountId = c.GeneralAccountId,
                    CustomerId = c.CustomerId,
                    CashId = c.CashId,
                    BranchId = c.BranchId
                }).ToArray()
        );
        
        // Currency
        var currency = CsvDataReader.ReadCurrency();
        modelBuilder.Entity<Currency>().HasData(
            currency.Select(c => 
                new Currency
                {
                    Id = c.Id,
                    Name = c.Name,
                    Label = c.Label
                }).ToArray()
        );
        
        // Cash
        var cash = CsvDataReader.ReadCash();
        modelBuilder.Entity<Cash>().HasData(
            cash.Select(c => 
                new Cash
                {
                    Id = c.Id,
                    Name = c.Name,
                    Label = c.Label,
                    CurrencyId = c.CurrencyId,
                    Description = c.Description
                }).ToArray()
        );
        
        // Setting
        var setting = CsvDataReader.ReadSetting();
        modelBuilder.Entity<Setting>().HasData(
            setting.Select(c => 
                new Setting
                {
                    Id = c.Id,
                    CustomerImageUrl = c.CustomerImageUrl
                }).ToArray()
        );
    }
}