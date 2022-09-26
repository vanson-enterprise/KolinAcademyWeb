using AutoMapper;
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
    public class CourseService : BaseService<Course>, ICourseService
    {
        private IRepository<Course> _courseRepo;
        private IRepository<Lesson> _lessonRepo;
        private IMapper _mapper;
        public CourseService(IRepository<Course> baseReponsitory, IMapper mapper, IRepository<Lesson> lessonRepo) : base(baseReponsitory)
        {
            _courseRepo = baseReponsitory;
            _mapper = mapper;
            _lessonRepo = lessonRepo;
        }

        public async Task CreateOnlineCourse(CreateCourseModel input, List<CreateLessonModel> lessons)
        {
            var course = _mapper.Map<Course>(input);
            var newCourse = await _courseRepo.AddAsync(course);
            if (newCourse.Id > 0)
            {
                foreach (var lessonModel in lessons)
                {
                    var lesson = _mapper.Map<Lesson>(lessonModel);
                    lesson.CourseId = newCourse.Id;
                    _lessonRepo.Add(lesson);
                }
            }
        }

        public async Task<ResponseDto> Edit(EditCourseModel input)
        {
            var course = await _courseRepo.GetFirstOrDefaultAsync((c) => c.Id == input.Id);
            if (course == null)
            {
                return new ResponseDto()
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Không tìm thấy khóa học"
                };
            }
            else
            {
                course.Name = input.Name;
                course.IsActive = input.IsActive;
                course.Price = input.Price;
                course.DiscountPrice = input.DiscountPrice;
                course.Tag = input.Tag;
                course.Description = input.Description;
                course.ShortDescription = input.ShortDescription;
                course.MetaKeyWord = input.MetaKeyWord;
                course.MetaDescription = input.MetaDescription;
                course.MetaTitle = input.MetaTitle;
                course.Sort = input.Sort;
                course.PreviewImageFileName = input.PreviewImageFileName;
                course.PreviewImageTitle = input.PreviewImageTitle;
                course.IntroduceVideoLink = input.IntroduceVideoLink;
                course.UpdatedDate = DateTime.Now;
                await _courseRepo.UpdateAsync(course);

                return new ResponseDto()
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Cập nhật thành công"
                };
            }
        }

        public async Task<ResponseDto> Delete(int id)
        {
            var course = await _courseRepo.GetFirstOrDefaultAsync((c) => c.Id == id);
            if (course == null)
            {
                return new ResponseDto()
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Không tìm thấy khóa học"
                };
            }
            else
            {
                course.IsDeleted = true;
                await _courseRepo.UpdateAsync(course);
                return new ResponseDto()
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Xóa thành công"
                };
            }
        }
    }
}