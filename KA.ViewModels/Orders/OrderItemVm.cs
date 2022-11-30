using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Orders
{
    public class OrderItemVm
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }

        public string Price { get; set; }

        public string DiscountPrice { get; set; }
        public string TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        //public string Note { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string CreatedDate { get; set; }
    }
}