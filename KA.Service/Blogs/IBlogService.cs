using KA.ViewModels.Blogs;
using KA.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Blogs
{
    public interface IBlogService : IService<Blog>
    {
        Task CreateBlog(CreateBlogVm input);
        ResponseDto DeleteById(object id);
        Task<DataGridResponse<BlogItem>> GetAllBlogPaging(int skip, int top);
        Task<BlogSitePageVm> GetAllBlogPagingForSite(int skip, int top);
        Task<BlogSitePageVm> GetAllNewsPagingForSite(int skip, int top);
        EditBlogVm GetBlogForEdit(int blogId);
        Task<DetailBlogVm> GetDetailBlog(int blogId);
        Task<List<BlogViewModel>> GetTopFourBlogForHomePage();
    }
}