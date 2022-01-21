using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PickPoint.back.Extensions;

public static class Authentication
{
  public static IServiceCollection ConfigureAuthentication(this IServiceCollection isc, string jwtSecretKey)
  {
    isc.AddAuthentication(x =>
    {
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
      x.RequireHttpsMetadata = false;
      x.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
        ValidateIssuerSigningKey = true,
      };
    });
    return isc;
  }
}