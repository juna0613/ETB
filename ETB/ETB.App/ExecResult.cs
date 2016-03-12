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
        public static ExecResult SuccessResult(string message = "")
        {
            return new ExecResult
            {
                Status = ExecStatus.Success,
                Message = message,
                Error = null
            };
        }
        public static ExecResult ErrorResult(string message = "", Exception ex = null)
        {
            return new ExecResult
            {
                Status = ExecStatus.Error,
                Message = message,
                Error = ex ?? new Exception(message)
            };
        }
    }
}
