using Blazored.TextEditor;
using KA.Infrastructure.Enums;
using KA.Infrastructure.Enums.Extension;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using KAWebHost.Pages.Admin.Components;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Admin.Courses
{
    public partial class CreateOnlineCourse : OwningComponentBase
    {
        // Models
        private CreateLessonModel createLessonModel { get; set; }
        private CreateCourseModel createCourseModel { get; set; }

        // Properties
        private Dictionary<string, string> courseTypes = CourseType.ONLINE.ToDictionary();

        private FileSelector FileSelectorControl;
        BlazoredTextEditor QuillHtml;

        protected override async Task OnInitializedAsync()
        {
            createCourseModel = new CreateCourseModel()
            {
                IsActive = true,
                Type = CourseType.ONLINE
            };
        }


        // Methods
        public async void GetHTML()
        {
            var QuillHTMLContent = await this.QuillHtml.GetHTML();
            StateHasChanged();
        }

        public async void SetHTML()
        {
            string QuillContent =
                @"<a href='http://BlazorHelpWebsite.com/'>" +
                "<img src='images/BlazorHelpWebsite.gif' /></a>";

            await QuillHtml.LoadHTMLContent(QuillContent);
            StateHasChanged();
        }

        private void InsertImageClick()
        {
            FileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            await QuillHtml.InsertImage(paramImageURL);

            FileSelectorControl.SetShowFileManager(false);
        }
    }
}