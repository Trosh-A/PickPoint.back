using FluentValidation;

namespace PickPoint.back.Models.Validators;

public class RegistrationQueryModelValidator : AbstractValidator<RegistrationQueryModel>
{
  public RegistrationQueryModelValidator()
  {
    //ИНН физлиц 10, ИНН юрлиц 12
    RuleFor(l => l.INN).NotEmpty().Must(inn => inn.Length == 10 || inn.Length == 12);
    RuleFor(l => l.Password).NotEmpty().MinimumLength(5).MaximumLength(200);
  }
}
