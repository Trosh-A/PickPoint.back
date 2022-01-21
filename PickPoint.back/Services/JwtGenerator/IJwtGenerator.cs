using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PickPoint.back.Services.JwtGenerator;

public interface IJwtGenerator
{
  string CreateToken(IEnumerable<Claim> claims, TimeSpan lifetime);
}
