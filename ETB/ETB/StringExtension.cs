using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB
{
    public static class StringExtension
    {
        public static string DoubleQuote(this string self)
        {
            return '"' + self.Replace("\"","\"\"") + '"';
        }
        public static string SingleQuote(this string self)
        {
            return "'" + self + "'";
        }
        public static string Join(this IEnumerable<string> self, char separator)
        {
            return self.Join(separator.ToString());
        }
        public static string Join(this IEnumerable<string> self, string separator)
        {
            return string.Join(separator, self.ToArray());
        }
        public static int ToInt(this string self)
        {
            return int.Parse(self);
        }
        public static double ToDouble(this string self)
        {
            return double.Parse(self);
        }
        public static DateTime ToDateTime(this string self)
        {
            return DateTime.Parse(self);
        }
        public static DateTime ToDateTime(this string self, string format)
        {
            return DateTime.ParseExact(self, format, null);
        }
        public static System.Text.RegularExpressions.Regex ToRegex(this string self)
        {
            return new System.Text.RegularExpressions.Regex(self);
        }
        public static bool IsRegexMatch(this string self, string target)
        {
            return self.ToRegex().IsMatch(target);
        }
        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }
        public static string Format2(this string self, params object[] args)
        {
            return string.Format(self, args);
        }
        public static int CountMatches(this string self, string target)
        {
            return target.ToRegex().Matches(self).Count;
        }
        public static Type GuessType(this string self)
        {
            int resint;
            if (int.TryParse(self, out resint))
            {
                return typeof(int);
            }
            long reslong;
            if (long.TryParse(self, out reslong))
            {
                return typeof(long);
            }
            double resdbl;
            if (double.TryParse(self, out resdbl))
            {
                return typeof(double);
            }
            bool resbl;
            if (bool.TryParse(self, out resbl))
            {
                return typeof(bool);
            }
            DateTime resdt;
            if (DateTime.TryParse(self, out resdt))
            {
                return typeof(DateTime);
            }
            return typeof(string);
        }
    }
}
