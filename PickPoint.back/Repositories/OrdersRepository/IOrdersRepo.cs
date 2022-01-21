using PickPoint.back.Models.OrderModel;
using System;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.OrdersRepository;

public interface IOrdersRepo
{
  Task<Order?> GetOrderByGuidAuthAsync(Guid id);
  Task<Order?> CreateOrderAuthAsync(Order order);
  Task<int> SaveChangesAsync();
  Order? UpdateOrderAuth(Order order);
  Task<Order?> CancelOrderAuthAsync(Guid id);
}
