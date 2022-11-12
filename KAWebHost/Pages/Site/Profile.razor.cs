using KA.DataProvider.Entities;
using KA.Repository.Base;
using KA.Service.Users;
using KA.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class Profile : OwningComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;
        private IUserService _userService;
        private string userId;
        private UserProfileVm userProfileVm = new()
        {
            UserProfileInfo = new()
        };
        private string FullName;

        [Inject]
        NavigationManager navigationManager { get; set; }
        [Inject]
        private IJSRuntime jsr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            authState = await authenticationStateTask;
            _userService = ScopedServices.GetRequiredService<IUserService>();
            userId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            await GetUserProfile();
        }

        public async Task GetUserProfile()
        {
            userProfileVm = await _userService.GetUserProfile(userId);
            FullName = userProfileVm.UserProfileInfo.Name;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "./Pages/Site/Profile.razor.js");
            }
        }

        public async Task UpdateUserInfo()
        {
            await _userService.UpdateUserInfo(userProfileVm.UserProfileInfo);
            await GetUserProfile();
        }

    }
}