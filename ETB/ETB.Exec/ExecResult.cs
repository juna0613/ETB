using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB.Exec
{
    public enum ExecStatus
    {
        Success,
        Warn,
        Error,
        Fatal
    }
    public class ExecResult
    {
        public ExecStatus Status { get; set; }
        public string Message { get; set; }
        public Exception Error { get; set; }
    }
}
