using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class OnlineCourseViewModel
    {
        public int Id { get; set; }
        public string DetailLink { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string DiscountPrice { get; set; }
        public string ThumbNailImageLink { get; set; }
        public string IntroVideoLink { get; set; }
        public string? ShortDescription { get; set; }
    }
}
