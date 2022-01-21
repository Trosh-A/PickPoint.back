using FluentValidation;
using PickPoint.back.Constants;
using PickPoint.back.Models.OrderModel;

namespace PickPoint.back.Models.Validators;

public class OrderValidator : AbstractValidator<Order>
{
  public OrderValidator()
  {
    RuleFor(o => o.PhoneNumber).Matches(@"\+7\d{3}-\d{3}-\d{2}-\d{2}").WithMessage("Номер телефона не соответствует маске");
    RuleFor(o => o.FIO).NotEmpty().WithMessage("Поле ФИО не может быть пустым");
    RuleFor(o => o.FIO).MaximumLength(OrderConstants.FIO_MAX_LENTH).WithMessage("Превышена допустимая длина поля ФИО");
    RuleFor(o => o.Goods).Must(g => g.Count <= OrderConstants.MAX_GOODS_COUNT || g == null).WithMessage($"Количество товаров не должно превышать {OrderConstants.MAX_GOODS_COUNT}");
  }
}
