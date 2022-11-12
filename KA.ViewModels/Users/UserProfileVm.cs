using KA.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Users
{
    public class UserProfileVm
    {
        public UserProfileInfo UserProfileInfo { get; set; }
        public List<OwningCourseVm> OwningOfflineCourseVm { get; set; }
        public List<OwningCourseVm> OwningOnlineCourseVm { get; set; }
        public List<CourseTransactionVm> CourseTransactionVms { get; set; }
    }
}