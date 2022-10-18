using Blazored.TextEditor;
using KA.Infrastructure.Enums;
using KA.Infrastructure.Enums.Extension;
using KA.Service.Courses;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using KAWebHost.Pages.Admin.Components;
using KAWebHost.Shared.Validator;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Admin.Courses
{
    public partial class CreateOnlineCourse : OwningComponentBase
    {
        // Models
        private CreateLessonModel lessonModel { get; set; }
        private CreateOnlineCourseModel createCourseModel { get; set; }
        private static List<CreateLessonModel> s_lessons { get; set; }

        // Properties
        private Dictionary<string, string> courseTypes = CourseType.ONLINE.ToDictionary();
        private FileSelector fileSelectorControl;
        private bool isEditingLesson;
        private int editLessonIndex;
        private int deleteLessonIndex;
        private bool showConfirmDeleteLessonModal = false;
        private bool isSelectThumbNailImage = false;
        private CustomFormValidator customFormValidator;

        // Service
        BlazoredTextEditor quillHtml;
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "/scripts/courses/create-on-course.js");
                await jsr.InvokeVoidAsync("createOnCoursePageJs.init");
            }
        }


        // ============================  Function Methods =============================

        private void InitDataModel()
        {

            createCourseModel = new CreateOnlineCourseModel()
            {
                IsActive = true,
                Type = CourseType.ONLINE
            };
            lessonModel = new();
            s_lessons = new List<CreateLessonModel>();
        }
        private void GoToAddLessonStep()
        {
            customFormValidator.ClearFormErrors();
            // check duplicate course code
            if (_courseService.IsDuplicateCourseCode(createCourseModel.Code))
            {
                customFormValidator.DisplayFormErrors(new Dictionary<string, List<string>>()
                {
                    { nameof(createCourseModel.Code), new List<string>{ "Mã khóa học đã được sử dụng" } }
                });
            }
            else
            {
                jsr.InvokeVoidAsync("createOnCoursePageJs.goNextStep");
            }
        }


        private async Task SubmitLessonForm()
        {
            if (!isEditingLesson)
            {
                s_lessons.Add(lessonModel);
                lessonModel = new();
                jsr.InvokeVoidAsync("ShowAppAlert", "Thêm bài giảng thành công", "success");
            }
            else
            {
                s_lessons[editLessonIndex] = lessonModel;
                lessonModel = new();
                isEditingLesson = false;
                jsr.InvokeVoidAsync("ShowAppAlert", "Cập nhật bài giảng thành công", "success");
            }
        }

        private void TurnOnEditLessonForm(int index)
        {
            isEditingLesson = true;
            editLessonIndex = index;
            lessonModel.Name = s_lessons[index].Name;
            lessonModel.VideoLink = s_lessons[index].VideoLink;
        }

        private void ShowConfirmDeleteModal(int spOrderIndex)
        {
            showConfirmDeleteLessonModal = true;
            deleteLessonIndex = spOrderIndex;
        }


        private void DeleteLesson(bool accept)
        {
            if (accept && deleteLessonIndex >= 0)
            {
                s_lessons.RemoveAt(deleteLessonIndex);
                if (lessonModel.Name != null)
                {
                    lessonModel = new();
                    isEditingLesson = false;
                }
            }
            showConfirmDeleteLessonModal = false;
        }

        private void TurnOffEditLessonForm()
        {
            isEditingLesson = false;
            editLessonIndex = -1;
            lessonModel = new();
        }

        [JSInvokable]
        public static string CheckCourseHaveAnyLesson()
        {
            if (s_lessons.Count > 0)
            {
                return "allows";
            }
            return "prevent";
        }

        private async Task CreateNewCourse()
        {
            createCourseModel.Description = await GetHTML();
            await _courseService.CreateOnlineCourse(createCourseModel, s_lessons);
            jsr.InvokeVoidAsync("ShowAppAlert", "Tạo khóa học thành công", "success");
            // reset data state
            InitDataModel();
            await jsr.InvokeVoidAsync("createOnCoursePageJs.goFirstStep");
        }

        public async Task<string> GetHTML()
        {
            return await quillHtml.GetHTML();
        }

        //public async void SetHTML()
        //{
        //    string QuillContent =
        //        @"<a href='http://BlazorHelpWebsite.com/'>" +
        //        "<img src='images/BlazorHelpWebsite.gif' /></a>";

        //    await quillHtml.LoadHTMLContent(QuillContent);
        //    StateHasChanged();
        //}

        private void OpenSelectImageModal(bool isFromTextEditor)
        {

            isSelectThumbNailImage = !isFromTextEditor;
            fileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            if (isSelectThumbNailImage)
            {
                createCourseModel.ThumbNailImageLink = paramImageURL;
            }
            else
            {
                await quillHtml.InsertImage(paramImageURL);

            }
            fileSelectorControl.SetShowFileManager(false);
        }

    }
}