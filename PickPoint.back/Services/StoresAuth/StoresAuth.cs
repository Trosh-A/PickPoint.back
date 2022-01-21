using Microsoft.AspNetCore.Identity;
using PickPoint.back.Constants;
using PickPoint.back.Models;
using PickPoint.back.Models.CustomServiceUserModel;
using PickPoint.back.Services.JwtGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PickPoint.back.Services.StoresAuth;
//EXPLAIN Сейчас единая логика регистрации и аутентификации как магазинов, так и менеджеров,
//При реальном применении эту логику нужно разделить
public class StoresAuth : IStoresAuth
{
  private readonly UserManager<CustomServiceUser> _userManager;
  private readonly IJwtGenerator _jwtGenerator;

  public StoresAuth(UserManager<CustomServiceUser> userManager, IJwtGenerator jwtGenerator)
  {
    _userManager = userManager;
    _jwtGenerator = jwtGenerator;
  }
  public async Task<(string INN, string Token, IEnumerable<string> roles, IEnumerable<string> errors)> LoginAsync(LoginQueryModel lqm)
  {
    var store = await _userManager.FindByNameAsync(lqm.INN);
    if (await _userManager.CheckPasswordAsync(store, lqm.Password))
    {
      var roles = await _userManager.GetRolesAsync(store);
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, store.Id.ToString())
      };
      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }
      return (lqm.INN, _jwtGenerator.CreateToken(claims, TimeSpan.FromDays(1)), roles, Enumerable.Empty<string>());
    }
    var errors = new List<string>() { "Некорректный логин/пароль" };
    return (string.Empty, string.Empty, Enumerable.Empty<string>(), errors);
  }

  public async Task<(string INN, string Token, IEnumerable<string> roles, IEnumerable<string> errors)> RegisterAsync(RegistrationQueryModel rqm)
  {
    var newStore = new CustomServiceUser(rqm.INN);
    var createResult = await _userManager.CreateAsync(newStore, rqm.Password);
    var addToRoleResult = await _userManager.AddToRoleAsync(newStore, RolesConstants.UNDER_CONSIDERATION);
    var roles = await _userManager.GetRolesAsync(newStore);
    if (createResult.Succeeded && addToRoleResult.Succeeded)
    {
      var claims = await _userManager.GetClaimsAsync(newStore);
      return (rqm.INN, _jwtGenerator.CreateToken(claims, TimeSpan.FromDays(1)), roles, Enumerable.Empty<string>());
    }
    else
    {
      var errors = new List<string>();
      errors.AddRange(createResult.Errors.Select(e => e.Description));
      errors.AddRange(addToRoleResult.Errors.Select(e => e.Description));
      return (string.Empty, string.Empty, Enumerable.Empty<string>(), errors);
    }
  }
}
