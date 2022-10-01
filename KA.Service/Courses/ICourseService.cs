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
        Task CreateOfflineCourse(CreateCourseModel input);
        Task CreateOnlineCourse(CreateCourseModel input, List<CreateLessonModel> lessons);
        Task<ResponseDto> Delete(int id);
        Task<ResponseDto> Edit(EditCourseModel input);
        Task<DataGridResponse<CourseItem>> GetAllCoursePaging(int skip, int top);
        EditCourseModel GetCourseById(int id);
        bool IsDuplicateCourseCode(string code);
    }
}