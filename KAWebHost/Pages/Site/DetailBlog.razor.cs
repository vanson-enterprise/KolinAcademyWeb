using KA.Service.Blogs;
using KA.Service.Courses;
using KA.ViewModels.Blogs;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Site
{
    public partial class DetailBlog : OwningComponentBase
    {
        // parameters
        [Parameter]
        public string BlogSeoName { get; set; }
        // props
        private DetailBlogVm model { get; set; }
        private List<BlogViewModel> relatedNewsModel {get; set;}
        private List<OfflineCourseViewModel> offlineCourseViewModels { get; set; }
        // services
        public IBlogService _blogService { get; set; }
        public ICourseService _courseService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            _courseService = ScopedServices.GetRequiredService<ICourseService>();

            var blogId = Convert.ToInt32(BlogSeoName.Split('-').Last());
            model = await _blogService.GetDetailBlog(blogId);
            offlineCourseViewModels =  _courseService.GetAllOpeningSoonOfflineCourse();
            relatedNewsModel = await _blogService.GetRelatedNews();
        }


    }
}