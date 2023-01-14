using KA.DataProvider.Entities;
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
        Task CreateOfflineCourse(CreateOfflineCourseModel input, List<OfflineCourseStartDateVm> startDates);
        Task CreateOnlineCourse(CreateOnlineCourseModel input, List<CreateLessonModel> lessons);
        Task CreateStartDate(OfflineCourseStartDate input);
        ResponseDto DeleteById(object id);
        void DeleteLesson(int id);
        void DeleteStartDate(int id);
        Task Edit(Course input);
        ResponseDto EditLesson(Lesson input);
        Task<DataGridResponse<CourseItem>> GetAllCoursePaging(int skip, int top);
        List<Lesson> GetAllLessonInCourse(int courseId);
        Task<DataGridResponse<OfflineCourseRegisterVm>> GetAllOfflineCourseRegisterPaging(int skip, int top);
        List<OnlineCourseViewModel> GetAllOnlineCourse();
        List<OfflineCourseViewModel> GetAllOpeningSoonOfflineCourse();
        List<OfflineCourseStartDate> GetAllStartDatesOfCourse(int courseId);
        Course GetCourseById(int id);
        Task<DetailOnlineCourseModel> GetDetailOnlineCourse(int courseId);
        Task<List<OfflineCourseSelectedItem>> GetOfflineCourseSelectedItems();
        Task<List<OfflineCourseViewModel>> GetTopOffCourseForIndexPage(int offCourseNumber);
        Task<OnlineCourseViewModel?> GetTopOneCourseForIndexPage();
        Task<List<UserLessonViewModel>> GetUserLessons(string userId, int courseId);
        bool IsDuplicateCourseCode(string code);
        Task RegisterOfflineCourse(OfflineCourseRegisterInputDto input);
        ResponseDto UpdateStartDate(OfflineCourseStartDate input);
        Task<float> UpdateUserCourseProgress(string userId, int courseId);
        Task UpdateUserLessonStatus(int userLessonId, UserLessonStatus status);
    }
}