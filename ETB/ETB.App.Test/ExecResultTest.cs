using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace ETB.App.Test
{
    [TestFixture]
    public class ExecResultTest
    {
        [Test]
        public void TestBestAndWorst()
        {
            var data = new[] { ExecStatus.Fatal, ExecStatus.Warn };
            Assert.That(data.BestStatus(), Is.EqualTo(ExecStatus.Warn));
            Assert.That(data.WorstStatus(), Is.EqualTo(ExecStatus.Fatal));
        }
        [Test]
        public void TestNothing()
        {
            var data = new ExecStatus[] { };
            Assert.That(data.BestStatus(), Is.EqualTo(default(ExecStatus)));
            Assert.That(data.WorstStatus(), Is.EqualTo(default(ExecStatus)));
        }
    }
}
