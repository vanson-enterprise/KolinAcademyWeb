using AutoMapper;
using KA.DataProvider.Entities;
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
        private IRepository<OfflineCourseStartDate> _startDateOfflineCourseRepo;
        private IMapper _mapper;
        public CourseService(IRepository<Course> baseReponsitory, IMapper mapper, IRepository<Lesson> lessonRepo, IRepository<OfflineCourseStartDate> startDateOfflineCourseRepo) : base(baseReponsitory)
        {
            _courseRepo = baseReponsitory;
            _mapper = mapper;
            _lessonRepo = lessonRepo;
            _startDateOfflineCourseRepo = startDateOfflineCourseRepo;
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

        public async Task CreateOfflineCourse(CreateOfflineCourseModel input, List<OfflineCourseStartDateVm> startDates)
        {
            var course = _mapper.Map<Course>(input);
            await _courseRepo.AddAsync(course);
            if (course.Id > 0)
            {
                foreach (var startDate in startDates)
                {
                    var newStartDate = new OfflineCourseStartDate()
                    {
                        OfflineCourseId = course.Id,
                        Place = startDate.Place,
                        StartTime = startDate.StartTime.Value
                    };
                    _startDateOfflineCourseRepo.Add(newStartDate);
                }
            }
        }

        public async Task CreateStartDate(OfflineCourseStartDate input)
        {
            _startDateOfflineCourseRepo.Add(input);
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
            if (input != null)
            {
                _lessonRepo.Update(input);
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

        public ResponseDto UpdateStartDate(OfflineCourseStartDate input)
        {
            var updateResult = _startDateOfflineCourseRepo.Update(input);
            if (updateResult > 0)
            {
                return new ResponseDto()
                {
                    Message = "Cập nhật ngày khai giảng thành công",
                    Status = ResponseStatus.SUCCESS
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Message = "Cập nhật ngày khai giảng thất bại",
                    Status = ResponseStatus.ERROR
                };
            }
        }


        public void DeleteLesson(int id)
        {
            _lessonRepo.DeleteById(id);
        }

        public void DeleteStartDate(int id)
        {
            _startDateOfflineCourseRepo.DeleteById(id);
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

        public List<OfflineCourseStartDate> GetAllStartDatesOfCourse(int courseId)
        {
            return _startDateOfflineCourseRepo.GetAll().Where(i => i.OfflineCourseId == courseId).ToList();
        }
        #endregion

        #region Site
        public List<OfflineCourseViewModel> GetAllOpeningSoonOfflineCourse()
        {
            var result = new List<OfflineCourseViewModel>();
            var datas = (from c in _courseRepo.GetAll()
                         join csd in _startDateOfflineCourseRepo.GetAll() on c.Id equals csd.OfflineCourseId
                         where csd.StartTime > DateTime.Now
                         select new { c, csd }).AsEnumerable();
            var groups = from i in datas
                         group i by i.c into gc
                         select gc;

            foreach (var groupCourse in groups)
            {
                var offlineCourseVm = new OfflineCourseViewModel()
                {
                    Name = groupCourse.Key.Name,
                    DetailCourseLink = "/" + groupCourse.Key.Name.GetSeoName() + "-" + groupCourse.Key.Code,
                    StartDates = groupCourse.Select(i => new OfflineCourseStartDateVm()
                    {
                        Place = i.csd.Place,
                        StartTime = i.csd.StartTime,
                    }).ToList()
                };
                result.Add(offlineCourseVm);
            };

            return result;
        }
        #endregion
    }
}