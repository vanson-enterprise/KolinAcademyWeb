using AutoMapper;
using KA.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Orders
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IMapper _mapper;
        public OrderService(IRepository<Order> baseReponsitory, IMapper mapper) : base(baseReponsitory)
        {
            _orderRepo = baseReponsitory;
            _mapper = mapper;
        }

        public Order Create(CreateOrderVm input)
        {
            var nOrder = _mapper.Map<Order>(input);
            var result = _orderRepo.Add(nOrder);
            return result;
        }


    }
}
