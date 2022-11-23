using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Blogs
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ThumbNailImageLink { get; set; }
        public string? ShortDescription { get; set; }
        public string DetailBlogLink { get; set; }
    }
}
