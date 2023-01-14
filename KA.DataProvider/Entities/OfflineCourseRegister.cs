using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class OfflineCourseRegister
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? MemberAmount { get; set; }
        public int? CourseId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}