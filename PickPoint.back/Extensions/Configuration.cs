using Microsoft.Extensions.Configuration;
using System.IO;

namespace PickPoint.back.Extensions;

public static class Configuration
{
  public static IConfigurationBuilder AddJsonsFromDirectory(this IConfigurationBuilder cb, string dir)
  {
    if (Directory.Exists(dir))
    {
      foreach (var jsonFile in Directory
      .GetFiles(dir, "*.json", SearchOption.AllDirectories))
      {
        cb.AddJsonFile(jsonFile, false, false);
      }
    }
    return cb;
  }
}