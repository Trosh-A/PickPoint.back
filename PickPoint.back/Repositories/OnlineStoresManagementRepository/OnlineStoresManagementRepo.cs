using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PickPoint.back.Constants;
using PickPoint.back.Models.CustomServiceUserModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.OnlineStoresManagementRepository;

#pragma warning disable CA2254 // Template should be a static expression

public class OnlineStoresManagementRepo : IOnlineStoresManagementRepo
{
  private readonly ILogger<OnlineStoresManagementRepo> _logger;
  private readonly UserManager<CustomServiceUser> _um;
  private readonly string? managerName;
  public OnlineStoresManagementRepo(
    UserManager<CustomServiceUser> um,
    IHttpContextAccessor httpContextAccessor,
    ILogger<OnlineStoresManagementRepo> logger)
  {
    _logger = logger;
    _um = um;
    managerName = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
  }
  public async Task<(CustomServiceUser store, string role, IEnumerable<string> errors)?> ChangeOnlineStoreStatusAsync(string INN, string role)
  {
    var store = await _um.FindByNameAsync(INN);
    if (store == null) return null;
    var currentRoles = await _um.GetRolesAsync(store);
    var umResult1 = await _um.RemoveFromRolesAsync(store, currentRoles);
    var umResult2 = await _um.AddToRoleAsync(store, role);
    if (umResult1.Succeeded && umResult2.Succeeded && managerName is not null)
    {
      _logger.LogInformation($"Менеджер {managerName} изменил статус онлайн магазина {INN} на {role}");
      return (store, role, errors: Enumerable.Empty<string>());
    }
    else
    {
      var errors = new List<string>();
      errors.AddRange(umResult1.Errors.Select(e => e.Description));
      errors.AddRange(umResult2.Errors.Select(e => e.Description));
      return (store, role, errors);
    }
  }
  public async Task<(CustomServiceUser store, string role, IEnumerable<string> errors)?> BanOnlineStoreAsync(string INN) =>
    await ChangeOnlineStoreStatusAsync(INN, RolesConstants.BANNED);

  public async Task<(CustomServiceUser store, string role, IEnumerable<string> errors)?> ApproveOnlineStoreAsync(string INN) =>
    await ChangeOnlineStoreStatusAsync(INN, RolesConstants.ACTIVE);
}
