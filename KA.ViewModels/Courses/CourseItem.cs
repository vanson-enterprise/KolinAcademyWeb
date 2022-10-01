using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class CourseItem
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Price { get; set; }
        public string DiscountPrice { get; set; }
        public CourseType Type { get; set; }
        public bool IsActive { get; set; }
        public string? ThumbNailImageLink { get; set; }
        public string CreatedDate { get; set; }

    }
}