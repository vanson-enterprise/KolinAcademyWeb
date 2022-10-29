using KA.Service.Carts;
using KA.Service.Courses;
using KA.ViewModels.Carts;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class Product : OwningComponentBase
    {
        private ICourseService _courseService;
        private ICartService _cartService;
        private List<OnlineCourseViewModel> onlineCourseViewModels;
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;
        private string userId;

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
            _cartService.AddCourseToCart(new AddCourseToCartDto()
            {
                CourseId = courseId,
                UserId = userId
            });
        }
    }
}