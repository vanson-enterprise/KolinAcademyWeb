using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Carts
{
    public class CreateCartVm
    {
        public string UserId { get; set; }

        public CartStatus CartStatus { get; set; } = CartStatus.PreOrder;
    }
}
