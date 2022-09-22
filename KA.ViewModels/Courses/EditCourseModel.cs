using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class EditCourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public int Sort { get; set; }
        public string PreviewImageFileName { get; set; }
        public string PreviewImageTitle { get; set; }
        public string IntroduceVideoLink { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}