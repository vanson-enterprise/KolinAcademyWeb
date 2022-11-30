using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Carts
{
    public class AddCourseToCartDto
    {
        public string? UserId { get; set; }
        public int CourseId { get; set; }
    }
}