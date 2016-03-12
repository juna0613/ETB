using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ETB.App;
using ETB.Logging;
namespace ETB.Exec
{
    public class ExecMain
    {
        private static readonly Logger logger = Logger.Instance;
        public static void Main(string[] args)
        {
            ExecStatus worstStatus = default(ExecStatus);
            try
            {
                var writer = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                logger.LogError += writer.ErrorFormat;
                logger.LogFatal += writer.FatalFormat;
                logger.LogInfo += writer.InfoFormat;
                logger.LogWarn += writer.WarnFormat;

                logger.Info("Start Application");
                logger.Info("Arguments: {0}", args.Select(_ => _.DoubleQuote()).Join(","));
                AssertionHelper.DoAssert(helper => helper.Add(args.Length >= 2, "should have at least 2 arguments"));
                
                var targetDllPath = args[0];
                var targetExecName = args[1];
                var otherArgs = args.Skip(2);
                AssertionHelper.DoAssert(helper =>
                {
                    helper.AddNotNull(targetDllPath, "TargetDllPath");
                    helper.AddNotNull(targetExecName, "TargetExecName");
                });
                var typ = LoadType(targetDllPath, targetExecName);
                AssertionHelper.DoAssert(helper => helper.Add(typ != null, "Cannot load {0} at {1}".Format2(targetExecName, targetDllPath)));

                var cls = Activator.CreateInstance(typ) as IExecutable;
                AssertionHelper.DoAssert(helper => helper.Add(cls != null, "Loaded class[{0}] is not an IExecutable".Format2(targetExecName)));

                var setupResult = cls.Setup(otherArgs.ToArray());
                setupResult.PrettyLog(logger, "[Setup] Result" + Environment.NewLine);

                var result = cls.Execute();
                result.PrettyLog(logger, "[Execute] Result" + Environment.NewLine);

                var teardownResult = cls.Teardown();
                teardownResult.PrettyLog(logger, "[Teardown] Result" + Environment.NewLine);

                worstStatus = new[] { setupResult.Status, result.Status, teardownResult.Status }.WorstStatus();
            }
            catch (Exception e)
            {
                logger.Error("Error occurred\n{0}", e.PrettyPrint());
                worstStatus = ExecStatus.Error;
            }
            finally
            {
                logger.Info("Exit with exit code [{0}]", worstStatus);
            }
            Environment.Exit((int)worstStatus);
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


}
