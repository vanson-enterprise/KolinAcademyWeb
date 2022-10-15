using KA.DataProvider.Entities;
using KA.Service.Courses;
using KA.Service.Users;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using KA.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Radzen;

namespace KAWebHost.Pages.Admin.Users
{
    public partial class Index : OwningComponentBase
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        [Inject]
        private IJSRuntime jsr { get; set; }

        private DataGridResponse<UserItem> dataGrid;
        private IUserService _userService;
        private int pageSize = 10;
        private bool IsOpenCreateUserModal = false;
        private string editUserId;

        protected override async Task OnInitializedAsync()
        {
            _userService = ScopedServices.GetRequiredService<IUserService>();
            await GetAllUser();
        }

        private async Task GetAllUser()
        {
            dataGrid = await _userService.GetAllUserPaging(0, pageSize);
        }

        private async Task PageChanged(PagerEventArgs args)
        {
            dataGrid = await _userService.GetAllUserPaging(args.Skip, args.Top);
        }

        private void CreateUser()
        {
            IsOpenCreateUserModal = true;
        }

        private void OnCreatedUserModalClose(bool isSuccess)
        {
            if (isSuccess)
            {
                GetAllUser();
            }
            IsOpenCreateUserModal = false;
        }

        private void EditUser(string userId)
        {
            editUserId = userId;
        }

        private void OnEditedUserModalClose(bool isSuccess)
        {
            if (isSuccess)
            {
                GetAllUser();
            }
            editUserId = null;
        }

    }
}