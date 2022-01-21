using System;

namespace PickPoint.back.Models;

public interface ITimeStamped
{
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
