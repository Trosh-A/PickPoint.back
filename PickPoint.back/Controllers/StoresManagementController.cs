using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickPoint.back.Constants;
using PickPoint.back.Repositories.OnlineStoresManagementRepository;
using System.Linq;
using System.Threading.Tasks;

namespace PickPoint.back.Controllers;

[Authorize(Roles = RolesConstants.PICKPOINT_MANAGER)]
[ApiVersion("1.0")]
[Route("api")]
[Produces("application/json")]
[ApiController]
public class StoresManagementController : ControllerBase
{
  private readonly IOnlineStoresManagementRepo _storeRepo;
  public StoresManagementController(IOnlineStoresManagementRepo storeRepo)
  {
    _storeRepo = storeRepo;
  }

  [HttpPost("approve-online-store/{inn}")]
  public async Task<IActionResult> ApproveOnlineStoreStatusAsync(string inn)
  {
    if (string.IsNullOrWhiteSpace(inn)) return BadRequest();
    var ost = await _storeRepo.ApproveOnlineStoreAsync(inn);
    if (ost is not null)
    {
      var (store, _, errors) = ost.Value;
      if (errors.Any()) return BadRequest(errors);
      else return Ok(new { store, isApproved = true });
    }
    return BadRequest();
  }

  [HttpPost("ban-online-store/{inn}")]
  public async Task<IActionResult> BanOnlineStoreStatusAsync(string inn)
  {
    if (string.IsNullOrWhiteSpace(inn)) return BadRequest();
    var ost = await _storeRepo.BanOnlineStoreAsync(inn);
    if (ost is not null)
    {
      var (store, _, errors) = ost.Value;
      if (errors.Any()) return BadRequest(errors);
      else return Ok(new { store, isBanned = true });
    }
    return BadRequest();
  }
}
