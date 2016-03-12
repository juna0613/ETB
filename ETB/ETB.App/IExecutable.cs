using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB.App
{
    public interface IExecutable
    {
        ExecResult Setup(params string[] args);
        ExecResult Execute();
        ExecResult Teardown();
    }

    public abstract class ExecuteBase : IExecutable
    {
        public abstract ExecResult Execute();
        protected readonly Logging.Logger _logger = Logging.Logger.Instance;

        public virtual ExecResult Setup(params string[] args)
        {
            _logger.Info("Start Setup: [args]\t{0}", args.Join(","));
            var res = SetupInner(args);
            if (res.Status == ExecStatus.Success) _logger.Info("Setup Succeeded");
            else if (res.Status == ExecStatus.Warn) _logger.Warn("Setup has some warnings:\t{0}", res.Message);
            else if (res.Status == ExecStatus.Error) _logger.Error("Setup has error:\t{0}\nException\t{1}", res.Message, res.Error.PrettyPrint());
            else _logger.Fatal("Setup has fatal problems:\t{0}Exception\t{1}", res.Message, res.Error.PrettyPrint());
            return res;
        }
        protected abstract ExecResult SetupInner(params string[] args);

        public virtual ExecResult Teardown()
        {
            _logger.Info("No teardown");
            return new ExecResult { Status = ExecStatus.Success, Error = null, Message = "Successful teardown" };
        }
    }
}
