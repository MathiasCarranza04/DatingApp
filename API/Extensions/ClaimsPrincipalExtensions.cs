using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user) //este metodo me da un user ya authenticado
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value; // NameIdentifier mapea con NameId que esta en el tokenService create token
        }

    }
}