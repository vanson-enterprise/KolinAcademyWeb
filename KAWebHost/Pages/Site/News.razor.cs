using KA.Service.Blogs;
using KA.ViewModels.Blogs;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Site
{
    public partial class News : OwningComponentBase
    {
        private IBlogService _blogService;
        private const int BLOG_AMOUNT_PER_PAGE = 12;
        private int pageIndex = 1;
        private int blogsNumber = 0;
        private BlogSitePageVm model = new();
        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetService<IBlogService>();
            GetModelData();
        }

        private async Task OnPageChange(int pageNumber)
        {
            if (pageNumber == -1 && pageIndex > 1)
            {
                pageIndex--;
                GetModelData();
            }
            else if (pageNumber == 0 && pageIndex < BLOG_AMOUNT_PER_PAGE)
            {
                pageIndex++;
                GetModelData();
            }
            else if(pageIndex != pageNumber && pageNumber != 0 && pageNumber != -1)
            {
                pageIndex = pageNumber;
                GetModelData();
            }
        }

        private async Task GetModelData()
        {
            model = await _blogService.GetAllNewsPagingForSite((pageIndex - 1) * BLOG_AMOUNT_PER_PAGE, BLOG_AMOUNT_PER_PAGE);
            blogsNumber = model.Blogs.Count();
        }
    }
}