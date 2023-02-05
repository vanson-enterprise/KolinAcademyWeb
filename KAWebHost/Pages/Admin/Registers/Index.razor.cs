using DocumentFormat.OpenXml.Office.CustomUI;
using KA.Infrastructure.Enums;
using KA.Infrastructure.Enums.Extension;
using KA.Service.Courses;
using KA.Service.XLS;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;

namespace KAWebHost.Pages.Admin.Registers
{
    public partial class Index : OwningComponentBase
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        [Inject]
        IJSRuntime jsr { get; set; }

        DataGridResponse<OfflineCourseRegisterVm> dataGrid;
        ICourseService _courseService;
        int pageSize = 10;
        Dictionary<string, string> offlineCourses;
        GetAllOfflineCourseRegisterPagingInput searchModel = new GetAllOfflineCourseRegisterPagingInput
        {
            Skip = 0,
            Top = 10
        };

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();

            await GetAllOfflineCourseRegister();
            offlineCourses = (await _courseService
                            .GetOfflineCourseSelectedItems(null))
                            .ToDictionary(c => c.Id.ToString(), c => c.CourseName);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "./Pages/Admin/Registers/Index.razor.js");
                await jsr.InvokeVoidAsync("registerIndexPageJs.init");
            }
        }

        private async Task GetAllOfflineCourseRegister()
        {
            dataGrid = await _courseService.GetAllOfflineCourseRegisterPaging(searchModel);
        }

        private async Task PageChanged(PagerEventArgs args)
        {
            searchModel.Skip = args.Skip;
            searchModel.Top = args.Top;
            dataGrid = await _courseService.GetAllOfflineCourseRegisterPaging(searchModel);
        }

        private async Task SearchRegister()
        {
            searchModel.Skip = 0;
            searchModel.Top = 10;
            dataGrid = await _courseService.GetAllOfflineCourseRegisterPaging(searchModel);
        }

        private void DeleteCourse(int id)
        {
            var result = _courseService.DeleteById(id);
            if (result.Status == ResponseStatus.SUCCESS)
            {
                jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
                GetAllOfflineCourseRegister();
            }
            else
            {
                jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
            }
        }

        private async Task OnCourseSelected(int courseId)
        {

        }

        private async void ExportXLS()
        {
            var xls = new OfflineCourseRegisterXLS();
            var byteArray = xls.Edition(dataGrid.Items);
            var fileName = "OfflineCourseRegister_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
            await jsr.InvokeVoidAsync("downloadExcelFile", fileName, byteArray);
        }
    }
}