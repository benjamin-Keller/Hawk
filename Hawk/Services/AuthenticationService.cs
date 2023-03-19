using Hawk.Data;
using Hawk.Models;

namespace Hawk.Services;

public class AuthenticationService
{
    private readonly AuthenticationDataService _authHandler;

    public AuthenticationService(AuthenticationDataService authHandler)
    {
        _authHandler = authHandler;
    }

    public async Task<UserInfo> AuthenticateAsync(User user)
    {
        var (loginResponse, cookieCollection, accessToken) = await _authHandler.SignInAsync(user);
        var entitlementToken = await _authHandler.GetEntitlementAsync(accessToken, cookieCollection);

        var userInfo = new UserInfo();
        userInfo = await _authHandler.GetUserInfo();
        userInfo.Entitlement = entitlementToken;

        return userInfo;
    }
}
