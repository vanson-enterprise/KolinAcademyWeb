using KA.ViewModels.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class DetailOnlineCourseModel
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string? MetaKeyWord { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? IntroduceVideoLink { get; set; }

        public List<LessonViewModel> Lessons { get; set; }
    }
}