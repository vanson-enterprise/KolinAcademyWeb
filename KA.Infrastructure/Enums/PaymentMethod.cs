using KA.Infrastructure.Enums.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Infrastructure.Enums
{
    public enum PaymentMethod
    {
        [EnumDisplayName(DisplayName = "Chuyển khoản thường")]
        CK = 0,

        [EnumDisplayName(DisplayName = "Chuyển khoản qua VNPAY")]
        VNPAY = 1,

        [EnumDisplayName(DisplayName = "VISA / MASTERCARD")]
        VISA = 2
    }
}