using KA.DataProvider.Entities;
using KA.Infrastructure.Enums;
using KA.Service.Users;
using KA.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Pages.Admin.Users
{
    public partial class CreateUserModal : OwningComponentBase
    {
        [Parameter]
        public EventCallback<bool> OnClose { get; set; }
        [Parameter]
        public string Message { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;

        private CreateUserModel model;

        private IUserService _userService;
        private UserManager<AppUser> _userManager;

        [Inject]
        IJSRuntime jsr { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _userService = ScopedServices.GetRequiredService<IUserService>();
            _userManager = ScopedServices.GetRequiredService<UserManager<AppUser>>();
            authState = await authenticationStateTask;
            model = new();
            model.Roles = new();
            model.Roles = await _userService.GetAllRoleForSelect(null);

        }

        private async Task Submit()
        {
            model.CreatedDate = DateTime.Now;
            model.CreateUserId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var result = await _userService.CreateNewUser(model);

            if (result.Status == ResponseStatus.SUCCESS)
            {
                await jsr.InvokeVoidAsync("ShowAppAlert", "Đã tạo người dùng thành công", "success");
                OnClose.InvokeAsync(true);
            }
            else
            {
                await jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "error");
                OnClose.InvokeAsync(false);
            }

        }
        private async Task Cancel()
        {
            OnClose.InvokeAsync(false);
        }
    }
}