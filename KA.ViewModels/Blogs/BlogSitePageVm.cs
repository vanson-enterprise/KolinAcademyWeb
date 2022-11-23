using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Blogs
{
    public class BlogSitePageVm
    {
        public List<BlogViewModel> Blogs { get; set; }
        public int TotalPage { get; set; }
    }
}
