using KA.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CourseType Type { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string? Tag { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? MetaKeyWord { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int Sort { get; set; }
        public string? ThumbNailImageLink { get; set; }
        public string? IntroduceVideoLink { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? CreateUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ExternalCssLink { get; set; }
        public string? ExternalScriptLink { get; set; }
        public bool IsUseExternalHtml { get; set; }

        /// <summary>
        /// For online course - thời gian giới hạn
        /// </summary>
        public int? DurationTime { get; set; }

        public List<UserCourse>? UserCourses { get; set; }
    }
}