using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class GetAllOfflineCourseRegisterPagingInput
    {
        public int Skip { get; set; }
        public int Top { get; set; }
        public int CourseId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}