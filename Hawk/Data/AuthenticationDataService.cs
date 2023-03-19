using Hawk.Models;
using Newtonsoft.Json;
using RiotCloudflareAuthFix;
using System.Net;
using System.Text.RegularExpressions;

namespace Hawk.Data;

public class AuthenticationDataService
{
    private readonly AuthenticationJsonClient _client;

    public AuthenticationDataService()
    {
        _client = new AuthenticationJsonClient
        {
            SerializerOptions = new() { PropertyNameCaseInsensitive = true }
        };
        _client.DefaultRequestHeaders.Add("User-Agent", "RiotClient/62.0.1.4852117.4789131 rso-auth (Windows;11;;Professional, x64)");
        _client.DefaultRequestHeaders.Add("X-Curl-Source", "Api");
    }

    public async Task<(string AuthRequest, IEnumerable<Cookie> CookiesCollection, string AccessToken)> SignInAsync(User user)
    {
        var preAuth = new
        {
            client_id = "play-valorant-web-prod",
            redirect_uri = "https://playvalorant.com/opt_in",
            response_type = "token id_token",
            response_mode = "query",
            scope = "account openid",
            nonce = 1,
        };
        var authCookiesRequestResult = await _client.PostAsync<object>(new Uri("https://auth.riotgames.com/api/v1/authorization"), preAuth);

        var authCookies = CommonFunctions.ParseSetCookie(authCookiesRequestResult.Message.Headers);

        var auth = new
        {
            user.Username,
            user.Password,
            type = "auth",
            remember = user.RememberMe
        };
        var authRequestResult = await _client.PutAsync<object>(new Uri("https://auth.riotgames.com/api/v1/authorization"), auth, cookies: authCookies);
        var authResult = await authRequestResult.Message.Content.ReadAsStringAsync();
        
        return (authResult, authCookies, Regex.Match(authResult, @"access_token=(.+?)&scope=").Groups[1].Value);
    }

    public async Task<Entitlement> GetEntitlementAsync(string accessToken, IEnumerable<Cookie> cookieCollection)
    {
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        var entitlementRequestResult = await _client.PostAsync<object>(new Uri("https://entitlements.auth.riotgames.com/api/token/v1"), new { }, cookies: cookieCollection);

        return new Entitlement { EntitlementsToken = ((entitlementRequestResult.Value.ToString().Split("\":\""))[1].Split("\"}"))[0] };
    }

    public async Task<UserInfo> GetUserInfo()
    {
        var userInfoRequestResult = await _client.PostAsync<object>(new Uri("https://auth.riotgames.com/userinfo"), new { });

        var userInfoResult = await userInfoRequestResult.Message.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserInfo>(userInfoResult);
    }
}
