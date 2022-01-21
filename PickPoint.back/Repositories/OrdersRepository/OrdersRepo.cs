using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PickPoint.back.EFCore;
using PickPoint.back.Models;
using PickPoint.back.Models.OrderModel;
using System;
using System.Linq;
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
  public async Task<Order?> GetOrderByGuidAuthAsync(Guid id)
  {
    return await _ctx.Orders
      .Include(o => o.ParcelAutomat)
      .Include(o => o.Goods)
      .Include(o => o.CustomServiceUser)
      .AsNoTracking()
      .SingleOrDefaultAsync(o => o.Id == id && o.CustomServiceUserId == storeGuid);
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
  public async Task<Order?> CancelOrderAuthAsync(Guid id)
  {
    var order = await _ctx.Orders.SingleOrDefaultAsync(o => o.Id == id && o.CustomServiceUserId == storeGuid);
    if (order is null) return null;
    order.OrderStatus = OrderStatus.Cancelled;
    return UpdateOrderAuth(order);
  }
  public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

  /// <summary>
  /// Метод генерирует валидный номер заказа.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="OverflowException">Произойдёт только в случает, когда активны в обработке 2147483647 заказов.</exception>
   /* Ещё вариант Task.WhenAny запускать поиск свободного номера в  несколько параллельных задач и через Cancelation Token останавливать все после
   *  первой удачно завершившейся задачи(нахождение доступного номера)
   *  Такой "нетривиальный" алгоритм генерации нового номера для заказа нужен, потому что если генерировать номер заказа инкрементом,
   *  то конкуренты могут отследить объём обрабатываемых компанией заказов. А использовать guid не стоит, потому что в случае решении проблем с заказом
   *  по телефону(например) неудобно диктовать guid, удобнее несколько цифр.
   *  Проблема будет, когда в активной обработке будет 2147483647 заказов. Невозможно будет создать заказ, пока хотя бы 1 заказ не будет отменён или доставлен получателю.
   *  Задолго до этого в логи будут сыпаться ошибки, что количество свободных номеров исчерпано. Один из вариантов решения проблемы изменить в Api
   *  номер заказа с Int32 на Int64.(так например сделал Telegram). Будем решать проблемы по мере их поступления. Думаю 2 миллиарда заказов даже для PickPoint
   *  достаточно на ближайшие годы.
   */
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
    //Если все Int32 уже использованы, то использщую уже существовавший когда-то номер заказа(из числа выполненных или отменённых) 
    for (int i = 0; i < Int32.MaxValue; i++)
    {
      newOrderNumber = Random.Shared.Next(0, Int32.MaxValue);
      bool validNumber = !await _ctx.Orders
        .Where(o => o.OrderStatus == OrderStatus.DeliveredToRecipient || o.OrderStatus == OrderStatus.Cancelled)
        .AnyAsync(o => o.OrderNumber == newOrderNumber);
      if (validNumber)
      {
        _logger.LogError("Исчерпаны все номера заказов. Номер использован повторно из неактивных");
        return newOrderNumber;
      }
    }
    throw new OverflowException();
  }
}
