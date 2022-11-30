using KA.Infrastructure.Enums.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Infrastructure.Enums
{
    public enum OnlineCourseState
    {
        [EnumDisplayName(DisplayName = "Chưa đăng nhập")]
        UNAUTHEN = 0,

        [EnumDisplayName(DisplayName = "Đã hết hạn")]
        EXPIRED = 1,

        [EnumDisplayName(DisplayName = "Đang học")]
        STUDYING = 2,

        [EnumDisplayName(DisplayName = "Đã hoàn thành")]
        DONE = 3,

        [EnumDisplayName(DisplayName = "Chưa mua")]
        NOT_PURCHASED = 4,
    }
}