using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class UserCourse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public float StudyProgress { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? CreateUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}
