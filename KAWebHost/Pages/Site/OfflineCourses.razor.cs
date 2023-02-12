using KA.Service.Courses;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Site
{
    public partial class OfflineCourses : OwningComponentBase
    {
        ICourseService courseService;
        List<OfflineCourseViewModel> offlineCourseVms;

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            courseService = ScopedServices.GetService<ICourseService>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            offlineCourseVms = await courseService.GetTopOffCourseForIndexPage(1000);
            StateHasChanged();
        }

        private void GoToRegisterPage(int courseId)
        {
            NavigationManager.NavigateTo("/dang-ky-khoa-hoc/" + courseId);
        }

    }
}