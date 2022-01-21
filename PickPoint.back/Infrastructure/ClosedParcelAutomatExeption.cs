using System;

namespace PickPoint.back.Infrastructure;

public class ClosedParcelAutomatExeption : Exception
{
  public ClosedParcelAutomatExeption(string message = "Данный постамат закрыт") : base(message) { }
}
