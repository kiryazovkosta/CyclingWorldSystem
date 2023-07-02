namespace Infrastructure.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class JwtOptions
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
}
