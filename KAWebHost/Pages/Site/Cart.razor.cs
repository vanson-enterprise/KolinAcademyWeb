using KA.DataProvider.Entities;
using KA.Service.Carts;
using KA.ViewModels.Carts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class Cart : OwningComponentBase
    {
        private ICartService _cartService;
        private CartVm cartVm;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;
        private string userId;
        protected override async Task OnInitializedAsync()
        {
            _cartService = ScopedServices.GetRequiredService<ICartService>();
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
            cartVm = await _cartService.GetAllCartProduct(userId);
        }
    }
}