using System.Security.Claims;

namespace Core.Entity.Temp;

public static class LoginedUser
{
    public static ClaimsPrincipal ClaimsPrincipal { get; set; }
}