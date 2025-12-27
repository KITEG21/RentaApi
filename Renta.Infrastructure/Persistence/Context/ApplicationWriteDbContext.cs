using System;
using System.Reflection;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Renta.Infrastructure.Persistence.Context;

public class ApplicationWriteDbContext : IdentityDbContext<User, Role, Guid>
{
    private readonly IDateTimeService _dateTime;

    public ApplicationWriteDbContext(DbContextOptions<ApplicationWriteDbContext> options,
        IDateTimeService dateTime) : base(options)
    {
        _dateTime = dateTime;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected ApplicationWriteDbContext()
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = new())
    {
        foreach (var entry in ChangeTracker.Entries<IEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = _dateTime.NowUtc;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = _dateTime.NowUtc;
                    break;
            }

        return base.SaveChangesAsync(ct);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = _dateTime.NowUtc;
                    entry.Entity.StatusBaseEntity = StatusEntityType.Active;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = _dateTime.NowUtc;
                    break;
            }

        return base.SaveChanges();
    }

    /// <summary>
    /// Clears the ChangeTracker to avoid entity tracking conflicts
    /// </summary>
    public void ClearChangeTracker()
    {
        ChangeTracker.Clear();
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