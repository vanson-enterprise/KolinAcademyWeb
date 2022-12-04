using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Blogs
{
    public class DetailBlogVm
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}