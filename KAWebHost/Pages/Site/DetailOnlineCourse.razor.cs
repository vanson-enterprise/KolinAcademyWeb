using KA.DataProvider.Entities;
using KA.Service.Carts;
using KA.Service.Courses;
using KA.Service.Users;
using KA.ViewModels.Carts;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class DetailOnlineCourse : OwningComponentBase
    {

        [Parameter]
        public string CourseSeoName { get; set; }
        public ICourseService _courseService { get; set; }
        public ICartService _cartService { get; set; }
        public IUserService _userService { get; set; }
        public DetailOnlineCourseModel model { get; set; } = new()
        {
            Lessons = new()
        };
        private string userId { get; set; }
        private string state { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authState;

        [Inject]
        private IJSRuntime jsr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            authState = await authenticationStateTask;
            _courseService = ScopedServices.GetService<ICourseService>();
            _cartService = ScopedServices.GetService<ICartService>();
            _userService = ScopedServices.GetRequiredService<IUserService>();
            await InitData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await jsr.InvokeVoidAsync("import", "./Pages/Site/DetailOnlineCourse.razor.js");
        }

        private async Task InitData()
        {
            int courseId = Convert.ToInt32(CourseSeoName.Split('-').Last());
            model = await _courseService.GetDetailOnlineCourse(courseId);
            userId = authState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            if (userId == null)
            {
                state = "Chưa đăng nhập";
            }
            else
            {
                // check mua khóa học chưa
                var userCourseSubcription = await _userService.GetPurchasedCourse(userId, model.Id);
                if (userCourseSubcription != null)
                {
                    if (userCourseSubcription.ExpiredDate < DateTime.Now)
                    {
                        state = "Đã hết hạn";
                    }
                    else if (userCourseSubcription.StudyProgress < 100)
                    {
                        state = "Đang học";
                    }
                    else
                    {
                        state = "Đã hoàn thành";
                    }

                }
                else
                {
                    state = "Chưa mua";
                }
            }
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
