#nullable disable
using PickPoint.back.Models.OrderModel;
using System;

namespace PickPoint.back.Models.GoodModel;

public class Good : ITimeStamped
{
  public Good() { }
  public Good(string name)
  {
    Name = name;
  }
  public Guid Id { get; set; }
  public string Name { get; set; }
  public Guid OrderId { get; set; }
  public Order Order { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
