using KA.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public bool Published { get; set; }
        public string? MetaKeyWord { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }
        public string? ThumbNailImageLink { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? CreateUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdateUserId { get; set; }
        public BlogType BlogType { get; set; } = BlogType.KNOWLEDGE;
    }
}