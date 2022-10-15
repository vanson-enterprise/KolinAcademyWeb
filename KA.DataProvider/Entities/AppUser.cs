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
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreateUserId { get; set; }
        public string? UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<UserCourse> UserCourses { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}