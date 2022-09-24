using Blazored.TextEditor;
using KA.Infrastructure.Enums;
using KA.Infrastructure.Enums.Extension;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
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
        
       
        BlazoredTextEditor QuillHtml;
        string QuillHTMLContent;

        


        //      public string Code { get; set; }             3
        //      public CourseType Type { get; set; }        3
        //      public decimal Price { get; set; }            3
        //      public decimal DiscountPrice { get; set; }    3


        //      public string PreviewImageFileName { get; set; }
        //      public string PreviewImageTitle { get; set; }
        //      public string IntroduceVideoLink { get; set; }

        //      public string MetaKeyWord { get; set; }
        //      public string MetaTitle { get; set; }
        //      public string MetaDescription { get; set; }

        //      public string ShortDescription { get; set; }
        //      public string Description { get; set; }



        //      public string Tag { get; set; }
        //      public int Sort { get; set; }
        //      public bool IsActive { get; set; }
        // Consts

        // Lifecircle methods

        protected override async Task OnInitializedAsync()
        {
            createCourseModel = new CreateCourseModel();
        }


        // Methods
        public async void GetHTML()
        {
            QuillHTMLContent = await this.QuillHtml.GetHTML();
            StateHasChanged();
        }

        public async void SetHTML()
        {
            string QuillContent =
                @"<a href='http://BlazorHelpWebsite.com/'>" +
                "<img src='images/BlazorHelpWebsite.gif' /></a>";

            await this.QuillHtml.LoadHTMLContent(QuillContent);
            StateHasChanged();
        }
    }
}