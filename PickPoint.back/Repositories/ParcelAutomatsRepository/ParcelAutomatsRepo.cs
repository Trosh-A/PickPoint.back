using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PickPoint.back.EFCore;
using PickPoint.back.Models.ParcelAutomatModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.ParcelAutomatsRepository;

public class ParcelAutomatsRepo : IParcelAutomatsRepo
{
  private readonly AppDbContext _ctx;
  private readonly Guid? storeGuid = null;
  public ParcelAutomatsRepo(AppDbContext ctx, IHttpContextAccessor httpContextAccessor)
  {
    _ctx = ctx;
    var storeGuidString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (Guid.TryParse(storeGuidString, out Guid storeGuidTemp)) storeGuid = storeGuidTemp;
  }
  public async Task<IEnumerable<ParcelAutomat>> GetOrderedWorkingParcelAutomatsAsync()
  {
    if (storeGuid is null) return Enumerable.Empty<ParcelAutomat>();
    return await _ctx.ParcelAutomats.AsNoTracking().Where(pa => pa.IsWorking).OrderBy(pa => pa.Index).ToListAsync();
  }
  //Ограничения, что получить информацию о конкретном постамате можно только, если он рабочий в задании нет.
  public async Task<ParcelAutomat?> GetParcelAutomatByIndexAsync(string index)
  {
    return await _ctx.ParcelAutomats.AsNoTracking().SingleOrDefaultAsync(pa => pa.Index == index);
  }
  //null вернётся если постомт закрыт или если его в принципе не существует
  public async Task<ParcelAutomat?> GetParcelAutomatIfAvaliable(string index)
  {
    return await _ctx.ParcelAutomats.SingleOrDefaultAsync(pa => pa.IsWorking && pa.Index == index);
  }
}
