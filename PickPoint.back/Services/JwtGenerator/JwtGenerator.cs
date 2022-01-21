using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PickPoint.back.Services.JwtGenerator;

public class JwtGenerator : IJwtGenerator
{
  private readonly SymmetricSecurityKey _key;

  public JwtGenerator(IConfiguration config)
  {
    //EXPLAIN Сейчас для удобства тестирование и проверки ключ хранится в appsettings.json, но в реальных условиях нужно использовать dotnet user-secrets или переменные окружения
    _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSecretKey"]));
  }

  public string CreateToken(IEnumerable<Claim> claims, TimeSpan lifetime)
  {
    var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.Add(lifetime),
      SigningCredentials = credentials
    };
    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }
}