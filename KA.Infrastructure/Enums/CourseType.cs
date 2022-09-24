using KA.Infrastructure.Enums.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Infrastructure.Enums
{
    public enum CourseType
    {
        [EnumDisplayName(DisplayName = "Offline")]
        OFFLINE,
        [EnumDisplayName(DisplayName = "Online")]
        ONLINE
    }
}