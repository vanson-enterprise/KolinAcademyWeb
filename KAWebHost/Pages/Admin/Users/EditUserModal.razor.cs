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
    public partial class EditUserModal : OwningComponentBase
    {
        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string UserId { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;

        private EditUserModel model = new();

        private IUserService _userService;

        [Inject]
        IJSRuntime jsr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _userService = ScopedServices.GetRequiredService<IUserService>();
            authState = await authenticationStateTask;
            model = await _userService.GetUserForEdit(UserId);

        }

        private async Task Submit()
        {
            model.UpdateDate = DateTime.Now;
            model.UpdateUserId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var result = await _userService.EditUser(model);

            if (result.Status == ResponseStatus.SUCCESS)
            {
                await jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
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