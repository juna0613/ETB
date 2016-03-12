using System;

namespace ETB.App
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
