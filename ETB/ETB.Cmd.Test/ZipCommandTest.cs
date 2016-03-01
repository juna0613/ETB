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
        [SetUp]
        public void Setup()
        {
            if (File.Exists(@"d:\hoge222.zip"))
            {
                File.Delete(@"d:\hoge222.zip");
            }
        }
        [Test]
        public void Test1()
        {
            var cmd = new ZipCommand(@"d:\hoge222.zip", @"d:\dojo.js", @"d:\Samplecode\");
            var stat = cmd.DoCommand();
            Assert.That(stat.Status, Is.EqualTo(0));
            Assert.That(File.Exists(@"d:\hoge222.zip"));
        }
        [Test]
        public void TestError()
        {
            var cmd = new ZipCommand(@"d:\hoge222.zip", @"d:\dojo2  .js", @"d:\Samplecode2\");
            var stat = cmd.DoCommand();
            Assert.That(stat.Status, Is.Not.EqualTo(0));
            Console.WriteLine("----output-----");
            Console.WriteLine(stat.Output);
            Console.WriteLine("----Error-----");
            Console.WriteLine(stat.Error);
            
        }
    }
}
