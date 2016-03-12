using System;
using System.Collections.Generic;
using System.Linq;
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

    public static class ExecResultExtension
    {
        public static ExecStatus WorstStatus(this IEnumerable<ExecStatus> statuses)
        {
            return statuses.OrderBy(x => (int)x).LastOrDefault();
        }

        public static ExecStatus BestStatus(this IEnumerable<ExecStatus> statuses)
        {
            return statuses.OrderBy(x => (int)x).FirstOrDefault();
        }

        public static string PrettyPrint(this ExecResult self)
        {
            return new []
            {
               "Status:\t" + self.Status.ToString(), "Message:\t" + self.Message, "Error:\t" + (self.Error != null ? self.Error.PrettyPrint() : string.Empty)
            }.Join(Environment.NewLine);
        }

        public static void PrettyLog(this ExecResult self, Logging.ILogger logger, string prefix = "", string suffix = "")
        {
            var content = prefix + self.PrettyPrint() + suffix;
            switch (self.Status)
            {
                case ExecStatus.Success:
                    logger.Info(content);
                    break;
                case ExecStatus.Warn:
                    logger.Warn(content);
                    break;
                case ExecStatus.Error:
                    logger.Error(content);
                    break;
                case ExecStatus.Fatal:
                default:
                    logger.Fatal(content);
                    break;
            }
        }
    }
}
