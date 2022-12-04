using AutoMapper;
using KA.DataProvider.Entities;
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
            if (cart == null && input.UserId != null)
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
                    CourseId = input.CourseId,
                    Price = course.Price,
                    DiscountPrice = course.DiscountPrice,
                });
            }

        }

        public async Task<CartVm> GetCartByUserId(string userId)
        {
            var result = new CartVm();
            var cart = _cartRepo.GetAll().Where(c => c.UserId == userId
                            && c.CartStatus == CartStatus.PreOrder).FirstOrDefault();
            if (cart != null)
            {
                var cartProducts = await (from cp in _cartProductRepo.GetAll()
                                          join c in _courseRepo.GetAll() on cp.CourseId equals c.Id
                                          where cp.CartId == cart.Id
                                          select new { c, cp }).ToListAsync();
                result.CartProductVms = cartProducts.Select(i => new CartProductVm()
                {
                    Id = i.cp.Id,
                    CourseId = i.cp.CourseId,
                    CourseName = i.c.Name,
                    DiscountPrice = i.cp.DiscountPrice,
                    Price = i.cp.Price
                }).ToList();
                result.Total = cartProducts.Select(i => i.cp.DiscountPrice).Sum();
                result.StringTotal = string.Format("{0:0,0.00 vnđ}", result.Total);
                result.Amount = cartProducts.Count;
                result.Id = cart.Id;

            }

            return result;

        }

        public async Task UpdateCartStatus(int cartId, CartStatus cartStatus)
        {
            var cart = _cartRepo.GetById(cartId);
            cart.CartStatus = cartStatus;
            _cartRepo.Update(cart);
        }

        public CartVm GetTempCart(int[] courseIds)
        {
            var result = new CartVm();
            var courses = _courseRepo.GetAll().Where(c => courseIds.Contains(c.Id));
            result.CartProductVms = courses.Select(i => new CartProductVm()
            {
                CourseId = i.Id,
                CourseName = i.Name,
                DiscountPrice = i.DiscountPrice,
                Price = i.Price
            }).ToList();
            result.Amount = courseIds.Count();
            result.Total = courses.Select(c => c.DiscountPrice).Sum();
            result.StringTotal = string.Format("{0:0,0 vnđ}", result.Total);
            return result;
        }

        public async Task<int> GetCartProductAmount(string userId)
        {
            var cart = _cartRepo.GetAll().Where(c => c.UserId == userId
                            && c.CartStatus == CartStatus.PreOrder).FirstOrDefault();
            if (cart == null)
                return 0;
            return await _cartProductRepo.GetAll().Where(cp => cp.CartId == cart.Id).CountAsync();
        }

        public async Task RemoveCourseFromCart(int cartPorductId)
        {
            _cartProductRepo.DeleteById(cartPorductId);
        }
    }

}