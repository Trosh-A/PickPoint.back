using Microsoft.EntityFrameworkCore;
using PickPoint.back.EFCore;
using PickPoint.back.Models.ParcelAutomatModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.ParcelAutomatsRepository;

public class ParcelAutomatsRepo : IParcelAutomatsRepo
{
  private readonly AppDbContext _ctx;
  public ParcelAutomatsRepo(AppDbContext ctx)
  {
    _ctx = ctx;
  }
  public async Task<IEnumerable<ParcelAutomat>> GetOrderedWorkingParcelAutomatsAsync()
  {
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
