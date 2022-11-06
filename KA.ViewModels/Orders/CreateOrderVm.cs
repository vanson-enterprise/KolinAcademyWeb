using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Orders
{
    public class CreateOrderVm
    {
        public string? UserId { get; set; }
        public int? CartId { get; set; }
        public string Code { get; set; }
        // Giá tạm tính
        public decimal Price { get; set; }
        // Giá giảm (triết khấu)
        public decimal DiscountPrice { get; set; }
        // Thành tiền 
        public decimal TotalPrice { get; set; }
        // Phương thức thanh toán
        public PaymentMethod PaymentMethod { get; set; }

        // Trạng thái order
        public OrderStatus OrderStatus { get; set; }
        // Trạng thái thanh toán
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}