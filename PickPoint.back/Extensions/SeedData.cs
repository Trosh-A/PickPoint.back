using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using PickPoint.back.EFCore;
using PickPoint.back.Models.CustomServiceUserModel;
using System;
using System.Threading.Tasks;

namespace PickPoint.back.Extensions;

public static class SeedData
{
  public static async Task AddSeedDataAsync(this WebApplication wa, Logger logger)
  {
    try
    {
      using var scope = wa.Services.CreateScope();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomServiceUser>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
      var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      await DbInitializer.SeedDataAsync(userManager, roleManager, ctx);
    }
    catch (Exception ex)
    {
      logger.Error(ex, "Stopped program because of seeding exception");
      throw;
    }
  }
}
