using AutoMapper;
using KA.ViewModels.Carts;
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
        private readonly IRepository<CartProduct> _cartProductRepo;
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<UserCourse> _userCourseRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly IMapper _mapper;
        public OrderService(IRepository<Order> baseReponsitory,
            IMapper mapper,
            IRepository<AppUser> userRepo,
            IRepository<CartProduct> cartProductRepo,
            IRepository<Cart> cartRepo,
            IRepository<UserCourse> userCourseRepo,
            IRepository<Course> courseRepo) :
        base(baseReponsitory)
        {
            _orderRepo = baseReponsitory;
            _mapper = mapper;
            _userRepo = userRepo;
            _cartProductRepo = cartProductRepo;
            _cartRepo = cartRepo;
            _userCourseRepo = userCourseRepo;
            _courseRepo = courseRepo;
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

        public async Task<OrderDetailViewModel> GetDetailOrder(int orderId)
        {
            var orderProducts = (from o in _orderRepo.GetAll()
                                 join c in _cartRepo.GetAll() on o.CartId equals c.Id
                                 join cp in _cartProductRepo.GetAll() on c.Id equals cp.CartId
                                 join co in _courseRepo.GetAll() on cp.CourseId equals co.Id
                                 where o.OrderStatus == OrderStatus.INIT && o.Id == orderId
                                 select new { o, cp, co }).ToList();
            if (orderProducts.Count > 0)
            {
                var groupOrderProduct = (from op in orderProducts
                                         group op by op.o into gop
                                         select gop).FirstOrDefault();
                var result = new OrderDetailViewModel()
                {
                    Id = groupOrderProduct.Key.Id,
                    Price = groupOrderProduct.Key.Price,
                    DiscountPrice = groupOrderProduct.Key.DiscountPrice,
                    TotalPrice = groupOrderProduct.Key.TotalPrice,
                    PaymentMethod = groupOrderProduct.Key.PaymentMethod,
                    CartProductVms = groupOrderProduct.Select(i => new CartProductVm()
                    {
                        Id = i.cp.Id,
                        CourseName = i.co.Name,
                        DiscountPrice = string.Format("{0:0,0.00 vnđ}", i.cp.DiscountPrice),
                    }).ToList()
                };
                return result;
            }
            else
            {
                return null;
            }
        }

        public void UpdateOrderInfo(OrderDetailViewModel input)
        {
            var order = _orderRepo.GetById(input.Id);
            order.PaymentMethod = input.PaymentMethod;

            order.TotalPrice = input.TotalPrice - input.DiscountPrice;
            _orderRepo.Update(order);
        }

        public void UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var order = _orderRepo.GetById(orderId);

            order.OrderStatus = orderStatus;
            if (orderStatus == OrderStatus.COMPLETED)
            {
                // get all courses
                var courses = (from c in _cartRepo.GetAll()
                               join cp in _cartProductRepo.GetAll() on c.Id equals cp.CartId
                               join co in _courseRepo.GetAll() on cp.CourseId equals co.Id
                               where order.CartId == c.Id
                               select co).AsEnumerable();

                // add user course
                foreach (var course in courses)
                {
                    _userCourseRepo.Add(new UserCourse()
                    {
                        CourseId = course.Id,
                        CreatedDate = DateTime.UtcNow,
                        ExpiredDate = DateTime.UtcNow.AddMonths(course.DurationTime.Value),
                        StudyProgress = 0,
                        UserId = order.UserId,
                        CreateUserId = order.UserId,
                    });
                }
            }
        }
    }
}