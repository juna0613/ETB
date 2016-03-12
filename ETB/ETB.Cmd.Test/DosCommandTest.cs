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
            var cmd = @"dir c:\";
            var command = new DosCommand(cmd);
            var res = command.DoCommand();
            Assert.That(res.Status, Is.EqualTo(0), res.Error);
            Assert.That(res.Error, Is.Empty);
            Assert.That(res.Output, Is.Not.Empty);
            Assert.That(res.Output, Contains.Substring(@"c:\"));
        }

        [Test]
        public void TestErrorDir()
        {
            var cmd = @"dir b:\";
            var command = new DosCommand(cmd);
            var res = command.DoCommand();
            Assert.That(res.Status, Is.Not.EqualTo(0));
            Assert.That(res.Error, Is.Not.Empty);
            Assert.That(res.Output, Is.Empty);
        }
    }
}
