
using KA.Infrastructure.Attributes.Validate;


namespace KA.ViewModels.Authen
{
    public class RegisterInputModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên tài khoản (Viết liền không dấu)")]
        public string UserName { get; set; }

        [VietnamesePhoneAttribute(ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]
        [Required(ErrorMessage ="Vui lòng nhập số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Email sai định dạng.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(20, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }
        
    }
}
