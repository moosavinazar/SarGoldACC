using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Auth.Models;
using SarGoldACC.Core.Data.SeedData;
using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupPermission> GroupPermissions { get; set; }
    public DbSet<Branch> Branches { get; set; }
    
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
                    Name = g.Name
                }).ToArray()
        );
        
        // Permission
        var permissions = CsvDataReader.ReadPermissions();
        modelBuilder.Entity<Permission>().HasData(
            permissions.Select(p => 
                new Permission
                {
                    Id = p.Id, 
                    Name = p.Name
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
                    GroupId = u.GroupId,
                    BranchId = u.BranchId
                }).ToArray()
        );
    }
}