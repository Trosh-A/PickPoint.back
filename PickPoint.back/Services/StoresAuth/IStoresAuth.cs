using PickPoint.back.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickPoint.back.Services.StoresAuth;

public interface IStoresAuth
{
  Task<(string INN, string Token, IEnumerable<string> roles, IEnumerable<string> errors)> LoginAsync(LoginQueryModel lqm);
  Task<(string INN, string Token, IEnumerable<string> roles, IEnumerable<string> errors)> RegisterAsync(RegistrationQueryModel rqm);
}
