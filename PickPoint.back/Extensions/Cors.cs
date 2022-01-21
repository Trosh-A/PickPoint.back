using Microsoft.Extensions.DependencyInjection;
using PickPoint.back.Constants;

namespace PickPoint.back.Extensions;

public static class Cors
{
  public static IServiceCollection ConfigureCors(this IServiceCollection services)
  {
    return services.AddCors(options =>
    {
      options.AddPolicy(CorsConstants.CORS_ANY_POLICY,
              builder => builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
    });
  }
}
