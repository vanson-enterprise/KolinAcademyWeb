using KA.Service.Blogs;
using KA.Service.Courses;
using KA.ViewModels.Blogs;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Site
{
    public partial class Knowledge : OwningComponentBase
    {
        private IBlogService _blogService;
        private ICourseService _courseService;
        private const int BLOG_AMOUNT_PER_PAGE = 7;
        private int pageIndex = 1;
        private int blogsNumber = 0;
        private BlogSitePageVm model = new();
        private List<BlogViewModel> relatedNewsModel {get; set;} = new();
        private List<OfflineCourseViewModel> offlineCourseViewModels { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetService<IBlogService>();
            _courseService = ScopedServices.GetService<ICourseService>();
            await GetModelData();
        }

        private async Task OnPageChange(int pageNumber)
        {
            if (pageNumber == -1 && pageIndex > 1)
            {
                pageIndex--;
            }
            else if (pageNumber == 0 && pageIndex < BLOG_AMOUNT_PER_PAGE)
            {
                pageIndex++;
            }
            else
            {
                pageIndex = pageNumber;
            }
            GetModelData();
        }

        private async Task GetModelData()
        {
            model = await _blogService.GetAllBlogPagingForSite((pageIndex - 1) * BLOG_AMOUNT_PER_PAGE, BLOG_AMOUNT_PER_PAGE);
            blogsNumber = model.Blogs.Count();
            offlineCourseViewModels =  _courseService.GetAllOpeningSoonOfflineCourse();
            relatedNewsModel = await _blogService.GetRelatedNews();
        }
    }
}
