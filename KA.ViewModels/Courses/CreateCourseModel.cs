using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Courses
{
    public class CreateCourseModel
    {
        [Required(ErrorMessage ="Vui lòng nhập tên khóa học")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã khóa học")]
        public string Code { get; set; }
        public CourseType Type { get; set; }
        public bool IsActive { get; set; }

        [Range(0.0,double.MaxValue,ErrorMessage ="Giá không được nhỏ hơn 0")]
        public decimal Price { get; set; }

        [Range(0,double.MaxValue,ErrorMessage ="Giá khuyến mãi không được nhỏ hơn 0")]
        public decimal DiscountPrice { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public int Sort { get; set; }
        public string PreviewImageFileName { get; set; }
        public string PreviewImageTitle { get; set; }
        public string IntroduceVideoLink { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? CreateUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdateUserId { get; set; }
    }
}