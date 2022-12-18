using KA.DataProvider.Entities;
using KA.Infrastructure.Enums;
using KA.Service.Blogs;
using KA.Service.Courses;
using KA.ViewModels.Blogs;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace KAWebHost.Pages.Admin.Blogs
{
    public partial class Index
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        [Inject]
        private IJSRuntime jsr { get; set; }

        private DataGridResponse<BlogItem> dataGrid;
        private IBlogService _blogService;
        private int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            await GetAllBlog();
        }

        private async Task GetAllBlog()
        {
            dataGrid = await _blogService.GetAllBlogPaging(0, pageSize);
        }
        private async Task PageChanged(PagerEventArgs args)
        {
            dataGrid = await _blogService.GetAllBlogPaging(args.Skip, args.Top);
        }
        private void RedirectToCreatePage()
        {
            NavigationManager.NavigateTo($"/manager/create-blog",true);
        }
        private void EditRow(int blogId)
        {
            NavigationManager.NavigateTo($"/manager/edit-blog/{blogId}",true);
        }

        private void DeleteBlog(int id)
        {
            var result = _blogService.DeleteById(id);
            if (result.Status == ResponseStatus.SUCCESS)
            {
                jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "success");
                GetAllBlog();
            }
            else
            {
                jsr.InvokeVoidAsync("ShowAppAlert", result.Message, "error");
            }
        }

    }
}