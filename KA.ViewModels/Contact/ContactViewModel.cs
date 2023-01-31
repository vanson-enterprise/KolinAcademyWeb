using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Contact
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string CreatedDate { get; set; }
        public string? Note { get; set; }
    }
}
