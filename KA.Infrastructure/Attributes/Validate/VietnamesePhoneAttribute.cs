using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Infrastructure.Attributes.Validate
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class VietnamesePhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            var phone = "";
            if (value == null)
            {
                result = true;
            }
            else if (IsNumberString(phone = value.ToString()))
            {
                if (phone.StartsWith("+84"))
                {
                    phone = phone.Replace("+84", "0");
                    result = CheckPhone(phone);
                }
                else if (phone.StartsWith("0"))
                {
                    result = CheckPhone(phone);
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        public bool CheckPhone(string phone)
        {
            var listHeadPhone = new List<string>()
                {
                    "086","096","097","098","032","033","034","035","036","037","038","039",// đầu số Viettel
                    "089","090","093","070","079","077","076","078", // đầu số Mobi
                    "088","091","094","083","084","085","081","082", // đầu số Vina
                    "092","056","058", // đầu số VietnamMobile
                    "099","056" // đầu số Gmobile
                };
            var threeFirstNumber = phone.Substring(0, 3);
            if (!listHeadPhone.Contains(threeFirstNumber))
            {
                return false;
            }
            return true;
        }
        public bool IsNumberString(string phone)
        {
            phone = phone.Replace("+", "");
            if (phone.Length < 10 || phone.Length > 11)
            {
                return false;
            }
            var result = phone.All(char.IsDigit);
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }


    }
}
