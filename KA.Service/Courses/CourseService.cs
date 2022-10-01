using AutoMapper;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public async Task<DataGridResponse<CourseItem>> GetAllCoursePaging(int skip, int top)
        {
            var result = new DataGridResponse<CourseItem>();

            var courses = _courseRepo.GetAll().Where(c => !c.IsDeleted);

            result.TotalItem = courses.Count();
            result.Items = courses.Skip(skip).Take(top).ToList().Select((c, i) =>
            {
                var ci = _mapper.Map<CourseItem>(c);
                ci.Index = (i + 1) + skip;
                ci.Price = string.Format("{0:0,0.00 vnđ}", c.Price);
                ci.DiscountPrice = string.Format("{0:0,0.00 vnđ}", c.Price);
                ci.CreatedDate = c.CreatedDate.Value.ToString("dd/MM/yyyy");
                return ci;
            }).ToList();
            return result;
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

        public async Task CreateOfflineCourse(CreateCourseModel input)
        {
            var course = _mapper.Map<Course>(input);
            await _courseRepo.AddAsync(course);
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
                course.MetaKeyWord = input.MetaKeyWord;
                course.MetaDescription = input.MetaDescription;
                course.MetaTitle = input.MetaTitle;
                course.Sort = input.Sort;
                course.ThumbNailImageLink = input.ThumbNailImageLink;
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

        public bool IsDuplicateCourseCode(string code)
        {
            if (_courseRepo.GetAll().Any(c => c.Code.ToLower().Contains(code)))
                return true;
            return false;

        }

        public EditCourseModel GetCourseById(int id)
        {
            var course = _courseRepo.GetById(id);
            return _mapper.Map<EditCourseModel>(course);
        }
    }
}