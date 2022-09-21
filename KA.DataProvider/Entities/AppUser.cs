using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public List<UserCourse> UserCourses { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
