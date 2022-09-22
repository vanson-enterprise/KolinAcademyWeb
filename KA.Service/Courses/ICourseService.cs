using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Courses
{
    public interface ICourseService : IService<Course>
    {
        Task Create(CreateCourseModel input);
        Task<ResponseDto> Delete(int id);
        Task<ResponseDto> Edit(EditCourseModel input);
    }
}