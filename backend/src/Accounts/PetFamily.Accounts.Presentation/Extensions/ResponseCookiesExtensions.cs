using Microsoft.AspNetCore.Http;

namespace PetFamily.Accounts.Presentation.Extensions;

public static class ResponseCookiesExtensions
{
    public static void SetRefreshToken(this IResponseCookies responseCookies, Guid refreshToken)
    {
        responseCookies.Append("refreshToken",
            refreshToken.ToString(),
            new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
    }
}
