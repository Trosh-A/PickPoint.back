using PickPoint.back.Models.ParcelAutomatModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.ParcelAutomatsRepository;

public interface IParcelAutomatsRepo
{
  Task<IEnumerable<ParcelAutomat>> GetOrderedWorkingParcelAutomatsAsync();
  Task<ParcelAutomat?> GetParcelAutomatByIndexAsync(string index);
  Task<ParcelAutomat?> GetParcelAutomatIfAvaliable(string index);
}
