using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class OfflineCourseRegisterInputDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng để lại số điện thoại")]
        public string PhoneNumber { get; set; }
        public int? MemberAmount { get; set; }
        public int? CourseId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}