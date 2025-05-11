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
            .HasOne(i => i.Document)
            .WithMany(d => d.Invoices)
            .HasForeignKey(i => i.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Counterparty)
            .WithMany(a => a.Invoices)
            .HasForeignKey(i => i.CounterpartyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InvoiceRow>()
            .HasOne(r => r.Invoice)
            .WithMany(i => i.InvoiceRows)
            .HasForeignKey(r => r.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        
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
        
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Counterparty)
            .WithOne(cp => cp.Customer)
            .HasForeignKey<Counterparty>(cp => cp.CustomerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<City>()
            .HasOne(c => c.Customer)
            .WithOne(cu => cu.City)
            .HasForeignKey<Customer>(cu => cu.CityId);
        
        modelBuilder.Entity<CustomerBank>()
            .HasOne(cb => cb.Customer)
            .WithMany(c => c.CustomerBanks)
            .HasForeignKey(cb => cb.CustomerId)
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
                    BranchId = c.BranchId
                }).ToArray()
        );
    }
}