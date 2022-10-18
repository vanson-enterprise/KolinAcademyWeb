using AutoMapper;
using KA.DataProvider.Entities;
using KA.ViewModels.Authen;
using KA.ViewModels.Carts;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using KA.ViewModels.Users;
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

        public MappingProfile()
        {
            MappingEntityToViewModel();
            MappingDtoToEntity();

        }

        private void MappingEntityToViewModel()
        {
            CreateMap<AppUser, UserItem>();
            CreateMap<Course, CourseItem>();
            CreateMap<Course, EditOnlineCourseModel>()
            .ForMember(m => m.Price,
                    cf => cf.MapFrom(c => decimal.Round(c.Price, 2, MidpointRounding.AwayFromZero))
                 )
            .ForMember(m => m.DiscountPrice,
                    cf => cf.MapFrom(c => decimal.Round(c.DiscountPrice, 2, MidpointRounding.AwayFromZero))
                 );

            CreateMap<Course, EditOfflineCourseModel>()
            .ForMember(m => m.Price,
                    cf => cf.MapFrom(c => decimal.Round(c.Price, 2, MidpointRounding.AwayFromZero))
                 )
            .ForMember(m => m.DiscountPrice,
                    cf => cf.MapFrom(c => decimal.Round(c.DiscountPrice, 2, MidpointRounding.AwayFromZero))
                 );

            CreateMap<Lesson, EditLessonModel>();
        }

        private void MappingDtoToEntity()
        {
            CreateMap<RegisterInputModel, AppUser>();
            CreateMap<CreateUserModel, AppUser>();
            CreateMap<CreateOnlineCourseModel, Course>();
            CreateMap<CreateOfflineCourseModel, Course>();
            CreateMap<CreateLessonModel, Lesson>();
            CreateMap<CreateCartVm, Cart>();
        }
    }
}