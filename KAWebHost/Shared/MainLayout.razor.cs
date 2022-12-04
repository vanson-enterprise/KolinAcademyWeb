using KA.Service.Carts;
using KA.ViewModels.Carts;
using KAWebHost.Shared.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        public string alertTitle { get; set; }
        public string alertMessage { get; set; }
        private bool isShowAlert { get; set; }
        private bool isShowForceAuthenAlert { get; set; }
        private int cartProductAmount;
        private ClaimsPrincipal user;
        private string userId;


        // service
        [Inject]
        private IJSRuntime jsr { get; set; }
        [Inject]
        private ICartService _cartService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = (await authenticationStateTask).User;
            userId = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "/scripts/common/common.js");
                await jsr.InvokeVoidAsync("import", "./Shared/MainLayout.razor.js");
                await AddTempCartForUser();
                GetCartProductAmount();
            }
        }

        private async Task AddTempCartForUser()
        {
            // get temp cart
            var tempCourseCarts = await jsr.InvokeAsync<int[]>("mainLayoutPageJs.getTempCart");

            if (user.Identity.IsAuthenticated && tempCourseCarts != null && tempCourseCarts.Length > 0)
            {
                await AddTempCartIntoDb(tempCourseCarts);
                await jsr.InvokeVoidAsync("mainLayoutPageJs.removeTempCart");
            }
        }

        private async Task AddTempCartIntoDb(int[] tempCourseCarts)
        {
            foreach (var item in tempCourseCarts)
            {
                await _cartService.AddCourseToCart(new AddCourseToCartDto()
                {
                    CourseId = item,
                    UserId = userId
                });
            }
        }

        public void ShowAlert(string message, string title)
        {
            isShowAlert = true;
            alertTitle = title;
            alertMessage = message;
            StateHasChanged();
        }

        public void ShowForceAuthenAlert()
        {
            isShowForceAuthenAlert = true;
            alertTitle = "Thông báo";
            alertMessage = "Bạn vui lòng đăng nhập hoặc đăng kí tài khoản để tiến hành thanh toán!";
            StateHasChanged();
        }

        public void HideAlert()
        {
            isShowAlert = false;
            StateHasChanged();
        }

        public void HideAuthenAlert()
        {
            isShowForceAuthenAlert = false;
            StateHasChanged();
        }

        public void AddCourseToTempCart(int courseId)
        {
            jsr.InvokeVoidAsync("mainLayoutPageJs.addCourseToTempCart", courseId);
        }

        public async Task GetCartProductAmount()
        {

            if (user.Identity.IsAuthenticated)
            {
                cartProductAmount = await _cartService.GetCartProductAmount(userId);
            }
            else
            {
                cartProductAmount = await jsr.InvokeAsync<int>("mainLayoutPageJs.countCartProductAmount");
            }
            StateHasChanged();
        }
    }
}