using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
namespace ETB.IO.Text.Test
{
    [TestFixture]
    public class JsonReadWriteTest
    {
        private readonly string _filepath = TempFileHelper.CreateTempFileName();
        [SetUp]
        public void Setup()
        {
            if (File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
        }
        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
        }
        [Test]
        public void TestJsonWriteRead()
        {
            TestHelper.Test(_filepath, w => new ETB.IO.Text.Json.JsonWriter<MyClass>(w), r => new ETB.IO.Text.Json.JsonLoader<MyClass>(r));
        }

        [Test]
        public void TestJsonDirWriteRead()
        {
            var data = new[]
            {
                new MyClass { Data = 0.1, Hoge = DateTime.Today, Is = false, Muu = @"it, was.
                OK" },
                new MyClass { Data = 0.1, Hoge = DateTime.Today, Is = true, Muu = null },
            };
            List<MyClass> rdata = null;
            TempFileHelper.CreateTempFolder(dir =>
            {
                var writer = new Json.JsonDirWriter<MyClass>(dir, m => m.GetHashCode().ToString() + ".json");
                writer.Save(data);

                var loder = new Json.JsonDirLoader<MyClass>(dir);
                rdata = loder.Load().ToList();
            });

            var d1 = rdata.ElementAt(0);
            var d2 = data.First(x => x.Muu == d1.Muu);
            Assert.That(d1.Data, Is.EqualTo(d2.Data));
            Assert.That(d1.Hoge, Is.EqualTo(d2.Hoge));
            Assert.That(d1.Is, Is.EqualTo(d2.Is));
            Assert.That(d1.Muu, Is.EqualTo(d2.Muu));
        }


    }
}
