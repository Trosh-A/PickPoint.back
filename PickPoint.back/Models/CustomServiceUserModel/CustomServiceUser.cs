#nullable disable
using Microsoft.AspNetCore.Identity;
using PickPoint.back.Models.OrderModel;
using System;
using System.Collections.Generic;

namespace PickPoint.back.Models.CustomServiceUserModel;

public class CustomServiceUser : IdentityUser<Guid>, ITimeStamped
{
  public CustomServiceUser(string INN) : base(INN) { }
  public string CompactName { get; set; }
  public string FullName { get; set; }

  //В Identity удобно оперировать UserName (привязан к стандартному Claim), а уникально идентифицирует клиента ИНН, поэтому такая обёртка
  public string INN { get => UserName; set => UserName = value; }
  public List<Order> Orders { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
