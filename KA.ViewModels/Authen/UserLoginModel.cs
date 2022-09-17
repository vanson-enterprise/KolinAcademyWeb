

namespace KA.ViewModels.Authen
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập hoặc email không được để trống.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
