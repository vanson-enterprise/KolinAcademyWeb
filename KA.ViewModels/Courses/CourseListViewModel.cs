using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class CourseListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public CourseType Type { get; set; }
        public bool IsActive { get; set; }
        public string PreviewImageFileName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}