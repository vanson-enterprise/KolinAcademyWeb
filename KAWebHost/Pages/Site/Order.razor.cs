using KA.Service.Orders;
using KA.ViewModels.Orders;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Site
{
    public partial class Order : OwningComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        private IOrderService _orderService;
        [Inject]
        private NavigationManager navigationManager { get; set; }

        private OrderDetailViewModel model = new();

        protected override async Task OnInitializedAsync()
        {
            _orderService = ScopedServices.GetRequiredService<IOrderService>();
            model = await _orderService.GetDetailOrder(Id);
        }

        private void CheckOut()
        {
            _orderService.UpdateOrderInfo(model);
            if (model.PaymentMethod == KA.Infrastructure.Enums.PaymentMethod.VISA)
            {
                navigationManager.NavigateTo("/visa-payment/" + model.Id);
            }
        }
    }
}