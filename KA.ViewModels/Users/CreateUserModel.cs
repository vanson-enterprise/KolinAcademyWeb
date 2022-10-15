using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Users
{
    public class CreateUserModel
    {
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập username")]
        public string UserName { get; set; }

        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(20, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreateUserId { get; set; }
        public List<RoleModel> Roles { get; set; }
    }
}