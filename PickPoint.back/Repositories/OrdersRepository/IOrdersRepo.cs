using PickPoint.back.Models.OrderModel;
using System.Threading.Tasks;

namespace PickPoint.back.Repositories.OrdersRepository;

public interface IOrdersRepo
{
  Task<Order?> GetOrderByNumberAuthAsync(int number);
  Task<Order?> CreateOrderAuthAsync(Order order);
  Task<int> SaveChangesAsync();
  Order? UpdateOrderAuth(Order order);
  Task<Order?> CancelOrderAuthAsync(int number);
}
