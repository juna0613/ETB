using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB
{
    public static class NumericExtension
    {
        public static bool IsIn<T>(this T value, T start, T end, bool leftIncluded = true, bool rightIncluded = true) where T : IComparable
        {
            return (leftIncluded ? start.CompareTo(value) <= 0 : start.CompareTo(value) < 0) &&
                   (rightIncluded ? value.CompareTo(end) <= 0 : value.CompareTo(end) < 0);
        }
        
    }
}
