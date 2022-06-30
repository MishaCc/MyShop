using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Areas.Identity.Data;
using Shop.Models;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace Shop.Areas.Identity.Data;

public class ApplicationContext : IdentityDbContext<ShopUser>
{

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        
    }

    public DbSet<Product>? Product { get; set; }
    public DbSet<Basket>? Basket { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<DeviceType> DeviceType { get; set; }
    public DbSet<Specification> Specification { get; set; }
    public DbSet<UsersPostedProduct> PostedProducts { get; set; }
    public DbSet<SpecificationDescription> Descriptions { get; set; }
    public DbSet<Photo> Photo { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }

    
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ShopUser>
{
    void IEntityTypeConfiguration<ShopUser>.Configure(EntityTypeBuilder<ShopUser> builder)
    {
        builder.Property(u => u.LastName).HasMaxLength(255).IsRequired(false);
        builder.Property(u => u.UserLogin).HasMaxLength(255);
    }
}
