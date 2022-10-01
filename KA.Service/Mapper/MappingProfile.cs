using AutoMapper;
using KA.ViewModels.Authen;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Mapper
{
    public class MappingProfile : Profile
    {
        private readonly IConfiguration _configuration;

        public MappingProfile(IConfiguration configuration)
        {
            MappingEntityToViewModel();
            MappingDtoToEntity();

        }

        private void MappingEntityToViewModel()
        {
            CreateMap<Course, CourseItem>();
            CreateMap<Course, EditCourseModel>();
        }

        private void MappingDtoToEntity()
        {
            CreateMap<RegisterInputModel, AppUser>();
            CreateMap<CreateCourseModel, Course>();
            CreateMap<CreateLessonModel, Lesson>();
        }
    }
}