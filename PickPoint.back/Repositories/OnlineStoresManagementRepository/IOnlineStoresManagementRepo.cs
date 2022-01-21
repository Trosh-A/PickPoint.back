using PickPoint.back.Models.CustomServiceUserModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.OnlineStoresManagementRepository;

public interface IOnlineStoresManagementRepo
{
  Task<(CustomServiceUser store, string role, IEnumerable<string> errors)?> ApproveOnlineStoreAsync(string INN);
  Task<(CustomServiceUser store, string role, IEnumerable<string> errors)?> BanOnlineStoreAsync(string INN);
  Task<(CustomServiceUser store, string role, IEnumerable<string> errors)?> ChangeOnlineStoreStatusAsync(string INN, string role);
}
