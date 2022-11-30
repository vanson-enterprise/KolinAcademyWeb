using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Lessons
{
    public class UserLessonViewModel
    {
        public int UserLessonId { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string VideoLink { get; set; }
        public UserLessonStatus UserLessonStatus { get; set; }
    }
}