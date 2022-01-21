using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace PickPoint.back.Extensions;

public static class ApiVersioning
{
  public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
  {
    return services.AddApiVersioning(o =>
    {
      o.ReportApiVersions = true;
      o.AssumeDefaultVersionWhenUnspecified = true;
      o.DefaultApiVersion = new ApiVersion(1, 0);
      o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
    });
  }
}
