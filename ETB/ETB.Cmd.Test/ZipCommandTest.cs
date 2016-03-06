using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
namespace ETB.Cmd.Test
{
    [TestFixture]
    public class ZipCommandTest
    {
        private readonly string _filePath = new[] { System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName() + ".zip" }.Join(@"\");
        [SetUp]
        public void Setup()
        {
            _filePath.DeleteIfExist();
        }
        [TearDown]
        public void Teardown()
        {
           _filePath.DeleteIfExist();
        }
        [Test]
        public void TestIfAnyOfInputFilesAreEmpty_ReturnsErrorStatus()
        {
            var cmd = new ZipCommand(_filePath, new[] { "d", Path.GetRandomFileName() }.PathJoin());
            var stat = cmd.DoCommand();
            Assert.That(File.Exists(_filePath));
            Assert.That(stat.Status, Is.EqualTo(-1));
            Assert.That(stat.Error, Is.Not.Empty);
            Assert.That(stat.Output, Is.Empty);
        }
        [Test]
        public void TestTrueCase()
        {
            var cmd = new ZipCommand(_filePath, Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)));
            var stat = cmd.DoCommand();
            Assert.That(_filePath, Is.Not.Null);
            Assert.That(stat.Status, Is.EqualTo(0), stat.Error);
            Assert.That(File.Exists(_filePath));
            Assert.That(stat.Output, Contains.Substring("Successfully zipped"));

        }
    }
}
