using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace PickPoint.back.Extensions;

public static class SwaggerGen
{
  public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
  {
    return services.AddSwaggerGen(o =>
    {
      o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
      {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Введите ваш токен в поле ниже.",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
      });
      o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme,
                }
            },
            Array.Empty<string>()
        }
    });
    });
  }
}
