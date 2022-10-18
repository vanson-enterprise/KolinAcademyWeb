using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}