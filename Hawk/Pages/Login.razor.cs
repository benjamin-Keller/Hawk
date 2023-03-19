using Blazored.LocalStorage;
using Hawk.Models;
using Hawk.Services;
using Microsoft.AspNetCore.Components;

namespace Hawk.Pages;

public partial class Login
{
    [Inject] private AuthenticationService AuthenticationService { get; set; }
    [Inject] private ILocalStorageService localStorage { get; set; }
    public UserInfo User { get; set; }

    public User FormModel { get; set; } = new();

    private async Task SubmitFormAsync()
    {
        User = await AuthenticationService.AuthenticateAsync(FormModel);
        if (User is not null)
        {
            await localStorage.SetItemAsync("player", User);
            await localStorage.SetItemAsync("player__login_time", (DateTime?) DateTime.Now);
        }
    }
}