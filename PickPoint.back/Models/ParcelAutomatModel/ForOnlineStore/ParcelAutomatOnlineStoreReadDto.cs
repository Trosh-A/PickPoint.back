#nullable disable

using System;

namespace PickPoint.back.Models.ParcelAutomatModel.ForOnlineStore;

public class ParcelAutomatOnlineStoreReadDto
{
  public Guid Id { get; set; }
  public string Index { get; set; }
  public string Address { get; set; }
  public bool IsWorking { get; set; }
}
