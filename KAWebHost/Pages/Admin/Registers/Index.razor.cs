using KA.Infrastructure.Enums;
using KA.Service.Courses;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace KAWebHost.Pages.Admin.Registers
{
    public partial class Index : OwningComponentBase
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        [Inject]
        private IJSRuntime jsr { get; set; }

        private DataGridResponse<OfflineCourseRegisterVm> dataGrid;
        private ICourseService _courseService;
        private int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            await GetAllCourse();
        }

        private async Task GetAllCourse()
        {
            dataGrid = await _courseService.GetAllOfflineCourseRegisterPaging(0, pageSize);
        }

        private async Task PageChanged(PagerEventArgs args)
        {
            dataGrid = await _courseService.GetAllOfflineCourseRegisterPaging(args.Skip, args.Top);
        }

        private void EditRow(OfflineCourseRegisterVm course)
        {
            //if (course.Type == CourseType.OFFLINE)
            //{
            //    NavigationManager.NavigateTo($"/manager/edit-off-course/{course.Id}");
            //}
            //else
            //{
            //    NavigationManager.NavigateTo($"/manager/edit-on-course/{course.Id}");
            //}
        }

        private void DeleteCourse(int id)
        {
            var result = _courseService.DeleteById(id);
            if (result.Status == ResponseStatus.SUCCESS)
            {
                jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
                GetAllCourse();
            }
            else
            {
                jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
            }
        }
    }
}
