using Blazored.TextEditor;
using KA.Infrastructure.Enums;
using KA.Service.Courses;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using KAWebHost.Pages.Admin.Components;
using KAWebHost.Shared.Validator;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Admin.Courses
{
    public partial class CreateOfflineCourse : OwningComponentBase
    {
        private CreateCourseModel model;
        private FileSelector fileSelectorControl;
        private BlazoredTextEditor quillHtml;
        private CustomFormValidator customFormValidator;

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

            model = new CreateCourseModel()
            {
                IsActive = true,
                Type = CourseType.OFFLINE
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }

        private async Task SubmitForm()
        {
            customFormValidator.ClearFormErrors();
            // check duplicate course code
            if (_courseService.IsDuplicateCourseCode(model.Code))
            {
                customFormValidator.DisplayFormErrors(new Dictionary<string, List<string>>()
                {
                    { nameof(model.Code), new List<string>{ "Mã khóa học đã được sử dụng" } }
                });
                return;
            }
            else
            {
                model.Description = await quillHtml.GetHTML();
                await _courseService.CreateOfflineCourse(model);
                jsr.InvokeVoidAsync("ShowAppAlert", "Thêm khóa học thành công", "success");
                model = new()
                {
                    IsActive = true,
                    Type = CourseType.OFFLINE
                };
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
    }
}