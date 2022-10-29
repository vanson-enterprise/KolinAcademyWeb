using KA.ViewModels.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Carts
{
    public interface ICartService : IService<Cart>
    {
        Task AddCourseToCart(AddCourseToCartDto input);
        Task<CartVm> GetAllCartProduct(string userId);
    }
}