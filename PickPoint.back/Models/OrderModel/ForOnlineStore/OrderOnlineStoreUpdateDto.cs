#nullable disable


namespace PickPoint.back.Models.OrderModel.ForOnlineStore;
/*EXPLAIN Позволять магазину клиенту менять список товаров в уже зарегистрированном заказе нелогично,
 но в задании ограничений на это нет*/
public class OrderOnlineStoreUpdateDto
{
  public string[] Goods { get; set; }
  public decimal Cost { get; set; }
  public string PhoneNumber { get; set; }
  public string FIO { get; set; }
}
