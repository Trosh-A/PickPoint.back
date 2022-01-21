using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PickPoint.back.EFCore;
using PickPoint.back.Models;
using PickPoint.back.Models.OrderModel;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.OrdersRepository;
#pragma warning disable CA2254 // Template should be a static expression
public class OrdersRepo : IOrdersRepo
{
  private readonly ILogger<OrdersRepo> _logger;
  private readonly AppDbContext _ctx;
  private readonly Guid? storeGuid = null;
  public OrdersRepo(AppDbContext ctx, IHttpContextAccessor httpContextAccessor, ILogger<OrdersRepo> logger)
  {
    _logger = logger;
    _ctx = ctx;
    var storeGuidString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (Guid.TryParse(storeGuidString, out Guid storeGuidTemp)) storeGuid = storeGuidTemp;

    _logger.LogInformation($"Запрос от пользователя: {storeGuid}");
  }

  public async Task<Order?> GetOrderByNumberAuthAsync(int number)
  {
    return await _ctx.Orders
      .Include(o => o.ParcelAutomat)
      .Include(o => o.Goods)
      .Include(o => o.CustomServiceUser)
      .AsNoTracking()
      .SingleOrDefaultAsync(o => o.OrderNumber == number && o.CustomServiceUserId == storeGuid);
  }
  public async Task<Order?> CreateOrderAuthAsync(Order order)
  {
    if (order is null || storeGuid is null) return null;
    order.CustomServiceUserId = (Guid)storeGuid;
    order.OrderNumber = await GetValidOrderNumberAsync();
    order.OrderStatus = OrderStatus.Registered;
    _ctx.Orders.Add(order);
    await _ctx.SaveChangesAsync();
    return order;
  }
  public Order? UpdateOrderAuth(Order order)
  {
    if (order is null || storeGuid is null) return null;
    return order;
  }
  public async Task<Order?> CancelOrderAuthAsync(int number)
  {
    var order = await _ctx.Orders.SingleOrDefaultAsync(o => o.OrderNumber == number && o.CustomServiceUserId == storeGuid);
    if (order is null) return null;
    order.OrderStatus = OrderStatus.Cancelled;
    return UpdateOrderAuth(order);
  }
  public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

  /// <summary>
  /// Метод генерирует валидный номер заказа.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="OverflowException">Произойдёт в случает, когда количество заказов достигнет 2147483647.</exception>
  /// <remarks>
  /// Ещё вариант Task.WhenAny запускать поиск свободного номера в  несколько параллельных задач
  /// и через Cancelation Token останавливать все послепервой удачно завершившейся задачи(нахождение доступного номера)
  /// Такой алгоритм генерации нового номера для заказа нужен, потому что если генерировать номер заказа инкрементом,
  /// то конкуренты могут отследить объём обрабатываемых компанией заказов.А использовать guid не стоит, потому что в случае решении проблем с заказом
  /// по телефону(например) неудобно диктовать guid, удобнее несколько цифр.
  /// Один из вариантов решения проблемы - внести изменения в Api
  /// номер заказа с Int32 на Int64.(так например сделал Telegram).
  /// </remarks>
  private async Task<int> GetValidOrderNumberAsync()
  {
    int newOrderNumber = -1;
    //Генерирую ещё никогда не существовавший номер заказа
    for (int i = 0; i < Int32.MaxValue; i++)
    {
      newOrderNumber = Random.Shared.Next(0, Int32.MaxValue);
      bool validNumber = !await _ctx.Orders.AnyAsync(o => o.OrderNumber == newOrderNumber);
      if (validNumber) return newOrderNumber;
    }
    throw new OverflowException();
  }
}
