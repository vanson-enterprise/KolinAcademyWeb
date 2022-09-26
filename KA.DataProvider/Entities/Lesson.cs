using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VideoLink { get; set; }
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public List<UserLesson> UserLessons { get; set; }
    }
}