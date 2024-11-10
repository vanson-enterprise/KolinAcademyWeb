using AutoMapper;
using KA.Infrastructure.Util;
using KA.ViewModels.Blogs;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using Microsoft.EntityFrameworkCore;

namespace KA.Service.Blogs
{
    public class BlogService : BaseService<Blog>, IBlogService
    {
        public IRepository<Blog> _blogRepo { get; set; }
        public IRepository<AppUser> _userRepo { get; set; }
        public IMapper _mapper { get; set; }
        public BlogService(IRepository<Blog> baseReponsitory, IMapper mapper, IRepository<AppUser> userRepo) : base(baseReponsitory)
        {
            _blogRepo = baseReponsitory;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public async Task CreateBlog(CreateBlogVm input)
        {
            var blog = _mapper.Map<Blog>(input);

            await _blogRepo.AddAsync(blog);
        }

        public async Task<DataGridResponse<BlogItem>> GetAllBlogPaging(int skip, int top)
        {
            var result = new DataGridResponse<BlogItem>();

            var blogs = (from b in _blogRepo.GetAll()
                         join u in _userRepo.GetAll() on b.CreateUserId equals u.Id
                         select new
                         {
                             b.Id,
                             b.Title,
                             b.CreatedDate,
                             u.FullName,
                             b.Published,
                             b.BlogType
                         });

            result.TotalItem = blogs.Count();
            result.Items = blogs.Skip(skip).Take(top).ToList().Select((c, i) =>
            {
                var bi = new BlogItem();
                bi.Id = c.Id;
                bi.Index = (i + 1) + skip;
                bi.CreatedDate = c.CreatedDate.Value.ToString("dd/MM/yyyy");
                bi.Published = c.Published;
                bi.Title = c.Title;
                bi.CreateUser = c.FullName;
                bi.BlogType = c.BlogType.ToString();
                return bi;
            }).ToList();
            return result;
        }

        public new ResponseDto DeleteById(object id)
        {
            var result = _blogRepo.DeleteById(id);
            if (result > 0)
            {
                return new ResponseDto()
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Xóa bài viết thành công"
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Xóa bài viết thất bại"
                };
            }
        }

        public EditBlogVm GetBlogForEdit(int blogId)
        {
            return _blogRepo.GetAll().Where(b => b.Id == blogId).Select(b => _mapper.Map<EditBlogVm>(b)).First();
        }

        #region Site
        public async Task<BlogSitePageVm> GetAllBlogPagingForSite(int skip, int top)
        {
            BlogSitePageVm result = new();
            var blogs = _blogRepo.GetAll().Where(b => b.Published && b.BlogType == BlogType.KNOWLEDGE).OrderByDescending(b => b.CreatedDate);
            result.TotalPage = (int)Math.Ceiling((decimal)blogs.Count() / top);
            result.Blogs = blogs.Skip(skip).Take(top).Select(b => new BlogViewModel()
            {
                Title = b.Title,
                DetailBlogLink = "/bai-viet/" + b.Title.GetSeoName() + "-" + b.Id,
                ShortDescription = b.ShortDescription,
                ThumbNailImageLink = b.ThumbNailImageLink
            }).ToList();
            return result;

        }

        public async Task<BlogSitePageVm> GetAllNewsPagingForSite(int skip, int top)
        {
            BlogSitePageVm result = new();
            var blogs = _blogRepo.GetAll().Where(b => b.Published && b.BlogType == BlogType.NEWS).OrderByDescending(b => b.CreatedDate);
            result.TotalPage = (int)Math.Ceiling((decimal)blogs.Count() / top);
            result.Blogs = blogs.Skip(skip).Take(top).Select(b => new BlogViewModel()
            {
                Title = b.Title,
                DetailBlogLink = "/bai-viet/" + b.Title.GetSeoName() + "-" + b.Id,
                ShortDescription = b.ShortDescription,
                ThumbNailImageLink = b.ThumbNailImageLink
            }).ToList();
            return result;

        }

        public async Task<DetailBlogVm> GetDetailBlog(int blogId)
        {
            return await _blogRepo.GetAll().Where(b => b.Id == blogId).Select(b => new DetailBlogVm()
            {
                Content = b.Content,
                CreatedDate = b.CreatedDate.Value,
                ShortDescription = b.ShortDescription,
                Title = b.Title,
            }).FirstOrDefaultAsync();
        }

        public async Task<List<BlogViewModel>> GetTopFourBlogForHomePage()
        {
            return await _blogRepo.GetAll().Where(b => b.Published && b.BlogType == BlogType.KNOWLEDGE).OrderByDescending(b => b.CreatedDate).Take(4).Select(b => new BlogViewModel()
            {
                Title = b.Title,
                DetailBlogLink = "/bai-viet/" + b.Title.GetSeoFriendlyString() + "-" + b.Id,
                ShortDescription = b.ShortDescription,
                ThumbNailImageLink = b.ThumbNailImageLink
            }).ToListAsync();
        }

        public async Task<List<BlogViewModel>> GetRelatedNews()
        {
            return await _blogRepo.GetAll().Where(b => b.Published && b.BlogType == BlogType.NEWS).OrderByDescending(b => b.CreatedDate).Take(2).Select(b => new BlogViewModel()
            {
                Title = b.Title,
                DetailBlogLink = "/bai-viet/" + b.Title.GetSeoFriendlyString() + "-" + b.Id,
                ShortDescription = b.ShortDescription,
                ThumbNailImageLink = b.ThumbNailImageLink
            }).ToListAsync();
        }
        #endregion
    }
}