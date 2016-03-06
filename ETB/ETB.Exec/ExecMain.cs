using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine.Text;
using System.Reflection;

namespace ETB.Exec
{
    public class ExecMain
    {
        public static void Main(string[] args)
        {
            var targetDllPath = args[0];
            var targetExecName = args[1];
            var otherArgs = args.Skip(2);
            var typ = LoadType(targetDllPath, targetExecName);
            if (typ == null) Environment.Exit(-1);
            var cls = Activator.CreateInstance(typ) as IExecutable;
            if(cls == null)
            {
                Console.WriteLine("[ERROR] Loaded class[{0}] is not an IExecutable", targetExecName);
                Environment.Exit(-1);
            }
            try
            {
                cls.Setup(otherArgs.ToArray());
                cls.Execute();
                cls.Teardown();
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] Error occurred\n{0}", e.PrettyPrint());
            }
            finally
            {
                Console.WriteLine("Exit");
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
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] Could not load class :\n\tAssemblyPath:{0}\n\tClassName{1}\n\tReason:{2}", path, name, e.Message);
            }
            return typ;
        }
    }
}
