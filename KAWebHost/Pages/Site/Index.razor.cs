using KA.Service.Courses;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Site
{
    public partial class Index : OwningComponentBase
    {
        private ICourseService _courseService;
        private List<OfflineCourseViewModel> offlineCourseViewModels;

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            offlineCourseViewModels = _courseService.GetAllOpeningSoonOfflineCourse();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/owl.carousel.min.js");
                var module = await jsr.InvokeAsync<IJSObjectReference>("import", "./Pages/Site/Index.razor.js");
                await module.InvokeVoidAsync("indexPageModule.init");
            }
        }
    }
}