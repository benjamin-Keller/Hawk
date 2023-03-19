using Hawk.Models;

namespace Hawk.Pages;

public partial class Index
{
    protected override async Task OnInitializedAsync()
    {
        var userData = await localStorage.GetItemAsync<UserInfo>("player");
        if (userData is null)
            navigationManager.NavigateTo("login");
        else
            navigationManager.NavigateTo("home");
    }
}