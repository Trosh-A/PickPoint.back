using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using PickPoint.back.Models.Validators;

namespace PickPoint.back.Extensions;

public static class FluentValidation
{
  public static IMvcBuilder ConfigureFluentValidation(this IMvcBuilder mvcBuilder)
  {
    return mvcBuilder.AddFluentValidation(x =>
    {
      x.DisableDataAnnotationsValidation = true;
      x.RegisterValidatorsFromAssemblyContaining<OrderValidator>();
    });
  }
}
