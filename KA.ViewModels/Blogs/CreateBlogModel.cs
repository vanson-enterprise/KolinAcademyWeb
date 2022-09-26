using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Blogs
{
    public class CreateBlogModel
    {
        [Required(ErrorMessage ="Vui lòng nhập tiêu đề")]
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public bool Published { get; set; }
        public string MetaKeyWord { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string MainImageFileName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? CreateUserId { get; set; }
    }
}
