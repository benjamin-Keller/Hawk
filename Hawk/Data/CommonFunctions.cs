using System.Net.Http.Headers;
using System.Net;

namespace Hawk.Data;

public class CommonFunctions
{
    public static IEnumerable<Cookie> ParseSetCookie(HttpHeaders headers)
    {
        if (headers.TryGetValues("Set-Cookie", out var cookies))
        {
            return cookies.Select(cookie => cookie.Split('=', 2))
                .Select(cookieParts => new Cookie(cookieParts[0], cookieParts[1]));
        }
        return Enumerable.Empty<Cookie>();
    }
}
