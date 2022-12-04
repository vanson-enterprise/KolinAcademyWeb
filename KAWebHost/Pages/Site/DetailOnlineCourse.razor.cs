using Castle.DynamicProxy.Contributors;
using KA.DataProvider.Entities;
using KA.Infrastructure.Enums;
using KA.Service.Carts;
using KA.Service.Courses;
using KA.Service.Users;
using KA.ViewModels.Carts;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using KAWebHost.Pages.Admin.Components;
using KAWebHost.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class DetailOnlineCourse : OwningComponentBase
    {
        // Parameter
        [Parameter]
        public string CourseSeoName { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [CascadingParameter]
        private MainLayout mainLayout { get; set; }

        // services
        public ICourseService _courseService { get; set; }
        public ICartService _cartService { get; set; }
        public IUserService _userService { get; set; }
        [Inject]
        private IJSRuntime jsr { get; set; }



        // Properies
        public DetailOnlineCourseModel model { get; set; } = new()
        {
            Lessons = new()
        };
        public List<UserLessonViewModel> userLessonVms { get; set; } = new();
        private string userId { get; set; }
        private OnlineCourseState state { get; set; }
        private string currentVideoLink { get; set; }
        private ClaimsPrincipal appUser;
        private int courseId;
        private int currentLessonIndex;
        public DotNetObjectReference<DetailOnlineCourse> DotNetRef;

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetService<ICourseService>();
            _cartService = ScopedServices.GetService<ICartService>();
            _userService = ScopedServices.GetRequiredService<IUserService>();

            appUser = (await authenticationStateTask).User;
            userId = appUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            DotNetRef = DotNetObjectReference.Create(this);
            await InitData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "./Pages/Site/DetailOnlineCourse.razor.js");
                await jsr.InvokeVoidAsync("onlineCoursePageJs.init", DotNetRef, "Unlock_And_Go_To_Next_Lesson");
                LoadVideo();
            }
        }

        private async Task LoadVideo()
        {
            jsr.InvokeVoidAsync("onlineCoursePageJs.loadVideo");
        }

        private async Task InitData()
        {
            courseId = Convert.ToInt32(CourseSeoName.Split('-').Last());
            model = await _courseService.GetDetailOnlineCourse(courseId);

            currentVideoLink = model.IntroduceVideoLink;

            if (!appUser.Identity.IsAuthenticated)
            {
                state = OnlineCourseState.UNAUTHEN;
            }
            else
            {
                // check course status
                var userCourseSubcription = await _userService.GetPurchasedCourse(userId, model.CourseId);
                if (userCourseSubcription != null)
                {
                    if (userCourseSubcription.ExpiredDate < DateTime.Now)
                    {
                        state = OnlineCourseState.EXPIRED;
                    }
                    else
                    {
                        GetUserLessons();
                        if (userCourseSubcription.StudyProgress < 100)
                        {
                            state = OnlineCourseState.STUDYING;
                            // get last lesson
                        }
                        else
                        {
                            state = OnlineCourseState.DONE;
                        }
                    }

                }
                else
                {
                    state = OnlineCourseState.NOT_PURCHASED;
                }
            }
        }

        private void StudyLesson(UserLessonViewModel lesson, int lessonIndex)
        {
            if (lesson.UserLessonStatus == UserLessonStatus.BLOCK)
            {
                mainLayout.ShowAlert("Bạn phải học xong bài hiện tại mới học được bài kế tiếp", "Thông báo");

            }
            else
            {
                currentVideoLink = lesson.VideoLink;
                currentLessonIndex = lessonIndex;
                LoadVideo();
            }

        }

        private async Task GetUserLessons()
        {
            userLessonVms = await _courseService.GetUserLessons(userId, courseId);
            currentVideoLink = userLessonVms.First(ul => ul.UserLessonStatus == UserLessonStatus.PROCESSING).VideoLink;
        }

        [JSInvokable("Unlock_And_Go_To_Next_Lesson")]
        public void UnlockAndGoToNextLesson()
        {
            // check is the last lesson
            if ((currentLessonIndex + 1) == userLessonVms.Count)
            {

                return;
            }
            else
            {
                var nextLesson = userLessonVms[currentLessonIndex + 1];
                nextLesson.UserLessonStatus = UserLessonStatus.PROCESSING;
                _courseService.UpdateUserLessonStatus(userLessonVms[currentLessonIndex].UserLessonId, UserLessonStatus.DONE);
                _courseService.UpdateUserLessonStatus(nextLesson.UserLessonId, UserLessonStatus.PROCESSING);

                currentVideoLink = nextLesson.VideoLink;
                currentLessonIndex++;
                StateHasChanged();
                LoadVideo();
            }
        }

        private void AddToCart(int courseId)
        {
            _cartService.AddCourseToCart(new AddCourseToCartDto()
            {
                CourseId = courseId,
                UserId = userId
            });
            // Show alert
            mainLayout.ShowAlert("Thêm khóa học vào giỏ hàng thành công", "Thông báo");
        }
    }
}