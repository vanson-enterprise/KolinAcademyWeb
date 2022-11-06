using KA.DataProvider.Entities;
using KA.Repository.Base;
using KA.Service.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Site
{
    public partial class Profile : OwningComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;
        private IUserService _userRepo;

        [Inject]
        NavigationManager navigationManager { get; set; }
        [Inject]
        private IJSRuntime jsr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            authState = await authenticationStateTask;
            _userRepo = ScopedServices.GetRequiredService<IUserService>();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "./Pages/Site/Profile.razor.js");
            }
        }


    }
}