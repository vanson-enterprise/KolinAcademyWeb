using AutoMapper;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
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
        private readonly IRepository<AppUser> _userRepo;
        private readonly IMapper _mapper;
        public OrderService(IRepository<Order> baseReponsitory, IMapper mapper, IRepository<AppUser> userRepo) : base(baseReponsitory)
        {
            _orderRepo = baseReponsitory;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public Order CreateNewOrder(CreateOrderVm input)
        {
            var nOrder = _mapper.Map<Order>(input);
            var order = _orderRepo.Add(nOrder);
            order.Code = CreateOrderCode(order.Id);
            _orderRepo.Update(order);
            return order;
        }

        private string CreateOrderCode(int id)
        {
            var stringId = id.ToString();
            var idLength = stringId.Length;
            if (stringId.Length < 6)
            {
                for (int i = 0; i < 6 - idLength; i++)
                {
                    stringId = "0" + stringId;
                }
            }
            return stringId;
        }

        public async Task<DataGridResponse<OrderViewModel>> GetAllOrderPaging(int skip, int top)
        {
            var result = new DataGridResponse<OrderViewModel>();

            var orders = from o in _orderRepo.GetAll()
                         join u in _userRepo.GetAll() on o.UserId equals u.Id
                         select new { o, u };

            result.TotalItem = orders.Count();
            result.Items = orders.Skip(skip).Take(top).ToList().Select((ou, i) =>
            {
                var ov = _mapper.Map<OrderViewModel>(ou.o);
                ov.Index = (i + 1) + skip;
                ov.Price = string.Format("{0:0,0.00 vnđ}", ou.o.Price);
                ov.DiscountPrice = string.Format("{0:0,0.00 vnđ}", ou.o.DiscountPrice);
                ov.TotalPrice = string.Format("{0:0,0.00 vnđ}", ou.o.TotalPrice);
                ov.CreatedDate = ou.o.CreatedDate.Value.ToString("dd/MM/yyyy");
                ov.CustomerName = ou.u.FullName;
                ov.PaymentMethod = ou.o.PaymentMethod;
                ov.PaymentStatus = ou.o.PaymentStatus;
                ov.OrderStatus = ou.o.OrderStatus;
                return ov;
            }).ToList();
            return result;
        }
    }
}