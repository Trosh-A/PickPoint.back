using Microsoft.Extensions.DependencyInjection;

namespace PickPoint.back.Extensions;

public static class Controllers
{
  public static IMvcBuilder ConfigureControllers(this IServiceCollection services)
  {
    return services.AddControllers(x => { x.SuppressAsyncSuffixInActionNames = false; });
  }
}
