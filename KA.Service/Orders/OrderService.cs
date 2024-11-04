using AutoMapper;
using KA.DataProvider.Entities;
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
        private readonly IRepository<OrderDetail> _orderDetailRepo;
        private readonly IRepository<UserCourse> _userCourseRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly IRepository<Lesson> _lessonRepo;
        private readonly IRepository<UserLesson> _userLessonRepo;
        private readonly IMapper _mapper;
        public OrderService(IRepository<Order> baseReponsitory,
            IMapper mapper,
            IRepository<AppUser> userRepo,
            IRepository<CartProduct> cartProductRepo,
            IRepository<Cart> cartRepo,
            IRepository<UserCourse> userCourseRepo,
            IRepository<Course> courseRepo,
            IRepository<OrderDetail> orderDetailRepo,
            IRepository<Lesson> lessonRepo,
            IRepository<UserLesson> userLessonRepo) :
        base(baseReponsitory)
        {
            _orderRepo = baseReponsitory;
            _mapper = mapper;
            _userRepo = userRepo;
            _cartProductRepo = cartProductRepo;
            _cartRepo = cartRepo;
            _userCourseRepo = userCourseRepo;
            _courseRepo = courseRepo;
            _orderDetailRepo = orderDetailRepo;
            _lessonRepo = lessonRepo;
            _userLessonRepo = userLessonRepo;
        }

        public Order CreateNewOrder(CreateOrderVm input)
        {
            var nOrder = _mapper.Map<Order>(input);
            var order = _orderRepo.Add(nOrder);
            order.Code = CreateOrderCode(order.Id);
            var orderDetails = input.CartProducts.Select(cp => new OrderDetail()
            {
                CourseId = cp.CourseId,
                DiscountPrice = cp.DiscountPrice,
                Price = cp.Price,
                OrderId = order.Id,
            });
            _orderDetailRepo.AddMany(orderDetails);
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

        public async Task<DataGridResponse<OrderItemVm>> GetAllOrderPaging(int skip, int top)
        {
            var result = new DataGridResponse<OrderItemVm>();

            var orders = from o in _orderRepo.GetAll()
                         join u in _userRepo.GetAll() on o.UserId equals u.Id
                         orderby o.CreatedDate descending
                         select new { o, u };

            result.TotalItem = orders.Count();
            result.Items = orders.Skip(skip).Take(top).ToList().Select((ou, i) =>
            {
                var ov = _mapper.Map<OrderItemVm>(ou.o);
                ov.Index = (i + 1) + skip;
                ov.Price = string.Format("{0:0,0 vnđ}", ou.o.Price);
                ov.DiscountPrice = string.Format("{0:0,0 vnđ}", ou.o.DiscountPrice);
                ov.TotalPrice = string.Format("{0:0,0 vnđ}", ou.o.TotalPrice);
                ov.CreatedDate = ou.o.CreatedDate.Value.ToString("dd/MM/yyyy");
                ov.CustomerName = ou.u.FullName;
                ov.PaymentMethod = ou.o.PaymentMethod;
                ov.PaymentStatus = ou.o.PaymentStatus;
                ov.OrderStatus = ou.o.OrderStatus;
                return ov;
            }).ToList();
            return result;
        }

        public async Task<OrderViewModel> GetDetailOrder(int orderId)
        {
            var orderProducts = (from o in _orderRepo.GetAll()
                                 join od in _orderDetailRepo.GetAll() on o.Id equals od.OrderId
                                 join co in _courseRepo.GetAll() on od.CourseId equals co.Id
                                 where o.OrderStatus == OrderStatus.INIT && o.Id == orderId
                                 select new { o, od, co }).ToList();
            if (orderProducts.Count > 0)
            {
                var groupOrderProduct = (from op in orderProducts
                                         group op by op.o into gop
                                         select gop).FirstOrDefault();
                var result = new OrderViewModel()
                {
                    Id = groupOrderProduct.Key.Id,
                    Price = groupOrderProduct.Key.Price,
                    DiscountPrice = groupOrderProduct.Key.DiscountPrice,
                    TotalPrice = groupOrderProduct.Key.TotalPrice,
                    PaymentMethod = groupOrderProduct.Key.PaymentMethod,
                    Code = groupOrderProduct.Key.Code,
                    OrderDetailViewModels = groupOrderProduct.Select(i => new OrderDetailViewModel()
                    {
                        Id = i.od.Id,
                        CourseName = i.co.Name,
                        DiscountPrice = i.co.DiscountPrice,
                        Price = i.co.Price
                    }).ToList()
                };
                return result;
            }
            else
            {
                return null;
            }
        }

        public void UpdateOrderInfo(OrderViewModel input)
        {
            var order = _orderRepo.GetById(input.Id);
            order.PaymentMethod = input.PaymentMethod;

            order.TotalPrice = input.TotalPrice - input.DiscountPrice;
            _orderRepo.Update(order);
        }

        public void UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var order = _orderRepo.GetById(orderId);
            if (order != null && order.OrderStatus == OrderStatus.INIT)
            {
                order.OrderStatus = orderStatus;
                if (orderStatus == OrderStatus.COMPLETED)
                {
                    // get all courses
                    var courses = (from od in _orderDetailRepo.GetAll()
                                   join co in _courseRepo.GetAll() on od.CourseId equals co.Id
                                   where od.OrderId == orderId
                                   select co).ToList();

                    // add user course
                    foreach (var c in courses)
                    {
                        InitUserCourse(order, c);
                    }
                    _orderRepo.Update(order);
                }
            }

        }

        public void UpdatePaymentStatus(int orderId, PaymentStatus paymentStatus)
        {
            var order = _orderRepo.GetById(orderId);
            if (order != null)
            {
                order.PaymentStatus = paymentStatus;
                _orderRepo.Update(order);
            }
        }

        private void InitUserCourse(Order order, Course c)
        {
            var existUc = _userCourseRepo.GetAll().Where((uc) => uc.UserId == order.UserId && uc.CourseId == c.Id).FirstOrDefault();
            if (existUc == null)
            {
                var newUserCourse = new UserCourse()
                {
                    CourseId = c.Id,
                    CreatedDate = DateTime.UtcNow,
                    ExpiredDate = DateTime.UtcNow.AddMonths(c.DurationTime.Value),
                    StudyProgress = 0,
                    UserId = order.UserId,
                    CreateUserId = order.UserId,
                };
                InitUserLesson(order.UserId, c.Id);
                _userCourseRepo.Add(newUserCourse);
            }
            else
            {
                if (existUc.ExpiredDate > DateTime.UtcNow)
                {
                    existUc.ExpiredDate = existUc.ExpiredDate.Value.AddMonths(c.DurationTime.Value);
                }
                else
                {
                    existUc.ExpiredDate = DateTime.UtcNow.AddMonths(c.DurationTime.Value);
                }
                existUc.UpdatedDate = DateTime.UtcNow;
                _userCourseRepo.Update(existUc);
            }
        }

        private void InitUserLesson(string userId, int courseId)
        {
            var userLessons = _lessonRepo.GetAll()
                .Where(l => l.CourseId == courseId)
                .Select(l => new UserLesson()
                {
                    LessonId = l.Id,
                    UserId = userId,
                    Status = UserLessonStatus.BLOCK
                }).ToList();
            userLessons[0].Status = UserLessonStatus.PROCESSING;
            _userLessonRepo.AddMany(userLessons);
        }
    }
}