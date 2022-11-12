using KA.ViewModels.Common;
using KA.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Orders
{
    public interface IOrderService : IService<Order>
    {
        Order CreateNewOrder(CreateOrderVm input);
        Task<DataGridResponse<OrderViewModel>> GetAllOrderPaging(int skip, int top);
        Task<OrderDetailViewModel> GetDetailOrder(int orderId);
        void UpdateOrderInfo(OrderDetailViewModel input);
    }
}