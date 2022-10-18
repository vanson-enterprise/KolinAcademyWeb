using AutoMapper;
using KA.Infrastructure.Util;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using KA.ViewModels.Lessons;

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

        #region Admin
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
                ci.DiscountPrice = string.Format("{0:0,0.00 vnđ}", c.DiscountPrice);
                ci.CreatedDate = c.CreatedDate.Value.ToString("dd/MM/yyyy");
                return ci;
            }).ToList();
            return result;
        }

        public async Task CreateOnlineCourse(CreateOnlineCourseModel input, List<CreateLessonModel> lessons)
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

        public async Task CreateOfflineCourse(CreateOfflineCourseModel input)
        {
            var course = _mapper.Map<Course>(input);
            await _courseRepo.AddAsync(course);
        }

        public async Task Edit(Course input)
        {
            await _courseRepo.UpdateAsync(input);
        }

        public bool IsDuplicateCourseCode(string code)
        {
            if (_courseRepo.GetAll().Any(c => c.Code.ToLower().Contains(code)))
                return true;
            return false;

        }

        public Course GetCourseById(int id)
        {
            return _courseRepo.GetById(id);
        }

        public List<Lesson> GetAllLessonInCourse(int courseId)
        {
            return _lessonRepo
                .GetAll()
                .Where(l => l.CourseId == courseId)
                .ToList();
        }

        public Lesson AddLessonToCourse(Lesson input)
        {
            return _lessonRepo.Add(input);
        }

        public ResponseDto EditLesson(Lesson input)
        {
            var lesson = _lessonRepo.GetById(input.Id);
            if (lesson != null)
            {
                lesson.Name = input.Name;
                lesson.VideoLink = input.VideoLink;
                _lessonRepo.Update(lesson);
                return new ResponseDto()
                {
                    Message = "Cập nhật bài giảng thành công",
                    Status = ResponseStatus.SUCCESS
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Message = "Cập nhật bài giảng thất bại",
                    Status = ResponseStatus.ERROR
                };
            }
        }

        public void DeleteLesson(int id)
        {
            _lessonRepo.DeleteById(id);
        }

        public new ResponseDto DeleteById(object id)
        {
            var course = _courseRepo.GetById(id);
            if (course != null)
            {
                course.IsDeleted = true;
                _courseRepo.Update(course);
                return new ResponseDto()
                {
                    Status = ResponseStatus.SUCCESS,
                    Message = "Xóa khóa học thành công"
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Status = ResponseStatus.ERROR,
                    Message = "Xóa khóa học thất bại"
                };
            }
        }
        #endregion

        #region Site
        public List<OfflineCourseViewModel> GetAllOpeningSoonOfflineCourse()
        {
            return _courseRepo.GetAll()
                .Where(c => c.StartDate > DateTime.Now && c.Type == CourseType.OFFLINE)
                .Select(c => new OfflineCourseViewModel
                {
                    Place = c.Place,
                    StartDate = c.StartDate.Value.ToString("dd/MM/yyy"),
                    Name = c.Name,
                    DetailCourseLink = c.Name.GetSeoName() + "-" + c.Code
                }).ToList();
        }
        #endregion
    }
}