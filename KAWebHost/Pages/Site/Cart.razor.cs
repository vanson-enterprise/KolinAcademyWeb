using KA.Infrastructure.Enums;
using KA.PaymentAPI.CyberSource;
using KA.Service.Carts;
using KA.Service.Orders;
using KA.ViewModels.Carts;
using KA.ViewModels.Orders;
using KAWebHost.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class Cart : OwningComponentBase
    {
        private CartVm cartVm;
        private string userId;
        private int[] tempCourseCarts;
        private AuthenticationState authState;

        private ICartService _cartService;
        private IOrderService _orderService;
        private CyberSourceService _cyberSourceService;

        // paramters
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [CascadingParameter]
        private MainLayout mainLayout { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }
        [Inject]
        IJSRuntime jsr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _cartService = ScopedServices.GetRequiredService<ICartService>();
            _orderService = ScopedServices.GetRequiredService<IOrderService>();
            _cyberSourceService = ScopedServices.GetRequiredService<CyberSourceService>();
            authState = await authenticationStateTask;

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await jsr.InvokeVoidAsync("import", "./Pages/Site/Cart.razor.js");
            if (firstRender)
            {
                InitData();
            }
        }

        private async Task InitData()
        {
            cartVm = new()
            {
                CartProductVms = new()
            };
            userId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();

            // get temp cart
            tempCourseCarts = await jsr.InvokeAsync<int[]>("cartPageJs.getTempCart");

            if (userId != null)
            {
                if (tempCourseCarts != null && tempCourseCarts.Length > 0)
                {
                    await AddTempCartIntoDb();
                }
                cartVm = await _cartService.GetCartByUserId(userId);
                StateHasChanged();
                jsr.InvokeVoidAsync("cartPageJs.removeTempCart");
            }
            else if (tempCourseCarts != null && tempCourseCarts.Length > 0)
            {
                cartVm = _cartService.GetTempCart(tempCourseCarts);
                cartVm.Id = null;
                StateHasChanged();
            }
        }

        private async Task AddTempCartIntoDb()
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

        private async Task InitOrder()
        {
            if (userId == null)
            {
                mainLayout.ShowForceAuthenAlert();
            }
            else
            {
                var newOrder = new CreateOrderVm()
                {
                    CartId = cartVm.Id,
                    Code = "temp",
                    CreatedDate = DateTime.Now,
                    DiscountPrice = 0,
                    OrderStatus = OrderStatus.INIT,
                    PaymentMethod = PaymentMethod.CK,
                    PaymentStatus = PaymentStatus.WAITING,
                    Price = cartVm.Total,
                    TotalPrice = cartVm.Total,
                    UserId = userId,
                    CartProducts = cartVm.CartProductVms
                };
                var order = _orderService.CreateNewOrder(newOrder);
                if (order.Id > 0)
                {
                    await _cartService.UpdateCartStatus(cartVm.Id.Value, CartStatus.Ordered);
                    navigationManager.NavigateTo("/don-hang/" + order.Id);
                }
                else
                {
                    mainLayout.ShowAlert("Đã có lỗi xảy ra, vui lòng thử lại!", "Lỗi!!");
                }
            }
        }
    }
}