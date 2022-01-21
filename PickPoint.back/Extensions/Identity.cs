using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PickPoint.back.EFCore;
using PickPoint.back.Models.CustomServiceUserModel;
using System;

namespace PickPoint.back.Extensions;

public static class Identity
{
  public static IdentityBuilder ConfigureIdentity(
    this IServiceCollection isc)
  {
    return isc.AddIdentityCore<CustomServiceUser>(x =>
    {
      x.Password.RequiredLength = 5;
      x.Password.RequireNonAlphanumeric = false;
      x.Password.RequireLowercase = false;
      x.Password.RequireUppercase = false;
      x.Password.RequireDigit = false;
    })
      .AddRoles<IdentityRole<Guid>>()
      .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
      .AddRoleValidator<RoleValidator<IdentityRole<Guid>>>()
      .AddEntityFrameworkStores<AppDbContext>();
  }
}
