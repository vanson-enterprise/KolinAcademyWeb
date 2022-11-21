using Blazored.TextEditor;
using KA.Infrastructure.Enums;
using KA.Service.Courses;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using KAWebHost.Pages.Admin.Components;
using KAWebHost.Shared.Validator;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace KAWebHost.Pages.Admin.Courses
{
    public partial class CreateOfflineCourse : OwningComponentBase
    {
        private CreateOfflineCourseModel model;
        private static List<OfflineCourseStartDateVm> s_startDates { get; set; }
        private OfflineCourseStartDateVm startDateModel { get; set; }
        private FileSelector fileSelectorControl;
        private BlazoredTextEditor quillHtml;
        private CustomFormValidator customFormValidator;
        private bool isEditingStartDate = false;
        private int editStartDateIndex;
        private bool showConfirmDeleteStartDateModal;
        private int deleteStartDateIndex;
        private IJSObjectReference jsModule;

        [Inject]
        IJSRuntime jsr { get; set; }
        ICourseService _courseService { get; set; }

        // ========================== Life circle methods =================================
        protected override async Task OnInitializedAsync()
        {
            // Inject Service
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            // Init Model State
            InitDataModel();
        }

        private void InitDataModel()
        {

            model = new CreateOfflineCourseModel()
            {
                IsActive = true,
                Type = CourseType.OFFLINE
            };

            startDateModel = new();
            s_startDates = new List<OfflineCourseStartDateVm>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                jsModule = await jsr.InvokeAsync<IJSObjectReference>("import", "./Pages/Admin/Courses/CreateOfflineCourse.razor.js");
                await jsModule.InvokeVoidAsync("createOffCoursePageJsModule.init");
            }
        }

        private async Task GoToAddStartDateStep()
        {
            customFormValidator.ClearFormErrors();
            // check duplicate course code
            if (_courseService.IsDuplicateCourseCode(model.Code))
            {
                customFormValidator.DisplayFormErrors(new Dictionary<string, List<string>>()
                {
                    { nameof(model.Code), new List<string>{ "Mã khóa học đã được sử dụng" } }
                });
            }
            else
            {
                await jsModule.InvokeVoidAsync("createOffCoursePageJsModule.goNextStep");
            }
        }

        private void OpenSelectImageModal(bool isFromTextEditor)
        {
            fileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            await quillHtml.InsertImage(paramImageURL);
            fileSelectorControl.SetShowFileManager(false);
        }

        private async Task SubmitStartDateForm()
        {
            if (!isEditingStartDate)
            {
                s_startDates.Add(startDateModel);
                startDateModel = new();
                jsr.InvokeVoidAsync("ShowAppAlert", "Thêm ngày khai giảng thành công", "success");
            }
            else
            {
                s_startDates[editStartDateIndex] = startDateModel;
                startDateModel = new();
                isEditingStartDate = false;
                jsr.InvokeVoidAsync("ShowAppAlert", "Cập nhật ngày khai giảng thành công", "success");
            }
        }

        private void TurnOnEditStartDateForm(int index)
        {
            isEditingStartDate = true;
            editStartDateIndex = index;
            startDateModel.StartTime = s_startDates[index].StartTime;
            startDateModel.Place = s_startDates[index].Place;
        }

        private void TurnOffEditStartDateForm()
        {
            isEditingStartDate = false;
            editStartDateIndex = -1;
            startDateModel = new();
        }

        private void ShowConfirmDeleteModal(int index)
        {
            showConfirmDeleteStartDateModal = true;
            deleteStartDateIndex = index;
        }

        private async Task CreateNewCourse()
        {
            if (!model.IsUseExternalHtml)
            {
                model.Description = await quillHtml.GetHTML();
            }
            await _courseService.CreateOfflineCourse(model, s_startDates);
            jsr.InvokeVoidAsync("ShowAppAlert", "Thêm khóa học thành công", "success");

            // reset data state
            InitDataModel();
            await jsModule.InvokeVoidAsync("createOffCoursePageJsModule.goFirstStep");

        }

        [JSInvokable]
        public static string CheckCourseHaveAnyStartDate()
        {
            if (s_startDates.Count > 0)
            {
                return "allows";
            }
            return "prevent";
        }
    }
}