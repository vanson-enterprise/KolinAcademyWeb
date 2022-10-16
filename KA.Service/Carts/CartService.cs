using AutoMapper;
using KA.Service.Base;
using KA.ViewModels.Carts;
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
        private readonly IMapper _mapper;

        public CartService(IRepository<Cart> baseReponsitory, IMapper mapper) : base(baseReponsitory)
        {
            this._cartRepo = baseReponsitory;
            _mapper = mapper;
        }


        public async Task<Cart> CreateNewCart(CreateCartVm input)
        {
            var cart = _mapper.Map<Cart>(input);   
            var result = await _cartRepo.AddAsync(cart);
            return result;
        }
    }
}
