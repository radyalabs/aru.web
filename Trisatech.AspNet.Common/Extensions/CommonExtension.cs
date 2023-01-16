using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Trisatech.AspNet.Common.Extensions
{
    public static class CommonExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string GetInitial(this string fullname)
        {
            // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(fullname, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

            // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            // Extract up to 2 initials from the remaining cleaned name.
            initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

            if (initials.Length > 2)
            {
                // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                initials = initials.Substring(0, 2);
            }

            string initial = initials.ToUpperInvariant();

            if (initial.Length == 1)
            {
                initial += initial;
            }

            return initial;
        }

        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        public static string ToMD5(this string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(str);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
                s.Append(b.ToString("x2").ToLowerInvariant());

            return s.ToString();
        }

        public static string MobilePhoneFormat(this string str)
        {
            string final = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                if (str.StartsWith("0"))
                {
                    str = str.Remove(0, 1);
                    final = string.Format("{0}{1}", "+62", str);
                }else if (str.StartsWith("62"))
                {
                    final = "+" + str;
                }
                else
                {
                    final = "+62" + str; 
                }
            }

            return final;
        }

        public static string ToSHA256(this string str)
        {
            SHA256Managed crypt = new SHA256Managed();

            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(str), 0, Encoding.ASCII.GetByteCount(str));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }

            return hash;
        }

        public static string ToTimestamp(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssfff");
        }

        public static string ToFormattedNumberID(this decimal obj, int numDecimal = 0)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)
            CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = ".";

            return obj.ToString(string.Format("n{0}", numDecimal), nfi);
        }

        public static double ToDoubleCulture(this string obj)
        {
            string strTemp = "0";
            char decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (decimalSeparator == '.')
            {
                strTemp = obj.Replace(",", ".");
            }
            else if (decimalSeparator == ',')
            {
                strTemp = obj.Replace(".", ",");
            }
            Convert.ToDouble(strTemp);
            return Convert.ToDouble(strTemp);
        }

        public static string ToZeroDecimal(this decimal obj)
        {
            return String.Format("{0:0.##}", obj);
        }

        public static DateTime ToUtcID(this DateTime dateTime)
        {
            DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

            return dt;
        }

        public static DateTime ToUtc(this DateTime dateTime, int hour = 0)
        {
            return dateTime.ToUniversalTime().AddHours(hour);
        }

        public static DateTime Add7Hour(this DateTime dateTime)
        {
            return dateTime.AddHours(7);
        }

        internal static object ToUtcID(DateTime? created_date)
        {
            throw new NotImplementedException();
        }

        public static string ToHtmlNewLine(this string str)
        {
            return str.Replace(System.Environment.NewLine, "<br/>");
        }
    }
}
