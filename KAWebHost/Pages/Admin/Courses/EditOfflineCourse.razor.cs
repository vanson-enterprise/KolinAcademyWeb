using AutoMapper;
using Blazored.TextEditor;
using KA.DataProvider.Entities;
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
        private EditCourseModel courseModel;
        private FileSelector fileSelectorControl;
        private BlazoredTextEditor quillHtml;

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
                await jsr.InvokeVoidAsync("import", "/scripts/courses/edit-off-course.js");
                await jsr.InvokeVoidAsync("editOffCoursePageJs.init");
            }
        }

        private void InitDataModel()
        {
            course = _courseService.GetCourseById(Id);
            courseModel = _mapper.Map<EditCourseModel>(course);
        }

        private async Task SubmitForm()
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

            await jsr.InvokeVoidAsync("ShowAppAlert", "Đã cập nhật thành công", "success");
            GoToCourseListPage();

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
    }

}
