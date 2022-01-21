namespace PickPoint.back.Models;

public enum OrderStatus
{
  None = 0,
  Registered,
  AcceptedAtWarehouse,
  GivenToCourier,
  DeliveredToParcelAutomat,
  DeliveredToRecipient,
  Cancelled
}
