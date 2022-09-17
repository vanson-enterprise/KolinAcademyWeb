
namespace KA.ViewModels.Authen
{
    public class UserLoginOutputDto
    {
        public bool Success { get; set; } = false;
        public bool NotFound { get; set; } = false;
        public bool EmailConfirm { get; set; } = false;
        public bool LockOut { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}
