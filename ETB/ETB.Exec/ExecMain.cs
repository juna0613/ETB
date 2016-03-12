using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine.Text;
using System.Reflection;
using ETB.App;
using ETB.Logging;
using CommandLine;
namespace ETB.Exec
{
    public class ExecMain
    {
        private static readonly Logger logger = Logger.Instance;
        public static void Main(string[] args)
        {
            try
            {
                var writer = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                logger.LogError += writer.ErrorFormat;
                logger.LogFatal += writer.FatalFormat;
                logger.LogInfo += writer.InfoFormat;
                logger.LogWarn += writer.WarnFormat;

                logger.Info("Start Application");
                logger.Info("Arguments: {0}", args.Select(_ => _.DoubleQuote()).Join(","));

                var opt = new Options();
                var status = CommandLine.Parser.Default.ParseArguments(args, opt);
                if (!status)
                {
                    logger.Info("Target:\t{0}",opt.Target);
                    logger.Info("ExecutableName:\t{0}", opt.ExecutableName);
                    logger.Error("Failed to parse arguments. Command Help:{0}", opt.GetUsage());
                    Environment.Exit(-1);
                }
                var targetDllPath = opt.Target;
                var targetExecName = opt.ExecutableName;
                var otherArgs = args.Skip(2);
                AssertionHelper.DoAssert(helper =>
                {
                    helper.AddNotNull(targetDllPath, "TargetDllPath");
                    helper.AddNotNull(targetExecName, "TargetExecName");
                });
                var typ = LoadType(targetDllPath, targetExecName);
                if (typ == null)
                {
                    logger.Error("Cannot load {0} at {1}", targetExecName, targetDllPath);
                    System.Environment.Exit(-1);
                }
                var cls = Activator.CreateInstance(typ) as IExecutable;
                if (cls == null)
                {
                    logger.Error("Loaded class[{0}] is not an IExecutable", targetExecName);
                    Environment.Exit(-1);
                }

                cls.Setup(otherArgs.ToArray());
                var result = cls.Execute();
                cls.Teardown();
            }
            catch (Exception e)
            {
                logger.Error("Error occurred\n{0}", e.PrettyPrint());
            }
            finally
            {
                logger.Info("Exit");
            }
        }

        private static Type LoadType(string path, string name)
        {
            Type typ = null;
            try
            {
                var asm = Assembly.LoadFrom(path);
                typ = asm.GetType(name);
            }
            catch (Exception e)
            {
                logger.Error(new[] {
                    "Could not load class :",
                    "AssemblyPath:\t{0}",
                    "ClassName:\t{1}",
                    "Reason:\t{2}" }.Join(Environment.NewLine + "\t"),
                    path, name, e.Message);
            }
            return typ;
        }
    }

    public class Options
    {
        [Option('t', "target", HelpText = "Target dll path", Required = true)]
        public string Target
        {
            get; set;
        }
        [Option('e', "exec", HelpText = "Target Executable Name", Required = true)]
        public string ExecutableName
        {
            get; set;
        }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, _ => { HelpText.DefaultParsingErrorsHandler(this, _); });
        }
    }
}
