

namespace KA.ViewModels.Lessons
{
    public class CreateLessonModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tên bài giảng")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập video link")]
        public string VideoLink { get; set; }
    }
}