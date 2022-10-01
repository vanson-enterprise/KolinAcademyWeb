using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Lessons
{
    public class EditLessonModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên bài giảng")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập video link")]
        public string VideoLink { get; set; }
    }
}