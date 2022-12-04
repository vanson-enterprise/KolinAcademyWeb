using KA.Service.Blogs;
using KA.ViewModels.Blogs;
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
        // services
        public IBlogService _blogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            var blogId = Convert.ToInt32(BlogSeoName.Split('-').Last());
            model = await _blogService.GetDetailBlog(blogId);
        }
    }
}