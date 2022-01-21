using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PickPoint.back.Constants;
using PickPoint.back.Models.OrderModel;
using PickPoint.back.Models.OrderModel.ForOnlineStore;
using PickPoint.back.Repositories.OrdersRepository;
using PickPoint.back.Repositories.ParcelAutomatsRepository;
using System;
using System.Threading.Tasks;

#pragma warning disable CA2254 // Template should be a static expression

namespace PickPoint.back.Controllers;

[ApiVersion("1.0")]
[Route("api/orders")]
[Authorize(Roles = RolesConstants.ACTIVE)]
[ApiController]
public class OrdersForStoresController : ControllerBase
{
  private readonly IOrdersRepo _ordersRepo;
  private readonly IParcelAutomatsRepo _parcelAutomatRepo;
  private readonly ILogger<OrdersForStoresController> _logger;
  private readonly IValidator<Order> _orderValidator;
  private readonly IMapper _mapper;

  public OrdersForStoresController(
    IOrdersRepo ordersRepo,
    IParcelAutomatsRepo parcelAutomatRepo,
    IMapper mapper,
    ILogger<OrdersForStoresController> logger,
    IValidator<Order> orderValidator
    )
  {
    _parcelAutomatRepo = parcelAutomatRepo;
    _ordersRepo = ordersRepo;
    _mapper = mapper;
    _logger = logger;
    _orderValidator = orderValidator;
  }

  // GET: api/orders/{id}
  [HttpGet("{id:guid}")]
  public async Task<ActionResult<OrderOnlineStoreReadDTO>> GetOrderByIdAsync(Guid id)
  {
    var order = await _ordersRepo.GetOrderByGuidAuthAsync(id);
    if (order is not null)
    {
      _logger.LogInformation($"Запрошен заказ с номером: {id}");
      return Ok(_mapper.Map<OrderOnlineStoreReadDTO>(order));
    }
    _logger.LogInformation($"Запрошенный заказ с id: {id} не найден");
    return NotFound();
  }

  [HttpPost]
  public async Task<ActionResult<OrderOnlineStoreReadDTO>> CreateOrderAsync(OrderOnlineStoreCreateDto orderCreateDto)
  {
    if (orderCreateDto is null) return BadRequest();
    //Здесь модель в смысле модель для базы данных
    var orderModel = _mapper.Map<Order>(orderCreateDto);
    var parcellAutomat = await _parcelAutomatRepo.GetParcelAutomatIfAvaliable(orderCreateDto.ParcelAutomatIndex);
    if (parcellAutomat is null) return Forbid();
    orderModel.ParcelAutomat = parcellAutomat;
    _orderValidator.Validate(orderModel).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      //Логичнее ответ ValidationProblem, но задание требует BadRequest
      return BadRequest(ModelState);
    }
    //Да, этот метод возвращает модель заказа, но нет смысла сохранять её здесь в отдельную переменную. ChangeTracker обновит уже существующую переменную.
    await _ordersRepo.CreateOrderAuthAsync(orderModel);
    await _ordersRepo.SaveChangesAsync();
    var orderReadDto = _mapper.Map<OrderOnlineStoreReadDTO>(orderModel);
    return StatusCode(201, orderReadDto);
  }

  [HttpPut("{id:guid}")]
  public async Task<ActionResult<OrderOnlineStoreReadDTO>> UpdateOrderAsync(Guid id, OrderOnlineStoreUpdateDto orderUpdateDto)
  {
    if (orderUpdateDto is null)
    {
      return BadRequest();
    }
    var orderModelFromRepo = await _ordersRepo.GetOrderByGuidAuthAsync(id);
    if (orderModelFromRepo is null)
    {
      return NotFound();
    }
    _mapper.Map(orderUpdateDto, orderModelFromRepo);
    _orderValidator.Validate(orderModelFromRepo).AddToModelState(ModelState, null);
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }
    _ordersRepo.UpdateOrderAuth(orderModelFromRepo);
    await _ordersRepo.SaveChangesAsync();
    var orderReadDto = _mapper.Map<OrderOnlineStoreReadDTO>(orderModelFromRepo);
    //Согласно стандартному REST надо вернуть 200 или 204(NoContent).
    return Ok(orderReadDto);
  }

  [HttpPost("cancel/{id:guid}")]
  public async Task<IActionResult> CancelOrderAsync(Guid id)
  {
    var orderModelFromRepo = await _ordersRepo.CancelOrderAuthAsync(id);
    if (orderModelFromRepo is null)
    {
      return NotFound();
    }
    await _ordersRepo.SaveChangesAsync();
    return NoContent();
  }
}
