using KA.Infrastructure.Enums;
using KA.Service.Courses;
using KA.Service.Orders;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using KA.ViewModels.Orders;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace KAWebHost.Pages.Admin.Orders
{
    public partial class Index
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        [Inject]
        private IJSRuntime jsr { get; set; }

        private DataGridResponse<OrderViewModel> dataGrid;
        private IOrderService _orderService;
        private int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {
            _orderService = ScopedServices.GetRequiredService<IOrderService>();
            await GetAllOrders();
        }

        private async Task GetAllOrders()
        {
            dataGrid = await _orderService.GetAllOrderPaging(0, pageSize);
        }

        private async Task PageChanged(PagerEventArgs args)
        {
            dataGrid = await _orderService.GetAllOrderPaging(args.Skip, args.Top);
        }

        private void ShowDetailOrder(int id)
        {
            //var result = _courseService.DeleteById(id);
            //if (result.Status == ResponseStatus.SUCCESS)
            //{
            //    jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
            //    GetAllCourse();
            //}
            //else
            //{
            //    jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
            //}
        }
    }
}