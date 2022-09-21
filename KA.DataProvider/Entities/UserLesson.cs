using KA.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class UserLesson
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int LessonId { get; set; }
        public UserLessonStatus Status  { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }
    }
}
