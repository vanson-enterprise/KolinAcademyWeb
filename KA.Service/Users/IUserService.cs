using KA.ViewModels.Common;
using KA.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Users
{
    public interface IUserService : IService<AppUser>
    {
        Task<UserCourse> GetPurchasedCourse(string userId, int courseId);
        Task<ResponseDto> CreateNewUser(CreateUserModel input);
        Task<ResponseDto> EditUser(EditUserModel input);
        Task<List<RoleModel>> GetAllRoleForSelect(string? userId);
        Task<DataGridResponse<UserItem>> GetAllUserPaging(int skip, int top);
        Task<EditUserModel> GetUserForEdit(string userId);
        Task<UserProfileVm> GetUserProfile(string userId);
        Task UpdateUserInfo(UserProfileInfo input);
    }
}