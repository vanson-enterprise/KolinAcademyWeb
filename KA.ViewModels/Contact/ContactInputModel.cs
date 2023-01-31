using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Contact
{
    public class ContactInputModel
    {
        [Required(ErrorMessage ="Vui lòng để lại họ và tên")]
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Note { get; set; }
    }
}
