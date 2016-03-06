using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB
{
    public static class ExceptionExtensions
    {
        public static string PrettyPrint(this Exception e)
        {
            return new[] { e.Message, e.StackTrace, e.Source }.Join(Environment.NewLine);
        }
    }
}
