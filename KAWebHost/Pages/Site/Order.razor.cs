using KA.Infrastructure.Enums;
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

        private OrderViewModel model = new()
        {
            OrderDetailViewModels = new()
        };

        protected override async Task OnInitializedAsync()
        {
            _orderService = ScopedServices.GetRequiredService<IOrderService>();
            var orderVm = await _orderService.GetDetailOrder(Id);
            if (orderVm == null)
            {
                navigationManager.NavigateTo("/");
            }
            else
            {
                model = orderVm;
            }
        }



        private void CheckOut()
        {
            _orderService.UpdateOrderInfo(model);
            if (model.PaymentMethod == PaymentMethod.VISA)
            {
                navigationManager.NavigateTo("/visa-payment/" + model.Id);
            }
            else if (model.PaymentMethod == PaymentMethod.CK)
            {
#if DEBUG
                _orderService.UpdateOrderStatus(model.Id, OrderStatus.COMPLETED);
                navigationManager.NavigateTo("/");
#endif
            }

        }
    }
}