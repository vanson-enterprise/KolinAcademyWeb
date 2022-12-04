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
        Task<CartVm> GetCartByUserId(string userId);
        Task<int> GetCartProductAmount(string userId);
        CartVm GetTempCart(int[] courseIds);
        Task RemoveCourseFromCart(int cartPorductId);
        Task UpdateCartStatus(int cartId, CartStatus cartStatus);
    }
}