using KA.Infrastructure.Enums;
using KA.PaymentAPI.CyberSource;
using KA.Service.Carts;
using KA.Service.Orders;
using KA.ViewModels.Carts;
using KA.ViewModels.Orders;
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

        private ICartService _cartService;
        private IOrderService _orderService;
        private CyberSourceService _cyberSourceService;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;

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
            await InitData();
        }

        private async Task InitData()
        {
            cartVm = new()
            {
                CartProductVms = new()
            };
            userId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            cartVm = await _cartService.GetCartByUserId(userId);
        }

        private async Task InitOrder()
        {
            var newOrder = new CreateOrderVm()
            {
                CartId = cartVm.Id,
                Code = "temp",
                CreatedDate = DateTime.Now,
                DiscountPrice = 0,
                OrderStatus = OrderStatus.INIT,
                PaymentMethod = PaymentMethod.VNPAY,
                PaymentStatus = PaymentStatus.WAITING,
                Price = cartVm.Total,
                TotalPrice = cartVm.Total,
                UserId = userId,
            };
            var order = _orderService.CreateNewOrder(newOrder);
            if (order.Id > 0)
            {
                await _cartService.UpdateCartStatus(cartVm.Id, CartStatus.Ordered);
                await jsr.InvokeVoidAsync("ShowAlert", "Tạo đơn thành công");
                navigationManager.NavigateTo("/don-hang/" + order.Id);
            }
            else
            {
                jsr.InvokeVoidAsync("ShowAlert", "Đã có lỗi xảy ra");
            }

        }
    }
}