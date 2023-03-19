using Blazored.LocalStorage;
using Hawk.Models;
using Hawk.Services;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace Hawk.Pages;

public partial class Login
{
    [Inject] private AuthenticationService AuthenticationService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private UserInfo User { get; set; }

    private User FormModel { get; set; } = new();
    private bool Loading { get; set; }
    private string ErrorMessage { get; set; }
    private bool ShowPopover { get; set; }
    private void ToggleOpen()
    {
        if (ShowPopover)
            ShowPopover = false;
        else
            ShowPopover = true;
    }
    private async Task SubmitFormAsync()
    {
        Loading = true;
        try
        {
            User = await AuthenticationService.AuthenticateAsync(FormModel);
            if (User is not null)
            {
                await LocalStorage.SetItemAsync("session__signed_in", true);
                await LocalStorage.SetItemAsync("player", User);
                await LocalStorage.SetItemAsync("player__login_time", (DateTime?)DateTime.Now);

                NavigationManager.NavigateTo("/");
            }
        }
        catch (Exception ex)
        {
            ShowPopover = true;
            ErrorMessage = ex.Message;

            await LocalStorage.SetItemAsync("session__signed_in", false);
        }
        finally
        {
            Loading = false;
        }
        
    }
}