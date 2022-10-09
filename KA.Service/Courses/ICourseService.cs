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
        Lesson AddLessonToCourse(Lesson input);
        Task CreateOfflineCourse(CreateCourseModel input);
        Task CreateOnlineCourse(CreateCourseModel input, List<CreateLessonModel> lessons);
        Task<ResponseDto> Delete(int id);
        void DeleteLesson(int id);
        Task Edit(Course input);
        ResponseDto EditLesson(Lesson input);
        Task<DataGridResponse<CourseItem>> GetAllCoursePaging(int skip, int top);
        List<Lesson> GetAllLessonInCourse(int courseId);
        Course GetCourseById(int id);
        bool IsDuplicateCourseCode(string code);
    }
}