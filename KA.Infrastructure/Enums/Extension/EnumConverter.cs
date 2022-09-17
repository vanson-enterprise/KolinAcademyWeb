using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KA.Infrastructure.Enums.Extension
{
    public static class EnumConverter
    {
        public static Dictionary<string, string> ToDictionary<TEnum>(this TEnum obj)
        {
            return Enum.GetValues(typeof(TEnum)).OfType<Enum>().ToDictionary(t => Convert.ToInt32(t).ToString(), t => t.DisplayName());
        }

        public static string DisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            EnumDisplayNameAttribute attribute = Attribute.GetCustomAttribute(field, typeof(EnumDisplayNameAttribute)) as EnumDisplayNameAttribute;
            return attribute == null ? value.ToString() : attribute.DisplayName;
        }
    }
}
