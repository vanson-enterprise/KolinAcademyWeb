using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class OfflineCourseStartDateVm
    {
        public int? Id { get; set; }
        public int? OfflineCourseId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa điểm")]
        public string Place { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày khai giảng")]
        public DateTime? StartTime { get; set; }
    }
}