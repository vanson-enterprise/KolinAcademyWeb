using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class OfflineCourseRegisterVm
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? MemberAmount { get; set; }
        public string? CourseName { get; set; }
        public string CreatedDate { get; set; }
    }
}