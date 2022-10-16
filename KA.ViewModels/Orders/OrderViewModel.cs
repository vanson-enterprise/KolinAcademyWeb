using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        //public string Note { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
