#nullable disable
using PickPoint.back.Models.OrderModel;
using System;
using System.Collections.Generic;

namespace PickPoint.back.Models.ParcelAutomatModel;
public class ParcelAutomat : ITimeStamped
{
  public ParcelAutomat() { }
  public ParcelAutomat(string index) { Index = index; }
  public Guid Id { get; set; }
  public string Index { get; set; }
  public string Address { get; set; }
  public bool IsWorking { get; set; }
  public List<Order> Orders { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

}