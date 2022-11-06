using AutoMapper;
using Blazored.TextEditor;
using KA.DataProvider.Entities;
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
    public partial class EditOnlineCourse : OwningComponentBase
    {
        // Models
        private EditLessonModel lessonModel { get; set; }
        private EditOnlineCourseModel courseModel { get; set; }
        private Course course { get; set; }
        private static List<Lesson> s_lessons { get; set; }

        // Parameter
        [Parameter]
        public int Id { get; set; }

        // Properties
        private Dictionary<string, string> courseTypes = CourseType.ONLINE.ToDictionary();
        private FileSelector fileSelectorControl;
        private bool isEditingLesson;
        private int editLessonIndex;
        private int deleteLessonId;
        private bool showConfirmDeleteLessonModal = false;
        private bool isSelectThumbNailImage = false;

        // Service
        BlazoredTextEditor quillHtml;
        [Inject]
        IJSRuntime jsr { get; set; }
        ICourseService _courseService { get; set; }
        IMapper _mapper { get; set; }


        // ========================== Life circle methods =================================
        protected override async Task OnInitializedAsync()
        {
            // Inject Service
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            _mapper = ScopedServices.GetRequiredService<IMapper>();
            // Init Model State
            InitDataModel();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "./Pages/Admin/Courses/EditOnlineCourse.razor.js");
                await jsr.InvokeVoidAsync("editOnCourseJsModule.init");
            }
        }

        protected override async void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        // ============================  Function Methods =============================

        private void InitDataModel()
        {

            course = _courseService.GetCourseById(Id);
            courseModel = _mapper.Map<EditOnlineCourseModel>(course);
            lessonModel = new();
            GetAllLesson();
        }

        private void GetAllLesson()
        {
            s_lessons = _courseService.GetAllLessonInCourse(Id);
        }

        private async Task UpdateCourseAndGoToEditLessonStep()
        {
            courseModel.Description = await GetHTML();
            course.Name = courseModel.Name;
            course.IsActive = courseModel.IsActive;
            course.Price = courseModel.Price;
            course.DiscountPrice = courseModel.DiscountPrice;
            course.Tag = courseModel.Tag;
            course.Description = courseModel.Description;
            course.MetaKeyWord = courseModel.MetaKeyWord;
            course.MetaDescription = courseModel.MetaDescription;
            course.MetaTitle = courseModel.MetaTitle;
            course.Sort = courseModel.Sort;
            course.ThumbNailImageLink = courseModel.ThumbNailImageLink;
            course.IntroduceVideoLink = courseModel.IntroduceVideoLink;
            course.UpdatedDate = DateTime.Now;
            course.ShortDescription = courseModel.ShortDescription;
            course.DurationTime = courseModel.DurationTime;
            await _courseService.Edit(course);

            jsr.InvokeVoidAsync("ShowAppAlert", "Đã cập nhật thông tin chung", "success");
            jsr.InvokeVoidAsync("editOnCourseJsModule.goNextStep");
        }


        private async Task SubmitLessonForm()
        {
            if (!isEditingLesson)
            {
                var newLesson = _courseService.AddLessonToCourse(new Lesson()
                {
                    Name = lessonModel.Name,
                    CourseId = courseModel.Id,
                    VideoLink = lessonModel.VideoLink
                });
                GetAllLesson();
                lessonModel = new();
                jsr.InvokeVoidAsync("ShowAppAlert", "Thêm bài giảng thành công", "success");
            }
            else
            {
                var lesson = s_lessons[editLessonIndex];
                lesson.Name = lessonModel.Name;
                lesson.VideoLink = lessonModel.VideoLink;
                var updateResult = _courseService.EditLesson(lesson);
                if (updateResult.Status == ResponseStatus.SUCCESS)
                {
                    GetAllLesson();
                    lessonModel = new();
                    isEditingLesson = false;
                    jsr.InvokeVoidAsync("ShowAppAlert", updateResult.Message, "success");
                }
                else
                {
                    lessonModel = new();
                    isEditingLesson = false;
                    jsr.InvokeVoidAsync("ShowAppAlert", updateResult.Message, "success");
                }
            }
        }

        private void TurnOnEditLessonForm(int index)
        {
            isEditingLesson = true;
            editLessonIndex = index;
            lessonModel.Id = s_lessons[index].Id;
            lessonModel.Name = s_lessons[index].Name;
            lessonModel.VideoLink = s_lessons[index].VideoLink;
        }

        private void ShowConfirmDeleteModal(int spOrderIndex)
        {
            showConfirmDeleteLessonModal = true;
            deleteLessonId = spOrderIndex;
        }


        private void DeleteLesson(bool accept)
        {
            if (accept && deleteLessonId >= 0)
            {
                _courseService.DeleteLesson(deleteLessonId);
                GetAllLesson();
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


        private async Task CompleteEdit()
        {

            NavigationManager.NavigateTo($"/manager/courses");

        }

        public async Task<string> GetHTML()
        {
            return await quillHtml.GetHTML();
        }

        private void OpenSelectImageModal(bool isFromTextEditor)
        {

            isSelectThumbNailImage = !isFromTextEditor;
            fileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            if (isSelectThumbNailImage)
            {
                courseModel.ThumbNailImageLink = paramImageURL;
            }
            else
            {
                await quillHtml.InsertImage(paramImageURL);

            }
            fileSelectorControl.SetShowFileManager(false);
        }

        [JSInvokable]
        public static string CheckCourseValid()
        {
            if (s_lessons.Count > 0)
            {
                return "allows";
            }
            return "prevent";
        }
    }
}