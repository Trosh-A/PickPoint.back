using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PickPoint.back.Models;
using PickPoint.back.Services.StoresAuth;
using System.Linq;
using System.Threading.Tasks;

namespace PickPoint.back.Controllers;

#pragma warning disable CA2254 // Template should be a static expression

[AllowAnonymous]
[ApiVersion("1.0")]
[Route("api")]
[ApiController]
public class StoresAuthController : ControllerBase
{
  private readonly ILogger<StoresAuthController> _logger;
  private readonly IStoresAuth _storesManagement;
  private readonly IValidator<RegistrationQueryModel> _validator;

  public StoresAuthController(
    ILogger<StoresAuthController> logger,
    IStoresAuth storessManagement,
    IValidator<RegistrationQueryModel> validator)
  {
    _logger = logger;
    _storesManagement = storessManagement;
    _validator = validator;
  }

  [HttpPost("register")]
  public async Task<IActionResult> RegisterAsync(RegistrationQueryModel rqm)
  {
    if (rqm is null)
    {
      return BadRequest();
    }
    _validator.Validate(rqm).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }
    var (INN, token, roles, errors) = await _storesManagement.RegisterAsync(rqm);
    if (errors.Any())
    {
      foreach (var error in errors)
      {
        ModelState.AddModelError("", error);
      }
      return ValidationProblem(ModelState);
    }
    _logger.LogInformation($"Зарегистрирован магазин \"{INN}\" и выпущен токен");
    return StatusCode(201, new { INN, token, roles });
  }

  [HttpPost("token")]
  public async Task<IActionResult> LoginAsync(LoginQueryModel lqm)
  {
    if (lqm is null)
    {
      return BadRequest();
    }
    var (INN, token, roles, errors) = await _storesManagement.LoginAsync(lqm);
    if (errors.Any())
    {
      foreach (var error in errors)
      {
        ModelState.AddModelError("", error);
      }
      return ValidationProblem(ModelState);
    }
    _logger.LogInformation($"Выпущен токен для {INN}");
    return Ok(new { INN, token, roles });
  }
}
