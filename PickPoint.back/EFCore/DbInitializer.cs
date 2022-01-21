using Microsoft.AspNetCore.Identity;
using PickPoint.back.Constants;
using PickPoint.back.Models;
using PickPoint.back.Models.CustomServiceUserModel;
using PickPoint.back.Models.GoodModel;
using PickPoint.back.Models.OrderModel;
using PickPoint.back.Models.ParcelAutomatModel;
using System;
using System.Threading.Tasks;
using static PickPoint.back.Constants.SeedConstants;


namespace PickPoint.back.EFCore;

public static class DbInitializer
{
  public static async Task SeedDataAsync(UserManager<CustomServiceUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext ctx)
  {
    await SeedRolesAsync(roleManager);
    await SeedUsersAsync(userManager);
    await SeedParcelAutomatAsync(ctx);
    await SeedOrdersAsync(ctx);
    await SeedGoodsAsync(ctx);
  }

  private static async Task SeedOrdersAsync(AppDbContext ctx)
  {
    var orders = new Order[]
    {
      new() { Id = Guid.Parse(ORDER_GUID_1), OrderNumber = 1, OrderStatus = OrderStatus.Registered,Cost=123,PhoneNumber="+7999-99-99", FIO = "euihg earg ert", ParcelAutomatId=Guid.Parse(PA_GUID_1), CustomServiceUserId = Guid.Parse(ONLINESTORE_GUID_2)},
      new() { Id = Guid.Parse(ORDER_GUID_2), OrderNumber = 2, OrderStatus = OrderStatus.AcceptedAtWarehouse,Cost=4573,PhoneNumber="+7435-99-99", FIO = "oipaurg era jaii", ParcelAutomatId=Guid.Parse(PA_GUID_1), CustomServiceUserId = Guid.Parse(ONLINESTORE_GUID_2)},
      new() { Id = Guid.Parse(ORDER_GUID_3), OrderNumber = 6546, OrderStatus = OrderStatus.GivenToCourier,Cost=3568,PhoneNumber="+7435-23-99", FIO = "tykje et drth", ParcelAutomatId=Guid.Parse(PA_GUID_1), CustomServiceUserId = Guid.Parse(ONLINESTORE_GUID_2)},
      new() { Id = Guid.Parse(ORDER_GUID_4), OrderNumber = 1346, OrderStatus = OrderStatus.DeliveredToParcelAutomat,Cost=1234,PhoneNumber="+7435-23-12", FIO = "ardshg eawry tdk", ParcelAutomatId=Guid.Parse(PA_GUID_2), CustomServiceUserId = Guid.Parse(ONLINESTORE_GUID_3)},
      new() { Id = Guid.Parse(ORDER_GUID_5), OrderNumber = 124, OrderStatus = OrderStatus.DeliveredToRecipient,Cost=34267,PhoneNumber="+7123-23-12", FIO = "sruhjr eawry tdk", ParcelAutomatId=Guid.Parse(PA_GUID_2), CustomServiceUserId = Guid.Parse(ONLINESTORE_GUID_3)},
      new() { Id = Guid.Parse(ORDER_GUID_6), OrderNumber = 78098, OrderStatus = OrderStatus.Cancelled,Cost=76845,PhoneNumber="+7457-23-25", FIO = "sruhjr gdmfhj fsdfm", ParcelAutomatId=Guid.Parse(PA_GUID_2), CustomServiceUserId = Guid.Parse(ONLINESTORE_GUID_4)},
    };
    foreach (var order in orders)
    {
      if (await ctx.Orders.FindAsync(order.Id) is null)
      {
        ctx.Orders.Add(order);
        await ctx.SaveChangesAsync();
      }
    }

  }

