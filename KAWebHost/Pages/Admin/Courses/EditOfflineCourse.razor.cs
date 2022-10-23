using AutoMapper;
using Blazored.TextEditor;
using KA.DataProvider.Entities;
using KA.Infrastructure.Enums;
using KA.Service.Courses;
using KA.ViewModels.Courses;
using KAWebHost.Pages.Admin.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Admin.Courses
{
    public partial class EditOfflineCourse : OwningComponentBase
    {
        private Course course;
        private EditOfflineCourseModel courseModel;
        private static List<OfflineCourseStartDate> s_startDates;
        private OfflineCourseStartDateVm startDateModel;
        private FileSelector fileSelectorControl;
        private BlazoredTextEditor quillHtml;
        private bool isEditingStartDate;
        private int editStartDateIndex;

        private bool showConfirmDeleteStartDate;
        private int deleteStartDateId;

        // Parameter
        [Parameter]
        public int Id { get; set; }

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
                await jsr.InvokeVoidAsync("import", "./Pages/Admin/Courses/EditOfflineCourse.razor.js");
                await jsr.InvokeVoidAsync("editOffCoursePageJs.init");
            }
        }

        private void InitDataModel()
        {
            course = _courseService.GetCourseById(Id);
            startDateModel = new OfflineCourseStartDateVm();
            courseModel = _mapper.Map<EditOfflineCourseModel>(course);
            GetAllStartDate();
        }

        private void GetAllStartDate()
        {
            s_startDates = _courseService.GetAllStartDatesOfCourse(course.Id);
        }

        private async Task UpdateCourse()
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
            await _courseService.Edit(course);

            jsr.InvokeVoidAsync("ShowAppAlert", "Đã cập nhật thông tin chung", "success");
            jsr.InvokeVoidAsync("editOffCoursePageJs.goNextStep");
            //GoToCourseListPage();

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

        private async Task<string> GetHTML()
        {
            return await quillHtml.GetHTML();
        }

        private void GoToCourseListPage()
        {
            NavigationManager.NavigateTo($"/manager/courses");
        }

        private async Task CompleteEdit()
        {
            NavigationManager.NavigateTo($"/manager/courses");
        }

        // ==================================== Start Date ====================================

        private void TurnOnEditStartDateForm(int index)
        {
            isEditingStartDate = true;
            editStartDateIndex = index;
            startDateModel.StartTime = s_startDates[index].StartTime;
            startDateModel.Place = s_startDates[index].Place;
            startDateModel.Id = s_startDates[index].Id;
        }

        private void TurnOffEditStartDateForm()
        {
            isEditingStartDate = false;
            editStartDateIndex = -1;
            startDateModel = new();
        }

        private void SubmitStartDateForm()
        {
            if (isEditingStartDate)
            {
                s_startDates[editStartDateIndex].Place = startDateModel.Place;
                s_startDates[editStartDateIndex].StartTime = startDateModel.StartTime.Value;
                var updateResult = _courseService.UpdateStartDate(s_startDates[editStartDateIndex]);
                if (updateResult.Status == ResponseStatus.SUCCESS)
                {
                    GetAllStartDate();
                    startDateModel = new();
                    isEditingStartDate = false;
                    jsr.InvokeVoidAsync("ShowAppAlert", updateResult.Message, "success");
                }
                else
                {
                    startDateModel = new();
                    isEditingStartDate = false;
                    jsr.InvokeVoidAsync("ShowAppAlert", updateResult.Message, "error");
                }
            }
            else
            {
                _courseService.CreateStartDate(new OfflineCourseStartDate()
                {
                    OfflineCourseId = course.Id,
                    Place = startDateModel.Place,
                    StartTime = startDateModel.StartTime.Value
                });
                GetAllStartDate();
                startDateModel = new();
                jsr.InvokeVoidAsync("ShowAppAlert", "Thêm ngày khai giảng thành công", "success");
            }
        }
        private void ShowConfirmDeleteStartDateModal(int id)
        {
            showConfirmDeleteStartDate = true;
            deleteStartDateId = id;
        }
        private void DeleteStartDate(bool accept)
        {
            if (accept && deleteStartDateId >= 0)
            {
                _courseService.DeleteStartDate(deleteStartDateId);
                GetAllStartDate();
                if (startDateModel.StartTime != null)
                {
                    startDateModel = new();
                    isEditingStartDate = false;
                }
            }
            showConfirmDeleteStartDate = false;
        }

        [JSInvokable]
        public static async Task<string> CheckCourseStartDateValid()
        {
            if (s_startDates.Count > 0)
            {
                return "allows";
            }
            else
            {
                return "prevent";
            }
        }
    }

}