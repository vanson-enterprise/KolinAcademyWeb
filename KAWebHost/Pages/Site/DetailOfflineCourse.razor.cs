using KA.DataProvider.Entities;
using KA.Service.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Site
{
    public partial class DetailOfflineCourse : OwningComponentBase
    {
        [Parameter]
        public string CourseSeoName { get; set; }
        public ICourseService _courseService { get; set; }
        public Course model { get; set; } = new();
        private static bool isOpenRegisterModal = false;

        [Inject]
        private IJSRuntime jsr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetService<ICourseService>();
            int courseId = Convert.ToInt32(CourseSeoName.Split('-').Last());
            model = _courseService.GetById(courseId);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (model.IsUseExternalHtml && model.ExternalScriptLink != null)
            {
                await jsr.InvokeVoidAsync("import", model.ExternalScriptLink);
                //await jsr.InvokeVoidAsync("detailCoursePageJs.init");
            }
            await jsr.InvokeVoidAsync("import", "./Pages/Site/DetailOfflineCourse.razor.js");
            await jsr.InvokeVoidAsync("detailOfflineCoursePageJs.init");
        }

        public void ShowRegisterOfflineCourseModal()
        {
            isOpenRegisterModal = true;
        }

        private void HideRegisterOfflineCourseModal()
        {
            isOpenRegisterModal = false;
            StateHasChanged();
        }
    }
}