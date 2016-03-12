using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
namespace ETB.App.Test
{
    public class Options
    {
        [Option('v', "value")]
        public string ValueReturned { get; set; }
    }

    public class TestApp : ExecuteBase
    {
        private Options _opt = new Options();
        protected override ExecResult SetupInner(params string[] args)
        {
            if(!Parser.Default.ParseArguments(args, _opt))
            {
                return ExecResult.ErrorResult(message: "Error in parsing arguments: {0}".Format2(args.Join(",")));
            }
            return ExecResult.SuccessResult("Setup has uccessfully finished");
        }
        public override ExecResult Execute()
        {
            _logger.Info(_opt.ValueReturned);
            if (_opt.ValueReturned.Length > 3)
                return ExecResult.SuccessResult("OK");
            else
                return ExecResult.ErrorResult("NG");
        }
    }
}
