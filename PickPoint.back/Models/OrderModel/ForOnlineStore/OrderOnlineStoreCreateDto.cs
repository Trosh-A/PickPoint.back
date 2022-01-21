#nullable disable


namespace PickPoint.back.Models.OrderModel.ForOnlineStore;

public class OrderOnlineStoreCreateDto
{
  public string[] Goods { get; set; }
  public string ParcelAutomatIndex { get; set; }
  public decimal Cost { get; set; }
  public string PhoneNumber { get; set; }
  public string FIO { get; set; }
}
