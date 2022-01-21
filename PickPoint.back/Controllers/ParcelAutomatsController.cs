using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PickPoint.back.Models.ParcelAutomatModel;
using PickPoint.back.Models.ParcelAutomatModel.ForOnlineStore;
using PickPoint.back.Repositories.ParcelAutomatsRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickPoint.back.Controllers;

#pragma warning disable CA2254 // Template should be a static expression

[ApiVersion("1.0")]
[Route("api/parcel-automats")]
[Authorize()]
[Produces("application/json")]
[ApiController]
public class ParcelAutomatsController : ControllerBase
{
  private readonly IParcelAutomatsRepo _parcelAutomatsRepo;
  private readonly IMapper _mapper;
  private readonly ILogger<ParcelAutomatsController> _logger;
  private readonly IValidator<ParcelAutomat> _parcelAutomatValidator;
  public ParcelAutomatsController(IParcelAutomatsRepo parcelAutomatsRepo, IMapper mapper, ILogger<ParcelAutomatsController> logger, IValidator<ParcelAutomat> parcelAutomatValidator)
  {
    _parcelAutomatsRepo = parcelAutomatsRepo;
    _mapper = mapper;
    _logger = logger;
    _parcelAutomatValidator = parcelAutomatValidator;
  }
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ParcelAutomatOnlineStoreReadDto>>> GetOrderedWorkingParcelAutomatsAsync()
  {
    var parcelAutomats = await _parcelAutomatsRepo.GetOrderedWorkingParcelAutomatsAsync();
    if (parcelAutomats is not null)
    {
      _logger.LogInformation($"Запрошены все рабочие постаматы");
      return Ok(_mapper.Map<IEnumerable<ParcelAutomatOnlineStoreReadDto>>(parcelAutomats));
    }
    _logger.LogError($"Ошибка при запросе всех доступных постаматов");
    return NotFound();
  }

  [HttpGet("{index}")]
  public async Task<ActionResult<ParcelAutomatOnlineStoreReadDto>> GetOrderByIdAsync(string index)
  {
    _parcelAutomatValidator.Validate(new ParcelAutomat(index)).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      //Логичнее ответ ValidationProblem, но задание требует BadRequest
      return BadRequest(ModelState);
    }
    var parcelAutomat = await _parcelAutomatsRepo.GetParcelAutomatByIndexAsync(index);
    if (parcelAutomat is not null)
    {
      _logger.LogInformation($"Запрошена информация по постамату с индексом: {index}");
      return Ok(_mapper.Map<ParcelAutomatOnlineStoreReadDto>(parcelAutomat));
    }
    _logger.LogInformation($"Запрошенный постамат с индексом: {index} не найден");
    return NotFound();
  }
}
