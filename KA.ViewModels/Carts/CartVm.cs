using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Carts
{
    public class CartVm
    {
        public int Id { get; set; }
        public List<CartProductVm> CartProductVms { get; set; }
        public decimal Total { get; set; }
        public string StringTotal { get; set; }
        public int Amount { get; set; }
    }
}