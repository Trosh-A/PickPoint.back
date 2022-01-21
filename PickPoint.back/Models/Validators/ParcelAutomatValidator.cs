using FluentValidation;
using PickPoint.back.Models.ParcelAutomatModel;

namespace PickPoint.back.Models.Validators;

public class ParcelAutomatValidator : AbstractValidator<ParcelAutomat>
{
  public ParcelAutomatValidator()
  {
    RuleFor(pa => pa.Index).Matches(@"\d{4}-\d{3}").WithMessage("Номер постамата не соответствует маске");
  }
}
