using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Infrastructure.Util
{
    public static class DecimalExtention
    {
        public static string ToVietNameseCurrency(this decimal input)
        {
            return string.Format("{0:0,0 vnđ}", input);
        }

    }
}