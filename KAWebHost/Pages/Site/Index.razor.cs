using KA.Service.Blogs;
using KA.Service.Courses;
using KA.ViewModels.Blogs;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Site
{
    public partial class Index : OwningComponentBase
    {
        private ICourseService _courseService;
        private IBlogService _blogService;
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        //private List<OfflineCourseViewModel> offlineCourseViewModels;
        private List<OnlineCourseViewModel> topTwoOnlineCourses;
        private List<OfflineCourseViewModel> offlineCourses;
        private List<BlogViewModel> blogViewModels;
        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            InitData();
        }

        private async Task InitData()
        {

            //offlineCourseViewModels = _courseService.GetAllOpeningSoonOfflineCourse();
            topTwoOnlineCourses = await _courseService.GetTopTwoOnlineCourseForIndexPage();
            offlineCourses = await _courseService.GetTopOffCourseForIndexPage(6);
            blogViewModels = await _blogService.GetTopFourBlogForHomePage();
            StateHasChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var module = await jsr.InvokeAsync<IJSObjectReference>("import", "./Pages/Site/Index.razor.js");
                await module.InvokeVoidAsync("indexPageModule.init");
            }
        }

        private void GoToRegisterPage(int courseId)
        {
            NavigationManager.NavigateTo("/dang-ky-khoa-hoc/" + courseId);
        }

        private void GoToDetailOnlineCourse(string path)
        {
            NavigationManager.NavigateTo(path);
        }
    }
}