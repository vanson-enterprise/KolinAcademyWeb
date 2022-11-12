using KA.Infrastructure.Attributes.Validate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Users
{
    public class UserProfileInfo
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        [VietnamesePhoneAttribute(ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}