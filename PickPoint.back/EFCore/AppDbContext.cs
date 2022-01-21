#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickPoint.back.EFCore.Configurations;
using PickPoint.back.Models;
using PickPoint.back.Models.CustomServiceUserModel;
using PickPoint.back.Models.GoodModel;
using PickPoint.back.Models.OrderModel;
using PickPoint.back.Models.ParcelAutomatModel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PickPoint.back.EFCore;

public class AppDbContext : IdentityDbContext<CustomServiceUser, IdentityRole<Guid>, Guid>
{
  public DbSet<Order> Orders { get; set; }
  public DbSet<Good> Goods { get; set; }
  public DbSet<ParcelAutomat> ParcelAutomats { get; set; }
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
    Database.EnsureCreated();
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParcelAutomatConfiguration).Assembly);
  }
  public override int SaveChanges()
  {
    AddTimeStamps();
    return base.SaveChanges();
  }
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    AddTimeStamps();
    return await base.SaveChangesAsync(cancellationToken);
  }
  private void AddTimeStamps()
  {
    var newEntities = ChangeTracker.Entries()
        .Where(
            x => x.State == EntityState.Added &&
            x.Entity != null &&
            x.Entity as ITimeStamped != null
            )
        .Select(x => x.Entity as ITimeStamped);

    var modifiedEntities = ChangeTracker.Entries()
        .Where(
            x => x.State == EntityState.Modified &&
            x.Entity != null &&
            x.Entity as ITimeStamped != null
            )
        .Select(x => x.Entity as ITimeStamped);

    foreach (var newEntity in newEntities)
    {
      newEntity.CreatedAt = DateTime.UtcNow;
      newEntity.UpdatedAt = DateTime.UtcNow;
    }

    foreach (var modifiedEntity in modifiedEntities)
    {
      modifiedEntity.UpdatedAt = DateTime.UtcNow;
    }
  }
}
