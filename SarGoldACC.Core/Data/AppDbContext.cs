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
    }
}