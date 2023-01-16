using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trisatech.AspNet.Common.Helpers
{
    public class TextHelper
    {
        public static string ConvertToHtmlNewLine(string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str.Replace(System.Environment.NewLine, "<br/>");
        }

        public static string RandomString(int length, string chars)
        {
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetRandAlphanumeric(int length)
        {
            return RandomString(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
        }

        public static string GetRandAlphanumericInLowAndUp(int length)
        {
            return RandomString(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        }

        public static string GetRandNumeric(int length)
        {
            return RandomString(length, "0123456789");
        }

        public static string GetRandAlphabet(int length)
        {
            return RandomString(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        public static string GetRandomUsername(int length)
        {
            return RandomString2(length);
        }

        private static string RandomString2(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
