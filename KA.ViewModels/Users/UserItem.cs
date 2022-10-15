using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Users
{
    public class UserItem
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; }
        public string? CreatedDate { get; set; }
    }
}