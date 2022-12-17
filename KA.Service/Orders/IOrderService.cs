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
        Task<DataGridResponse<OrderItemVm>> GetAllOrderPaging(int skip, int top);
        Task<OrderViewModel> GetDetailOrder(int orderId);
        void UpdateOrderInfo(OrderViewModel input);
        void UpdateOrderStatus(int orderId, OrderStatus orderStatus);
        void UpdatePaymentStatus(int orderId, PaymentStatus paymentStatus);
    }
}