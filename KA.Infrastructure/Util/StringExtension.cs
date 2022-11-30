using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KA.Infrastructure.Util
{
    public static class StringExtension
    {
        public static string ConvertToUnSign(this string input)
        {
            input = input.Trim().ToLower();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?", StringComparison.Ordinal) >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?", StringComparison.Ordinal), 1);
            }
            return str2;
        }
        public static string RemoveSign4VietnameseString(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static string GetSeoName(this string str)
        {
            var seName = str?.RemoveSign4VietnameseString().Trim()
                .Replace(" ", "-")
                .Replace("/", "")
                .Replace("+", "")
                .Replace("?", "")
                .ToLower();

            return seName;
        }

        public static string GetSeoFriendlyString(this string str)
        {
            var seName = str?.RemoveSign4VietnameseString().Trim().Replace(" ", "-").ToLower();
            Regex rgx = new Regex("[^a-zA-Z0-9-]");
            if (!string.IsNullOrEmpty(seName))
            {
                seName = rgx.Replace(seName, "");
            }
            return seName;
        }

        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return $"{input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()}...";
        }

        public static string HidingEmail(this string email, char mark = '*')
        {
            if (!email.Contains("@"))
            {
                return email;
            }

            var name = email.Split("@")[0];
            var showChar = name.Length / 2;


            string pattern = @"(?<=[\w]{" + showChar + @"})[\w-\._\+%]*(?=[\w]{1}@)";
            string result = Regex.Replace(email, pattern, m => new string(mark, m.Length));
            return result;
        }

        public static string StripHtml(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string RemoveNonAlphabet(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            var temp = rgx.Replace(input, " ");
            Regex trimmer = new Regex(@"\s\s+");

            return trimmer.Replace(temp, " ");
        }

        public static bool CompareWithoutSpaceAndCaseInsensitive(this string str1, string str2)
        {
            return str1.Replace(" ", "").Equals(str2.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase);
        }

        public static string UpperCaseText(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            var textArray = input.Split(" ");
            var newTextArray = new List<string>();
            foreach (var textItem in textArray)
            {
                if (!string.IsNullOrEmpty(textItem))
                {
                    newTextArray.Add(textItem.First().ToString().ToUpper() + textItem.Substring(1));
                }
            }
            return string.Join(" ", newTextArray);
        }

        public static string ConvertToVietNameseCurrentcy(this string input)
        {
            return string.Format("{0:0,0 vnđ}", input);
        }

    }
}