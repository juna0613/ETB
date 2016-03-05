using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace ETB.Cmd.Test
{
    [TestFixture]
    public class DosCommandTest
    {
        [Test]
        public void TestDir()
        {
            var cmd = @"dir d:\";
            var command = new DosCommand(cmd);
            var res = command.DoCommand();
            Console.WriteLine(res.Output);
        }
    }
}
