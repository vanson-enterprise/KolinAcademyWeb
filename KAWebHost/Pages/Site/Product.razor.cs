using KA.Service.Carts;
using KA.Service.Courses;
using KA.ViewModels.Carts;
using KA.ViewModels.Courses;
using KAWebHost.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class Product : OwningComponentBase
    {
        // service
        private ICourseService _courseService;
        private ICartService _cartService;

        // model
        private List<OnlineCourseViewModel> onlineCourseViewModels;

        // parameters
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [CascadingParameter]
        private MainLayout mainLayout { get; set; }


        // properties
        private string userId;
        private AuthenticationState authState;

        protected override async void OnInitialized()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            _cartService = ScopedServices.GetRequiredService<ICartService>();
            authState = await authenticationStateTask;
            InitData();
        }

        private void InitData()
        {
            onlineCourseViewModels = _courseService.GetAllOnlineCourse();
            userId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
        }


        private void AddToCart(int courseId)
        {
            if (userId != null)
            {
                _cartService.AddCourseToCart(new AddCourseToCartDto()
                {
                    CourseId = courseId,
                    UserId = userId
                });
            }
            else
            {
                mainLayout.AddCourseToTempCart(courseId);
            }
            // Show alert
            mainLayout.ShowAlert("Thêm khóa học vào giỏ hàng thành công", "Thông báo");
        }
    }
}