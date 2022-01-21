#nullable disable

using PickPoint.back.Models.CustomServiceUserModel;
using PickPoint.back.Models.GoodModel;
using PickPoint.back.Models.ParcelAutomatModel;
using System;
using System.Collections.Generic;

namespace PickPoint.back.Models.OrderModel;

public class Order : ITimeStamped
{
  public Guid Id { get; set; }
  public int OrderNumber { get; set; }
  public OrderStatus OrderStatus { get; set; }
  public List<Good> Goods { get; set; }
  public decimal Cost { get; set; }
  public string PhoneNumber { get; set; }
  public string FIO { get; set; }

  public Guid? ParcelAutomatId { get; set; }
  public ParcelAutomat ParcelAutomat { get; set; }

  public Guid CustomServiceUserId { get; set; }
  public CustomServiceUser CustomServiceUser { get; set; }

  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
