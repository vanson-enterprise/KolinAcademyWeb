using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Courses
{
    public interface ICourseService : IService<Course>
    {
        Task CreateOnlineCourse(CreateCourseModel input, List<CreateLessonModel> lessons);
        Task<ResponseDto> Delete(int id);
        Task<ResponseDto> Edit(EditCourseModel input);
    }
}