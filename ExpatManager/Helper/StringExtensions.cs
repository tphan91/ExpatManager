using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpatManager.Helper
{
    public static class StringExtensions
    {
        public static int TryToParseInt(string value)
        {
            int number;
            bool result = Int32.TryParse(value, out number);
            if (!result)
            {
                if (value == null) value = "";
                number = 0;
            }
            return number;
        }

        public static string Left(this string s, int left)
        {
            return s.Substring(0, Math.Min(s.Length, left));
        }
    }
}