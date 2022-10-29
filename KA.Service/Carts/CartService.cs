using AutoMapper;
using KA.Service.Base;
using KA.ViewModels.Carts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Carts
{
    public class CartService : BaseService<Cart>, ICartService
    {
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly IRepository<CartProduct> _cartProductRepo;
        private readonly IMapper _mapper;

        public CartService(IRepository<Cart> baseReponsitory, IMapper mapper, IRepository<CartProduct> cartProductRepo, IRepository<Course> courseRepo) : base(baseReponsitory)
        {
            this._cartRepo = baseReponsitory;
            _mapper = mapper;
            _cartProductRepo = cartProductRepo;
            _courseRepo = courseRepo;
        }

        public async Task AddCourseToCart(AddCourseToCartDto input)
        {
            Cart? cart = _cartRepo.GetAll().Where(c => c.UserId == input.UserId && c.CartStatus == CartStatus.PreOrder).FirstOrDefault();
            if (cart == null)
            {
                cart = _cartRepo.Add(new Cart()
                {
                    UserId = input.UserId,
                    CartStatus = CartStatus.PreOrder
                });
            }
            else
            {
                // Check have cart product
                var cartProduct = _cartProductRepo.GetAll()
                    .Where(cp => cp.CartId == cart.Id &&
                    cp.CourseId == input.CourseId)
                    .FirstOrDefault();
                if (cartProduct != null)
                {
                    return;
                }
            }
            var course = _courseRepo.GetById(input.CourseId);
            if (course != null)
            {
                _cartProductRepo.Add(new CartProduct()
                {
                    CartId = cart.Id,
                    CourseName = course.Name,
                    CourseId = input.CourseId,
                    Price = course.Price,
                    DiscountPrice = course.DiscountPrice,
                });
            }

        }

        public async Task<CartVm> GetAllCartProduct(string userId)
        {
            var result = new CartVm();
            var cartProducts = await (from c in _cartRepo.GetAll()
                                      join cp in _cartProductRepo.GetAll() on c.Id equals cp.CartId
                                      where c.CartStatus == CartStatus.PreOrder && c.UserId == userId
                                      select cp).ToListAsync();
            result.CartProductVms = cartProducts.Select(cp => new CartProductVm()
            {
                Id = cp.Id,
                CourseName = cp.CourseName,
                DiscountPrice = string.Format("{0:0,0.00 vnđ}", cp.DiscountPrice),
                Price = string.Format("{0:0,0.00 vnđ}", cp.Price)
            }).ToList();
            result.Total = string.Format("{0:0,0.00 vnđ}", cartProducts.Select(cp => cp.DiscountPrice).Sum());
            result.Amount = cartProducts.Count;
            return result;
        }
    }
}