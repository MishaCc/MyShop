﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Areas.Identity.Data;

namespace Shop.Areas.Identity.Data;

public class ApplicationContext : IdentityDbContext<ShopUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

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
