using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class OfflineCourseViewModel
    {
        public string Name { get; set; }
        public string DetailCourseLink { get; set; }
        public List<OfflineCourseStartDateVm> StartDates { get; set; }
    }
}