  private static async Task SeedGoodsAsync(AppDbContext ctx)
  {
    var goods = new Good[]
    {
      new() { Id = Guid.Parse(GOOD_GUID_1), Name = GOOD_NAME_1, OrderId = Guid.Parse(ORDER_GUID_1)},
      new() { Id = Guid.Parse(GOOD_GUID_2), Name = GOOD_NAME_2, OrderId = Guid.Parse(ORDER_GUID_1)},
      new() { Id = Guid.Parse(GOOD_GUID_3), Name = GOOD_NAME_3, OrderId = Guid.Parse(ORDER_GUID_2)},
      new() { Id = Guid.Parse(GOOD_GUID_4), Name = GOOD_NAME_4, OrderId = Guid.Parse(ORDER_GUID_3)},
      new() { Id = Guid.Parse(GOOD_GUID_5), Name = GOOD_NAME_5, OrderId = Guid.Parse(ORDER_GUID_3)},
      new() { Id = Guid.Parse(GOOD_GUID_6), Name = GOOD_NAME_6, OrderId = Guid.Parse(ORDER_GUID_4)},
      new() { Id = Guid.Parse(GOOD_GUID_7), Name = GOOD_NAME_7, OrderId = Guid.Parse(ORDER_GUID_4)},
      new() { Id = Guid.Parse(GOOD_GUID_8), Name = GOOD_NAME_8, OrderId = Guid.Parse(ORDER_GUID_5)},
      new() { Id = Guid.Parse(GOOD_GUID_9), Name = GOOD_NAME_9, OrderId = Guid.Parse(ORDER_GUID_5)},
      new() { Id = Guid.Parse(GOOD_GUID_10), Name = GOOD_NAME_10, OrderId = Guid.Parse(ORDER_GUID_6)},
    };
    foreach (var good in goods)
    {
      if (await ctx.Goods.FindAsync(good.Id) is null)
      {
        ctx.Goods.Add(good);
        await ctx.SaveChangesAsync();
      }
    }
  }

  private static async Task SeedParcelAutomatAsync(AppDbContext ctx)
  {
    var pas = new ParcelAutomat[]
    {
      new (){ Id = Guid.Parse(PA_GUID_1), Index = PA_INDEX_1, IsWorking = true },
      new() { Id = Guid.Parse(PA_GUID_2), Index = PA_INDEX_2, IsWorking = true },
      new() { Id = Guid.Parse(PA_GUID_3), Index = PA_INDEX_3, IsWorking = false },
      new() { Id = Guid.Parse(PA_GUID_4), Index = PA_INDEX_4, IsWorking = false }
    };
    foreach (var pa in pas)
    {
      if (await ctx.ParcelAutomats.FindAsync(pa.Id) is null)
      {
        ctx.ParcelAutomats.Add(pa);
        await ctx.SaveChangesAsync();
      }
    }
  }

  private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
  {
    var seedRoles = new[] {
      RolesConstants.UNDER_CONSIDERATION,
      RolesConstants.ACTIVE,
      RolesConstants.BANNED,
      RolesConstants.PICKPOINT_MANAGER };
    foreach (var seedRole in seedRoles)
    {
      if (!await roleManager.RoleExistsAsync(seedRole))
      {
        await roleManager.CreateAsync(new IdentityRole<Guid>(seedRole));
      }
    }
  }
  private static async Task SeedUsersAsync(UserManager<CustomServiceUser> userManager)
  {
    string ppManagerName = "Ivan";
    if (await userManager.FindByNameAsync(ppManagerName) is null)
    {
      var manager1 = new CustomServiceUser(ppManagerName);
      await userManager.CreateAsync(manager1, "12345");
      await userManager.AddToRoleAsync(manager1, RolesConstants.PICKPOINT_MANAGER);
    }

    string someStore1INN = "1234567890";
    if (await userManager.FindByNameAsync(someStore1INN) is null)
    {
      var someStore1 = new CustomServiceUser(someStore1INN) { Id = Guid.Parse(ONLINESTORE_GUID_1) };
      await userManager.CreateAsync(someStore1, "12345");
      await userManager.AddToRoleAsync(someStore1, RolesConstants.UNDER_CONSIDERATION);
    }

    string someStore2INN = "0987654321";
    if (await userManager.FindByNameAsync(someStore2INN) is null)
    {
      var someStore2 = new CustomServiceUser(someStore2INN) { Id = Guid.Parse(ONLINESTORE_GUID_2) };
      await userManager.CreateAsync(someStore2, "12345");
      await userManager.AddToRoleAsync(someStore2, RolesConstants.ACTIVE);
    }

    string someStore3INN = "7894561230";
    if (await userManager.FindByNameAsync(someStore3INN) is null)
    {
      var someStore3 = new CustomServiceUser(someStore3INN) { Id = Guid.Parse(ONLINESTORE_GUID_3) };
      await userManager.CreateAsync(someStore3, "12345");
      await userManager.AddToRoleAsync(someStore3, RolesConstants.ACTIVE);
    }

    string someStore4INN = "6549871230";
    if (await userManager.FindByNameAsync(someStore4INN) is null)
    {
      var someStore4 = new CustomServiceUser(someStore4INN) { Id = Guid.Parse(ONLINESTORE_GUID_4) };
      await userManager.CreateAsync(someStore4, "12345");
      await userManager.AddToRoleAsync(someStore4, RolesConstants.BANNED);
    }
  }
}
