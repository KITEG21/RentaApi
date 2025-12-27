using System;
using System.Reflection;
using Renta.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Renta.Infrastructure.Persistence.Context;

public class ApplicationReadDbContext : IdentityDbContext<User, Role, Guid>
{

    public ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected ApplicationReadDbContext()
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();

        builder.Entity<User>().ToTable("User").Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Entity<Role>().ToTable("Role").Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");
        builder.Entity<IdentityUserClaim<Guid>>();
        builder.Entity<IdentityRoleClaim<Guid>>().Property(rc => rc.Id).ValueGeneratedOnAdd();
    }
}