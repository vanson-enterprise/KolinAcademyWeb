using KA.DataProvider.Entities;
using KA.Service.Courses;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Diagnostics;

namespace KAWebHost.Pages.Admin.Courses
{
    public partial class Index : OwningComponentBase
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        private DataGridResponse<CourseItem> dataGrid;
        private ICourseService _courseService;
        private int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            await GetAllCourse();
        }

        private async Task GetAllCourse()
        {
            dataGrid = await _courseService.GetAllCoursePaging(0, pageSize);
        }

        private async Task PageChanged(PagerEventArgs args)
        {
            dataGrid = await _courseService.GetAllCoursePaging(args.Skip, args.Top);
        }

        private void EditRow(CourseItem course)
        {
            if(course.Type == KA.Infrastructure.Enums.CourseType.OFFLINE)
            {
                NavigationManager.NavigateTo($"vnexpress.vn");
            }
            else
            {
                NavigationManager.NavigateTo($"/manager/edit-on-course/{course.Id}");
            }
        }

    }
}