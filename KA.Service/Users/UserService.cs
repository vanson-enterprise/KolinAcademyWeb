using AutoMapper;
using Castle.Core.Logging;
using KA.Infrastructure.Util;
using KA.ViewModels.Common;
using KA.ViewModels.Courses;
using KA.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KA.Service.Users
{
    public class UserService : BaseService<AppUser>, IUserService
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IRepository<AppRole> _roleRepo;
        private readonly IRepository<UserCourse> _userCourseRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly IRepository<IdentityUserRole<string>> _userRoleRepo;
        private IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(IRepository<AppUser> baseReponsitory,
            IMapper mapper,
            IRepository<AppRole> roleRepo,
            IRepository<IdentityUserRole<string>> userRoleRepo,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IRepository<UserCourse> userCourseRepo,
            IRepository<Course> courseRepo) : base(baseReponsitory)
        {
            _userRepo = baseReponsitory;
            _mapper = mapper;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _userManager = userManager;
            _roleManager = roleManager;
            _userCourseRepo = userCourseRepo;
            _courseRepo = courseRepo;
        }
        #region Admin
        public async Task<DataGridResponse<UserItem>> GetAllUserPaging(int skip, int top)
        {
            var result = new DataGridResponse<UserItem>();

            var userRoles = (from u in _userRepo.GetAll()
                             join ur in _userRoleRepo.GetAll() on u.Id equals ur.UserId into jur
                             from ur in jur.DefaultIfEmpty()
                             join r in _roleRepo.GetAll() on ur.RoleId equals r.Id into jr
                             from r in jr.DefaultIfEmpty()
                             where u.IsDeleted == false
                             orderby u.Id descending
                             select new { u, r }).AsEnumerable();

            var userGroups = from ur in userRoles
                             group ur by ur.u into gur
                             select gur;

            result.TotalItem = userGroups.Count();

            result.Items = userGroups.Skip(skip).Take(top).ToList().Select((g, i) =>
            {
                var ci = _mapper.Map<UserItem>(g.Key);
                ci.Index = (i + 1) + skip;
                ci.Roles = string.Join(", ", g.Select(g => g.r != null ? g.r.Name : ""));
                ci.CreatedDate = g.Key.CreatedDate.ToString("dd/MM/yyyy");
                return ci;
            }).ToList();

            return result;
        }

        public async Task<List<RoleModel>> GetAllRoleForSelect(string? userId)
        {
            var result = await _roleRepo.GetAll().Where(r => r.Name != "Administrators").Select(r => new RoleModel()
            {
                Id = r.Id,
                RoleName = r.Name,
                IsSelected = false
            }).ToListAsync();

            return result;
        }

        public async Task<EditUserModel> GetUserForEdit(string userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            var result = new EditUserModel()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };

            result.Roles = _roleManager.Roles
                .Where(r => r.NormalizedName != "ADMINISTRATORS")
                .Select(r => new RoleModel()
                {
                    Id = r.Id,
                    RoleName = r.Name,
                    IsSelected = false
                }).ToList();

            foreach (var role in result.Roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.RoleName))
                {
                    role.IsSelected = true;
                }
            }
            return result;
        }

        public async Task<ResponseDto> CreateNewUser(CreateUserModel input)
        {
            var user = _mapper.Map<AppUser>(input);
            var result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                if (await AssignRole(user, input.Roles))
                {
                    return new ResponseDto()
                    {
                        Message = "Tạo mới người dùng thành công",
                        Status = ResponseStatus.SUCCESS,
                    };
                }
                else
                {
                    return new ResponseDto()
                    {
                        Message = "Gán role thất bại",
                        Status = ResponseStatus.ERROR,
                    };
                }
                return new ResponseDto()
                {
                    Message = "Gán role thất bại",
                    Status = ResponseStatus.ERROR,
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Message = "Tạo mới người dùng thất bại",
                    Status = ResponseStatus.ERROR
                };
            }
        }

        public async Task<ResponseDto> EditUser(EditUserModel input)
        {
            var user = await _userRepo.GetFirstOrDefaultAsync(u => u.Id == input.Id);
            if (user != null)
            {
                user.Email = input.Email;
                user.FullName = input.FullName;
                user.PhoneNumber = input.PhoneNumber;
                user.UserName = input.UserName;
                user.UpdateUserId = input.UpdateUserId;
                user.UpdateDate = input.UpdateDate;

                var updateRoleResult = await AssignRole(user, input.Roles);
                var updateUserResult = await _userManager.UpdateAsync(user);
                var resetPasswordResult = false;
                if (input.Password != null)
                {
                    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    resetPasswordResult = (await _userManager.ResetPasswordAsync(user, resetToken, input.Password)).Succeeded;
                }
                if (updateRoleResult && updateUserResult.Succeeded && (input.Password == null || resetPasswordResult))
                {
                    return new ResponseDto()
                    {
                        Message = "Cập nhật người dùng thành công",
                        Status = ResponseStatus.SUCCESS,
                    };
                }
                else if (!updateUserResult.Succeeded)
                {
                    return new ResponseDto()
                    {
                        Message = "Cập nhật người dùng thất bại",
                        Status = ResponseStatus.ERROR,
                    };
                }
                else
                {
                    return new ResponseDto()
                    {
                        Message = "Cập nhật role thất bại",
                        Status = ResponseStatus.ERROR,
                    };
                }
            }
            else
            {
                return new ResponseDto()
                {
                    Message = "Người dùng không tồn tại",
                    Status = ResponseStatus.ERROR,
                };
            }

        }

        private async Task<bool> AssignRole(AppUser user, List<RoleModel> listRole)
        {
            if (user == null)
            {
                return false;
            }
            var removedRoles = listRole.Where(x => x.IsSelected == false).Select(x => x.RoleName).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = listRole.Where(x => x.IsSelected).Select(x => x.RoleName).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return true;
        }
        #endregion

        #region Site
        public async Task<UserProfileVm> GetUserProfile(string userId)
        {
            var user = _userRepo.GetById(userId);
            var userCourses = (from uc in _userCourseRepo.GetAll()
                               join c in _courseRepo.GetAll() on uc.CourseId equals c.Id
                               where uc.UserId == userId
                               select new { c, uc }
                              ).ToList();
            return new UserProfileVm()
            {
                UserProfileInfo = new()
                {
                    UserId = userId,
                    Email = user.Email,
                    Name = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                },
                OwningOfflineCourseVm = userCourses
                    .Where(i => i.c.Type == CourseType.OFFLINE)
                    .Select(i => new OwningCourseVm()
                    {
                        Name = i.c.Name,
                        DetailCourceLink = "/" + i.c.Name.GetSeoName() + "-" + i.c.Code,
                        ThumbnailImageLink = i.c.ThumbNailImageLink
                    }).ToList(),
                OwningOnlineCourseVm = userCourses
                    .Where(i => i.c.Type == CourseType.ONLINE)
                    .Select(i => new OwningCourseVm()
                    {
                        Name = i.c.Name,
                        DetailCourceLink = "/" + i.c.Name.GetSeoName() + "-" + i.c.Code,
                        ThumbnailImageLink = i.c.ThumbNailImageLink
                    }).ToList(),
                CourseTransactionVms = userCourses.Select(i => new CourseTransactionVm()
                {
                    CourseName = i.c.Name,
                    TransactionDate = i.uc.CreatedDate.ToString("dd/MM/yyyy")
                }).ToList()
            };
        }

        public async Task UpdateUserInfo(UserProfileInfo input)
        {
            var user = _userRepo.GetById(input.UserId);
            user.FullName = input.Name;
            user.PhoneNumber = input.PhoneNumber;
            user.Email = input.Email;
            if (input.Password != null)
            {
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, resetPasswordToken, input.Password);
            }
            _userRepo.Update(user);
        }

        public async Task<UserCourse> GetPurchasedCourse(string userId, int courseId)
        {
            return await _userCourseRepo.GetAll().OrderByDescending(uc=>uc.CreatedDate).Where(uc =>
                uc.CourseId == courseId && uc.UserId == userId
            ).FirstOrDefaultAsync();
        }
        #endregion

    }
}