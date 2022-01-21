#nullable disable
using System;
using System.Collections.Generic;

namespace PickPoint.back.Models.OrderModel.ForOnlineStore;
public class OrderOnlineStoreReadDTO
{
  public Guid Id { get; set; }
  public int OrderNumber { get; set; }
  public OrderStatus OrderStatus { get; set; }
  public IEnumerable<string> Goods { get; set; }
  public decimal Cost { get; set; }
  public string ParcelAutomatIndex { get; set; }
  public string PhoneNumber { get; set; }
  public string FIO { get; set; }
}
